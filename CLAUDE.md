# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

RetroWarden is a Terminal.GUI-based client for Bitwarden, providing a retro terminal UI experience for password management. The solution consists of two projects:

- **RetroWarden**: Main executable with Terminal.GUI UI layer (.NET 8.0)
- **RetrowardenSDK**: Shared library with models and Bitwarden CLI wrapper (.NET 8.0)

## Build and Run Commands

### Building the Solution
```bash
dotnet build RetroWarden.sln
```

### Building Individual Projects
```bash
dotnet build RetroWarden/RetroWarden.csproj
dotnet build RetrowardenSDK/RetrowardenSDK.csproj
```

### Running the Application
```bash
dotnet run --project RetroWarden/RetroWarden.csproj
```

### Debug Mode
The application accepts a debug flag in Program.cs (line 12):
```csharp
MainView mainView = new MainView(true);  // true = use DebugVaultRepository (mock data)
```

## Architecture

### Project Dependency Chain
```
RetroWarden (UI Layer)
    ↓
RetrowardenSDK (Business Logic & Data Access)
    ↓
Bitwarden CLI (bw.exe) - External process
```

### Key Architectural Patterns

**Repository Pattern**: `IVaultRepository` provides abstraction over Bitwarden CLI operations with two implementations:
- `VaultRepository`: Production implementation using System.Diagnostics.Process to execute `bw.exe` commands
- `DebugVaultRepository`: Mock implementation for testing without real CLI

**Worker Pattern**: All long-running operations inherit from `RetrowardenWorker` (abstract base):
- Uses .NET BackgroundWorker for async execution
- Automatically displays `WorkingDialog` during operations
- Examples: `LoginWorker`, `GetItemsWorker`, `SaveItemWorker`, `SyncWorker`

**Template Method Pattern**: `ItemDetailView` (abstract base) with type-specific implementations:
- `LoginDetailView`: Login credentials editor
- `CardDetailView`: Credit card information editor
- `SecureNoteDetailView`: Secure note editor
- `IdentityDetailView`: Personal identity information editor

**Dialog Pattern**: All dialogs inherit from `BaseDialog` and are modal. Examples:
- `ConnectDialog`: Login credentials input
- `UnlockDialog`: Vault unlock with master password
- `GeneratePasswordDialog`: Password generator with options
- `AddFolderDialog`, `SelectItemTypeDialog`, etc.

### Core Components

**MainView** (`Views/MainView.cs`, 1,826 lines): Primary application controller
- Manages all vault state (items, folders, collections, organizations)
- Orchestrates all worker operations
- Handles TreeView (hierarchical organization) and ListView (item list) binding
- Manages auto-lock timeout and activity tracking
- Implements menu handlers and keyboard shortcuts

**IVaultRepository** (`RetrowardenSDK/Repositories/IVaultRepository.cs`): 50 methods including:
- Auth: Login, Logout, Unlock, Lock
- Read: ListFolders, ListCollections, ListOrganizations, ListVaultItems, GetStatus
- Write: CreateVaultItem, UpdateVaultItem, DeleteVaultItem, MoveVaultItem, CreateFolder
- Generate: GeneratePassword, GeneratePassphrase

**TreeView Organization Structure**:
```
Root (Bitwarden)
├── Organization ("My Vault" or org name)
│   ├── CollectionGroup → Collection items
│   ├── FolderGroup → Folder items
│   ├── ItemGroup (All, Logins, Notes, Cards, Identities)
│   └── FavoriteGroup
└── [Additional Organizations...]
```

Node metadata stored in `NodeData` class (attached as TreeView node Tags).

### Terminal.GUI Framework Integration

**Version**: Terminal.Gui v2.0.0-develop.4639 (development build)

**Important**: Recent migration to v2 namespace structure. Use `Terminal.Gui.App` namespace imports:
```csharp
using Terminal.Gui.App;
```

**Local DLL Reference**: RetroWarden.csproj references a local Terminal.Gui.dll:
```xml
<Reference Include="Terminal.Gui">
  <HintPath>..\gui-cs\Terminal.Gui\Terminal.Gui\bin\Debug\net8.0\Terminal.Gui.dll</HintPath>
</Reference>
```

