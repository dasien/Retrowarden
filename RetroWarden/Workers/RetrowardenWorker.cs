/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * RetrowardenWorker.cs
 *
 * Copyright (C) 2024 Brian Gentry
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
    /// <summary>
    /// Abstract base class providing a standardized pattern for asynchronous vault operations
    /// with integrated progress dialog functionality in the Retrowarden Terminal.GUI application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class serves as the foundation for all background worker operations that interact
    /// with the Bitwarden vault through the IVaultRepository interface. It provides:
    /// </para>
    /// <list type="bullet">
    /// <item>Consistent asynchronous operation pattern using BackgroundWorker</item>
    /// <item>Integrated progress dialog display with customizable messages</item>
    /// <item>Standardized vault repository access for all operations</item>
    /// <item>Template method pattern for concrete worker implementations</item>
    /// <item>Thread-safe UI updates and dialog management</item>
    /// <item>Automatic dialog lifecycle management</item>
    /// </list>
    /// <para>
    /// Concrete implementations inherit from this base class and configure the BackgroundWorker's
    /// DoWork and RunWorkerCompleted event handlers in their constructors. The base class handles
    /// the common concerns of repository access, dialog display, and worker execution.
    /// </para>
    /// <para>
    /// The typical usage pattern involves:
    /// 1. Create a concrete worker instance with repository and dialog message
    /// 2. Configure DoWork handler for the specific vault operation
    /// 3. Configure RunWorkerCompleted handler for result processing and dialog cleanup
    /// 4. Call Run() to execute the operation with progress indication
    /// </para>
    /// <para>
    /// This pattern ensures consistent user experience across all vault operations by providing
    /// visual feedback during potentially long-running operations like authentication, synchronization,
    /// and item management tasks.
    /// </para>
    /// </remarks>
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
        public void Run(IApplication? app)
        {
            // Run the worker.
            _worker.RunWorkerAsync();

            // Show working dialog (app supplied by the running caller).
            _workingDialog.Show(app);
        }
    }    
}

