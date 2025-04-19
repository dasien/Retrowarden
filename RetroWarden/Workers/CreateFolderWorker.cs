/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CreateFolderWorker.cs
 *
 * Background worker responsible for creating new vault folders. Handles
 * the asynchronous folder creation process and manages related UI feedback
 * through Terminal.Gui dialogs.
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
    public sealed class CreateFolderWorker : RetrowardenWorker
    {
        private VaultFolder _results;
        
        public CreateFolderWorker(IVaultRepository repository, string dialogMessage, VaultFolder folder) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _results = new VaultFolder();
            
            // Run the create folder method.
            _worker.DoWork += (s, e) =>
            {
                // Get encoded item string.
                string encodedItem = folder.ToBase64EncodedString();
                
                // Execute the save.
                VaultFolder? result = _repository.CreateFolder(encodedItem);
                
                // Check to see that one was saved.
                if (result != null && _repository.ExitCode == "0")
                {
                    // Store the result.
                    _results = result;
                }
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
        
        public VaultFolder Folder
        {
            get
            {
                return _results;
            }
        }
    }    
}
