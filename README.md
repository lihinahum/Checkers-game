# Checkers Game 

A classic Checkers game implemented in C# with WinForms, featuring both two-player mode and an AI opponent.
The project was developed as part of an academic exercise to practice OOP, game logic, and GUI programming in C#.

## Features 
### Two Game Modes:
Player vs. Player (local two-player).
Player vs. Computer (AI with calculated moves, not random).

### Board Sizes: 6×6, 8×8, or 10×10.

### Core Rules Implemented:
Valid and invalid moves are enforced.
Mandatory captures rule supported.
Multiple captures in one turn.
Promotion to King when reaching the last row.
Win, lose, and draw detection.

### User Experience:
Players can choose their names before starting the game.
Available moves are highlighted after selecting a piece.
Invalid moves trigger clear error messages (e.g. “Invalid move: This is not your piece”, “Normal move not allowed: capture available”).
Visual feedback with coin and king graphics.

## Screenshots  
### Game Settings  
<img width="356" height="325" alt="Game Settings" src="https://github.com/user-attachments/assets/c450b212-6207-4932-957a-f80be37b9bee" />

### Gameplay  
<img width="427" height="617" alt="Gameplay" src="https://github.com/user-attachments/assets/60d876df-9856-4a89-96dd-94019f981afc" />

### Invalid Move Message  
<img width="489" height="184" alt="Invalid Move" src="https://github.com/user-attachments/assets/97b6c86d-4fcd-4ec6-b89f-241026602a1d" />

### King Promotion  
<img width="418" height="616" alt="King Promotion" src="https://github.com/user-attachments/assets/331213ca-6e5b-454b-968d-643a6a7ff28c" />

### Game Over  
<img width="355" height="177" alt="Game Over" src="https://github.com/user-attachments/assets/dd19639c-1eb6-435f-a8e8-67bf8752113d" />


# Technologies 
Language: C# 
Framework: .NET Framework / WinForms 
IDE: Visual Studio 2022 
Design Pattern: MVC-inspired structure 
Model: Game logic (Board, Move, Player, Piece, AI, enums). 
View: WinForms UI (GameBoard, Settings, Player forms). 
Controller: GameManager – coordinates between logic and UI. 

## How to Run 

Clone the repository:
git clone https://github.com/lihinahum/Checkers-game.git

Open Checkers game.sln in Visual Studio 2022.

Build and run the project (Ctrl+F5).

In the Game Settings window:
Choose board size (6×6, 8×8, 10×10).
Enter player names.
Optionally enable Computer as Player 2.

Play and enjoy!
