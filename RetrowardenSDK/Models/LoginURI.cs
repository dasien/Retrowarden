using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class LoginURI
    {
        [JsonProperty("uri")]
        public string? URI { get; set; }    
        
        [JsonProperty("match")]
        public int? Match { get; set; }    
    }
}