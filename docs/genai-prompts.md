
---

## 4) `docs/genai-prompts.md` (FULL)

```md
# GenAI Prompt Engineering Notes

This document describes how I use GenAI tools (e.g., ChatGPT, Copilot-style assistants) **responsibly** to improve development productivity.

> Guiding principles:
> - No proprietary or confidential code/data is shared.
> - Generated output is treated as a draft.
> - Everything is reviewed, tested, and adjusted manually.

---

## Where GenAI helps in this project

### 1) Architecture and design reasoning
**Goal:** validate the Clean Architecture boundaries and dependency direction.

Example prompt:
- "Given a WPF MVVM app with Clean Architecture, which layer should contain validation and use-cases, and which layer should implement EF Core repositories?"

Expected output:
- confirmation of boundaries + suggested folder structure + dependency rules

---

### 2) MVVM patterns and binding pitfalls
**Goal:** identify lifecycle problems and prevent runtime crashes.

Example prompt:
- "In WPF, why can a computed property like `IsNew => Model.Id <= 0` crash during view load, and how can I make it safe?"

Expected output:
- explanation of early binding evaluation + null-safe pattern suggestions + `PropertyChanged` reminders

---

### 3) Automated test generation (unit + integration)
**Goal:** speed up writing test cases and edge cases.

Example prompts:
- "Generate NUnit unit tests for a ViewModel property `IsNew` where Model can be null, Id=0, Id>0."
- "Generate an EF Core integration test for a repository using SQLite in-memory."

Expected output:
- test skeletons which are then adapted to the codebase

---

### 4) Refactoring and code quality
**Goal:** improve readability, reduce duplication, and align with clean code.

Example prompt:
- "Refactor this repository method to reduce duplication and improve error handling. Keep it simple and testable."

Expected output:
- suggested refactor options + trade-offs, which are then applied selectively

---

### 5) Documentation drafts
**Goal:** produce high-quality README and docs quickly, then refine them.

Example prompt:
- "Write a concise README section explaining Clean Architecture for a WPF MVVM demo project."

Expected output:
- documentation draft which is reviewed and edited for accuracy

---

## Safe usage checklist
Before using GenAI output, I confirm:
- [ ] No customer-specific identifiers are present
- [ ] The code compiles and tests pass
- [ ] Naming and architecture match the repo
- [ ] No security-sensitive changes were introduced unintentionally
- [ ] The final output is reviewed and understood

---

## What I do NOT use GenAI for
- copying/pasting proprietary code or confidential documents
- generating production secrets or unsafe security code
- making unchecked changes to critical logic without tests
