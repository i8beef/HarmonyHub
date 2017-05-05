using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Activity configuration element.
    /// </summary>
    [DataContract]
    public class ActivityConfigElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityConfigElement"/> class.
        /// </summary>
        public ActivityConfigElement()
        {
            ControlGroup = new List<ControlGroupConfigElement>();
            Sequences = new List<string>();
            Fixit = new Dictionary<int, FixitConfigElement>();
            Rules = new List<RuleConfigElement>();
        }

        /// <summary>
        /// Suggested Display.
        /// </summary>
        [DataMember(Name = "suggestedDisplay")]
        public string SuggestedDisplay { get; set; }

        /// <summary>
        /// Indicates if AV is active.
        /// </summary>
        [DataMember(Name = "isAVActivity")]
        public bool IsAVActivity { get; set; }

        /// <summary>
        /// Activity label.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>
        /// Activity ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Activity type display name.
        /// </summary>
        [DataMember(Name = "activityTypeDisplayName")]
        public string ActivityTypeDisplayName { get; set; }

        /// <summary>
        /// Activity control groups.
        /// </summary>
        [DataMember(Name = "controlGroup")]
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }

        /// <summary>
        ///  Activity sequences.
        /// </summary>
        [DataMember(Name = "sequences")]
        public IList<string> Sequences { get; set; }

        /// <summary>
        /// Current "fixit" elements for activity.
        /// </summary>
        [DataMember(Name = "fixit")]
        public IDictionary<int, FixitConfigElement> Fixit { get; set; }

        /// <summary>
        /// Activity rules.
        /// </summary>
        [DataMember(Name = "rules")]
        public IList<RuleConfigElement> Rules { get; set; }

        /// <summary>
        /// Icon for activity.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Activity type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
