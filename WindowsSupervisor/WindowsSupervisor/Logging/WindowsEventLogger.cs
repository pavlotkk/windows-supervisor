using System.Diagnostics;

namespace WindowsSupervisor.Logging
{
    public class WindowsEventLogger : Logger
    {
        private readonly EventLog _eventLog;
        public WindowsEventLogger(string source, string name)
        {
            this._eventLog = new EventLog();

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, name);
            }

            this._eventLog.Source = source;
            this._eventLog.Log = name;
        }
        public override void Error(string message)
        {
            this._eventLog.WriteEntry(message, EventLogEntryType.Error);
        }

        public override void Info(string message)
        {
            this._eventLog.WriteEntry(message, EventLogEntryType.Information);
        }
    }
}
