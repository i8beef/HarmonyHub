using System.Runtime.Serialization;

namespace HarmonyHub.LogitechDataContracts
{
    /// <summary>
    /// Request call to myharmony.com web service.
    /// </summary>
    [DataContract]
    internal class GetUserAuthTokenRequest
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
