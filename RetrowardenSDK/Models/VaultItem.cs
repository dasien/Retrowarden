/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * VaultItem.cs
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
    public class VaultItem
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
        
        [JsonProperty("folderId")]
        public string? FolderId { get; set; }
        
        [JsonProperty("type")]
        public int ItemType { get; set; }
        
        [JsonProperty("name")]
        public string? ItemName { get; set; }
        
        [JsonProperty("favorite")]
        public bool IsFavorite { get; set; }
        
        [JsonProperty("notes")]
        public string? Notes { get; set; }
        
        [JsonProperty("reprompt")]
        public int Reprompt { get; set; }
        
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("revisionDate")]
        public DateTime RevisionDate { get; set; }

        [JsonProperty("deleteDate")]
        public DateTime DeletionDate { get; set; }
        
        [JsonProperty("collectionIds")]
        public List<string>? CollectionIds { get; set; }
        
        [JsonProperty("fields")]
        public List<VaultItemCustomField>? CustomFields { get; set; }
        
        [JsonProperty("login")]
        public Login? Login { get; set; }
        
        [JsonProperty("card")]
        public Card? Card { get; set; }
        
        [JsonProperty("identity")]
        public Identity? Identity { get; set; }

        [JsonProperty("secureNote")]
        public SecureNote? SecureNote { get; set; }

        // Helper properties.
        [JsonIgnore]
        public string? ItemOwnerName { get; set; }
        
        [JsonIgnore]
        public string? ListSortValue { get; set; }
    }
    
}