using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Content configuration element.
    /// </summary>
    [DataContract]
    public class ContentConfigElement
    {
        /// <summary>
        /// Content user host.
        /// </summary>
        [DataMember(Name = "contentUserHost")]
        public string ContentUserHost { get; set; }

        /// <summary>
        /// Content device host.
        /// </summary>
        [DataMember(Name = "contentDeviceHost")]
        public string ContentDeviceHost { get; set; }

        /// <summary>
        /// Content service host.
        /// </summary>
        [DataMember(Name = "contentServiceHost")]
        public string ContentServiceHost { get; set; }

        /// <summary>
        /// Content image host.
        /// </summary>
        [DataMember(Name = "contentImageHost")]
        public string ContentImageHost { get; set; }

        /// <summary>
        /// Content household user profile URI.
        /// </summary>
        [DataMember(Name = "householdUserProfileUri")]
        public string HouseholdUserProfileUri { get; set; }
    }
}