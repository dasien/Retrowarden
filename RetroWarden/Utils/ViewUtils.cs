using System.Text;
using RetrowardenSDK.Models;
using Terminal.Gui;

namespace Retrowarden.Utils
{
    public static class ViewUtils
    {
        public static ITreeNode CreateCollectionNodes(List<VaultCollection> collection, 
            SortedDictionary<string, VaultItem> items, ITreeNode parent, string? orgId)
        {
            // Clear any folder nodes.
            parent.Children.Clear();

            // Loop through the collections list.
            foreach (VaultCollection col in collection)
            {
                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if the org id matches the parent.
                    if (item.OrganizationId != null && item.OrganizationId == orgId)
                    {
                        // Check to see if the item is part of a collection.
                        if (item.CollectionIds != null)
                        {
                            if (item.CollectionIds.Contains(col.Id))
                            {
                                // Create new branch node.
                                TreeNode branch = new TreeNode(col.Name)
                                {
                                    Tag = new NodeData()
                                    {
                                        Id= col.Id, NodeType = NodeType.Collection, Parent = parent, Text = col.Name
                                    }
                                };
                                    
                                parent.Children.Add(branch);
                                    
                                // Only add the collection once.
                                break;
                            }
                        }
                    }
                }
            }
            
            // Return tree.
            return parent;
        }
        
        public static ITreeNode CreateFolderNodes(List<VaultFolder> folders, SortedDictionary<string, VaultItem> items, 
            ITreeNode parent, string? orgId)
        {
            // Clear any folder nodes.
            parent.Children.Clear();
            
            // Loop through the folders list.
            foreach (VaultFolder folder in folders)
            {
                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if a folder was set.
                    if (item.FolderId != null)
                    {
                        // Check to see if it matches branch id.
                        if (item.FolderId == folder.Id)
                        {
                            // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                            if (item.OrganizationId == orgId)
                            {
                                TreeNode branch = new TreeNode(folder.Name)
                                {
                                    Tag = new NodeData()
                                    {
                                        Id = folder.Id, NodeType = NodeType.Folder, Parent = parent, Text = folder.Name
                                    }
                                };

                                parent.Children.Add(branch);
                                
                                // Only need to add the folder once.
                                break;
                            }
                        }
                    }

                    else
                    {
                        if (folder.Name == "No Folder")
                        {
                            // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                            if (item.OrganizationId == orgId)
                            {
                                TreeNode branch = new TreeNode(folder.Name)
                                {
                                    Tag = new NodeData()
                                    {
                                        Id = folder.Id, NodeType = NodeType.Folder, Parent = parent, Text = folder.Name
                                    }
                                };

                                parent.Children.Add(branch);
                                
                                // Only need to add the folder once.
                                break;
                            }
                        }
                    }
                }
            }

            // Return parent with folders added.
            return parent;
        }
        
        public static void CreateStaticNodesForOrg(ITreeNode parent, string? orgId)
        {
            // Create folders node.
            TreeNode foldersNode = new TreeNode("Folders")
            {
                Tag = new NodeData()
                {
                    Id = "Folders", NodeType = NodeType.FolderGroup, Parent = parent, Text = "Folders",
                    ItemGroupType = NodeItemGroupType.None
                }
            };
            
            // Create collections node.
            TreeNode collectionsNode = new TreeNode("Collections")
            {
                Tag = new NodeData()
                {
                    Id = "Collections", NodeType = NodeType.CollectionGroup, Parent = parent, Text = "Collections",
                    ItemGroupType = NodeItemGroupType.None
                }
            };
            
            // Create items node.
            TreeNode itemsNode = new TreeNode("All Items")
            {
                Tag = new NodeData()
                {
                    Id = "All Items", NodeType = NodeType.ItemGroup, Parent = parent, Text = "All Items",
                    ItemGroupType = NodeItemGroupType.AllItems
                }
            };
            
            // Branch nodes.
            TreeNode favorites = new TreeNode("Favorites")
            {
                Tag = new NodeData()
                {
                    Id = "Favorites", NodeType = NodeType.FavoriteGroup, Parent = itemsNode, Text = "Favorites",
                    ItemGroupType = NodeItemGroupType.Favorites
                }
            };

            TreeNode logins = new TreeNode("Logins")
            {
                Tag = new NodeData()
                {
                    Id = "Logins", NodeType = NodeType.ItemGroup, Parent = itemsNode, Text = "Logins",
                    ItemGroupType = NodeItemGroupType.Login
                }
            };

            TreeNode cards = new TreeNode("Cards")
            {
                Tag = new NodeData()
                {
                    Id = "Cards", NodeType = NodeType.ItemGroup, Parent = itemsNode, Text = "Cards",
                    ItemGroupType = NodeItemGroupType.Card
                }
            };

            TreeNode identities = new TreeNode("Identities")
            {
                Tag = new NodeData()
                {
                    Id = "Identities", NodeType = NodeType.ItemGroup, Parent = itemsNode, Text = "Identities",
                    ItemGroupType = NodeItemGroupType.Identity
                }
            };

            TreeNode notes = new TreeNode("Secure Notes")
            {
                Tag = new NodeData()
                {
                    Id = "Secure Notes", NodeType = NodeType.ItemGroup, Parent = itemsNode, Text = "Secure Notes",
                    ItemGroupType = NodeItemGroupType.SecureNote
                }
            };

            // Add nodes to all items.
            itemsNode.Children.Add(favorites);
            itemsNode.Children.Add(logins);
            itemsNode.Children.Add(cards);
            itemsNode.Children.Add(identities);
            itemsNode.Children.Add(notes);
            
            // Add base nodes to parent.
            parent.Children.Add(itemsNode);
            parent.Children.Add(foldersNode);
            parent.Children.Add(collectionsNode);
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