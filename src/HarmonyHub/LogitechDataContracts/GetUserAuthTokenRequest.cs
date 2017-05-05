using System.Runtime.Serialization;

namespace HarmonyHub.LogitechDataContracts
{
    /// <summary>
    /// Request call to myharmony.com web service.
    /// </summary>
    [DataContract]
    internal class GetUserAuthTokenRequest
    {
        /// <summary>
        /// Email address.
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
