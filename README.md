# Mini Rogue

Roguelike 2D de acción top-down hecho en **Unity 6** (6000.4.6f1) con C#, desarrollado como proyecto para el parcial de **Programación 1 (UTN)**.

🎮 **Jugalo en itch.io:** _(link a itch.io)_

## Características

- Salas con lógica de puertas y zonas de cámara
- Varios tipos de enemigos y un jefe final
- Sistema de stats del jugador (vida, daño, velocidad, cadencia, rango, velocidad de disparo)
- Ítems que modifican stats (definidos con ScriptableObjects)
- Máquina de apuestas para obtener ítems
- Menú principal, UI y manejo de audio

## Estructura del código

```
Assets/Scripts/
├── Player/    # Movimiento, combate y stats del jugador
├── Enemies/   # Enemigo base, variantes, jefe y proyectiles
├── Rooms/     # Salas, sala del jefe, puertas y cámara
├── Objects/   # Ítems, portal y máquina de apuestas
├── AudioManager.cs
├── UIManager.cs
├── Menu.cs
└── Bullet.cs
```

Conceptos de POO aplicados: herencia (`BaseEnemy`, `BaseRoom`, `BaseObject`), interfaces (`IDamageDealer`, `ICameraZone`), encapsulamiento de stats y ScriptableObjects para datos.

## Cómo abrirlo

1. Clonar el repositorio.
2. Abrirlo desde **Unity Hub** con Unity **6000.4.6f1** (o compatible).
3. Abrir la escena `Assets/Scenes/Menu.unity` y darle Play.

## Autor

Pablo Basile
