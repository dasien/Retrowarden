/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * NodeData.cs
 *
 * Represents the data structure for tree nodes in the Terminal.Gui interface,
 * managing the organization of items and folders within the UI.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Utils
{
    public class NodeData
    { 
        public string? Id { get; set; }
        public NodeType NodeType { get; set; }
        public NodeItemGroupType ItemGroupType { get; set; }
        public ITreeNode? Parent { get; set; }
        public string? Text { get; set; }
    }    
}

