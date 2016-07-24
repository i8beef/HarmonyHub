using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class GlobalConfigElement
    {
        [DataMember(Name = "timeStampHash")]
        public string TimeStampHash { get; set; }

        [DataMember(Name = "locale")]
        public string Locale { get; set; }
    }
}
