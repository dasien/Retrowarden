using Newtonsoft.Json;

namespace Retrowarden.Models
{
    public class Card :VaultItem
    {
        [JsonProperty("cardholderName")]
        public string? CardholderName { get; set; }
        
        [JsonProperty("brand")]
        public string? Brand { get; set; }
        
        [JsonProperty("number")]
        public string? CardNumber { get; set; }
        
        [JsonProperty("expMonth")]
        public string? ExpiryMonth { get; set; }
        
        [JsonProperty("expYear")]
        public string? ExpiryYear { get; set; }

        [JsonProperty("code")]
        public string? SecureCode { get; set; }
        
        public string GetListViewColumnText()
        {
            string retVal = " ";

            if (Brand != null && CardNumber != null)
            {
                retVal = string.Concat(Brand, " *", CardNumber.AsSpan(CardNumber.Length - 4));
            }
            
            else if (Brand != null)
            {
                retVal = Brand;
            }
            
            else if (CardNumber != null)
            {
                retVal = string.Concat("*", CardNumber.AsSpan(CardNumber.Length - 4));
            }
            
            return retVal;
        }
    }
}