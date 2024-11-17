using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{ 
    public class SendFile
    {
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("size")] 
        public string FileSize { get; set; }
        
        [JsonProperty("sizeName")] 
        public string FileSizeText { get; set; }

        [JsonProperty("fileName")] 
        public string FileName { get; set; }
    }
}