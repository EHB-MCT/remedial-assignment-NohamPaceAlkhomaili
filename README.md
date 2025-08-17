# Tycoon Game

## Logs

> **Important**: Due to technical issues (PC change), commit timestamps may not reflect the actual development timeline.  
> For accurate and full tracking, please refer to the Google Docs log:  
> [Google Docs – Tycoon Game Development Log](https://docs.google.com/document/d/1NElGcj0J5hjg0HIZlMkiOmj4OHkhI3ppi2XzSE5LUGs/edit?usp=sharing)

## Project Overview

This Unity project is a tycoon-style game where players place blocks that generate money automatically over time. The money can be used to purchase more blocks and upgrades to boost income.  
Player progress is associated with a username: upon first login, a new session is created and saved online (Firebase Firestore).  
When reconnecting with the same username, the previous game session is loaded with saved money, blocks owned, and upgrade levels.

## How to Run

1. Clone the repository:  
   [https://github.com/EHB-MCT/remedial-assignment-NohamPaceAlkhomaili.git](https://github.com/EHB-MCT/remedial-assignment-NohamPaceAlkhomaili.git)  
2. Open the project with Visual Studio Code if preferred, then add it via Unity Hub.  
3. Open Unity and load the main scene: `Tycoon.unity`  
4. Press Play to start the game.  
5. Enter a username to load an existing session or start a new game.

## Build With

- **Unity** (2021.3 LTS or later)  
- **Firebase Firestore** (for saving and loading player progress)  
- **Visual Studio Code** (script editing)

## Development Setup

- Simple authentication system based on a player username or ID.  
- Firebase must be configured with the project (place `google-services.json` for Android or `GoogleService-Info.plist` for iOS in the Assets folder).  
- Main Firebase-related scripts located in `Assets/Scripts`:  
    - `FirebaseSaver.cs` (saving/loading data)  
    - `FirebaseInitializer.cs` (Firebase SDK initialization)  
    - `PlayerLogin.cs` (player username input and session management)  
- Core gameplay logic managed by singleton managers such as `MoneyManager`

## Key Development Tasks & Scripts

- **Block Placement System**: Manage prefab blocks and income generation.  
  → `BlockManager.cs`  
- **Money & Income Simulation**: Track player’s money, update UI, calculate passive income.  
  → `MoneyManager.cs`  
- **Progress Persistence**:  
  - Create new player data on first launch.  
  - Restore saved sessions on login.  
  - Auto-save on application quit.  
  → `FirebaseSaver.cs`  
- **Firebase Initialization**: Dependency checks and setup.  
  → `FirebaseInitializer.cs`  
- **UI Management**: Username input, currency display, buttons for buying and upgrading.

## Visual Game Images

<img width="1019" height="561" alt="image" src="https://github.com/user-attachments/assets/27c29134-4b30-4b8e-b46e-c65859800ca0" />
<img width="1048" height="578" alt="image" src="https://github.com/user-attachments/assets/6d1b5a5d-8a98-4e1d-988c-44e12a656832" />


## Visual Database Images

<img width="2086" height="257" alt="image" src="https://github.com/user-attachments/assets/6c81e820-990d-423e-ad04-bac568dd9f81" />


## References

- AI Assistance: [ChatGPT](https://chatgpt.com/share/68a25d22-e0e8-8004-8362-b2158670a8b6)  
- AI Assistance: [Perplexity](https://www.perplexity.ai/search/j-ai-u-une-conversation-avec-c-SIkkQsxvQYiGygMFEIXP8Q)  
- Official Firebase Documentation: [Firebase Unity Docs](https://firebase.google.com/docs/)  
- Project inspired by tycoon game concepts for educational purposes.

## Contact

For any questions, contact me at:  
[Noham.pace.alkhomaili@student.ehb.be](mailto:Noham.pace.alkhomaili@student.ehb.be)
