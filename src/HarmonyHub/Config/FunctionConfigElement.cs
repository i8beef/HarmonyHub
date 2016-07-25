using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class FunctionConfigElement
    {
        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
