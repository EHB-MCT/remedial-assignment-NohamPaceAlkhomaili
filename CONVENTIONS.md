# Project Conventions

This document defines the coding, commit, naming, and organizational conventions for the Unity Firebase Game project. Following these standards ensures code quality, consistency, and ease of collaboration for all contributors.

---

## 1. Commit Message Conventions

We use the **Conventional Commits 1.0.0** specification.

- Commit messages **MUST** start with a type:
  - `feat:` for new features (e.g. `feat(auth): add Firebase authentication`)
  - `fix:` for bug fixes (e.g. `fix(economy): resolve negative money bug`)
  - Other allowed types: `chore:`, `docs:`, `refactor:`, `test:`, `style:`, `build:`, `ci:`, etc.
  - **Scope** should specify the affected module, in parentheses if relevant, e.g. `feat(blocks):`
- Breaking changes **MUST** use `!` after the type/scope, or the `BREAKING CHANGE:` footer.
- Bodies and footers may be used for extended explanations and references.
- Single changes per commit are preferred (multiple changes should be split).
- Example: feat(firebase): add Firestore saving for player assets
Added dynamic saving of player money, blocks, and upgrades to Firestore.

---

## 2. File and Folder Naming

- Use `PascalCase` for folder names and C# scripts (`BlockManager.cs`, `MoneyManager.cs`).
- Scene files use `CamelCase` or descriptive names (e.g. `MainScene.unity`).
- Prefab names match their main components and are descriptive (`BlockSphereUI`, `BlockCapsuleUI`).
- Scripts are organized in folders:
- `Scripts/Managers/` for manager classes
- `Scripts/Game/` for gameplay scripts
- `Scripts/Helpers/` for utility and dispatcher classes
- `Scripts/Data/` for data models (e.g. `PlayerData.cs`)

---

## 3. Code Structure and Logic

### C#

- Use `PascalCase` for class names and `camelCase` for variables/methods.
- Always use access modifiers (e.g. `public`, `private`) explicitly.
- Group related classes in namespaces where appropriate.
- Keep components and logic separated (e.g., `MoneyManager` for money, `BlockManager` for blocks).
- Use dependency injection or singleton pattern only when justified (`MoneyManager.Instance`, `FirebaseSaver.Instance`).
- Public API surface should be clean and documented.
- Static properties/fields for globally accessed managers.
- All events and actions are named with clear intent: (`OnUserConnected`, `RestoreMoney`)
- Use [Serializable] for data models sent to Firebase (`PlayerData`).

### Prefabs & UI

- Prefabs are named for their function and type (`BlockSphereUI`, `UnitPrefab`).
- UI objects linked in scripts by descriptive field names (`moneyText`, `buyButton`).

### Firebase

- Firestore collections and document names are lowercased and plural where appropriate (`players`, not `Player` or `Players`)
- Player data is serialized/deserialized via the `PlayerData` class.

---

## 4. Git Branching

- Main development occurs on the `main` branch.
- Features and fixes are developed on branches named by type and feature, e.g. `feat/firebase-auth`, `fix/block-logic`.
- Documentation is added via branches like `docs/code-of-conduct`.
- Pull requests reference the related issue and summarize changes.

---

## 5. Documentation

- Every new feature or module is documented in the README, and if needed, a separate markdown file.
- Files like `CODE_OF_CONDUCT.md`, `CONTRIBUTING.md`, and `CONVENTION.md` are maintained in the project root.

---

## 6. General Practices

- Always review and test code before merging.
- Leave meaningful comments only where the intention is not obvious—aim for clear, self-explanatory logic.
- Follow DRY (Don't Repeat Yourself) and KISS (Keep It Simple, Stupid) principles.
- If you add assets (images, models, files) give meaningful, lowercased names separated by hyphens or underscores if needed.

---

## 7. Naming Used in the Project

**Managers:**  
`MoneyManager`, `AuthManager`, `FirebaseSaver`, `FirestoreManager`

**Data Model:**  
`PlayerData`

**Enums:**  
`BlockType` (members: `TypeA`, `TypeB`, ...)

**Prefabs/Objects:**  
`unitPrefab`, `BlockSphereUI`, `BlockCapsuleUI`

**UI fields:**  
`moneyText`, `buyButton`, `upgradeButton`, `infoText`, `incomeText`

**Methods/Events:**  
`OnUserConnected`, `SpendMoney`, `RestoreMoney`, `SavePlayerData`, `RestoreSessionFromSnapshot`, `UpdatePlayerStats`, `FillPlayerData`

---

## 8. Logic Standards

- Every important action (buy, upgrade, save, restore) calls a manager or handles updates via a well-named method.
- Firestore saving/loading is always performed asynchronously and callbacks run through `UnityMainThreadDispatcher` as needed.
- Code interacts via methods and events, not direct field access.
- Data sent to Firebase is always encapsulated in a single structured object (`PlayerData`).

---

By following these conventions, all contributors will help maintain a scalable, organized, and maintainable Unity Firebase game project.

