using System;

namespace HarmonyHub.Events
{
    public class MessageSentEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public MessageSentEventArgs(string message)
        {
            Message = message;
        }
    }
}
