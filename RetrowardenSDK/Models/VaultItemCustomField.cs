using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class VaultItemCustomField
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("value")]
        public string? FieldValue { get; set; }
        
        [JsonProperty("type")]
        public int FieldType { get; set; }
        
        [JsonProperty("linkedId")]
        public int LinkedId { get; set; }
    }
}