using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Harmony notify.
    /// </summary>
    [DataContract]
    public class HarmonyNotify
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarmonyNotify"/> class.
        /// </summary>
        public HarmonyNotify()
        {
            Updates = new Dictionary<string, string>();
        }

        /// <summary>
        /// Sleep timer ID.
        /// </summary>
        [DataMember(Name = "sleepTimerId")]
        public int SleepTimerId { get; set; }

        /// <summary>
        /// Config version.
        /// </summary>
        [DataMember(Name = "configVersion")]
        public int ConfigVersion { get; set; }

        /// <summary>
        /// Activity ID.
        /// </summary>
        [DataMember(Name = "activityId")]
        public string ActivityId { get; set; }

        /// <summary>
        /// Sync status.
        /// </summary>
        [DataMember(Name = "syncStatus")]
        public int SyncStatus { get; set; }

        /// <summary>
        /// Time.
        /// </summary>
        [DataMember(Name = "time")]
        public int Time { get; set; }

        /// <summary>
        /// State version.
        /// </summary>
        [DataMember(Name = "stateVersion")]
        public int StateVersion { get; set; }

        /// <summary>
        /// Timezone offset.
        /// </summary>
        [DataMember(Name = "tzOffset")]
        public string TzOffset { get; set; }

        /// <summary>
        /// Mode.
        /// </summary>
        [DataMember(Name = "mode")]
        public int Mode { get; set; }

        /// <summary>
        /// Hub software version.
        /// </summary>
        [DataMember(Name = "hubSwVersion")]
        public string HubSwVersion { get; set; }

        /// <summary>
        /// Device setup state.
        /// </summary>
        [DataMember(Name = "deviceSetupState")]
        public IList<string> DeviceSetupState { get; set; }

        /// <summary>
        /// Indicates if setup has been completed.
        /// </summary>
        [DataMember(Name = "isSetupComplete")]
        public bool IsSetupComplete { get; set; }

        /// <summary>
        /// Content version.
        /// </summary>
        [DataMember(Name = "contentVersion")]
        public int ContentVersion { get; set; }

        /// <summary>
        /// Wifi status.
        /// </summary>
        [DataMember(Name = "wifiStatus")]
        public int WifiStatus { get; set; }

        /// <summary>
        /// Discovery server.
        /// </summary>
        [DataMember(Name = "discoveryServer")]
        public string DiscoveryServer { get; set; }

        /// <summary>
        /// Activity status.
        /// </summary>
        [DataMember(Name = "activityStatus")]
        public int ActivityStatus { get; set; }

        /// <summary>
        /// Running activity list.
        /// </summary>
        [DataMember(Name = "runningActivityList")]
        public string RunningActivityList { get; set; }

        /// <summary>
        /// Timezone.
        /// </summary>
        [DataMember(Name = "tz")]
        public string Tz { get; set; }

        /// <summary>
        /// Activity setup state.
        /// </summary>
        [DataMember(Name = "activitySetupState")]
        public bool ActivitySetupState { get; set; }

        /// <summary>
        /// Updates.
        /// </summary>
        [DataMember(Name = "updates")]
        public Dictionary<string, string> Updates { get; set; }

        /// <summary>
        /// Indicates if hub update.
        /// </summary>
        [DataMember(Name = "hubUpdate")]
        public bool HubUpdate { get; set; }

        /// <summary>
        /// Indicates if is sequence.
        /// </summary>
        [DataMember(Name = "sequence")]
        public bool Sequence { get; set; }

        /// <summary>
        /// Account ID.
        /// </summary>
        [DataMember(Name = "accountId")]
        public string AccountId { get; set; }
    }
}