/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * Card.cs
 * 
 * This class represents a credit/debit card stored in the vault.
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
    public class Card :VaultItem
    {
        [JsonProperty("cardholderName")]
        public string? CardholderName { get; set; }
        
        [JsonProperty("brand")]
        public string? Brand { get; set; }
        
        [JsonProperty("number")]
        public string? CardNumber { get; set; }
        
        [JsonProperty("expMonth")]
        public string? ExpiryMonth { get; set; }
        
        [JsonProperty("expYear")]
        public string? ExpiryYear { get; set; }

        [JsonProperty("code")]
        public string? SecureCode { get; set; }
        
        public string GetListViewColumnText()
        {
            string retVal = " ";

            if (Brand != null && CardNumber != null)
            {
                retVal = string.Concat(Brand, " *", CardNumber.AsSpan(CardNumber.Length - 4));
            }
            
            else if (Brand != null)
            {
                retVal = Brand;
            }
            
            else if (CardNumber != null)
            {
                retVal = string.Concat("*", CardNumber.AsSpan(CardNumber.Length - 4));
            }
            
            return retVal;
        }
    }
}