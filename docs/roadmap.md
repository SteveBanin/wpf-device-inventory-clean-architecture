# Roadmap / Planned commit history 


## Phase 0 — repo + baseline
1. **chore: initial repository structure**
   - add solution file, `/src`, `/tests`, `/docs`, `.github/workflows` folders
2. **docs: add initial README and project goal**
   - include architecture + feature overview

## Phase 1 — Clean Architecture foundations
3. **feat(domain): add Device entity**
4. **feat(app): add repository abstraction and basic use-cases**
   - `IDeviceRepository`, `GetDevices`, `CreateDevice`, etc.
5. **feat(infra): add EF Core DbContext and repository implementation**
   - SQLite default, SQL Server provider added

## Phase 2 — WPF + MVVM UI
6. **feat(wpf): add MVVM toolkit and base ViewModels**
7. **feat(wpf): add device list + detail views**
8. **fix(wpf): prevent null-model binding crashes in ViewModels**
   - add null-safe computed properties + notifications (“senior” signal)

## Phase 3 — Testing
9. **test(unit): add ViewModel unit tests (NUnit)**
10. **test(integration): add repository integration tests**
11. **chore: add test data helpers and improve assertions**

## Phase 4 — CI + polish
12. **ci: add GitHub Actions build and test workflow**
13. **docs: add architecture and genai prompt notes**
14. **refactor: clean naming, remove warnings, enable nullable**
15. **chore: add screenshots and final README improvements**

---


