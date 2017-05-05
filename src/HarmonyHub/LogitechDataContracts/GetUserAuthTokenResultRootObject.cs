using System.Runtime.Serialization;

namespace HarmonyHub.LogitechDataContracts
{
    /// <summary>
    /// Root containing object for auth token request message.
    /// </summary>
    [DataContract]
    internal class GetUserAuthTokenResultRootObject
    {
        /// <summary>
        /// Result of auth token request.
        /// </summary>
        [DataMember(Name = "GetUserAuthTokenResult")]
        public GetUserAuthTokenResult GetUserAuthTokenResult { get; set; }
    }
}
