using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class HarmonyConfig
    {
        public HarmonyConfig()
        {
            Activity = new List<ActivityConfigElement>();
            Device = new List<DeviceConfigElement>();
            Sequence = new List<SequenceConfigElement>();
        }

        [DataMember(Name = "activity")]
        public IList<ActivityConfigElement> Activity { get; set; }

        [DataMember(Name = "device")]
        public IList<DeviceConfigElement> Device { get; set; }

        [DataMember(Name = "sequence")]
        public IList<SequenceConfigElement> Sequence { get; set; }

        [DataMember(Name = "content")]
        public ContentConfigElement Content { get; set; }

        [DataMember(Name = "global")]
        public GlobalConfigElement Global { get; set; }
    }
}
