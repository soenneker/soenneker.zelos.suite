[![](https://img.shields.io/nuget/v/soenneker.zelos.suite.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zelos.suite/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.zelos.suite/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.zelos.suite/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.zelos.suite.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.zelos.suite/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Zelos.Suite
### The file-based JSON document database engine

---

# What is Zelos?

Zelos is a lightweight, embedded data store for .NET applications. It leverages JSON for persistent storage while maintaining an in-memory cache for rapid data access. With asynchronous, thread-safe operations, Zelos minimizes I/O overhead by periodically saving only modified containers.

---

## Key Benefits & Features

- **Embedded & Minimalistic:** Operates without the overhead of an external database server.
- **Responsive Asynchronous I/O:** Uses async operations to keep your application fast and non-blocking.
- **Robust Concurrency:** Employs asynchronous locks and semaphores to manage thread safety.
- **Container Organization:** Segregate your data logically using named containers.
- **Efficient Persistence:** Automatically saves only when necessary to reduce disk IO.
- **Resource Management:** Designed to automatically dispose resources when no longer needed, ensuring clean shutdowns.
- **Repository Pattern Integration:** Streamlines CRUD operations with a familiar design pattern.

---

## Architecture & Components

### `ZelosDatabase`

- **Role:** Core engine managing the persistence layer.
- **Key Aspects:**
  - Loads or creates the database file on startup.
  - Periodically saves dirty containers.
  - Manages container initialization and data consistency.

### `ZelosContainer` & `IZelosContainerUtil`

- **ZelosContainer:**
  - Represents an in-memory container for key/value pairs (stored as JSON).
  - Handles adding, updating, and deleting items, marking itself as “dirty” for periodic saves.
  
- **IZelosContainerUtil:**
  - **Usage:** Retrieves containers by combining the database file path and container name.
  - **Caching & Creation:** Manages container lifecycle, ensuring each container is only loaded once.

### `ZelosRepository`

- **Role:** Provides a higher-level abstraction using the repository pattern.
- **Key Aspects:**
  - Exposes common CRUD operations for generic document types.
  - Uses `IZelosContainerUtil` internally to obtain the right container.
  - Integrates logging and configuration for detailed diagnostics.

---

## Installation

```
dotnet add package Soenneker.Zelos.Suite
```

and then register it:

```csharp
services.AddZelosSuiteAsSingleton();
```
---

# Zelos User Management Example

This guide demonstrates how to build a simple user management system using Zelos. The example includes:

- **UserDocument**: A data model representing a user.
- **UsersRepository**: Inherits from `ZelosRepository<UserDocument>`.
- **UserManager**: Injects the repository to perform CRUD operations.

---

## Step 1: Define the User Document

Create a `UserDocument` class that represents the user data model. This class should inherit from your base `Document` class.

```csharp
using Soenneker.Documents.Document;

public class UserDocument : Document
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```
---

## Step 2: Create the Users Repository

Create an interface (`IUsersRepository`) for user-specific operations. Then, implement this interface in a class `UsersRepository` that inherits from `ZelosRepository<UserDocument>`. The repository will set the database file path and container name (in this case, `"users"`).

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Zelos.Repository;
using Soenneker.Zelos.Container.Util.Abstract;

public class UsersRepository : IZelosRepository<UserDocument>
{
    public UsersRepository(IConfiguration configuration, ILogger<UsersRepository> logger, 
        IZelosContainerUtil containerUtil) : base(configuration, logger, containerUtil)
    {
        // Set the database file path and container name
        DatabaseFilePath = "data/zelos.db";
        ContainerName = "users";
    }
}
```

---

## Step 3: Create the User Manager

Create a `UserManager` class that injects `UsersRepository` to perform operations. In this class, add a new user, update it, and then delete it.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class UserManager
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UserManager> _logger;

    public UserManager(IUsersRepository usersRepository, ILogger<UserManager> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        // Create a new user document.
        var user = new UserDocument
        {
            Id = Guid.NewGuid().ToString(),
            Name = "John Doe",
            Email = "john@example.com"
        };

        _logger.LogInformation("Adding user with Id: {UserId}", user.Id);
        await _usersRepository.AddItem(user, cancellationToken);

        // Update the user.
        user.Email = "john.doe@example.com";
        _logger.LogInformation("Updating user with Id: {UserId}", user.Id);
        await _usersRepository.UpdateItem(user, cancellationToken);

        // Delete the user.
        _logger.LogInformation("Deleting user with Id: {UserId}", user.Id);
        await _usersRepository.DeleteItem(user.Id, cancellationToken);
    }
}
```