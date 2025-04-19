/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CheckForUpdateWorker.cs
 *
 * Background worker for checking application updates. Handles version
 * comparison and update notification while maintaining UI responsiveness
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

namespace Retrowarden.Workers
{
    public class CheckForUpdateWorker : RetrowardenWorker
    {
        // Member variables.
        private bool _updateAvailable;
        
        public  CheckForUpdateWorker(IVaultRepository repository, string dialogMessage) : base(repository, dialogMessage)
        {
            // Create members.
            _updateAvailable = false;
            
            // Run the login method.
            _worker.DoWork += (s, e) =>
            {
                // Execute the login method.
                e.Result = _repository.CheckForUpdate();
            };

            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see that we have a result.
                if (e.Result != null)
                {
                    // Get the generated password.
                    _updateAvailable = (bool) e.Result;
                }

                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop)
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }
            };
        }

        public bool UpdateAvailable
        {
            get { return _updateAvailable; }
        }
    }    
}

