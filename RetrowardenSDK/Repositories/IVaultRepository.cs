/******************************************************************************
 * RetrowardenSDK - A secure password management library
 * IVaultRepository.cs
 * 
 * Copyright (C) 2024 RetrowardenSDK Project
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 ******************************************************************************/
using RetrowardenSDK.Models;

namespace RetrowardenSDK.Repositories
{
    public interface IVaultRepository
    {
        #region Auth Methods

        public void Login(string email, string password);
        public void Logout();
        public void Unlock(string password);
        public void Lock();
        #endregion

        #region Read Methods

        public List<VaultFolder>? ListFolders();
        public List<VaultCollection>? ListCollections();
        public List<Organization>? ListOrganizations();
        public List<Member>? ListMembersForOrganization(string orgId);
        public SortedDictionary<string, VaultItem>? ListVaultItems();
        public bool CheckForUpdate();
        public VaultStatus? GetStatus();

        #endregion

        #region Vault Item Write Methods

        public VaultItem? CreateVaultItem(string encodedItem);
        public VaultItem? UpdateVaultItem(string id, string encodedItem);
        public void DeleteVaultItem(string itemId);
        public VaultItem? MoveVaultItem(string id, string orgId, string encodedItem);

        #endregion

        #region Folder Write Method

        public VaultFolder? CreateFolder(string encodedItem);

        #endregion

        #region Collection Write Method

        public VaultCollection? CreateCollection(string encodedItem);

        #endregion

        #region Generate Methods

        public string GeneratePassword(bool useUpper, bool useLower, bool useDigits, bool useSpecial, int length);
        public string GeneratePassphrase(int numWords, string? sepChar, bool useUpper, bool useDigits);

        #endregion

        #region Send Methods

        public string CreateSend(Send send, string filePath = null);
        public List<Send>? ListSends();

        public void DeleteSend(string id);
        #endregion
        
        #region Properties

        public string ExitCode { get; }
        public string ErrorMessage { get; }
        public bool IsLoggedIn { get; }
        public bool IsLocked { get; }
        public bool IsOrgEngabled { get; }

        #endregion
    }
}