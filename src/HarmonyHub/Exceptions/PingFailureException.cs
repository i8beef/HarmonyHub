using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal ping failure exception.
    /// </summary>
    [Serializable]
    public class PingFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PingFailureException"/> class.
        /// </summary>
        public PingFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingFailureException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public PingFailureException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PingFailureException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public PingFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
