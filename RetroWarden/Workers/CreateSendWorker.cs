/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CreateSendWorker.cs
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
    public sealed class CreateSendWorker : RetrowardenWorker
    {
        private string _result;
        private Send _send;
        private string _filePath;

        /// <summary>
        /// Creates a worker for sending a secure text or file via Bitwarden Send
        /// </summary>
        /// <param name="repository">The vault repository</param>
        /// <param name="dialogMessage">Message to display in the working dialog</param>
        /// <param name="send">The Send object to be created</param>
        /// <param name="filePath">Path to the file (only needed for File sends)</param>
        public CreateSendWorker(IVaultRepository repository, string dialogMessage, Send send, string filePath = null)
            : base(repository, dialogMessage)
        {
            // Store the parameters
            _send = send;
            _filePath = filePath;
            _result = string.Empty;

            // Run the create send method
            _worker.DoWork += (s, e) =>
            {
                // Execute the send creation
                string result = _repository.CreateSend(_send, _filePath);

                // Check to see that it was successful
                if (_repository.ExitCode == "0")
                {
                    // Store the resulting URL
                    _result = result;
                }
            };

            // Add completion event handler
            _worker.RunWorkerCompleted += (s, e) =>
            {
                // Check to see if the dialog is present
                if (_workingDialog.IsCurrentTop)
                {
                    // Close the dialog
                    _workingDialog.Hide();
                }
            };
        }

        /// <summary>
        /// The URL of the created Send if successful, or empty string if failed
        /// </summary>
        public string SendUrl
        {
            get { return _result; }
        }
    }
}