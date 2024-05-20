using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class Organization
    {
        [JsonProperty("id")] 
        public string Id { get; set; } = "";
        
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("type")]
        public int OrgType { get; set; }
        
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        [JsonIgnore]
        public List<VaultCollection> Collections { get; set; } = new List<VaultCollection>();
        
        [JsonIgnore] 
        public List<Member> Members { get; set; } = new List<Member>();

        public override string ToString()
        {
            return Name;
        }
    }    
}