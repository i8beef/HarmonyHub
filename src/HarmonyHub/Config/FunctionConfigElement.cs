using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Function configuration element.
    /// </summary>
    [DataContract]
    public class FunctionConfigElement
    {
        /// <summary>
        /// Function action.
        /// </summary>
        [DataMember(Name = "action")]
        public string Action { get; set; }

        /// <summary>
        /// Function name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Function label.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
