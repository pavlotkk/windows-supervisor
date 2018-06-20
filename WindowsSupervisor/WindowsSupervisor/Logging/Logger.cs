using System;

namespace WindowsSupervisor.Logging
{
    public abstract class Logger
    {
        public abstract void Info(String message);
        public abstract void Error(String message);
    }
}
