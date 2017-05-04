using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class SequenceConfigElement
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
