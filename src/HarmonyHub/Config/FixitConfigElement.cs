using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Fixit configuration element.
    /// </summary>
    [DataContract]
    public class FixitConfigElement
    {
        /// <summary>
        /// Fixit ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Fixit power.
        /// </summary>
        [DataMember(Name = "power")]
        public string Power { get; set; }

        /// <summary>
        /// Indicates if fixit is manually powered.
        /// </summary>
        [DataMember(Name = "isManualPower")]
        public bool IsManualPower { get; set; }

        /// <summary>
        /// Indicates if fixit is relative powered.
        /// </summary>
        [DataMember(Name = "isRelativePower")]
        public bool IsRelativePower { get; set; }
    }
}
