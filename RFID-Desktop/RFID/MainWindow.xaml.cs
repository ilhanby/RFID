using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RFID.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections;
using Symbol.RFID3;

namespace RFID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register("Colors", typeof(List<KeyValuePair<string, Color>>),
            typeof(MainWindow), new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        private void toogle_Checked(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("Base" + "Dark"));
        }

        private void toogle_Unchecked(object sender, RoutedEventArgs e)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("Base" + "Light"));
        }

        private void ColorsSelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = this.ColorsSelector.SelectedItem as KeyValuePair<string, Color>?;
            if (selectedColor.HasValue)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);
                Application.Current.MainWindow.Activate();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(this, theme.Item2, theme.Item1);

            connectBackgroundWorker.DoWork += ConnectBackgroundWorker_DoWork;
            connectBackgroundWorker.RunWorkerCompleted += ConnectBackgroundWorker_RunWorkerCompleted;
            connectBackgroundWorker.ProgressChanged += ConnectBackgroundWorker_ProgressChanged;

            m_ReadTag = new Symbol.RFID3.TagData();
            m_UpdateStatusHandler = new UpdateStatus(myUpdateStatus);
            m_UpdateReadHandler = new UpdateRead(myUpdateRead);
            m_TagTable = new Hashtable();
            m_IsConnected = false;
            m_TagTotalCount = 0;
        }

        private void ConnectBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            IPtxt.IsEnabled = false;
        }

        private void ConnectBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (connectionButton.Content == "Connect")
            {
                if (e.Result.ToString() == "Connect Succeed")
                {
                    connectionButton.Content = "Disconnect";
                    start.IsEnabled = true;

                    /*
                     *  Events Registration
                     */
                    m_ReaderAPI.Events.ReadNotify += new Events.ReadNotifyHandler(Events_ReadNotify);
                    m_ReaderAPI.Events.AttachTagDataWithReadEvent = false;
                    m_ReaderAPI.Events.StatusNotify += new Events.StatusNotifyHandler(Events_StatusNotify);
                    m_ReaderAPI.Events.NotifyGPIEvent = true;
                    m_ReaderAPI.Events.NotifyBufferFullEvent = true;
                    m_ReaderAPI.Events.NotifyBufferFullWarningEvent = true;
                    m_ReaderAPI.Events.NotifyReaderDisconnectEvent = true;
                    m_ReaderAPI.Events.NotifyReaderExceptionEvent = true;
                    m_ReaderAPI.Events.NotifyAccessStartEvent = true;
                    m_ReaderAPI.Events.NotifyAccessStopEvent = true;
                    m_ReaderAPI.Events.NotifyInventoryStartEvent = true;
                    m_ReaderAPI.Events.NotifyInventoryStopEvent = true;

                }
            }
            else if (connectionButton.Content == "Disconnect")
            {
                if (e.Result.ToString() == "Disconnect Succeed")
                {
                    connectionButton.Content = "Connect";
                    this.start.IsEnabled = false;
                }
            }
            loglst.Items.Add(e.Result.ToString());
            connectionButton.IsEnabled = true;
        }

        private void ConnectBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((string)e.Argument == "Connect")
            {
                m_ReaderAPI = new RFIDReader(IPtxt.Text, uint.Parse("5064"), 0);

                try
                {
                    m_ReaderAPI.Connect();
                    m_IsConnected = true;
                    e.Result = "Connect Succeed";

                }
                catch (OperationFailureException operationException)
                {
                    e.Result = operationException.StatusDescription;
                }
                catch (Exception ex)
                {
                    e.Result = ex.Message;
                }
            }
            else if ((string)e.Argument == "Disconnect")
            {
                try
                {

                    m_ReaderAPI.Disconnect();
                    m_IsConnected = false;
                    e.Result = "Disconnect Succeed";
                    m_ReaderAPI = null;

                }
                catch (OperationFailureException ofe)
                {
                    e.Result = ofe.Result;
                }
            }
        }

        /// ////////////////////////////////////////////////////////////////////////////////////////

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connectBackgroundWorker.RunWorkerAsync(IPtxt.Text);
                if (m_IsConnected)
                {

                    m_ReaderAPI.Actions.Inventory.Perform(null, null, null);

                    datagrd.Items.Clear();
                    m_TagTable.Clear();
                    m_TagTotalCount = 0;

                }
                else
                {
                    loglst.Items.Add("Please connect to a reader");
                }
            }
            catch (Exception ex)
            {
                loglst.Items.Add(ex.Message);
            }
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            if (m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0)
            {
                m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence();
            }
            else
            {
                m_ReaderAPI.Actions.Inventory.Stop();
            }
        }

        TriggerInfo trigInfo = new TriggerInfo();
        BackgroundWorker conworker = new BackgroundWorker();
        BackgroundWorker accessBackgroundWorker = new BackgroundWorker();
        internal RFIDReader m_ReaderAPI;
        internal bool m_IsConnected;
        internal string m_SelectedTagID = null;

        BackgroundWorker connectBackgroundWorker = new BackgroundWorker();
        private delegate void UpdateStatus(Events.StatusEventData eventData);
        private UpdateStatus m_UpdateStatusHandler = null;
        private delegate void UpdateRead(Events.ReadEventData eventData);
        private UpdateRead m_UpdateReadHandler = null;
        private TagData m_ReadTag = null;
        private Hashtable m_TagTable;
        private uint m_TagTotalCount;
        List<Tags> GetTags;
        Tags tags;

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                trigInfo.StartTrigger.Type = START_TRIGGER_TYPE.START_TRIGGER_TYPE_IMMEDIATE;
                trigInfo.StopTrigger.Type = STOP_TRIGGER_TYPE.STOP_TRIGGER_TYPE_DURATION;
                trigInfo.StopTrigger.Duration = 10000;

                trigInfo.EnableTagEventReport = true;
                trigInfo.TagEventReportInfo.ReportNewTagEvent = TAG_EVENT_REPORT_TRIGGER.IMMEDIATE;
                trigInfo.TagEventReportInfo.ReportTagBackToVisibilityEvent = TAG_EVENT_REPORT_TRIGGER.NEVER;
                trigInfo.TagEventReportInfo.ReportTagInvisibleEvent = TAG_EVENT_REPORT_TRIGGER.NEVER;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("Bağlantı Hatası", "Cihaz bağlantısı sağlanamadı lütfen bağlantı ayarlarınızı kontrol ettikten sonra tekrar deneyiniz" + "\n" + ex + "");
                this.Close();
            }
        }

        private void myUpdateStatus(Events.StatusEventData eventData)
        {
            switch (eventData.StatusEventType)
            {
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_START_EVENT:
                    loglst.Items.Add("Inventory started");
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_STOP_EVENT:
                    loglst.Items.Add("Inventory stopped");
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_START_EVENT:
                    loglst.Items.Add("Access Operation started");
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_STOP_EVENT:
                    loglst.Items.Add("Access Operation stopped");

                    if (this.datagrd.Items == null)
                    {
                        uint successCount, failureCount;
                        successCount = failureCount = 0;
                        m_ReaderAPI.Actions.TagAccess.GetLastAccessResult(ref successCount, ref failureCount);
                        loglst.Items.Add("Access completed - Success Count: " + successCount.ToString()
                            + ", Failure Count: " + failureCount.ToString());
                    }
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.GPI_EVENT:
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ANTENNA_EVENT:
                    string status = (eventData.AntennaEventData.AntennaEvent == ANTENNA_EVENT_TYPE.ANTENNA_CONNECTED ? "connected" : "disconnected");
                    loglst.Items.Add("Antenna " + eventData.AntennaEventData.AntennaID.ToString() + " has been " + status);
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_WARNING_EVENT:
                    loglst.Items.Add(" Buffer full warning");
                    myUpdateRead(null);
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_EVENT:
                    loglst.Items.Add("Buffer full");
                    myUpdateRead(null);
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.DISCONNECTION_EVENT:
                    loglst.Items.Add("Disconnection Event " + eventData.DisconnectionEventData.DisconnectEventInfo.ToString());
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.READER_EXCEPTION_EVENT:
                    loglst.Items.Add("Reader ExceptionEvent " + eventData.ReaderExceptionEventData.ReaderExceptionEventInfo.ToString());
                    break;
                case Symbol.RFID3.Events.STATUS_EVENT_TYPE.TEMPERATURE_ALARM_EVENT:
                    loglst.Items.Add("Temperature Alarm " + eventData.TemperatureAlarmEventData.SourceName.ToString() + " Temperature " + eventData.TemperatureAlarmEventData.CurrentTemperature.ToString() + " Level " + eventData.TemperatureAlarmEventData.AlarmLevel.ToString());
                    break;
                default:
                    break;
            }
        }

        private void myUpdateRead(Events.ReadEventData eventData)
        {
            int index = 0;
            Symbol.RFID3.TagData[] tagData = m_ReaderAPI.Actions.GetReadTags(1000);
            List<Tags> list = (List<Tags>)datagrd.ItemsSource;
            GetTags = new List<Tags>();
            tags = new Tags();
            if (tagData != null)
            {
                for (int nIndex = 0; nIndex < tagData.Length; nIndex++)
                {
                    if (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_NONE ||
                        (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ &&
                        tagData[nIndex].OpStatus == ACCESS_OPERATION_STATUS.ACCESS_SUCCESS))
                    {
                        TagData tag = tagData[nIndex];
                        string tagID = tag.TagID;
                        bool isFound = false;

                        lock (m_TagTable.SyncRoot)
                        {
                            isFound = m_TagTable.ContainsKey(tagID);
                            if (!isFound)
                            {
                                tagID += ((uint)tag.MemoryBank + tag.MemoryBankDataOffset);
                                isFound = m_TagTable.ContainsKey(tagID);
                            }
                        }

                        if (isFound)
                        {
                            uint count = 0;
                            tags.EPC = tag.TagID;
                            tags.Antenna = tag.AntennaID.ToString();
                            tags.Count = count;
                            tags.RSSI = tag.PeakRSSI.ToString();
                            if (list == null)
                            {
                                list = new List<Tags>();
                            }
                            if (!list.Exists(a => a.EPC == tags.EPC))
                            {
                                GetTags.Add(tags);
                                list.AddRange(GetTags);
                                datagrd.ItemsSource = list;
                            }
                            else
                            {
                                GetTags.Add(tags);
                                list.AddRange(GetTags);
                                datagrd.ItemsSource = list;
                            }
                            tagstxt.Text = datagrd.Items.Count.ToString();
                            datagrd.Items.Refresh();
                        }
                    }
                }
                loglst.Items.Add( m_TagTable.Count + "(" + m_TagTotalCount + ")");
            }
        }

        private void Events_ReadNotify(object sender, Events.ReadEventArgs readEventArgs)
        {
            try
            {
                //this.Invoke(m_UpdateReadHandler, new object[] { readEventArgs.ReadEventData.TagData });
            }
            catch (Exception)
            {
            }
        }

        public void Events_StatusNotify(object sender, Events.StatusEventArgs statusEventArgs)
        {
            try
            {
                //this.Invoke(m_UpdateStatusHandler, new object[] { statusEventArgs.StatusEventData });
            }
            catch (Exception)
            {
            }
        }

        private void connectionButton_Checked(object sender, RoutedEventArgs e)
        {
            connectBackgroundWorker.RunWorkerAsync(connectionButton.Content);
        }
    }
}
