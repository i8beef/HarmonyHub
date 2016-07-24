using HarmonyHub.Config;
using System;

namespace HarmonyHub.Events
{
    public class ConfigUpdatedEventArgs : EventArgs
    {
        public HarmonyConfig Config { get; private set; }

        public ConfigUpdatedEventArgs(HarmonyConfig config)
        {

        }
    }
}
