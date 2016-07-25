using HarmonyHub.Config;
using System;

namespace HarmonyHub.Events
{
    public class CurrentActivityEventArgs : EventArgs
    {
        public ActivityConfigElement Activity { get; private set; }

        public CurrentActivityEventArgs(ActivityConfigElement activity)
        {
            Activity = activity;
        }
    }
}
