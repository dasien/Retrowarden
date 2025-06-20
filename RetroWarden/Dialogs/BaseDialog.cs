/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * BaseDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
    /// <summary>
    /// Base class for all application dialogs.  Handles the showing and
    /// cancelling of implementing dialogs.
    /// </summary>
    public abstract class BaseDialog
    {
        protected Dialog? _dialog;
        protected bool _okPressed;
        
        protected void CancelButton_Clicked(object? sender, CommandEventArgs e)
        {
            // Set ok button flag.
            _okPressed = false;

            if (_dialog != null)
            {
                // Close dialog.
                Application.RequestStop(_dialog);
            }
        }
        
        public void Show()
        {
            if (_dialog != null)
            {
                Application.Run(_dialog);    
            }
        }
        
        public bool OkPressed
        {
            get { return _okPressed; }
        }
    }
}

