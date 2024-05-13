using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class VaultCollection
    {
        public string ToBase64EncodedString()
        {
            string itemJSON = JsonConvert.SerializeObject(this);
            byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemJSON);
            return Convert.ToBase64String(itemBytes);    
        }

        [JsonProperty("id")] 
        public string Id { get; set; } = "";
        
        [JsonProperty("organizationId")]
        public string? OrganizationId { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("externalId")]
        public string? ExternalId { get; set; }

    }    
}