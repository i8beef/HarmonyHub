using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Harmony configuration.
    /// </summary>
    [DataContract]
    public class HarmonyConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarmonyConfig"/> class.
        /// </summary>
        public HarmonyConfig()
        {
            Activity = new List<ActivityConfigElement>();
            Device = new List<DeviceConfigElement>();
            Sequence = new List<SequenceConfigElement>();
        }

        /// <summary>
        /// Harmony configuration activities.
        /// </summary>
        [DataMember(Name = "activity")]
        public IList<ActivityConfigElement> Activity { get; set; }

        /// <summary>
        /// Harmony configuration devices.
        /// </summary>
        [DataMember(Name = "device")]
        public IList<DeviceConfigElement> Device { get; set; }

        /// <summary>
        /// Harmony configuration sequences.
        /// </summary>
        [DataMember(Name = "sequence")]
        public IList<SequenceConfigElement> Sequence { get; set; }

        /// <summary>
        /// Harmony configuration content.
        /// </summary>
        [DataMember(Name = "content")]
        public ContentConfigElement Content { get; set; }

        /// <summary>
        /// Harmony configuration global settings.
        /// </summary>
        [DataMember(Name = "global")]
        public GlobalConfigElement Global { get; set; }
    }
}