**Key Components Used**:
- `Application`: Lifecycle management (Init, Run, Shutdown)
- `Toplevel`: Main window base (MainView inherits)
- `Dialog`: Modal dialogs
- `FrameView`: Grouped containers
- `TreeView`: Hierarchical vault organization
- `ListView`: Item list with custom `IListDataSource` implementation
- `MenuBarv2`: Top menu bar
- `StatusBar`: Bottom status/command bar
- `SpinnerView`: Loading animation
- Layout system: `Pos` and `Dim` for dynamic positioning

### Data Flow Example: Creating a Login Item

```
User presses F1 → MainView.HandleCreateKeypress()
    ↓
Create empty VaultItem with ItemType=1 (Login)
    ↓
MainView.ShowDetailForm(VaultItemSaveAction.Create)
    ↓
LoginDetailView shown modally
    ↓
User fills Login properties, clicks OK (_okPressed = true)
    ↓
MainView.RunSaveItemWorker(items, VaultItemSaveAction.Create)
    ↓
SaveItemWorker.Run() creates WorkingDialog
    ↓
BackgroundWorker executes: _repository.CreateVaultItem(encodedItem)
    ↓
VaultRepository wraps item as Base64 JSON, calls: bw create item <json>
    ↓
CLI returns result, worker completes
    ↓
MainView adds to _vaultItems dictionary, reloads ListView
```

## Configuration

**appsettings.json**: Stores CLI executable location
```json
{
    "Retrowarden": {
        "CLI_Location": ""
    }
}
```

Managed by `ConfigurationManager` singleton (`Config/ConfigurationManager.cs`).

## Important Implementation Notes

### State Management in MainView
```csharp
IVaultRepository _vaultRepository;                    // Repository instance
SortedDictionary<string, VaultItem> _vaultItems;     // All vault items
List<VaultFolder> _folders;                           // Folders
List<VaultCollection> _collections;                   // Collections
List<Organization> _organizations;                    // Organizations
ITreeNode? _selectedNode;                             // Current TreeView selection
string? _currentOrg;                                  // Active organization ID
bool _boomerMode;                                     // Disable mouse input
bool _debugMode;                                      // Use mock repository
bool _keepAlive;                                      // Auto-lock timeout enabled
int _sortColumn;                                      // Current sort column
```

### Custom Data Source Pattern
`ItemListDataSource` implements Terminal.GUI's `IListDataSource` interface for virtual ListView binding. Handles marked rows with BitArray for multi-select operations.

### Partial Class Split
MainView uses partial class pattern:
- `MainView.cs`: Logic and event handlers
- `MainView.Designer.cs`: UI component initialization

### VaultItem Model Structure
All vault items use a discriminated union pattern with `ItemType` enum:
- 1 = Login (has Login object)
- 2 = SecureNote (has SecureNote object)
- 3 = Card (has Card object)
- 4 = Identity (has Identity object)

Each type has nullable type-specific properties plus shared properties (Name, Notes, Favorite, FolderId, CollectionIds, CustomFields, Attachments).

## File Organization

```
RetroWarden/
├── Config/              # ConfigurationManager, RetrowardenConfig
├── Controls/            # CustomFieldScrollView, UriScrollView
├── Dialogs/             # 15 dialog implementations + BaseDialog
├── Utils/               # ViewUtils, ItemListDataSource, NodeData, NodeType
├── Views/               # MainView + 4 ItemDetailView implementations
├── Workers/             # RetrowardenWorker + 13 worker implementations
└── Program.cs           # Application entry point

RetrowardenSDK/
├── Models/              # 18 model classes (VaultItem, Login, Card, etc.)
└── Repositories/        # IVaultRepository + 2 implementations
```

## Bitwarden CLI Integration

The `VaultRepository` class wraps the Bitwarden CLI using:
- `System.Diagnostics.Process` to spawn `bw.exe` process
- Environment variables to pass session keys
- JSON parsing of CLI stdout responses
- Base64 encoding for create/update operations

Example CLI commands executed:
```bash
bw login [email] [password]
bw unlock [password]
bw list items
bw create item <base64-json>
bw edit item <id> <base64-json>
bw delete item <id>
bw sync
```

Session management via `BW_SESSION` environment variable passed to child processes.
