using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class Member
    {
        [JsonProperty("id")] 
        public string Id { get; set; } = "";
        
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("type")]
        public int MemberType { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; } = "";
        
        [JsonProperty("twoFactorEnabled")]
        public bool IsTwoFactorEnabled { get; set; }
    }    
}

