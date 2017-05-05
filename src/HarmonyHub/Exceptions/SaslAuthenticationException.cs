using System;

namespace HarmonyHub.Exceptions
{
    /// <summary>
    /// Internal XMPP SASL authentication exception.
    /// </summary>
    [Serializable]
    public class SaslAuthenticationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaslAuthenticationException"/> class.
        /// </summary>
        public SaslAuthenticationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaslAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SaslAuthenticationException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaslAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public SaslAuthenticationException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
