using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal session token exception.
    /// </summary>
    [Serializable]
    public class SessionTokenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTokenException"/> class.
        /// </summary>
        public SessionTokenException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTokenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SessionTokenException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionTokenException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public SessionTokenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
