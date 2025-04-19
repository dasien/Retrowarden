/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * GeneratePasswordWorker.cs
 *
 * Background worker for generating secure passwords based on specified criteria
 * such as length and character types. Handles the asynchronous password
 * generation process while maintaining UI responsiveness.
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
    public sealed class GeneratePasswordWorker : RetrowardenWorker
    {
        private string _password;
        
        public GeneratePasswordWorker(IVaultRepository repository, bool useUpper, bool useLower, bool useDigits, 
            bool useSpecial, int length, string dialogMessage) 
            : base(repository, dialogMessage)
        {
            // Create members.
            _password = "";
            
            // Run the login method.
            _worker.DoWork += (s, e) => 
            {
                // Execute the method.
                e.Result = _repository.GeneratePassword(useUpper, useLower, useDigits, useSpecial, length);
            };
            
            // Add completion event handler.
            _worker.RunWorkerCompleted += (s, e) => 
            {
                // Check to see that we have a result.
                if (e.Result != null)
                {
                    // Get the generated password.
                    _password = (string) e.Result;
                }
                
                // Check to see if the dialog is present.
                if (_workingDialog.IsCurrentTop) 
                {
                    //Close the dialog
                    _workingDialog.Hide();
                }
            };    
        }

        public string Password
        {
            get { return _password; }
        }
    }    
}

