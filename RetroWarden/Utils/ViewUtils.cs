using System.Text;
using Retrowarden.Models;
using Terminal.Gui.Trees;

namespace Retrowarden.Utils
{
    public static class ViewUtils
    {
        public static TreeNode CreateCollectionsNode(List<VaultCollection> collection, 
            SortedDictionary<string, VaultItem> items, TreeNode parent, Organization org)
        {
            // Root node.
            TreeNode root = new TreeNode("Collections")
            {
                Tag = new NodeData()
                {
                    Id= "Collections", NodeType = NodeType.Collection, Parent = parent, Text = null
                }
            };
            
            // Loop through the collections list.
            foreach (VaultCollection col in collection)
            {
                // Create new branch node.
                TreeNode branch = new TreeNode(col.Name)
                {
                    Tag = new NodeData()
                    {
                        Id= col.Id, NodeType = NodeType.Collection, Parent = root, Text = col.Name
                    }
                };

                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if the org id matches the parent.
                    if (item.OrganizationId != null && item.OrganizationId == org.Id)
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
                                        Tag = new NodeData()
                                        {
                                            Id = item.Id, NodeType = NodeType.Item, Parent = branch,
                                            Text = item.ItemName
                                        }
                                    };
                                    branch.Children.Add(leaf);
                                }
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
        
        public static TreeNode CreateFoldersNode(List<VaultFolder> folders, 
            SortedDictionary<string, VaultItem> items, TreeNode parent, Organization? org)
        {
            // Root node.
            TreeNode root = new TreeNode("Folders")
            {
                Tag = new NodeData()
                {
                    Id= "Folders", NodeType = NodeType.Folder, Parent = parent, Text = null
                }
            };

            // Loop through the folders list.
            foreach (VaultFolder folder in folders)
            {
                TreeNode branch = new TreeNode(folder.Name)
                {
                    Tag = new NodeData()
                    {
                        Id= folder.Id, NodeType = NodeType.Folder, Parent = root, Text = folder.Name
                    }
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
                            // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                            if ((org != null && item.OrganizationId == org.Id) 
                                || (org == null && item.OrganizationId == null))
                            {
                                // Add leaf node.
                                TreeNode leaf = new TreeNode(item.ItemName)
                                {
                                    Tag = new NodeData()
                                    {
                                        Id = item.Id, NodeType = NodeType.Item, Parent = branch, Text = item.ItemName
                                    }
                                };
                                
                                branch.Children.Add(leaf);
                            }
                        }
                    }

                    else 
                    {
                        if (folder.Name == "No Folder")
                        {
                            // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                            if ((org != null && item.OrganizationId == org.Id) 
                                || (org == null && item.OrganizationId == null))
                            {
                                // Add leaf node.
                                TreeNode leaf = new TreeNode(item.ItemName)
                                {
                                    Tag = new NodeData()
                                    {
                                        Id = item.Id, NodeType = NodeType.Item, Parent = branch, Text = item.ItemName
                                    }
                                };
                                branch.Children.Add(leaf);
                            }
                        }
                    }
                }
                
                // Check to see if any children were added.
                if (branch.Children.Count > 0)
                {
                    root.Children.Add(branch);
                }
            }
            
            // Return tree.
            return root;
        }

        public static TreeNode CreateAllItemsNodes(SortedDictionary<string, VaultItem> items, TreeNode parent, Organization? org)
        {
            // Return value.
            TreeNode retVal = new TreeNode("All Items")
            {
                Tag = new NodeData()
                {
                    Id = "All Items", NodeType = NodeType.ItemGroup, Parent = parent, Text = "All Items"
                }
            };
            // Branch nodes.
            TreeNode favorites = new TreeNode("Favorites")
            {
                Tag = new NodeData()
                {
                    Id = "Favorites", NodeType = NodeType.ItemGroup, Parent = retVal, Text = "Favorites"
                }
            };

            TreeNode logins = new TreeNode("Logins")
            {
                Tag = new NodeData()
                {
                    Id = "Logins", NodeType = NodeType.ItemGroup, Parent = retVal, Text = "Logins"
                }
            };

            TreeNode cards = new TreeNode("Cards")
            {
                Tag = new NodeData()
                {
                    Id = "Cards", NodeType = NodeType.ItemGroup, Parent = retVal, Text = "Cards"
                }
            };

            TreeNode identities = new TreeNode("Identities")
            {
                Tag = new NodeData()
                {
                    Id = "Identities", NodeType = NodeType.ItemGroup, Parent = retVal, Text = "Identities"
                }
            };

            TreeNode notes = new TreeNode("Secure Notes")
            {
                Tag = new NodeData()
                {
                    Id = "Secure Notes", NodeType = NodeType.ItemGroup, Parent = retVal, Text = "Secure Notes"
                }
            };

            // Loop through the item list.
            foreach (VaultItem item in items.Values)
            {
                // Check to see if there is an org and this item is part of it, or this is the personal vault (no org).
                if ((org != null && item.OrganizationId == org.Id) 
                    || (org == null && item.OrganizationId == null))
                {
                    // Create node for this item.
                    TreeNode leaf = new TreeNode(item.ItemName);

                    // Check to see if it is a favorite.
                    if (item.IsFavorite)
                    {
                        leaf.Tag = new NodeData()
                        {
                            Id = item.Id, NodeType = NodeType.Item, Parent = favorites, Text = item.ItemName
                        };

                        favorites.Children.Add(leaf);
                    }

                    // Create node for this item.
                    leaf = new TreeNode(item.ItemName);

                    // Sort items based on type.
                    switch (item.ItemType)
                    {
                        // Login
                        case 1:

                            leaf.Tag = new NodeData()
                            {
                                Id = item.Id, NodeType = NodeType.Item, Parent = logins, Text = item.ItemName
                            };

                            // Add to branch.
                            logins.Children.Add(leaf);
                            break;

                        // Note
                        case 2:

                            leaf.Tag = new NodeData()
                            {
                                Id = item.Id, NodeType = NodeType.Item, Parent = notes, Text = item.ItemName
                            };

                            // Add to branch.
                            notes.Children.Add(leaf);
                            break;

                        // Card
                        case 3:

                            leaf.Tag = new NodeData()
                            {
                                Id = item.Id, NodeType = NodeType.Item, Parent = cards, Text = item.ItemName
                            };

                            // Add to branch.
                            cards.Children.Add(leaf);
                            break;

                        // Identity
                        case 4:

                            leaf.Tag = new NodeData()
                            {
                                Id = item.Id, NodeType = NodeType.Item, Parent = identities, Text = item.ItemName
                            };

                            // Add to branch.
                            identities.Children.Add(leaf);
                            break;
                    }
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