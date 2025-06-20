/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * DeleteSendWorker.cs
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
    public sealed class DeleteSendWorker : RetrowardenWorker
    {
        private bool _success;
        private readonly string _sendId;

        /// <summary>
        /// Creates a worker for deleting a Bitwarden Send
        /// </summary>
        /// <param name="repository">The vault repository</param>
        /// <param name="dialogMessage">Message to display in the working dialog</param>
        /// <param name="sendId">The ID of the Send to delete</param>
        public DeleteSendWorker(IVaultRepository repository, string dialogMessage, string sendId)
            : base(repository, dialogMessage)
        {
            _sendId = sendId;
            _success = false;

            // Run the delete send method
            _worker.DoWork += (s, e) =>
            {
                // Execute the deletion
                _repository.DeleteSend(_sendId);

                // Check to see that it was successful
                if (_repository.ExitCode == "0")
                {
                    // Store the resulting URL
                    _success = true;
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
        /// Gets whether the deletion was successful
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }

        /// <summary>
        /// Gets the ID of the Send that was deleted
        /// </summary>
        public string SendId
        {
            get { return _sendId; }
        }
    }
}