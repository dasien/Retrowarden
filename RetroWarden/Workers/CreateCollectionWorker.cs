/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CreateCollectionWorker.cs
 *
 * Copyright (C) 2024 Brian Gentry
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
    public sealed class CreateCollectionWorker : RetrowardenWorker
    {
        private VaultCollection _results;
        
        public  CreateCollectionWorker(IVaultRepository repository, string dialogMessage, VaultCollection collection) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _results = new VaultCollection();
            
            // Run the create folder method.
            _worker.DoWork += (s, e) =>
            {
                // Get encoded item string.
                string encodedItem = collection.ToBase64EncodedString();
                
                // Execute the save.
                VaultCollection? result = _repository.CreateCollection(encodedItem);
                
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
        
        public VaultCollection Collection
        {
            get
            {
                return _results;
            }
        }
    }    
}

