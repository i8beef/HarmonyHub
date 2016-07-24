using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class FixitConfigElement
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "power")]
        public string Power { get; set; }

        [DataMember(Name = "isManualPower")]
        public bool IsManualPower { get; set; }

        [DataMember(Name = "isRelativePower")]
        public bool IsRelativePower { get; set; }
    }
}
