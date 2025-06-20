/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * LoginWorker.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class LoginWorker : RetrowardenWorker
    {
        public LoginWorker(IVaultRepository repository, string userId, string password, string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Run the login method.
            _worker.DoWork += (s, e) =>
            {
                // Execute the login method.
                _repository.Login(userId, password);
            };

            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop)
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }
            };
        }
    }
}