================================================================================
                        SPACE SHOOTER – COMPLETE README
================================================================================

Space Shooter is a 2D arcade space shooter game built with C# and Windows Forms,
developed as the final project for the Advanced Programming course. It features
a full object-oriented architecture, a shop system, wave-based enemy progression,
power-ups, and persistent SQLite storage.

================================================================================
TABLE OF CONTENTS
================================================================================
1. INTRODUCTION
2. FEATURES
3. TECHNOLOGIES USED
4. PROJECT STRUCTURE
5. INSTALLATION & EXECUTION
6. CONTROLS GUIDE
7. ARCHITECTURE & DESIGN PRINCIPLES
8. DEVELOPER NOTES
9. DEVELOPERS
10. ACKNOWLEDGEMENTS
11. LICENSE

================================================================================
1. INTRODUCTION
================================================================================
Space Shooter is a classic "Shoot 'em Up" game. The player controls a spaceship
and must survive against waves of alien enemies. The game demonstrates:
- Object-Oriented Programming (Encapsulation, Inheritance, Polymorphism)
- Event-driven programming (Windows Forms)
- Database integration (SQLite)
- Separation of concerns (UI vs. Logic)
- GDI+ graphics rendering

================================================================================
2. FEATURES
================================================================================

2.1 GAMEPLAY
------------
- Smooth ship movement using W/A/S/D or Arrow Keys
- Continuous shooting with Space
- 4 enemy types:
    1. Standard    – moves straight down
    2. Scout       – sinusoidal (zigzag) movement
    3. Shooter     – slow movement + shoots downward
    4. HeavyTank   – high HP, 8-directional shooting
- Wave system: 10 waves with increasing difficulty
    * Speed multiplier: base_speed * (1 + 0.1 * wave)
    * HP bonus: base_hp + (2 * wave)
- 2 power-up types:
    1. Triple Shot – fires 3 bullets simultaneously (10 seconds)
    2. Health Pack – restores HP or grants an extra life
- Gold and Silver coins collected by direct contact with the ship
- Scoring system with persistent High Score storage
- Pause/Resume with P or Esc key

2.2 SHOP
--------
- Purchase cosmetic and upgrade items using collected coins
- Item categories: Skin, Bullet, Theme, Booster
- Equip owned items
- All purchases and equipped statuses are saved permanently in SQLite

2.3 PERSISTENT STORAGE
----------------------
- SQLite database (game_data.db) stores:
    * Total coins
    * High score
    * Purchased and equipped items
- Database is automatically created on first run

================================================================================
3. TECHNOLOGIES USED
================================================================================
- C# 10.0 / .NET 8.0
- Windows Forms (WinForms)
- SQLite (Microsoft.Data.Sqlite)
- GDI+ for graphics rendering
- Object-Oriented Programming principles
- Factory and Observer patterns

================================================================================
4. PROJECT STRUCTURE
================================================================================
SpaceShooter/
├── UI/
│   ├── MainForm.cs              # Main form (screen manager)
│   ├── MainForm.Designer.cs     # Designer file
│   ├── MenuControl.cs           # Main menu screen
│   ├── GameControl.cs           # Game screen
│   ├── ShopControl.cs           # Shop screen
│   ├── OptionsControl.cs        # Options screen
│   └── AboutControl.cs          # About screen
├── Managers/
│   ├── GameManager.cs           # Core game logic
│   ├── InputManager.cs          # User input handling
│   ├── CollisionManager.cs      # Collision detection
│   ├── WaveManager.cs           # Wave management
│   ├── ShopManager.cs           # Shop logic
│   ├── AudioManager.cs          # Audio playback (placeholder)
│   └── SpriteManager.cs         # Sprite generation & management
├── Data/
│   ├── DatabaseManager.cs       # SQLite operations
│   └── GameState.cs             # Current game state
├── Entities/
│   ├── GameObject.cs            # Base entity class
│   └── Player.cs                # Player class
├── Enemies/
│   ├── Enemy.cs                 # Base enemy class
│   ├── StandardEnemy.cs
│   ├── ScoutEnemy.cs
│   ├── ShooterEnemy.cs
│   └── HeavyTankEnemy.cs
├── Bullets/
│   ├── Bullet.cs                # Base bullet class
│   ├── PlayerBullet.cs
│   └── EnemyBullet.cs
├── PowerUps/
│   ├── PowerUp.cs               # Power-up class
│   ├── PowerUpType.cs           # Power-up enum
│   └── PowerUpFactory.cs        # Factory for creating power-ups
├── Models/
│   ├── PlayerSaveData.cs        # Saved data model
│   ├── ShopItem.cs              # Shop item model
│   └── WaveConfig.cs            # Wave configuration model
├── Program.cs                   # Application entry point
└── README.md / README.txt       # This file

