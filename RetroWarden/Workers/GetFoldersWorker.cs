/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GetFoldersWorker.cs
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

namespace Retrowarden.Workers;

public sealed class GetFoldersWorker : RetrowardenWorker
{
    // Member variables.
    private List<VaultFolder> _folders;
    
    public GetFoldersWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _folders = new List<VaultFolder>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch folders.
            e.Result = _repository.ListFolders();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _folders = (List<VaultFolder>) e.Result;
            }
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
        };    
    }

    public List<VaultFolder> Folders
    {
        get { return _folders; }
    }
}