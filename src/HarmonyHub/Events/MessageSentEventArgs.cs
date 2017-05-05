using System;

namespace HarmonyHub.Events
{
    /// <summary>
    /// Message sent event arguments.
    /// </summary>
    public class MessageSentEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSentEventArgs"/> class.
        /// </summary>
        /// <param name="message">The received message.</param>
        public MessageSentEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The message.
        /// </summary>
        public string Message { get; private set; }
    }
}
