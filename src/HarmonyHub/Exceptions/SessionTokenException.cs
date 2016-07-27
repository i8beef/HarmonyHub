using System;

namespace HarmonyHub.Exceptions
{
    public class SessionTokenException : Exception
    {
        public SessionTokenException()
        {
        }

        public SessionTokenException(string message)
        : base(message)
        {
        }

        public SessionTokenException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
