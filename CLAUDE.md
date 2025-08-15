# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Retrowarden is a terminal-based Bitwarden client built with C# and .NET 8, providing a retro 1985-style interface using Terminal.GUI. The project consists of two main components:

- **RetroWarden** - The main Terminal.GUI application with UI components, dialogs, and background workers
- **RetrowardenSDK** - Core SDK providing models and repository interfaces for vault operations

## Build and Development Commands

```bash
# Build the entire solution
dotnet build

# Run the application in debug mode
dotnet run --project RetroWarden

# Build specific projects
dotnet build RetroWarden/RetroWarden.csproj
dotnet build RetrowardenSDK/RetrowardenSDK.csproj

# Clean build artifacts
dotnet clean
```

## Architecture Overview

### Repository Pattern
The codebase uses the repository pattern with `IVaultRepository` as the core interface (`RetrowardenSDK/Repositories/IVaultRepository.cs`). This interface defines all vault operations including:
- Authentication (Login, Logout, Unlock, Lock)
- CRUD operations for vault items, folders, collections
- Password/passphrase generation
- Send functionality for secure sharing
- Organization management

### Background Worker Pattern
UI operations that interact with the Bitwarden CLI are handled asynchronously using a consistent worker pattern:
- Base class: `RetrowardenWorker` (`RetroWarden/Workers/RetrowardenWorker.cs`)
- All workers inherit from this base and use `BackgroundWorker` with progress dialogs
- Examples: `LoginWorker`, `GetItemsWorker`, `SaveItemWorker`

### View Architecture
- **MainView** (`RetroWarden/Views/MainView.cs`) - Primary application window with tree navigation and item list
- **Detail Views** - Specific item type views (LoginDetailView, CardDetailView, etc.)
- **Dialogs** - Modal dialogs inherit from `BaseDialog` for consistent behavior
- **Controls** - Custom Terminal.GUI controls like `CustomFieldScrollView`

### Configuration Management
- Static `ConfigurationManager` class loads settings from `appsettings.json`
- `RetrowardenConfig` model defines configuration structure
- Configuration loaded once at startup and accessed statically

### Data Models
All data models are in `RetrowardenSDK/Models/`:
- `VaultItem` - Base class for all vault items (Login, Card, Identity, SecureNote)
- `VaultFolder`, `VaultCollection`, `Organization` - Organizational structures
- `Send` - Secure sharing functionality with `SendText` and `SendFile` variants

## Key Development Patterns

### Dependency Injection
The main application creates an `IVaultRepository` instance (debug or production) and passes it to views and workers. This allows for easy testing and swapping between implementations.

### Error Handling
Repository operations set `ExitCode` and `ErrorMessage` properties. UI components check these after operations and display appropriate error dialogs.

### State Management
- `MainView` maintains vault state (_vaultItems, _folders, _collections, _organizations)
- Tree view selection drives the item list display
- Sort state is maintained per column with ascending/descending toggles

### Terminal.GUI Integration
- Uses Terminal.GUI v2.0 pre-alpha with custom styling
- Views implement partial classes with designer-generated UI layout
- Custom controls extend Terminal.GUI base controls for specific functionality

## Important Files

- `RetroWarden/Program.cs` - Application entry point
- `RetroWarden/Views/MainView.cs` - Primary UI controller
- `RetrowardenSDK/Repositories/IVaultRepository.cs` - Core repository interface
- `RetroWarden/Config/ConfigurationManager.cs` - Static configuration management
- `TODO.txt` - Current development tasks and completed features

## Development Notes

The project targets .NET 8 with nullable reference types enabled. The Terminal.GUI dependency uses a pre-alpha version with some local references, indicating active development against the latest Terminal.GUI features.

File headers indicate GPL v3 licensing. The codebase follows C# naming conventions with comprehensive XML documentation comments on public interfaces.