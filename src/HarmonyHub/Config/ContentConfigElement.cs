using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class ContentConfigElement
    {
        [DataMember(Name = "contentUserHost")]
        public string ContentUserHost { get; set; }

        [DataMember(Name = "contentDeviceHost")]
        public string ContentDeviceHost { get; set; }

        [DataMember(Name = "contentServiceHost")]
        public string ContentServiceHost { get; set; }

        [DataMember(Name = "contentImageHost")]
        public string ContentImageHost { get; set; }

        [DataMember(Name = "householdUserProfileUri")]
        public string HouseholdUserProfileUri { get; set; }
    }
}