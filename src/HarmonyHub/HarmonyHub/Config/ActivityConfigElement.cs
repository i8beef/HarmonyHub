using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class ActivityConfigElement
    {
        public ActivityConfigElement()
        {
            ControlGroup = new List<ControlGroupConfigElement>();
            Sequences = new List<SequenceConfigElement>();
            Fixit = new Dictionary<int, FixitConfigElement>();
            Rules = new List<RuleConfigElement>();
        }

        [DataMember(Name = "suggestedDisplay")]
        public string SuggestedDisplay { get; set; }

        [DataMember(Name = "isAVActivity")]
        public bool IsAVActivity { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "activityTypeDisplayName")]
        public string ActivityTypeDisplayName { get; set; }

        [DataMember(Name = "controlGroup")]
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }

        [DataMember(Name = "sequences")]
        public IList<SequenceConfigElement> Sequences { get; set; }

        [DataMember(Name = "fixit")]
        public IDictionary<int, FixitConfigElement> Fixit { get; set; }

        [DataMember(Name = "rules")]
        public IList<RuleConfigElement> Rules { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
