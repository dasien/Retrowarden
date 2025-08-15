/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ChatDialog.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using Terminal.Gui;
using Retrowarden.Services;

namespace Retrowarden.Dialogs
{
    public sealed class ChatDialog : BaseDialog
    {
        private TextField? _txtPrompt;
        private TextView? _txtResponse;
        private Button? _btnSubmit;
        private MCPClient? _mcpClient;
        private bool _isProcessing;
        
        public ChatDialog()
        {
            _okPressed = false;
            _mcpClient = new MCPClient();
            _isProcessing = false;
            InitializeComponent();
        }

        private async void SubmitButton_Clicked(object? sender, CommandEventArgs e)
        {
            if (_txtPrompt != null && _txtResponse != null && _btnSubmit != null && !_isProcessing)
            {
                string prompt = _txtPrompt.Text?.ToString() ?? string.Empty;
                
                if (string.IsNullOrWhiteSpace(prompt))
                {
                    MessageBox.ErrorQuery("Input Required", "Please enter a prompt to submit.", "Ok");
                    return;
                }

                // Disable submit button and show processing state
                _isProcessing = true;
                _btnSubmit.Text = "Processing...";
                _btnSubmit.Enabled = false;
                
                // Add user prompt to conversation immediately
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string userMessage = $"[{timestamp}] You: {prompt}\n";
                _txtResponse.Text += userMessage;
                _txtResponse.MoveEnd();
                
                // Clear the prompt field
                _txtPrompt.Text = "";
                
                try
                {
                    // Send prompt to MCP server and get response
                    string response = await SendToMCPServerAsync(prompt);
                    
                    // Add assistant response to conversation
                    string assistantMessage = $"[{timestamp}] Assistant: {response}\n\n";
                    _txtResponse.Text += assistantMessage;
                    _txtResponse.MoveEnd();
                }
                catch (Exception ex)
                {
                    string errorMessage = $"[{timestamp}] Error: {ex.Message}\n\n";
                    _txtResponse.Text += errorMessage;
                    _txtResponse.MoveEnd();
                }
                finally
                {
                    // Re-enable submit button
                    _isProcessing = false;
                    _btnSubmit.Text = "_Submit";
                    _btnSubmit.Enabled = true;
                    _txtPrompt.SetFocus();
                }
            }
        }

        private async Task<string> SendToMCPServerAsync(string prompt)
        {
            if (_mcpClient == null)
            {
                return "Error: MCP client not initialized.";
            }
            
            return await _mcpClient.SendPromptAsync(prompt);
        }

        private void CloseButton_Clicked(object? sender, CommandEventArgs e)
        {
            _okPressed = false;
            
            // Clean up MCP client resources
            _mcpClient?.Dispose();
            _mcpClient = null;
            
            if (_dialog != null)
            {
                Application.RequestStop(_dialog);
            }
        }

        private void InitializeComponent()
        {
            // Create the main dialog
            _dialog = new Dialog()
            {
                Title = "Chat with MCP Server",
                Width = 80,
                Height = 25,
                Modal = true,
                BorderStyle = LineStyle.Single
            };

            // Create prompt label
            var promptLabel = new Label()
            {
                Text = "Enter your prompt:",
                X = 1,
                Y = 1,
                Width = 20,
                Height = 1
            };

            // Create prompt text field
            _txtPrompt = new TextField()
            {
                X = 1,
                Y = 2,
                Width = Dim.Fill(12),
                Height = 1,
                CanFocus = true
            };

            // Create submit button
            _btnSubmit = new Button()
            {
                Text = "_Submit",
                X = Pos.AnchorEnd(10),
                Y = 2,
                Width = 10,
                Height = 1
            };
            _btnSubmit.Accepting += SubmitButton_Clicked;

            // Create response label
            var responseLabel = new Label()
            {
                Text = "Conversation:",
                X = 1,
                Y = 4,
                Width = 20,
                Height = 1
            };

            // Create response text area
            _txtResponse = new TextView()
            {
                X = 1,
                Y = 5,
                Width = Dim.Fill(2),
                Height = Dim.Fill(4),
                ReadOnly = true,
                WordWrap = true,
                Text = "Welcome to MCP Chat! Enter a prompt above and click Submit to get started.\n\n"
            };

            // Create close button
            var closeButton = new Button()
            {
                Text = "_Close",
                X = Pos.Center(),
                Y = Pos.AnchorEnd(2),
                Width = 10,
                Height = 1
            };
            closeButton.Accepting += CloseButton_Clicked;

            // Add controls to dialog
            _dialog.Add(promptLabel);
            _dialog.Add(_txtPrompt);
            _dialog.Add(_btnSubmit);
            _dialog.Add(responseLabel);
            _dialog.Add(_txtResponse);
            _dialog.Add(closeButton);

            // Set focus to prompt field
            _txtPrompt.SetFocus();
        }
    }
}