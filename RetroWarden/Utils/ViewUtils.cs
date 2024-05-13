using System.Text;
using Retrowarden.Models;
using Terminal.Gui.Trees;

namespace Retrowarden.Utils
{
    public static class ViewUtils
    {
        public static TreeNode CreateCollectionsNode(List<VaultCollection> collection, SortedDictionary<string, VaultItem> items)
        {
            // Root node.
            TreeNode root = new TreeNode("Collections");
            
            // Assign type.
            root.Tag = new Tuple<NodeType, string?>(NodeType.Collection, null);
            
            // Loop through the collections list.
            foreach (VaultCollection col in collection)
            {
                // Create new branch node.
                TreeNode branch = new TreeNode(col.Name)
                {
                    Tag = new Tuple<NodeType, string?>(NodeType.Collection, col.Id)
                };

                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if the item is part of a collection.
                    if (item.CollectionIds != null)
                    {
                        // Now loop through collection associations.
                        foreach (string id in item.CollectionIds)
                        {
                            // Check to see if it matches branch id.
                            if (id == col.Id)
                            {
                                // Add leaf node.
                                TreeNode leaf = new TreeNode(item.ItemName)
                                {
                                    Tag = new Tuple<NodeType, string?>(NodeType.Item, item.Id)
                                };
                                branch.Children.Add(leaf);
                            }
                        }
                    }
                }
                                    
                // Add branch.
                root.Children.Add(branch);
            }
            
            // Return tree.
            return root;
        }
        
        public static TreeNode CreateFoldersNode(List<VaultFolder> folders, SortedDictionary<string, VaultItem> items)
        {
            // Root node.
            TreeNode root = new TreeNode("Folders")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.Folder, null)
            };

            // Loop through the folders list.
            foreach (VaultFolder folder in folders)
            {
                TreeNode branch = new TreeNode(folder.Name)
                {
                    Tag = new Tuple<NodeType, string>(NodeType.Folder, folder.Id)
                };

                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if a folder was set.
                    if (item.FolderId != null)
                    {
                        // Check to see if it matches branch id.
                        if (item.FolderId == folder.Id)
                        {
                            // Add leaf node.
                            TreeNode leaf = new TreeNode(item.ItemName)
                            {
                                Tag = new Tuple<NodeType, string?>(NodeType.Item, item.Id)
                            };
                            branch.Children.Add(leaf);
                        }
                    }

                    else 
                    {
                        if (folder.Name == "No Folder")
                        {
                            // Add leaf node.
                            TreeNode leaf = new TreeNode(item.ItemName)
                            {
                                Tag = new Tuple<NodeType, string?>(NodeType.Item, item.Id)
                            };
                            branch.Children.Add(leaf);
                        }
                    }
                }
                
                // Add branches.
                root.Children.Add(branch);
            }
            
            // Return tree.
            return root;
        }

        public static TreeNode CreateAllItemsNodes(SortedDictionary<string, VaultItem> items)
        {
            // Return value.
            TreeNode retVal = new TreeNode("All Items")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.ItemGroup, null)
            };
            // Branch nodes.
            TreeNode favorites = new TreeNode("Favorites")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.Favorite, null)
            };

            TreeNode logins = new TreeNode("Logins")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.ItemGroup, null)
            };

            TreeNode cards = new TreeNode("Cards")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.ItemGroup, null)
            };

            TreeNode identities = new TreeNode("Identities")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.ItemGroup, null)
            };

            TreeNode notes = new TreeNode("Secure Notes")
            {
                Tag = new Tuple<NodeType, string?>(NodeType.ItemGroup, null)
            };

            // Loop through the item list.
            foreach (VaultItem item in items.Values)
            {
                // Create node for this item.
                TreeNode leaf = new TreeNode(item.ItemName)
                {
                    Tag = new Tuple<NodeType, string?>(NodeType.Item, item.Id)
                };

                // Check to see if it is a favorite.
                if (item.IsFavorite)
                {
                    favorites.Children.Add(leaf);
                }
                
                // Sort items based on type.
                switch (item.ItemType)
                {
                    // Login
                    case 1:
                        
                        // Add to branch.
                        logins.Children.Add(leaf);
                        break;
                    
                    // Note
                    case 2:
                        
                        // Add to branch.
                        notes.Children.Add(leaf);
                        break;
                    
                    // Card
                    case 3:
                        
                        // Add to branch.
                        cards.Children.Add(leaf);
                        break;

                    // Identity
                    case 4:
                        
                        // Add to branch.
                        identities.Children.Add(leaf);
                        break;
                }
            }
            
            // Add nodes to root.
            retVal.Children.Add(favorites);
            retVal.Children.Add(logins);
            retVal.Children.Add(cards);
            retVal.Children.Add(identities);
            retVal.Children.Add(notes);

            // Return tree.
            return retVal;
        }
        
        public static StringBuilder CreateAboutMessageAscii()
        {
            StringBuilder retVal = new StringBuilder();
            
            // Create ascii art.
            retVal.AppendLine (@" ******************               A terminal.gui based client for Bitwarden");
            retVal.AppendLine (@" ********       #**     ");			
            retVal.AppendLine (@" ********       #**     ______     _                                  _            ");
            retVal.AppendLine (@" ********       #**     | ___ \   | |                                | |           ");
            retVal.AppendLine (@" ********       ***     | |_/ /___| |_ _ __ _____      ____ _ _ __ __| | ___ _ __  ");
            retVal.AppendLine (@"  *******     ****      |    // _ \ __| '__/ _ \ \ /\ / / _` | '__/ _` |/ _ \ '_ \ ");
            retVal.AppendLine (@"   ******   ****        | |\ \  __/ |_| | | (_) \ V  V / (_| | | | (_| |  __/ | | |");
            retVal.AppendLine (@"    **********          \_| \_\___|\__|_|  \___/ \_/\_/ \__,_|_|  \__,_|\___|_| |_|");
            retVal.AppendLine (@"       ****             ");

            return retVal;
        }
    }
}