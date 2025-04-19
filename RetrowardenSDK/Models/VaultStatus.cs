/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * VaultItem.cs
 * 
 * This class represents the current state of the user vault.
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
    public class VaultStatus
    {
        public string? FormattedStatus()
        {
            string? retVal = "";
            
            // Check to see if we have a status yet.
            if (!string.IsNullOrEmpty(Status))
            {
                // Convert to char array of the string.
                char[] letters = Status.ToCharArray();
                
                // Upper case the first char.
                letters[0] = char.ToUpper(letters[0]);
                
                // Set the array made of the new char array.
                retVal =  new string(letters);              
            }
            
            // Return the formatted status.
            return retVal;
        }
        
        [JsonProperty("serverUrl")]
        public string? ServerUrl { get; set; }
        
        [JsonProperty("lastSync")]
        public DateTime? LastSync { get; set; }
        
        [JsonProperty("userEmail")]
        public string? UserEmail { get; set; }

        [JsonProperty("userId")]
        public string? UserId { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }
    }    
}

