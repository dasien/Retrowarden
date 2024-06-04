using System.Diagnostics;
using Terminal.Gui;
using Retrowarden.Utils;
using Retrowarden.Models;

namespace Retrowarden.Controls
{
    public class UriScrollView :ScrollView
    {
        private List<View[]> _rowControls;
        private readonly List<CodeListItem> _matchTypes;
        private List<LoginURI> _uris;
        
        public UriScrollView(List<LoginURI>? uris, List<CodeListItem> matches)
        {
            // Set member variables.
            _uris = uris == null ? new List<LoginURI>() : uris;
            _matchTypes = matches;
            
            // The List of an array of controls for each row.
            _rowControls = new List<View[]>();

            // Create controls.
            LoadControlRowsFromUriList();
        }

        private void LoadControlRowsFromUriList()
        {
            // Clear any current rows.
            RemoveAll();
            
            // Create header row.
            View[] header = CreateHeaderRow();
            
            // Add to control array and scroll view.
            _rowControls.Add(header);
            Add(header);
            
            // Row counter.
            int rowCnt = 1;
            
            // Loop through the URI list.
            foreach (LoginURI uri in _uris)
            {
                // Create control row.
                View[] rowctl = CreateNewControlRow(rowCnt, uri.URI ?? "", uri.Match);
                
                // Add to list.
                _rowControls.Add(rowctl);
                
                // Add controls to Uri scroll.
                Add(rowctl);
                
                // Increment counter.
                rowCnt++;
            }
        }

        private View[] CreateHeaderRow()
        {
            Label lblName = new Label()
            {
                X = 1, Y = 0, Text = "URI", Width = 30, Height = 1
            };

            Label lblValue = new Label()
            {
                X = 48, Y = 0, Text = "Match Type", Width = 30, Height = 1
            };

            View[] retVal = [lblName, lblValue];
            return retVal;
        }
        
        private View[] CreateNewControlRow(int cnt, string uri, int? match)
        {
            // Create controls for row.
            TextField txtUri = new TextField(1, cnt, 30, uri)
            {
                CanFocus = true, Visible = true, Enabled = true, Data = cnt
            };
            
            Button btnCopyUri = new Button(32, cnt, "Copy")
            {
                Width = 8, Height = 1, CanFocus = true, Visible = true, Data = cnt, 
                TextAlignment = TextAlignment.Centered
            };

            Button btnGoUri = new Button(41, cnt, "Go")
            {
                Width = 6, Height = 1, CanFocus = true, Visible = true, Data = cnt,
                TextAlignment = TextAlignment.Centered
            };

            ComboBox cboMatchUri = new ComboBox(new Rect(48, cnt, 30, 3), _matchTypes)
            {
                CanFocus = true, Visible = true, Text = "", Data = cnt
            };

            Button btnDeleteUri = new Button(79, cnt, "Delete")
            {
                Width = 10, Height = 1, CanFocus = true, Visible = true, Data = cnt,
                TextAlignment = TextAlignment.Centered
            };
            
            // Set the source for the combobox.
            cboMatchUri.SetSource(_matchTypes);
            
            // Set the match combo source and selected item to uri match or "Default" as a null default.
            cboMatchUri.SelectedItem = _matchTypes.FindIndex(o => Convert.ToInt32(o.Index) == match);
            
            // This is a hack because for some reason we love to allow "null" in lists as a default value. :/ 
            if (match == null)
            {
                cboMatchUri.SelectedItem = _matchTypes.FindIndex(o => o.Index == null);
            }

            // Create event handlers for the buttons.
            btnCopyUri.Clicked += () =>
            {
                // Copy password to clipboard.
                Clipboard.TrySetClipboardData(txtUri.Text.ToString());

                // Indicate data copied.
                MessageBox.Query("Action Completed", "Copied Uri to clipboard.", "Ok");
            };

            btnGoUri.Clicked += () =>
            {
                // Get the uri string.
                string? uriString = txtUri.Text.ToString();
                
                // Check to see if there is a value.
                if (uriString != null)
                {
                    // Check to see if we have a valid Uri.
                    if (Uri.TryCreate(uri, UriKind.Absolute, out _))
                    {
                        Process.Start(new ProcessStartInfo(uriString)
                            { UseShellExecute = true });
                    }

                    else
                    {
                        MessageBox.ErrorQuery("Action failed.", "This does not appear to be a valid Uri", "Ok");
                    }
                }

                else
                {
                    MessageBox.ErrorQuery("Action failed.", "Uri missing.", "Ok");
                }
            };

            btnDeleteUri.Clicked += () =>
            {
                // Get row index.
                int index = (int)btnDeleteUri.Data;
                
                // Check to see if the user is removing the last row in the list.
                if (index < _rowControls.Count)
                {
                    // Loop through control list staring with the index + 1 row
                    for (int row = index; row < _rowControls.Count; row++)
                    {
                        // Update any control rows so that they 'move up' in the scroll list.
                        View[] rowControls = _rowControls.ElementAt(row);
                    
                        // Loop through the row controls.
                        foreach (View ctl in rowControls)
                        {
                            // Check to see if this is the row to be deleted.
                            if (row == index)
                            {
                                // Remove the row controls from the scroll view.
                                Remove(ctl);
                            }

                            else
                            {
                                // Update the Y position up.
                                ctl.Y -= 1;
                        
                                // Update the data property.
                                ctl.Data = Convert.ToInt32(ctl.Data) - 1;
                            }
                        }
                    }
                }
            
                // Remove controls from array.
                _rowControls.Remove(_rowControls.ElementAt(index));
            
                // Flag that the scrollview needs to be redrawn.
                SetNeedsDisplay();
             };
            
            // Return control row.
            View[] retVal = [txtUri, btnCopyUri, btnGoUri, cboMatchUri, btnDeleteUri];
            return retVal;
        }
 
        private void UpdateItemUriList()
        {
            // Create new URI list.
            List<LoginURI> uris = new List<LoginURI>();
            
            // Loop through URI list.
            foreach (View[] rowCtrls in _rowControls)
            {
                // Need to skip the first row of controls (the label headers)
                if (rowCtrls[0] is TextField)
                {
                    // Create new login URI object.
                    LoginURI loginUri = new LoginURI();
                
                    // Get the txt and combo box views.
                    TextField uri = (TextField) rowCtrls[0];
                    ComboBox match = (ComboBox) rowCtrls[3];
                
                    // Set values.
                    loginUri.URI = uri.Text.ToString();
                    loginUri.Match = match.SelectedItem == 6 ? null : match.SelectedItem;
                
                    // Add uri to list.
                    uris.Add(loginUri);
                }
            }
            
            // Update URI list.
            _uris = uris;
        }

        public void CreateControlRow()
        {
            // Call private method to add blank control row.
            View [] newRow = CreateNewControlRow(_rowControls.Count, "", null);
            
            // Add to list.
            _rowControls.Add(newRow);
                
            // Add controls to Uri scroll.
            Add(newRow);
            
            // Set scroll for redraw.
            SetNeedsDisplay();
            
            // Set focus to new text field.
            newRow[0].SetFocus();
        }

        public List<LoginURI> URIs
        {
            get
            {
                // First update the list to the current state of controls.
                UpdateItemUriList();
                return _uris;
            }
        }
    }    
}

