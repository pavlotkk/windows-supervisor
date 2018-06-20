using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using WindowsSupervisor.Logging;
using WindowsSupervisor.Monitoring;

namespace WindowsSupervisor
{
    public partial class WinSupervisor : ServiceBase
    {
        private const string APP_DIR = "%appdir%";

        private Supervisor supervisor;

        public WinSupervisor()
        {
            InitializeComponent();

            var addDir = AppDomain.CurrentDomain.BaseDirectory;
            string filename = ConfigurationManager.AppSettings["filename"].ToString();
            string argsLine = ConfigurationManager.AppSettings["args"].ToString();

            filename = filename.Replace(APP_DIR, addDir);

            string[] args = argsLine.Split(' ');
            for(int i=0; i<args.Length; i++)
            {
                args[i] = args[i].Replace(APP_DIR, addDir);
            }

            WatchableInfo info = new WatchableInfo(
                filename,
                args,
                autorestart: ConfigurationManager.AppSettings["autorestart"].ToString().Equals("true"),
                restartTimeout: Int32.Parse(ConfigurationManager.AppSettings["restarttimeout"].ToString())
            );

            this.supervisor = new Supervisor(info);

            if (ConfigurationManager.AppSettings["logger.console.enable"].ToString().Equals("true"))
            {
                this.supervisor.AddLogger(new ConsoleLogger());
            }
            if (ConfigurationManager.AppSettings["logger.windows.event.enable"].ToString().Equals("true"))
            {
                string appName = ConfigurationManager.AppSettings["appname"].ToString();
                this.supervisor.AddLogger(new WindowsEventLogger(appName, appName));
            }
        }

        public void StartSupervisor()
        {
            this.supervisor.Start();
        }

        public void StopSupervisor()
        {
            this.supervisor.Stop();
        }

        protected override void OnStart(string[] args)
        {
            StartSupervisor();
        }

        protected override void OnStop()
        {
            StopSupervisor();
        }
    }
}
