/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GetOrganizationsWorker.cs
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

public sealed class GetOrganizationsWorker : RetrowardenWorker
{
    // Member variables.
    private List<Organization>? _organizations;
    
    public GetOrganizationsWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _organizations = new List<Organization>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch organizations.
            e.Result = _repository.ListOrganizations();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Set member variable to result.
            _organizations = e.Result as List<Organization>;
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
        };    
    }

    public List<Organization>? Organizations
    {
        get
        {
            return _organizations;
        }
    }
}