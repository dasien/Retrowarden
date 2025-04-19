/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * LoginURI.cs
 * 
 * Represents URL/URI information associated with login credentials.
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
    public class LoginURI
    {
        [JsonProperty("uri")]
        public string? URI { get; set; }    
        
        [JsonProperty("match")]
        public int? Match { get; set; }    
    }
}