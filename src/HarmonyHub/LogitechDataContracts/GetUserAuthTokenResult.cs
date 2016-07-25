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
        [DataMember(Name = "AccountId")]
        public int AccountId { get; set; }

        [DataMember(Name = "UserAuthToken")]
        public string UserAuthToken { get; set; }
    }

    [DataContract]
    internal class GetUserAuthTokenResultRootObject
    {
        [DataMember(Name = "GetUserAuthTokenResult")]
        public GetUserAuthTokenResult GetUserAuthTokenResult { get; set; }
    }
}
