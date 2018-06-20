using System;

namespace WindowsSupervisor.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Error(string message)
        {
            Console.WriteLine(String.Format("[ERROR]: {0}", message));
        }

        public override void Info(string message)
        {
            Console.WriteLine(String.Format("[INFO]: {0}", message));
        }
    }
}
