/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CodeListItem.cs
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
    public class CodeListItem
    {
        public string? Index { get; set; }
        public string DisplayText { get; set; }

        public CodeListItem(string? index, string displayText)
        {
            Index = index;
            
            DisplayText = displayText;
        }
        
        public override string ToString()
        {
            return DisplayText;
        }
    }
}

