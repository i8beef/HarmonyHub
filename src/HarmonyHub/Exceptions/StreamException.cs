using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal stream exception.
    /// </summary>
    [Serializable]
    public class StreamException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamException"/> class.
        /// </summary>
        public StreamException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public StreamException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public StreamException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
