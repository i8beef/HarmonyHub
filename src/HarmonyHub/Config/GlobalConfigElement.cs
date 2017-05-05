using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Global configuration element.
    /// </summary>
    [DataContract]
    public class GlobalConfigElement
    {
        /// <summary>
        /// Timestamp hash.
        /// </summary>
        [DataMember(Name = "timeStampHash")]
        public string TimeStampHash { get; set; }

        /// <summary>
        /// Current locale.
        /// </summary>
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
    }
}
