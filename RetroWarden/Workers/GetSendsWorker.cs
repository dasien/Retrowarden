/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GetSendsWorker.cs
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

public sealed class GetSendsWorker : RetrowardenWorker
{
    // Member variables.
    private List<Send> _sends;
    
    public GetSendsWorker(IVaultRepository repository, string dialogMessage) 
        : base(repository, dialogMessage)
    {
        // Create members.
        _sends = new List<Send>();
        
        // Add work event handler.
        _worker.DoWork += (s, e) => 
        {
            // Try to fetch sends.
            e.Result = _repository.ListSends();
        };
            
        // Add completion event handler.
        _worker.RunWorkerCompleted += (s, e) =>
        {
            // Check to see if we have a result.
            if (e.Result != null)
            {
                // Set member variable to result.
                _sends = (List<Send>) e.Result;
            }
            
            // Check to see if the dialog is present.
            if (_workingDialog.IsCurrentTop) 
            {
                //Close the dialog
                _workingDialog.Hide();
            }
        };    
    }

    /// <summary>
    /// Gets the list of Sends retrieved from the vault
    /// </summary>
    public List<Send> Sends
    {
        get { return _sends; }
    }
}