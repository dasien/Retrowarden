/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * NodeItemGroupType.cs
 *
 * Enumeration defining the types of groups that can contain vault items,
 * used for organizing and categorizing items in the Terminal.Gui interface.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
namespace Retrowarden.Utils
{
    public enum NodeItemGroupType
    {
        AllItems = 0,
        Login,
        SecureNote,
        Card,
        Identity,
        Favorites,
        None
    }
}