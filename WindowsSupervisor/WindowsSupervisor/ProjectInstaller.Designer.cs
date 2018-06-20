namespace WindowsSupervisor
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
            this.WinSupervisorProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.WinSupervisorInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // WinSupervisorProcessInstaller
            // 
            this.WinSupervisorProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.WinSupervisorProcessInstaller.Password = null;
            this.WinSupervisorProcessInstaller.Username = null;
            // 
            // WinSupervisorInstaller
            // 
            this.WinSupervisorInstaller.Description = "Windows Supervisor Service for runnig background tasks";
            this.WinSupervisorInstaller.DisplayName = "WinSupervisor";
            this.WinSupervisorInstaller.ServiceName = "WinSupervisor";
            this.WinSupervisorInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WinSupervisorProcessInstaller,
            this.WinSupervisorInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WinSupervisorProcessInstaller;
        private System.ServiceProcess.ServiceInstaller WinSupervisorInstaller;
    }
}