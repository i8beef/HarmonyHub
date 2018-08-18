using System;
using System.IO;
using System.Threading.Tasks;
using HarmonyHub.Config;
using HarmonyHub.Events;

namespace HarmonyHub
{
    /// <summary>
    /// Harmony Hub client.
    /// </summary>
    public interface IClient : IDisposable
    {
        /// <summary>
        /// The event that is raised when CurrentActivity is updated.
        /// </summary>
        event EventHandler<ActivityUpdatedEventArgs> CurrentActivityUpdated;

        /// <summary>
        /// The event that is raised when an unrecoverable error condition occurs.
        /// </summary>
        event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// The event that is raised when messages are received.
        /// </summary>0
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// The event that is raised when messages are sent.
        /// </summary>
        event EventHandler<MessageSentEventArgs> MessageSent;

        /// <summary>
        /// Connected.
        /// </summary>
        bool Authenticated { get; }

        /// <summary>
        /// Connected.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Allows for explicit closing of session.
        /// </summary>
        void Close();

        /// <summary>
        /// Connect to HarmonyHub.
        /// </summary>
        void Connect();

        /// <summary>
        /// Gets the current Harmony configuration.
        /// </summary>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<HarmonyConfig> GetConfigAsync();

        /// <summary>
        /// Send message to HarmonyHub to request current activity.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> GetCurrentActivityIdAsync();

        /// <summary>
        /// Send command to the HarmonyHub.
        /// </summary>
        /// <param name="command">Command to send.</param>
        /// <param name="press">Represents a press or a release.</param>
        /// <param name="timestamp">Timestamp which harmony uses to order requests.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendCommandAsync(string command, bool press = true, int? timestamp = null);

        /// <summary>
        /// Send a key press event (press and release combo).
        /// </summary>
        /// <param name="command">Command to send.</param>
        /// <param name="timespan">The time between the press and release, default 100ms</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendKeyPressAsync(string command, int timespan = 100);

        /// <summary>
        /// Sends a ping to HarmonyHub to keep connection alive.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SendPingAsync();

        /// <summary>
        /// Send message to HarmonyHub to start a given activity
        /// </summary>
        /// <remarks>
        /// Send "-1" to trigger turning off.
        /// </remarks>
        /// <param name="activityId">The id of the activity to activate.</param>
        /// <returns>>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartActivityAsync(int activityId);
    }
}