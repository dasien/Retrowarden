
/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GeneratePassphraseWorker.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public sealed class GeneratePassphraseWorker : RetrowardenWorker
    {
        private string? _passphrase;
        
        public GeneratePassphraseWorker(IVaultRepository repository, bool capitalize, bool useNumbers, 
            int words, string? sep, string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Run the login method.
            _worker.DoWork += (s, e) => 
            {
                // Execute the method.
                e.Result = _repository.GeneratePassphrase(words, sep, capitalize, useNumbers);
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Get the generated passphrase.
                _passphrase = (string?) e.Result;
                
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }
            };    
        }

        public string? Passphrase
        {
            get { return _passphrase; }
        }
    }    
}