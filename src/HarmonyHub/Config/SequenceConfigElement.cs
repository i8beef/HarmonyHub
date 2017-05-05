using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Sequence configuration element.
    /// </summary>
    [DataContract]
    public class SequenceConfigElement
    {
        /// <summary>
        /// Sequence ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Sequence name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
