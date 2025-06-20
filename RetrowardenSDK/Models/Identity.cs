/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Identity.cs
 *
 * Copyright (C) 2024 RetrowardenSDK Project
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{
    public class Identity : VaultItem
    {
        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }
        
        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }
        
        [JsonProperty("lastName")]
        public string? LastName { get; set; }
        
        [JsonProperty("address1")]
        public string? Address1 { get; set; }

        [JsonProperty("address2")]
        public string? Address2 { get; set; }
        
        [JsonProperty("address3")]
        public string? Address3 { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }
        
        [JsonProperty("state")]
        public string? State { get; set; }
        
        [JsonProperty("postalCode")]
        public string? PostalCode { get; set; }
        
        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("company")]
        public string? Company { get; set; }
        
        [JsonProperty("email")]
        public string? Email { get; set; }
        
        [JsonProperty("phone")]
        public string? Phone { get; set; }
        
        [JsonProperty("ssn")]
        public string? Ssn { get; set; }
        
        [JsonProperty("username")]
        public string? UserName { get; set; }

        [JsonProperty("passportNumber")]
        public string? PassportNumber { get; set; }

        [JsonProperty("licenseNumber")]
        public string? LicenseNumber { get; set; }
        
        public string GetListViewColumnText()
        {
            string retVal = " ";

            if (FirstName != null && LastName != null)
            {
                retVal = string.Concat(FirstName, " ", LastName);
            }
            
            else if (FirstName != null)
            {
                retVal = FirstName;
            }
            
            else if (LastName != null)
            {
                retVal = LastName;
            }
            
            return retVal;
        }

    }
}