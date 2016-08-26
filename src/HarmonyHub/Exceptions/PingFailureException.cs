using System;

namespace HarmonyHub.Exceptions
{
    public class PingFailureException : Exception
    {
        public PingFailureException()
        {
        }

        public PingFailureException(string message)
        : base(message)
        {
        }

        public PingFailureException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
