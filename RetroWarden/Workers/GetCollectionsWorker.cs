/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GetCollectionsWorker.cs
 *
 * Background worker for retrieving vault collections. Handles asynchronous
 * fetching of collection data and updates the Terminal.Gui interface
 * with the results.
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

public sealed class GetCollectionsWorker : RetrowardenWorker
{
    private List<VaultCollection> _collections;

    public GetCollectionsWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _collections = new List<VaultCollection>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) =>
        {
            // Try to fetch collections.
            e.Result = _repository.ListCollections();
        };

        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _collections = (List<VaultCollection>) e.Result;
            }

            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop)
            {
                //Close the dialog
                _workingDialog.Hide();
            }
        };
    }
    
    public List<VaultCollection> Collections
    {
        get { return _collections; }
    }
}