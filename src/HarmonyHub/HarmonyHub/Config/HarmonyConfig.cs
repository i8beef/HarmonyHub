using System.Collections.Generic;

namespace HarmonyHub.Config
{
    public class HarmonyConfig
    {
        public HarmonyConfig()
        {
            Activity = new List<ActivityConfigElement>();
            Device = new List<DeviceConfigElement>();
            Sequence = new List<SequenceConfigElement>();
            Content = new Dictionary<string, string>();
        }

        public IList<ActivityConfigElement> Activity { get; set; }
        public IList<DeviceConfigElement> Device { get; set; }
        public IList<SequenceConfigElement> Sequence { get; set; }
        public IDictionary<string, string> Content { get; set; }
        public GlobalConfigElement Global { get; set; }
    }
}
