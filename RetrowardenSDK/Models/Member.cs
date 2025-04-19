/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Member.cs
 * 
 * Represents a member of an organization with associated permissions.
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
    public class Member
    {
        [JsonProperty("id")] 
        public string Id { get; set; } = "";
        
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("type")]
        public int MemberType { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; } = "";
        
        [JsonProperty("twoFactorEnabled")]
        public bool IsTwoFactorEnabled { get; set; }
    }    
}

