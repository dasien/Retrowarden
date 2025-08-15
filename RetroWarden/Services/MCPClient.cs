/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * MCPClient.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Text;
using System.Text.Json;

namespace Retrowarden.Services
{
    public class MCPClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl;
        
        public MCPClient(string serverUrl = "http://localhost:3000")
        {
            _httpClient = new HttpClient();
            _serverUrl = serverUrl;
            
            // Set reasonable timeout for MCP requests
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> SendPromptAsync(string prompt)
        {
            try
            {
                // Create the request payload
                var requestPayload = new
                {
                    prompt = prompt,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                var json = JsonSerializer.Serialize(requestPayload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send request to MCP server
                var response = await _httpClient.PostAsync($"{_serverUrl}/chat", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    // Try to parse the JSON response
                    try
                    {
                        var responseObj = JsonSerializer.Deserialize<JsonElement>(responseContent);
                        
                        // Check if response has a 'message' or 'response' field
                        if (responseObj.TryGetProperty("response", out var responseField))
                        {
                            return responseField.GetString() ?? "No response from server";
                        }
                        else if (responseObj.TryGetProperty("message", out var messageField))
                        {
                            return messageField.GetString() ?? "No message from server";
                        }
                        else
                        {
                            // Return the raw response if no standard field found
                            return responseContent;
                        }
                    }
                    catch (JsonException)
                    {
                        // If JSON parsing fails, return the raw response
                        return responseContent;
                    }
                }
                else
                {
                    return $"Error: Server returned {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Network error: {ex.Message}. Please check if the MCP server is running.";
            }
            catch (TaskCanceledException)
            {
                return "Request timed out. The MCP server may be slow to respond.";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}