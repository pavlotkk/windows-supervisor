using System;
using System.Linq;

namespace WindowsSupervisor.Monitoring
{
    class WatchableInfo
    {
        public bool AutoRestart { get; }
        public string FileName { get; }
        public string Arguments { get; }
        public int RestartTimeout { get; }

        public WatchableInfo(string filename, string[] args, bool autorestart = false, int restartTimeout = 3000)
        {
            this.FileName = filename;
            this.Arguments = String.Join(" ", args.Select(i => String.Format("\"{0}\"", i)).ToArray());
            this.AutoRestart = autorestart;
            this.RestartTimeout = restartTimeout;
        }
    }
}
