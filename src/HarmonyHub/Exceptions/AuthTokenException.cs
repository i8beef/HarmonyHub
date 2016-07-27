using System;

namespace HarmonyHub.Exceptions
{
    public class AuthTokenException : Exception
    {
        public AuthTokenException()
        {
        }

        public AuthTokenException(string message)
        : base(message)
        {
        }

        public AuthTokenException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
