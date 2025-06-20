/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * NodeType.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
namespace Retrowarden.Utils
{
    public enum NodeType
    {
        Root = 0,
        Organization,
        CollectionGroup,
        Collection,
        FolderGroup,
        Folder,
        ItemGroup,
        FavoriteGroup,
        Item
    }
}