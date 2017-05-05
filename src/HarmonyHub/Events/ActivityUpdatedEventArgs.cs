using System;

namespace HarmonyHub.Events
{
    /// <summary>
    /// Activity updated event arguments.
    /// </summary>
    public class ActivityUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The updated activity ID.</param>
        public ActivityUpdatedEventArgs(int id)
        {
            Id = id;
        }

        /// <summary>
        /// The updated activity ID.
        /// </summary>
        public int Id { get; private set; }
    }
}
