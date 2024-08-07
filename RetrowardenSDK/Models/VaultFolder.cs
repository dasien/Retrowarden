using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class VaultFolder
    {
        public string ToBase64EncodedString()
        {
            string itemJSON = JsonConvert.SerializeObject(this);
            byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemJSON);
            return Convert.ToBase64String(itemBytes);    
        }
        
        [JsonProperty("id")]
        public string Id { get; set; } = "";
        
        [JsonProperty("name")]
        public string Name { get; set; }= "";

        public override string ToString()
        {
            return Name;
        }
    }    
}

