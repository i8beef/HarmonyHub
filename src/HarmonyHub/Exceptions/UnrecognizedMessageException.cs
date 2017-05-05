using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal unrecognized message exception.
    /// </summary>
    [Serializable]
    public class UnrecognizedMessageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedMessageException"/> class.
        /// </summary>
        public UnrecognizedMessageException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedMessageException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public UnrecognizedMessageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedMessageException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public UnrecognizedMessageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
