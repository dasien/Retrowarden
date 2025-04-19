/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CheckStatusWorker.cs
 *
 * Background worker for checking vault and session status. Monitors
 * authentication state and vault accessibility while providing
 * updates through Terminal.Gui interface.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using RetrowardenSDK.Models;
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class CheckStatusWorker : RetrowardenWorker
    {
        // Member variables.
        private VaultStatus _status;

        public CheckStatusWorker(IVaultRepository repository, string dialogMessage)
            : base(repository, dialogMessage)
        {
            // Create members.
            _status = new VaultStatus();

            // Add work event handler.
            _worker.DoWork += (s, e) =>
            {
                // Try to fetch folders.
                e.Result = _repository.GetStatus();
            };

            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see if we have a result.
                if (e.Result != null)
                {
                    // Set member variable to result.
                    _status = (VaultStatus) e.Result;
                }
            
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }
            };    
        }

        public VaultStatus Status
        {
            get { return _status; }
        }
    }    
}

