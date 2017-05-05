using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HarmonyHub.Config
{
    /// <summary>
    /// Control group configuration element.
    /// </summary>
    [DataContract]
    public class ControlGroupConfigElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlGroupConfigElement"/> class.
        /// </summary>
        public ControlGroupConfigElement()
        {
            Function = new List<FunctionConfigElement>();
        }

        /// <summary>
        /// Control group name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Control group funcctions.
        /// </summary>
        [DataMember(Name = "function")]
        public IList<FunctionConfigElement> Function { get; set; }
    }
}
