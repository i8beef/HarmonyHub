using System;

namespace HarmonyHub.Events
{
    /// <summary>
    /// Message received event arguments.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The received message.</param>
        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The message.
        /// </summary>
        public string Message { get; private set; }
    }
}
