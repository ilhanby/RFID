namespace RFIDService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RFIDServiceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.RFIDSERVICE = new System.ServiceProcess.ServiceInstaller();
            // 
            // RFIDServiceProcessInstaller1
            // 
            this.RFIDServiceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.RFIDServiceProcessInstaller1.Password = null;
            this.RFIDServiceProcessInstaller1.Username = null;
            // 
            // RFIDSERVICE
            // 
            this.RFIDSERVICE.ServiceName = "RFID";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.RFIDServiceProcessInstaller1,
            this.RFIDSERVICE});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller RFIDServiceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller RFIDSERVICE;
    }
}