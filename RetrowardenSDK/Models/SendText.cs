/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * SendText.cs
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Newtonsoft.Json;

namespace RetrowardenSDK.Models
{ 
    public class SendText
    {
        [JsonProperty("text")] 
        public string Text { get; set; }
        
        [JsonProperty("hidden")] 
        public bool IsHidden { get; set; }
    }
}