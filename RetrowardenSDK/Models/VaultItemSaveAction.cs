/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * VaultItemSaveAction.cs
 * 
 * This enumeration defines the possible actions that can be performed
 * when saving vault items, such as create, update, or delete.
 * 
 * Copyright (C) 2024 RetrowardenSDK Project
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
namespace RetrowardenSDK.Models
{
    public enum VaultItemSaveAction
    {
        Create = 0,
        Update,
        Delete,
        MoveToFolder,
        MoveToOrganization
    }    
}

