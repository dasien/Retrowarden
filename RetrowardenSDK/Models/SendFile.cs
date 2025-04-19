/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * SendFile.cs
 * 
 * Represents file metadata for secure file sharing functionality.
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
    public class SendFile
    {
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("size")] 
        public string FileSize { get; set; }
        
        [JsonProperty("sizeName")] 
        public string FileSizeText { get; set; }

        [JsonProperty("fileName")] 
        public string FileName { get; set; }
    }
}