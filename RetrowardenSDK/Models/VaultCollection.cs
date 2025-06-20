/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * VaultCollection.cs
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