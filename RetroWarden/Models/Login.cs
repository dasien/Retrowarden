using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class Login : VaultItem
    {
        [JsonProperty("username")]
        public string? UserName { get; set; }
        
        [JsonProperty("password")]
        public string? Password { get; set; }
        
        [JsonProperty("totp")]
        public string? TOTP { get; set; }

        [JsonProperty("passwordRevisionDate")]
        public DateTime PasswordRevisionDate { get; set; }

        [JsonProperty("uris")]
        public List<LoginURI>? URIs { get; set; }

        public string GetListViewColumnText()
        {
            string retVal = " ";

            if (UserName != null)
            {
                retVal = UserName;
            }
            
            return retVal;
        }

    }    
}

