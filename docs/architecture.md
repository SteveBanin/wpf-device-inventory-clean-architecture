
---

## 3) `docs/architecture.md` (FULL)

```md
# Architecture

This project demonstrates a **Clean Architecture** approach combined with **WPF + MVVM**.  
The goal is to keep the business logic independent from UI and persistence, while keeping the WPF application maintainable, testable, and scalable.

---

## High-level design (Clean Architecture)

### Layers
- **Domain**: core entities and rules (no dependencies on UI or persistence)
- **Application**: use cases, validation, and abstractions (interfaces)
- **Infrastructure**: EF Core DbContext + repository implementations (SQLite default, SQL Server-ready)
- **Presentation.Wpf**: WPF UI, ViewModels (MVVM), and UI-specific state management

### Dependency rule
Dependencies always point inward:
- Presentation → Application → Domain
- Infrastructure → Application → Domain  
Domain never depends on anything else.

---

## Folder structure

```text
/src
  /Domain
  /Application
  /Infrastructure
  /Presentation.Wpf
/tests
  /UnitTests
  /IntegrationTests
