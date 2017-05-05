using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Device configuration element.
    /// </summary>
    [DataContract]
    public class DeviceConfigElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceConfigElement"/> class.
        /// </summary>
        public DeviceConfigElement()
        {
            Capabilities = new List<int>();
            ControlGroup = new List<ControlGroupConfigElement>();
        }

        /// <summary>
        /// Device transport.
        /// </summary>
        [DataMember(Name = "Transport")]
        public int Transport { get; set; }

        /// <summary>
        /// Device suggested display.
        /// </summary>
        [DataMember(Name = "suggestedDisplay")]
        public string SuggestedDisplay { get; set; }

        /// <summary>
        /// Device type display name.
        /// </summary>
        [DataMember(Name = "deviceTypeDisplayName")]
        public string DeviceTypeDisplayName { get; set; }

        /// <summary>
        /// Device label.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>
        /// Device ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Device capabilities.
        /// </summary>
        [DataMember(Name = "Capabilities")]
        public IList<int> Capabilities { get; set; }

        /// <summary>
        /// Device type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Device dongle RFID number.
        /// </summary>
        [DataMember(Name = "DongleRFID")]
        public int DongleRFID { get; set; }

        /// <summary>
        /// Device control groups.
        /// </summary>
        [DataMember(Name = "controlGroup")]
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }

        /// <summary>
        /// Device control port.
        /// </summary>
        [DataMember(Name = "controlPort")]
        public int ControlPort { get; set; }

        /// <summary>
        /// Indicates if device is keyboard associated.
        /// </summary>
        [DataMember(Name = "isKeyboardAssociated")]
        public bool IsKeyboardAssociated { get; set; }

        /// <summary>
        /// Device model.
        /// </summary>
        [DataMember(Name = "model")]
        public string Model { get; set; }

        /// <summary>
        /// Device profile URI.
        /// </summary>
        [DataMember(Name = "deviceProfileUri")]
        public string DeviceProfileUri { get; set; }

        /// <summary>
        /// Device manufacturer.
        /// </summary>
        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Device icon.
        /// </summary>
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Indicates if device is manually powered.
        /// </summary>
        [DataMember(Name = "isManualPower")]
        public string IsManualPower { get; set; }
    }
}
