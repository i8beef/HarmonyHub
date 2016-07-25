using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class DeviceConfigElement
    {
        public DeviceConfigElement()
        {
            Capabilities = new List<int>();
            ControlGroup = new List<ControlGroupConfigElement>();
        }

        [DataMember(Name = "Transport")]
        public int Transport { get; set; }

        [DataMember(Name = "suggestedDisplay")]
        public string SuggestedDisplay { get; set; }

        [DataMember(Name = "deviceTypeDisplayName")]
        public string DeviceTypeDisplayName { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "Capabilities")]
        public IList<int> Capabilities { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "DongleRFID")]
        public int DongleRFID { get; set; }

        [DataMember(Name = "controlGroup")]
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }

        [DataMember(Name = "controlPort")]
        public int ControlPort { get; set; }

        [DataMember(Name = "isKeyboardAssociated")]
        public bool IsKeyboardAssociated { get; set; }

        [DataMember(Name = "model")]
        public string Model { get; set; }

        [DataMember(Name = "deviceProfileUri")]
        public string DeviceProfileUri { get; set; }

        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "isManualPower")]
        public string IsManualPower { get; set; }
    }
}
