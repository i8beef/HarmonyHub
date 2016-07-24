using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    [DataContract]
    public class ControlGroupConfigElement
    {
        public ControlGroupConfigElement()
        {
            Function = new List<FunctionConfigElement>();
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "function")]
        public IList<FunctionConfigElement> Function { get; set; }
    }
}
