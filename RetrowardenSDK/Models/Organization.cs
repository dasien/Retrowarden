/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Organization.cs
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