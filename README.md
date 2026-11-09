# Mini Rogue

A top-down 2D action roguelike made with **Unity 6** (6000.4.6f1) and C#, developed as a project for the **Programming 1** course at UTN (Universidad Tecnológica Nacional, Argentina).

🎮 **Play it on itch.io:** [pablocolo.itch.io/mini-rogue](https://pablocolo.itch.io/mini-rogue)

## Features

- Rooms with door logic and camera zones
- Several enemy types and a final boss
- Player stat system (health, damage, speed, fire rate, range, shot speed)
- Stat-modifying items (defined with ScriptableObjects)
- Gamble machine to obtain items
- Main menu, UI and audio management

## Code structure

```
Assets/Scripts/
├── Player/    # Player movement, combat and stats
├── Enemies/   # Base enemy, variants, boss and projectiles
├── Rooms/     # Rooms, boss room, doors and camera
├── Objects/   # Items, portal and gamble machine
├── AudioManager.cs
├── UIManager.cs
├── Menu.cs
└── Bullet.cs
```

OOP concepts applied: inheritance (`BaseEnemy`, `BaseRoom`, `BaseObject`), interfaces (`IDamageDealer`, `ICameraZone`), stat encapsulation and ScriptableObjects for data.

## How to open

1. Clone the repository.
2. Open it from **Unity Hub** with Unity **6000.4.6f1** (or a compatible version).
3. Open the `Assets/Scenes/Menu.unity` scene and press Play.

## Author

Pablo Basile
