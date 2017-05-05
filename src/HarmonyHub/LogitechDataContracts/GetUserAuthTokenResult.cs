using System.Runtime.Serialization;

namespace HarmonyHub.LogitechDataContracts
{
    /// <summary>
    /// Result of call to myharmony.com web service. Contains UserAuthToken.
    /// AccountId is always (so far) 0.
    /// </summary>
    [DataContract]
    internal class GetUserAuthTokenResult
    {
        /// <summary>
        /// Account ID.
        /// </summary>
        [DataMember(Name = "AccountId")]
        public int AccountId { get; set; }

        /// <summary>
        /// Auth token.
        /// </summary>
        [DataMember(Name = "UserAuthToken")]
        public string UserAuthToken { get; set; }
    }
}
