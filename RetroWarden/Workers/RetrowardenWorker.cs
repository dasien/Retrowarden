/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * RetrowardenWorker.cs
 *
 * Base class for all background workers in the application. Provides
 * common functionality for asynchronous operations, progress reporting,
 * and Terminal.Gui dialog management across all worker implementations.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.ComponentModel;
using Retrowarden.Dialogs;
using RetrowardenSDK.Repositories;

namespace Retrowarden.Workers
{
    public class RetrowardenWorker
    {
        // Member variables.
        protected readonly IVaultRepository _repository;
        protected readonly BackgroundWorker _worker;
        protected readonly WorkingDialog _workingDialog;

        protected RetrowardenWorker(IVaultRepository repository, string dialogMessage)
        {
            _repository = repository;
            _worker = new BackgroundWorker();
            _workingDialog = new WorkingDialog(dialogMessage);
        }
        public void Run()
        {
            // Run the worker.
            _worker.RunWorkerAsync();
            
            // Show working dialog.
            _workingDialog.Show();
        }
    }    
}

