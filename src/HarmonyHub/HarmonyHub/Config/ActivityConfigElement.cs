using System.Collections.Generic;

namespace HarmonyHub.Config
{
    public class ActivityConfigElement
    {
        public ActivityConfigElement()
        {
            ControlGroup = new List<ControlGroupConfigElement>();
            Sequences = new List<SequenceConfigElement>();
            Fixit = new Dictionary<int, FixitConfigElement>();
            Rules = new List<RuleConfigElement>();
        }

        public string SuggestedDisplay { get; set; }
        public bool IsAVActivity { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public string ActivityTypeDisplayName { get; set; }
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }
        public IList<SequenceConfigElement> Sequences { get; set; }
        public IDictionary<int, FixitConfigElement> Fixit { get; set; }
        public IList<RuleConfigElement> Rules { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
    }
}
