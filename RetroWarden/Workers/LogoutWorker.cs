/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * LogoutWorker.cs
 *
 * Background worker responsible for user logout operations. Handles
 * session termination and cleanup while maintaining UI responsiveness
 * through Terminal.Gui dialogs.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers;

public sealed class LogoutWorker : RetrowardenWorker
{
    public LogoutWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Run the logout method.
        _worker.DoWork += (s, e) => 
        {
            // Execute the login method.
            _repository.Logout();
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