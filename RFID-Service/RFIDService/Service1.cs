using com.datalogic.DLRFIDLibrary;
using RFIDService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RFIDService
{
    public partial class Service1 : ServiceBase
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Baglanti"].ConnectionString);
        SqlDataReader datareader;
        public System.Timers.Timer timer;
        public System.Timers.Timer exporttimer;
        public DLRFIDReader MyReader = new DLRFIDReader();
        List<Rfidlist> GetRfidlists;
        List<Parameters> GetParameters;
        List<Cars> GetCars;
        Rfidlist rfidlist;
        Parameters parameter;
        Cars cars;
        public string EXPORT_CYCLE_VALUE;
        public string READER_DELAY_VALUE;

        public Service1()
        {
            InitializeComponent();
        }

        public void Baslat()
        {
            MyReader.Connect(DLRFIDPort.DLRFID_TCP, "192.168.1.4");
            double Gain = 8.0;
            double Loss = 1.5;
            double ERPPower = 2000.0;
            int OutPower;
            OutPower = (int)(ERPPower / Math.Pow(10, ((Gain - Loss - 2.14) / 32)));
            MyReader.SetPower(OutPower);

            GetParameters = new List<Parameters>();
            parameter = new Parameters();
            SqlCommand com1 = new SqlCommand("SELECT Value FROM Parameter WHERE Parameter = 'EXPORT_CYCLE' ", con);
            con.Open();
            datareader = com1.ExecuteReader();
            while (datareader.Read())
            {
                EXPORT_CYCLE_VALUE = datareader[0].ToString();
            }
            con.Close();

            SqlCommand com2 = new SqlCommand("SELECT Value FROM Parameter WHERE Parameter = 'READER_DELAY' ", con);
            con.Open();
            datareader = com2.ExecuteReader();
            while (datareader.Read())
            {
                READER_DELAY_VALUE = datareader[0].ToString();
            }
            con.Close();

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            timer.Enabled = true;
            timer.Interval = 250;

            exporttimer = new System.Timers.Timer();
            exporttimer.Elapsed += new System.Timers.ElapsedEventHandler(Exporttimer_Elapsed);
            exporttimer.Enabled = true;
            exporttimer.Interval = Convert.ToDouble(EXPORT_CYCLE_VALUE) * 60000;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            GetRfidlists = new List<Rfidlist>();
            rfidlist = new Rfidlist();
            DLRFIDLogicalSource MySource = MyReader.GetSource("Source_0");
            DLRFIDTag[] MyTags = MySource.InventoryTag();
            MySource.SetQ_EPC_C1G2(6);
            MySource.SetSession_EPC_C1G2(DLRFIDLogicalSourceConstants.EPC_C1G2_SESSION_S0);
            MySource.SetSession_EPC_C1G2(DLRFIDLogicalSourceConstants.EPC_C1G2_ALL_SELECTED);
            MySource.SetReadCycle(0);
            byte[] Mask = new byte[4];
            MySource.EventInventoryTag(Mask, 0x0, 0x0, 0x06);
            MyReader.InventoryAbort();
            if (MyTags == null)
            {

            }
            else
            {
                for (int i = 0; i < MyTags.Length; i++)
                {
                    rfidlist.EPC = BitConverter.ToString(MyTags[i].GetId());
                    rfidlist.Antenna = MyTags[i].GetReadPoint().ToString();

                    SqlCommand komut2 = new SqlCommand("SELECT ISNULL(ID,0),EPC,CreationTime FROM Rfidlist WHERE EPC='" + rfidlist.EPC + "'", con);
                    con.Open();
                    datareader = komut2.ExecuteReader();
                    while (datareader.Read())
                    {
                        rfidlist.Id = (int)datareader[0];
                        rfidlist.CreationTime = Convert.ToDateTime(datareader[2]);
                        GetRfidlists.Add(rfidlist);
                    }
                    con.Close();
                    if (rfidlist.Id == 0)
                    {
                        con.Open();
                        SqlCommand komut = new SqlCommand("insert into Rfidlist(EPC, Antenna) values(@p1, @p2)", con);
                        komut.Parameters.AddWithValue("@p1", rfidlist.EPC);
                        komut.Parameters.AddWithValue("@p2", rfidlist.Antenna);
                        komut.ExecuteNonQuery();
                        komut.Dispose();
                        con.Close();
                    }
                    else
                    {
                        if (rfidlist.CreationTime.AddMinutes(Convert.ToDouble(READER_DELAY_VALUE)) <= DateTime.Now)
                        {
                            con.Open();
                            SqlCommand komut = new SqlCommand("insert into Rfidlist(EPC, Antenna) values(@p1, @p2)", con);
                            komut.Parameters.AddWithValue("@p1", rfidlist.EPC);
                            komut.Parameters.AddWithValue("@p2", rfidlist.Antenna);
                            komut.ExecuteNonQuery();
                            komut.Dispose();
                            con.Close();
                        }
                    }
                    con.Close();
                }
            }
            timer.Start();
            exporttimer.Start();
        }

        private void Exporttimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            exporttimer.Stop();
            Write wrt = new Write();
            byte[] bytes = new byte[0];
            StringBuilder sbLog = new StringBuilder();
            StringBuilder sbExLog = new StringBuilder();
            try
            {
                sbLog.AppendLine("Kayıt Zamanı     :    " + DateTime.Now.ToString() + "");
                sbLog.AppendLine("");
                sbLog.Append("Car Number" + "\t");
                sbLog.Append("Antenna" + "\t\t");
                sbLog.AppendLine("Creation Time" + "\t\t");
                sbLog.AppendLine("");
                GetRfidlists = new List<Rfidlist>();
                SqlCommand komut = new SqlCommand("SELECT DISTINCT c.CarNumber,r.Antenna,r.CreationTime FROM Rfidlist r INNER JOIN Cars c ON r.EPC=c.EPC1 or r.EPC=c.EPC2 or r.EPC=c.EPC3 or r.EPC=c.EPC4 or r.EPC=c.EPC5", con);
                con.Open();
                datareader = komut.ExecuteReader();
                while (datareader.Read())
                {
                    sbLog.Append(datareader[0].ToString() + "\t\t");
                    sbLog.Append(datareader[1].ToString() + "\t\t");
                    sbLog.Append(Convert.ToDateTime(datareader[2] + "\t\t"));
                    sbLog.AppendLine("");
                }
                con.Close();
                wrt.Writing("C:\\Users\\Asus\\Desktop\\KSFORKLIFTS\\", "" + DateTime.Now.ToLongDateString().ToString() + ".txt", sbLog.ToString());
            }
            catch (Exception exc)
            {
                sbExLog.AppendLine(DateTime.Now.ToString());
                sbExLog.AppendLine(exc.Message);
                sbExLog.AppendLine(exc.StackTrace);
                wrt.Writing("C:\\Users\\Asus\\Desktop\\KSFORKLIFTS\\", "HATA.txt", sbExLog.ToString());
                con.Close();
            }
            exporttimer.Start();
        }

        protected override void OnStart(string[] args)
        {
            Baslat();
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            exporttimer.Stop();
            con.Close();
        }
    }
}
