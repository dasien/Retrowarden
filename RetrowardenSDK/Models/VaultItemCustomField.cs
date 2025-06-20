/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * VaultItemCustomField.cs
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
    public class VaultItemCustomField
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("value")]
        public string? FieldValue { get; set; }
        
        [JsonProperty("type")]
        public int FieldType { get; set; }
        
        [JsonProperty("linkedId")]
        public int LinkedId { get; set; }
    }
}