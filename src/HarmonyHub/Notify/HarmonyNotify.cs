using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class HarmonyNotify
    {
        public HarmonyNotify()
        {
            Updates = new Dictionary<string, string>();
        }

        [DataMember(Name = "sleepTimerId")]
        public int SleepTimerId { get; set; }

        [DataMember(Name = "configVersion")]
        public int ConfigVersion { get; set; }

        [DataMember(Name = "activityId")]
        public string ActivityId { get; set; }

        [DataMember(Name = "syncStatus")]
        public int SyncStatus { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        [DataMember(Name = "stateVersion")]
        public int StateVersion { get; set; }

        [DataMember(Name = "tzOffset")]
        public string TzOffset { get; set; }

        [DataMember(Name = "mode")]
        public int Mode { get; set; }

        [DataMember(Name = "hubSwVersion")]
        public string HubSwVersion { get; set; }

        [DataMember(Name = "deviceSetupState")]
        public IList<string> DeviceSetupState { get; set; }

        [DataMember(Name = "isSetupComplete")]
        public bool IsSetupComplete { get; set; }

        [DataMember(Name = "contentVersion")]
        public int ContentVersion { get; set; }

        [DataMember(Name = "wifiStatus")]
        public int WifiStatus { get; set; }

        [DataMember(Name = "discoveryServer")]
        public string DiscoveryServer { get; set; }

        [DataMember(Name = "activityStatus")]
        public int ActivityStatus { get; set; }

        [DataMember(Name = "runningActivityList")]
        public string RunningActivityList { get; set; }

        [DataMember(Name = "tz")]
        public string Tz { get; set; }

        [DataMember(Name = "activitySetupState")]
        public bool ActivitySetupState { get; set; }

        [DataMember(Name = "updates")]
        public Dictionary<string, string> Updates { get; set; }

        [DataMember(Name = "hubUpdate")]
        public bool HubUpdate { get; set; }

        [DataMember(Name = "sequence")]
        public bool Sequence { get; set; }

        [DataMember(Name = "accountId")]
        public string AccountId { get; set; }
    }
}