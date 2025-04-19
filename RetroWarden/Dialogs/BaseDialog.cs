/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * BaseDialog.cs
 *
 * Abstract base class for all dialog windows in the application.
 * Provides common functionality and consistent behavior for modal
 * dialogs using Terminal.Gui framework.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;

namespace Retrowarden.Dialogs
{
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

