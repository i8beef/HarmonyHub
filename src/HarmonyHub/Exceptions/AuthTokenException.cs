using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal Auth token exception.
    /// </summary>
    [Serializable]
    public class AuthTokenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthTokenException"/> class.
        /// </summary>
        public AuthTokenException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthTokenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public AuthTokenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthTokenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public AuthTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
