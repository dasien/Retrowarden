/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * ItemListDataSource.cs
 *
 * Copyright (C) 2024 Brian Gentry
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Terminal.Gui;
using RetrowardenSDK.Models;

namespace Retrowarden.Utils
{
    public class ItemListDataSource : IListDataSource
    {
        // Member variables.
        private const int NameColumnWidth = 30;
        private const int UserIdColumnWidth = 30;
        private int _count;
        private int _maxLength;
        private List<VaultItem> _items;
        private BitArray _markedRows;

        // This event helps identify when a row is marked or unmarked by a mouse click, because the 
        // SelectedItemChanged event on the listview doesn't fire in that case.
        public event EventHandler? OnSetMark;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ItemListDataSource(List<VaultItem> items)
        {
            // Set item list and associated values.
            _count = items.Count;
            _markedRows = new BitArray(_count);
            _items = items;
            _maxLength = GetMaxLengthItem();
        }

        public void Render(ListView container, bool selected, int item, int col, int line, int width, int start = 0)
        {
            // Placeholder for 2nd column text.
            string itemText = " ";

            // Get the vault item.
            VaultItem rowItem = _items[item];

            // Move to the next row.
            container.Move(col, line);

            // This column is the same regardless of the item type.
            string itemName = String.Format(String.Format("{{0,{0}}}", -NameColumnWidth), _items[item].ItemName);

            // Based
            switch (rowItem.ItemType)
            {
                // Login.
                case 1:
                    {
                        // Item text is the username.
                        itemText = String.Format(String.Format("{{0,{0}}}", -UserIdColumnWidth),
                            rowItem.Login == null ? " " : rowItem.Login.GetListViewColumnText());
                        break;
                    }

                // Secure Note.
                case 2:
                    {
                        // There is no real item text here for now.
                        itemText = String.Format(String.Format("{{0,{0}}}", -UserIdColumnWidth),
                            rowItem.SecureNote == null ? " " : rowItem.SecureNote.GetListViewColumnText());
                        break;
                    }

                // Card.
                case 3:
                    {
                        // Item text is the card brand + last 4 of card number.
                        itemText = String.Format(String.Format("{{0,{0}}}", -UserIdColumnWidth),
                            rowItem.Card == null ? " " : rowItem.Card.GetListViewColumnText());
                        break;
                    }

                // Identity.
                case 4:
                    {
                        // Item text is the first and last name.
                        itemText = String.Format(String.Format("{{0,{0}}}", -UserIdColumnWidth),
                            rowItem.Identity == null ? " " : rowItem.Identity.GetListViewColumnText());
                        break;
                    }
            }

            // Write row text.
            RenderUstr(container, $"{itemName} {itemText} {_items[item].ItemOwnerName}", col, line, width, start);
        }

        public bool IsMarked(int item)
        {
            bool retVal = false;

            if (item >= 0 && item < _count)
            {
                retVal = _markedRows[item];
            }

            return retVal;
        }
        
        public void SetMark(int item, bool value)
        {
            if (item >= 0 && item < _count)
            {
                _markedRows[item] = value;

                // Raise marked event.
                OnSetMark?.Invoke(this, EventArgs.Empty);
            }
        }
        public IList ToList()
        {
            return _items;
        }

        #region Private Methods
        private int GetMaxLengthItem()
        {
            int retVal = 0;

            if (_items.Count != 0)
            {
                for (int cnt = 0; cnt < _items.Count; cnt++)
                {
                    // Get the string for the item value.
                    string? itemVal = GetItemName(_items[cnt]);

                    // Create cols for name and value.
                    string col1 = string.Format(string.Format("{{0,{0}}}", -NameColumnWidth), _items[cnt].ItemName);
                    string col2 = string.Format(string.Format("{{0,{0}}}", -UserIdColumnWidth), itemVal == null ? " " : itemVal);

                    // Create entire line string.
                    string sc = $"{col1}  {col2} {_items[cnt].ItemOwnerName}";

                    // Get the length.
                    int l = sc.Length;

                    // Check to see if this is the longest row. 
                    if (l > retVal)
                    {
                        retVal = l;
                    }
                }
            }

            return retVal;
        }

        private string? GetItemName(VaultItem item)
        {
            string? retVal = " ";

            // Check the item type.
            switch (item.ItemType)
            {
                // Login
                case 1:
                    if (item.Login != null)
                    {
                        retVal = item.Login.UserName;
                    }
                    break;

                // Note
                case 2:
                    retVal = string.Empty;
                    break;

                // Card
                case 3:
                    if (item.Card != null)
                    {
                        retVal = item.Card.GetListViewColumnText();
                    }
                    break;

                // Identity
                case 4:
                    if (item.Identity != null)
                    {
                        retVal = item.Identity.GetListViewColumnText();
                    }
                    break;
            }

            // Return the item string.
            return retVal;
        }

        private void RenderUstr(View container, string ustr, int col, int line, int width, int start = 0)
        {
            int used = 0;
            int index = start;
            while (index < ustr.Length)
            {
                (Rune rune, int size) = ustr.DecodeRune(index, index - ustr.Length);
                int count = rune.GetColumns();
                if (used + count >= width)
                {
                    break;
                }
                
                container.AddRune(used, line, rune);
                used += count;
                index += size;
            }

            while (used < width)
            {
                container.AddRune(used, line, new Rune(' '));
                used++;
            }
        }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return _items != null ? _items.Count : 0;
            }
        }

        public int MarkedItemCount
        {
            get
            {
                int retVal = 0;

                // Count items.
                foreach (bool bit in _markedRows)
                {
                    if (bit)
                    {
                        retVal++;
                    }
                }

                return retVal;
            }
        }
        public int Length
        {
            get
            {
                return _maxLength;
            }
        }

        public bool SuspendCollectionChangedEvent { get; set; }

        public List<VaultItem> ItemList
        {
            get
            {
                return _items;
            }

            set
            {
                if (value != null)
                {
                    _count = value.Count;
                    _markedRows = new BitArray(_count);
                    _items = value;
                    _maxLength = GetMaxLengthItem();
                }
            }
        }

        public List<VaultItem> MarkedItemList
        {
            get
            {
                List<VaultItem> retVal = new List<VaultItem>();

                // Loop through marked items.
                for (int cnt = 0; cnt < _markedRows.Count; cnt++)
                {
                    if (_markedRows[cnt])
                    {
                        // Add the item to the return list.
                        retVal.Add(_items[cnt]);
                    }
                }

                return retVal;
            }
        }
        #endregion

        public void Dispose()
        {

        }
    }
}