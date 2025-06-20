/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Login.cs
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

