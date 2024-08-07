using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class SecureNote : VaultItem
    {
        [JsonProperty("type")]
        public int? Type { get; set; }
        
        public string GetListViewColumnText()
        {
            return " ";
        }
    }    
}

