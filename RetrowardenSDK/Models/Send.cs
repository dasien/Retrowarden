/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Send.cs
 * 
 * Represents a secure sharing mechanism for vault items.
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
    public class Send
    {
        public string ToBase64EncodedString()
        {
            string itemJSON = JsonConvert.SerializeObject(this);
            byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemJSON);
            return Convert.ToBase64String(itemBytes);    
        }

        [JsonProperty("object")] 
        public string ObjectType { get; set; }

        [JsonProperty("name")] 
        public string Name { get; set; }

        [JsonProperty("notes")] 
        public string Notes { get; set; }
    
        [JsonProperty("type")] 
        public string SendType { get; set; }

        [JsonProperty("text")] 
        public SendText? Text { get; set; }
    
        [JsonProperty("file")] 
        public SendFile? File { get; set; }
    
        [JsonProperty("maxAccessCount")] 
        public int? MaxAccessCount { get; set; }

        [JsonProperty("deletionDate")] 
        public DateTime DeletionDate { get; set; }

        [JsonProperty("expirationDate")] 
        public DateTime? ExpirationDate { get; set; }

        [JsonProperty("password")] 
        public string? Password { get; set; }

        [JsonProperty("disabled")] 
        public bool IsDisabled { get; set; }
    
        [JsonProperty("hideEmail")] 
        public bool IsEmailHidden { get; set; }
    }    
}