================================================================================
5. INSTALLATION & EXECUTION
================================================================================

Prerequisites:
- .NET SDK 8.0 or later (https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022 (or VS Code with C# extension)
- Git (optional, for cloning)

Steps:

1. Clone the repository (or extract the ZIP file):
   git clone https://github.com/your-username/SpaceShooter.git
   cd SpaceShooter

2. Restore dependencies:
   dotnet restore

3. Build and run:
   dotnet run

   Alternatively, open the .sln file in Visual Studio and press F5.

Note: The SQLite database (game_data.db) is automatically created in the output
folder on first run. No manual database setup is required.

================================================================================
6. CONTROLS GUIDE
================================================================================
Key          Action
-------------------------------------------------------------------------------
W / Up       Move Up
S / Down     Move Down
A / Left     Move Left
D / Right    Move Right
Space        Shoot
P or Esc     Pause / Resume
Enter        Return to menu (when game over or victory)

================================================================================
7. ARCHITECTURE & DESIGN PRINCIPLES
================================================================================

7.1 Separation of Concerns
--------------------------
- UI layer (MainForm, Controls) handles display and user input only.
- Game logic (GameManager, WaveManager, CollisionManager) is completely
  decoupled from UI.
- Data layer (DatabaseManager, GameState) manages state and persistence
  independently.
- This follows the MVC pattern implicitly.

7.2 Object-Oriented Principles
------------------------------
- Encapsulation: Each class has a single, well-defined responsibility.
- Inheritance: All entities inherit from GameObject; enemies inherit from Enemy.
- Polymorphism: Enemies override Update() and Draw() to implement unique
  behaviors (e.g., ScoutEnemy's sinusoidal movement).

7.3 Design Patterns Used
------------------------
- Factory Pattern: Used for creating enemies and power-ups.
- Observer Pattern: Event-driven communication between controls
  (ReturnToMenu, PlayClicked, etc.).

7.4 Key Technical Decisions
---------------------------
- Using Timer for the game loop (20ms interval = 50 FPS) for simplicity and
  compatibility with WinForms.
- DoubleBuffered = true to eliminate flickering during rendering.
- Using List<Enemy> and List<Bullet> for dynamic entity management.
- Using SQLite instead of text files for data persistence as required by the
  project specification.

================================================================================
8. DEVELOPER NOTES
================================================================================

8.1 Adding a New Enemy
-----------------------
1. Create a new class inheriting from Enemy.
2. Implement Update() and Draw() with the specific behavior.
3. Add the new type to the WaveManager's enemy spawning logic.
4. Update WaveConfig to include the new enemy in wave definitions.

8.2 Adding a New Power-Up
--------------------------
1. Add a new value to the PowerUpType enum.
2. Implement the effect in GameManager.ApplyPowerUp().
3. Add the corresponding sprite in SpriteManager.
4. Add the power-up to the random drop list in the spawning logic.

8.3 Adjusting Game Constants
-----------------------------
All configurable values are currently spread across various classes.
For maintainability, consider centralizing them into a static GameConfig class.

8.4 Database Schema
-------------------
The SQLite database has two tables:
- PlayerData (Id, TotalCoins, HighScore, ExtraLifeOwned, EquippedSkin,
  EquippedBulletStyle, EquippedBackground)
- ShopItems (Id, ItemKey, ItemType, DisplayName, Price, Owned, Equipped)

The schema is created automatically on first run.

================================================================================
9. DEVELOPERS
================================================================================
- Taha Mousavi    – Advanced Programming Student  |  ID: 403414054
- Mahdi Farzaneh  – Advanced Programming Student  |  ID: 403413221

================================================================================
10. ACKNOWLEDGEMENTS
================================================================================
Special thanks to Dr. Marzieh Maleki Majd for her invaluable guidance and
excellent teaching throughout the semester.

================================================================================
11. LICENSE
================================================================================
This project is created for educational purposes only.
Use in similar projects is permitted with proper attribution.

================================================================================
                        END OF README
================================================================================
