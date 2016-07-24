using System.Collections.Generic;

namespace HarmonyHub.Config
{
    public class DeviceConfigElement
    {
        public DeviceConfigElement()
        {
            Capabilities = new List<int>();
            ControlGroup = new List<ControlGroupConfigElement>();
        }

        public int Transport { get; set; }
        public string SuggestedDisplay { get; set; }
        public string DeviceTypeDisplayName { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public IList<int> Capabilities { get; set; }
        public string Type { get; set; }
        public int DongleRFID { get; set; }
        public IList<ControlGroupConfigElement> ControlGroup { get; set; }
        public int ControlPort { get; set; }
        public bool IsKeyboardAssociated { get; set; }
        public string Model { get; set; }
        public string DeviceProfileUri { get; set; }
        public string Manufacturer { get; set; }
        public string Icon { get; set; }
        public string IsManualPower { get; set; }
    }
}
