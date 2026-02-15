# WPF Device Inventory — Clean Architecture + MVVM (net8.0)

A small but production-style **WPF (.NET 8)** desktop application that demonstrates:

-  **WPF + MVVM** (state-safe ViewModels, clean separation)
-  **Clean Architecture** (Domain / Application / Infrastructure / Presentation)
-  **Entity Framework Core + SQL-ready design** (default: SQLite, SQL Server supported)
-  **Automated tests** (Unit + Integration)
-  **CI-ready build** with GitHub Actions (restore → build → test)
-  Practical **GenAI prompt engineering** notes for modern development workflows

This repository was created as a showcase project for **Senior C#/.NET / WPF Software Developer** roles.

---

## Architecture (Clean Architecture)

### Why this matters
- The **Domain** stays independent of UI and persistence
- The **Application** contains use cases and abstractions (interfaces)
- The **Infrastructure** implements EF Core repositories + persistence
- The **Presentation.Wpf** contains WPF UI + ViewModels (MVVM)

More details: see [`docs/architecture.md`](docs/architecture.md)

---

## Features
- Device CRUD (create, update, delete)
- Search/filter-ready list UI
- MVVM ViewModels with **null-safe computed properties** (prevents binding lifecycle crashes)
- EF Core persistence with repository abstraction
- Unit tests for Domain logic
- Integration tests for repository behavior (SQLite in-memory)

---

## Tech Stack
- .NET 8 (WPF)
- C#, WPF, XAML
- MVVM: CommunityToolkit.Mvvm
- Entity Framework Core (SQLite default, SQL Server provider included)
- NUnit (Unit tests + Integration tests)
- GitHub Actions CI

---

## Getting Started

### Requirements
- Windows (WPF)
- .NET SDK
- Visual Studio 2022 (recommended)

> Note: This repo targets **net8.0** and **net8.0-windows**.  
> Newer SDKs (e.g., 9.x) can still build it, but using the .NET 8 SDK is recommended for consistency.

### Build the solution
From the repository root (where `WpfDeviceInventory.sln` is located):

```bash
dotnet restore WpfDeviceInventory.sln
dotnet build WpfDeviceInventory.sln -c Release
```

### Run the app

```bash
dotnet run --project src/Presentation.Wpf -c Release
```

### Testing 

#### Run all tests
```bash 
dotnet test WpfDeviceInventory.sln -c Release
```
#### Testing strategy
- UnitTests validate Domain/Application-level logic (portable net8.0)
- IntegrationTests validate Infrastructure behavior using EF Core (SQLite in-memory, net8.0)

### CI (GitHub Actions)
A CI workflow is included here:
- .github/workflows/ci.yml

It runs on Windows (required for WPF) and executes:
- dotnet restore
- dotnet build
- dotnet test

### Documentation
- Architecture: [`docs/architecture.md`](docs/architecture.md)
- GenAI prompt engineering notes: [`docs/genai-prompts.md`](docs/genai-prompts.md)
- Roadmap (planned commit history): [`docs/roadmap.md`](docs/roadmap.md)

### License
MIT — see [`LICENSE`](LICENSE)

---
## Repository Structure

```text
/src
  /Domain
  /Application
  /Infrastructure
  /Presentation.Wpf
/tests
  /UnitTests
  /IntegrationTests
/docs
  architecture.md
  genai-prompts.md
  roadmap.md
```