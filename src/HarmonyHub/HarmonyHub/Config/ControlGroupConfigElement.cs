using System.Collections.Generic;

namespace HarmonyHub.Config
{
    public class ControlGroupConfigElement
    {
        public ControlGroupConfigElement()
        {
            Function = new List<FunctionConfigElement>();
        }

        public string Name { get; set; }
        public IList<FunctionConfigElement> Function { get; set; }
    }
}
