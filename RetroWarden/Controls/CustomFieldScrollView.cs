using System.Collections.ObjectModel;
using Retrowarden.Utils;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Controls
{
    public class CustomFieldScrollView : ScrollView
    {
        private List<View[]> _rowControls;
        private List<VaultItemCustomField> _fields;
        private readonly int _itemType;
        
        public CustomFieldScrollView(List<VaultItemCustomField>? fields, int itemType)
        {
            // Set members.
            _fields = fields == null ? new List<VaultItemCustomField>() : fields;
            _itemType = itemType;
            
            // The List of an array of controls for each row.
            _rowControls = new List<View[]>();

            // Create the control rows.
            LoadControlsFromFieldList();
        }

        private void LoadControlsFromFieldList()
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
            foreach (VaultItemCustomField field in _fields)
            {
                // The control row.
                View[]? rowctl = CreateNewControlRow(rowCnt, field);
                
                // Check to see it was created.
                if (rowctl != null)
                {
                    // Add to list.
                    _rowControls.Add(rowctl);

                    // Add controls to Uri scroll.
                    Add(rowctl);

                    // Increment counter.
                    rowCnt++;
                }
            }
            
            // Refresh control.
            SetNeedsDisplay();
            //SetChildNeedsDisplay();
        }

        private View[] CreateHeaderRow()
        {
            Label lblName = new Label()
            {
                X = 1, Y = 0, Text = "Name", Width = 30, Height = 1
            };

            Label lblValue = new Label()
            {
                X = 32, Y = 0, Text = "Value", Width = 30, Height = 1
            };

            View[] retVal = [lblName, lblValue];
            return retVal;
        }

        private View[]? CreateNewControlRow(int rowNum, VaultItemCustomField field)
        {
            View[]? retVal = null;
            
            // Based on the field type, create the correct row.
            switch (field.FieldType)
            {
                case 0:
                    retVal = CreateTextRow(rowNum, field.Name, field.FieldValue);
                    break;
                    
                case 1:
                    retVal = CreateHiddenRow(rowNum, field.Name, field.FieldValue);
                    break;

                case 2:
                    retVal = CreateBooleanRow(rowNum, field.Name, field.FieldValue);
                    break;
                    
                case 3:
                    retVal = CreateLinkedRow(rowNum, field.Name, field.LinkedId);
                    break;
            }

            return retVal;
        }
        
        private View[] CreateTextRow(int rowNum, string? name, string? value)
        {
            // Create controls for row.
            TextField txtName = new TextField()
            {
                X = 1, Y = rowNum, Width = 30, Text = name, CanFocus = true, Visible = true, 
                Enabled = true, Data = rowNum, TabStop = TabBehavior.TabStop
            };

            TextField txtValue = new TextField()
            {
                X = 32, Y = rowNum, Width = 30, Text = value,
                CanFocus = true, Visible = true, Enabled = true, Data = rowNum, TabStop = TabBehavior.TabStop
            };
            
            Button btnCopyValue = new Button()
            {
                X=63, Y = rowNum, Text = "Copy", 
                Width = 8, Height = 1, CanFocus = true, Visible = true, Data = rowNum, 
                TextAlignment = Alignment.Center, TabStop = TabBehavior.TabStop
            };

            // Create the delete button.
            Button btnDeleteRow = CreateDeleteButton(rowNum);

            // Create event handlers for the buttons.
            btnCopyValue.Accept += (s,e) =>
            {
                // Copy password to clipboard.
                Clipboard.TrySetClipboardData(txtValue.Text);

                // Indicate data copied.
                MessageBox.Query("Action Completed", "Copied Value to clipboard.", "Ok");
            };

            // Return control row.
            View[] retVal = [txtName, txtValue, btnCopyValue, btnDeleteRow];
            return retVal;
        }

        private View[] CreateHiddenRow(int rowNum, string? name, string? value)
        {
            // Create controls for row.
            TextField txtName = new TextField()
            {
                X=1, Y=rowNum, Width = 30, Text = name,
                CanFocus = true, Visible = true, Enabled = true, Data = rowNum, TabStop = TabBehavior.TabStop
            };

            TextField txtValue = new TextField()
            {
                X = 32, Y=rowNum, Width = 30, Text = value,
                CanFocus = true, Visible = true, Enabled = true, Data = rowNum, Secret = true, TabStop = TabBehavior.TabStop
            };
            
            Button btnShowValue = new Button()
            {
                X=63, Y=rowNum, Text = "View",
                Width = 8, Height = 1, CanFocus = true, Visible = true, Data = rowNum, 
                TextAlignment = Alignment.Center, TabStop = TabBehavior.TabStop
            };
           
            Button btnCopyValue = new Button()
            {
                X=72, Y =rowNum, Text = "Copy", 
                Width = 8, Height = 1, CanFocus = true, Visible = true, Data = rowNum, 
                TextAlignment = Alignment.Center, TabStop = TabBehavior.TabStop
            };

            // Create the delete button.
            Button btnDeleteRow = CreateDeleteButton(rowNum);
            
            // Create event handlers for the buttons.
            btnShowValue.Accept += (s,e) =>
            {
                // Toggle Flag.
                txtValue.Secret = !txtValue.Secret;

                // Flip button text to opposite action.
                btnShowValue.Text = txtValue.Secret ? "View" : "Hide";
            };
            
            btnCopyValue.Accept += (s,e) =>
            {
                // Copy password to clipboard.
                Clipboard.TrySetClipboardData(txtValue.Text);

                // Indicate data copied.
                MessageBox.Query("Action Completed", "Copied Value to clipboard.", "Ok");
            };
            
            // Return control row.
            View[] retVal = [txtName, txtValue, btnShowValue, btnCopyValue, btnDeleteRow];
            return retVal;
        }

        private View[] CreateBooleanRow(int rowNum, string? name, string? value)
        {
            // Create controls for row.
            TextField txtName = new TextField()
            {
                X=1, Y = rowNum, Width = 30, Text = name,
                CanFocus = true, Visible = true, Enabled = true, Data = rowNum, TabStop = TabBehavior.TabStop
            };

            CheckBox chkValue = new CheckBox()
            {
                X=32, Y=rowNum, CanFocus = true, Visible = true, Enabled = true, Data = rowNum,
                CheckedState = value == "true" ? CheckState.Checked : CheckState.UnChecked, TabStop = TabBehavior.TabStop
            };
            
            // Create the delete button.
            Button btnDeleteRow = CreateDeleteButton(rowNum);

            // Return control row.
            View[] retVal = [txtName, chkValue, btnDeleteRow];
            return retVal;

        }

        private View[] CreateLinkedRow(int rowNum, string? name, int linkId)
        {
            // Create controls for row.
            TextField txtName = new TextField()
            {
                X=1, Y=rowNum, Width = 30, Text = name,
                CanFocus = true, Visible = true, Enabled = true, Data = rowNum, TabStop = TabBehavior.TabStop
            };

            ComboBox cboValue = new ComboBox()
            {
                X = 32, Y = rowNum, Width = 30, CanFocus = true, Visible = true, Enabled = true, Data = rowNum,
                TabStop = TabBehavior.TabStop
            };
            
            // Set combo source and selected item.
            ObservableCollection<CodeListItem> linkList = GetComboSource();
            cboValue.SetSource(linkList);
            cboValue.SelectedItem = linkList.IndexOf(linkList.First(o => o.Index == linkId.ToString()));

            // Create the delete button.
            Button btnDeleteRow = CreateDeleteButton(rowNum);

            // Return control row.
            View[] retVal = [txtName, cboValue, btnDeleteRow];
            return retVal;
        }

        private Button CreateDeleteButton(int rowNum)
        {
            Button retVal = new Button()
            {
                X=81, Y=rowNum, Text = "Delete",
                Width = 10, Height = 1, CanFocus = true, Visible = true, Data = rowNum,
                TextAlignment = Alignment.Center, TabStop = TabBehavior.TabStop
            };
            
            retVal.Accept += (s,e) =>
            {
                // Get row index.
                int index = (int) retVal.Data;
                
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

            return retVal;
        }
        
        private ObservableCollection<CodeListItem> GetComboSource()
        {
            ObservableCollection<CodeListItem> retVal = new ObservableCollection<CodeListItem>();
            
            // Check to see what type of item we have.
            switch (_itemType)
            {
                // Login
                case 1:
                    // Login link list.
                    retVal = CodeListManager.GetObservableCollection("LoginLinkedId");
                    break;
                
                // Card
                case 3:
                    retVal = CodeListManager.GetObservableCollection("CardLinkedId");
                    break;
                
                // Identity
                case 4:
                    retVal = CodeListManager.GetObservableCollection("IdentityLinkedId");
                    break;
            }
            
            // Return the list.
            return retVal;
        }
        
        private void UpdateCustomFieldList()
        {
            // Create new URI list.
            List<VaultItemCustomField> fields = new List<VaultItemCustomField>();
            
            // Loop through URI list.
            foreach (View[] rowCtrls in _rowControls)
            {
                // Need to skip the first row of controls (the label headers)
                if (rowCtrls[0] is TextField)
                {
                    // Create new custom field object.
                    VaultItemCustomField field = new VaultItemCustomField();
                
                    // Get the name textfield.
                    TextField name = (TextField) rowCtrls[0];
                    field.Name = name.Text;
                    
                    // Check the type of the second control.
                    if (rowCtrls[1] is TextField)
                    {
                        // Get the control.
                        TextField value = (TextField) rowCtrls[1];
                        
                        // Set the value.
                        field.FieldValue = value.Text;
                        
                        // Check to see if the value is hidden.
                        field.FieldType = value.Secret ? 1 : 0;
                    }
                    
                    else if (rowCtrls[1] is CheckBox)
                    {
                        // Get the control.
                        CheckBox value = (CheckBox) rowCtrls[1];
                        
                        // Set value and type.
                        field.FieldValue = value.CheckedState == CheckState.Checked ? "true" : "false";
                        field.FieldType = 2;
                    }
                    
                    else if (rowCtrls[1] is ComboBox)
                    {
                        // Get the control.
                        ComboBox value = (ComboBox) rowCtrls[1];
                        
                        // Set value and type.
                        field.FieldValue = value.SelectedItem.ToString();
                        field.FieldType = 3;
                    }
                
                    // Add field to list.
                    fields.Add(field);
                }
            }
            
            // Update field list.
            _fields = fields;
        }

        public void CreateControlRow(int fieldType)
        {
            // Create field and add type.
            VaultItemCustomField field = new VaultItemCustomField();
            field.FieldType = fieldType;
            
            // Call private method to add blank control row.
            View []? newRow = CreateNewControlRow(_rowControls.Count, field);
            
            // Check to see that the row was created.
            if (newRow != null)
            {
                // Add to list.
                _rowControls.Add(newRow);

                // Add controls to Uri scroll.
                Add(newRow);

                // Set scroll for redraw.
                SetNeedsDisplay();
                
                // Set focus to new text field.
                newRow[0].SetFocus();
            }
        }

        public List<VaultItemCustomField> Fields
        {
            get
            {
                // First update the fields.
                UpdateCustomFieldList();
                return _fields;
            }
        }
    }
}