/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * BaseSendDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/

using RetrowardenSDK.Models;

namespace Retrowarden.Dialogs
{
    public class BaseSendDialog : BaseDialog
    {
        // Send object to be created
        protected readonly Send _send;

        protected BaseSendDialog(string sendType)
        {
            // Create send object.
            _send = new Send() { SendType = sendType, DeletionDate = DateTime.Now.AddDays(7) };
        }
        
        /// <summary>
        /// Gets the configured Send object
        /// </summary>
        public Send Send
        {
            get { return _send; }
        }
    }
}

