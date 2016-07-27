using System;

namespace HarmonyHub.Exceptions
{
    public class StreamException : Exception
    {
        public StreamException()
        {
        }

        public StreamException(string message)
        : base(message)
        {
        }

        public StreamException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
