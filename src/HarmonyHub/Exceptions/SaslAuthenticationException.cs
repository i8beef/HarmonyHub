using System;

namespace HarmonyHub.Exceptions
{
    public class SaslAuthenticationException : Exception
    {
        public SaslAuthenticationException()
        {
        }

        public SaslAuthenticationException(string message)
        : base(message)
        {
        }

        public SaslAuthenticationException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
