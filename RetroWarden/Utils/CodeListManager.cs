/******************************************************************************
 * Retrowarden - A Terminal.Gui based client for Bitwarden
 * CodeListManager.cs
 *
 * Utility class for managing lists of codes. Provides for a 
 * centralized handling of application-specific code mappings and their
 * corresponding descriptions for consistent error handling and status reporting.
 *
 * Copyright (C) 2024 Retrowarden Project
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using System.Collections.ObjectModel;

namespace Retrowarden.Utils
{
    public sealed class CodeListManager
    {
        private static Dictionary<string, List<CodeListItem>>? _codeLists;

        static CodeListManager()
        {
            // Create list holder.
            _codeLists = new Dictionary<string, List<CodeListItem>>();
            
            // Load lists.
            LoadLists();
        }

        private static void LoadLists()
        {
            // Check to see that code lists have been initialized.
            if (_codeLists == null)
            {
                // Create it.
                _codeLists = new Dictionary<string, List<CodeListItem>>();
            }
            
            // Create list of card brands.
            _codeLists.Add("CardBrands", 
            [
                new CodeListItem("Visa", "Visa"),
                new CodeListItem("Mastercard", "Mastercard"),
                new CodeListItem("American Express", "American Express"),
                new CodeListItem("Discover", "Discover"),
                new CodeListItem("Diners Club", "Diners Club"),
                new CodeListItem("JCB", "JCB"),
                new CodeListItem("Maestro", "Maestro"),
                new CodeListItem("UnionPay", "UnionPay"),
                new CodeListItem("RuPay", "RuPay"),
                new CodeListItem("Other", "Other")
            ]);
            
            // Create list of expiration months.
            _codeLists.Add("ExpiryMonths", 
            [
                new CodeListItem("1", "1 - January"),
                new CodeListItem("2", "2 - February"),
                new CodeListItem("3", "3 - March"),
                new CodeListItem("4", "4 - April"),
                new CodeListItem("5", "5 - May"),
                new CodeListItem("6", "6 - June"),
                new CodeListItem("7", "7 - July"),
                new CodeListItem("8", "8 - August"),
                new CodeListItem("9", "9 - September"),
                new CodeListItem("10", "10 - October"),
                new CodeListItem("11", "11 - November"),
                new CodeListItem("12", "12 - December")
            ]);
            
            // Create list of titles.
            _codeLists.Add("Titles", 
            [
                new CodeListItem("Mr", "Mr"),
                new CodeListItem("Mrs", "Mrs"),
                new CodeListItem("Ms", "Ms"),
                new CodeListItem("Mx", "Mx"),
                new CodeListItem("Dr", "Dr")
            ]);
            
            _codeLists.Add("MatchDetections", 
            [
                new CodeListItem("0", "Base Domain"),
                new CodeListItem("1", "Host"),
                new CodeListItem("2", "Starts With"),
                new CodeListItem("3", "Regular Expression"),
                new CodeListItem("4", "Exact Match"),
                new CodeListItem("5", "Never"),
                new CodeListItem(null, "Default")
            ]);
            
            _codeLists.Add("CustomFieldType", 
            [
                new CodeListItem("0", "Text"),
                new CodeListItem("1", "Hidden"),
                new CodeListItem("2", "Boolean"),
                new CodeListItem("3", "Linked")
            ]);

            _codeLists.Add("LoginLinkedId", 
            [
                new CodeListItem("100", "User Name"),
                new CodeListItem("101", "Password")
            ]);
            
            _codeLists.Add("CardLinkedId", 
            [
                new CodeListItem("300", "Base Domain"),
                new CodeListItem("301", "Cardholder Name"),
                new CodeListItem("302", "Expiration Month"),
                new CodeListItem("303", "Expiration Year"),
                new CodeListItem("304", "Card Brand"),
                new CodeListItem("305", "Card Number")
            ]);
            
            _codeLists.Add("IdentityLinkedId", 
            [
                new CodeListItem("400", "Title"),
                new CodeListItem("401", "Middle Name"),
                new CodeListItem("402", "Address 1"),
                new CodeListItem("403", "Address 2"),
                new CodeListItem("404", "Address 3"),
                new CodeListItem("405", "City"),
                new CodeListItem("406", "State"),
                new CodeListItem("407", "Postal Code"),
                new CodeListItem("408", "Country"),
                new CodeListItem("409", "Company"),
                new CodeListItem("410", "Email"),
                new CodeListItem("411", "Phone"),
                new CodeListItem("412", "SSN"),
                new CodeListItem("413", "User Name"),
                new CodeListItem("414", "Passport Number"),
                new CodeListItem("415", "License Number"),
                new CodeListItem("416", "First Name"),
                new CodeListItem("417", "Last Name"),
                new CodeListItem("418", "Full Name")
            ]);
            
            // List of org user types.
            _codeLists.Add("OrgUserTypes", 
            [
                new CodeListItem("0", "Owner"),
                new CodeListItem("1", "Admin"),
                new CodeListItem("2", "User"),
                new CodeListItem("3", "Manager"),
                new CodeListItem("4", "Custom")
            ]);
            
            // List of org user status.
            _codeLists.Add("OrgUserStatus", 
            [
                new CodeListItem("0", "Invited"),
                new CodeListItem("1", "Accepted"),
                new CodeListItem("2", "Confirmed"),
                new CodeListItem("-1", "Revoked")
            ]);


        }

        public static List<CodeListItem> GetList(string listName)
        {
            List<CodeListItem> retVal = new List<CodeListItem>();
            
            if (_codeLists != null)
            {
                retVal = _codeLists[listName];
            }
            
            // Return the list.
            return retVal;
        }

        public static ObservableCollection<CodeListItem> GetObservableCollection(string listName)
        {
            // The return value.
            return new ObservableCollection<CodeListItem>(GetList(listName));
        }
    }
}