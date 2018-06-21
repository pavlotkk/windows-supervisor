using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using WindowsSupervisor.Logging;

namespace WindowsSupervisor.Monitoring
{
    class Supervisor
    {
        private WatchableInfo _watchableInfo;
        private ProcessStartInfo _processStartInfo;
        private Process _process = null;
        private Thread _thread;

        private readonly System.Object _locker = new System.Object();

        private bool _isRunning = false;
        private List<Logger> _loggers = new List<Logger>();

        public Supervisor(WatchableInfo watchableInfo)
        {
            this._watchableInfo = watchableInfo;
            this._processStartInfo = new ProcessStartInfo();
            this._processStartInfo.FileName = watchableInfo.FileName;
            this._processStartInfo.Arguments = watchableInfo.Arguments;
            this._processStartInfo.UseShellExecute = false;
            this._processStartInfo.CreateNoWindow = true;
            this._processStartInfo.RedirectStandardOutput = true;
            this._processStartInfo.RedirectStandardError = true;
        }

        /// <summary>
        /// Start supervising Watchable task in separate thread.
        /// </summary>
        public void Start()
        {
            lock (this._locker)
            {
                if (this._isRunning)
                {
                    return;
                }

                this._isRunning = true;
            }

            this._thread = new Thread(new ThreadStart(this.Run));
            this._thread.Start();
        }

        /// <summary>
        /// Stop supervisioning
        /// </summary>
        public void Stop()
        {
            lock (this._locker)
            {
                if (!this._isRunning)
                {
                    return;
                }

                this._isRunning = false;
            }

            this.Kill();
        }

        /// <summary>
        /// Restart supervisor
        /// </summary>
        public void Restart()
        {
            this.Stop();
            this.Start();
        }

        /// <summary>
        /// Run supervisor's task in main thead
        /// </summary>
        public void Run()
        {
            while (true)
            {
                lock (this._locker)
                {
                    if (!this._isRunning)
                    {
                        break;
                    }
                }

                if (this._process == null)
                {
                    try
                    {
                        this._process = Process.Start(this._processStartInfo);
                    }
                    catch (Exception e)
                    {
                        this.LogError(
                            String.Format(
                                "Fail to start process '{0} {1}' : {2}\n=======",
                                this._watchableInfo.FileName,
                                this._watchableInfo.Arguments,
                                e.ToString()));

                        if (this._watchableInfo.AutoRestart)
                        {
                            this.Sleep();
                            this.LogInfo("Restarting...");
                            continue;
                        }
                        else
                        {
                            lock (this._locker)
                            {
                                this._isRunning = false;
                            }
                            break;
                        }

                    }
                }

                string stdoutx = this._process.StandardOutput.ReadToEnd();
                string stderrx = this._process.StandardError.ReadToEnd();
                this._process.WaitForExit();

                this.LogError(
                    String.Format(
                        "Application Failed: \nExit code : {0}\nStderr : {1}\n=======",
                        this._process.ExitCode,
                        stderrx
                     )
                );

                if (this._watchableInfo.AutoRestart)
                {
                    this._process = null;

                    this.Sleep();
                    this.LogInfo("Restarting...");
                    continue;
                }
                else
                {
                    lock (this._locker)
                    {
                        this._isRunning = false;
                        this._process = null;
                    }
                    break;
                }
            }

        }

        /// <summary>
        /// Add logger
        /// </summary>
        /// <param name="logger">logger instance</param>
        public void AddLogger(Logger logger)
        {
            this._loggers.Add(logger);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">message</param>
        protected void LogError(string message)
        {
            foreach (var log in this._loggers)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="message">message</param>
        protected void LogInfo(string message)
        {
            foreach (var log in this._loggers)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// Sleep task execution. 
        /// You can set up sleep value in Watchable object
        /// </summary>
        private void Sleep()
        {
            if (this._thread == null)
            {
                return;
            }

            Thread.Sleep(this._watchableInfo.RestartTimeout);
        }

        /// <summary>
        /// Kill process and thread
        /// </summary>
        private void Kill()
        {
            if (this._process != null)
            {
                try
                {
                    this._process.Kill();
                }
                finally
                {
                    this._process = null;
                }
            }

            if (this._thread != null)
            {
                try
                {
                    this._thread.Join(1000);
                }
                finally
                {
                    try
                    {
                        this._thread.Abort();
                    }
                    finally
                    {
                        this._thread = null;
                    }
                }
            }

        }
    }
}
