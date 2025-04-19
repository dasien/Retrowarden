/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GetItemsWorker.cs
 *
 * Background worker for retrieving vault items. Handles asynchronous
 * fetching of encrypted vault items and decryption while updating
 * the Terminal.Gui interface with the results.
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

namespace Retrowarden.Workers;

public sealed class GetItemsWorker : RetrowardenWorker
{
    // Member variables.
    private SortedDictionary<string, VaultItem> _items;
    
    public GetItemsWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _items = new SortedDictionary<string, VaultItem>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch vault items.
            e.Result = _repository.ListVaultItems();
        };
        
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _items = (SortedDictionary<string, VaultItem>) e.Result;
            }
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
        };    
    }
    
    public SortedDictionary<string, VaultItem> Items
    {
        get
        {
            return _items;
        }
    }
}