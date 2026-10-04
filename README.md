# Afterimage-Game
A Unity 3D underwater exploration prototype built around a playable time-replay mechanic where the player's previous movement becomes a ghost diver that can interact with the world.

## Prototype overview
This repository contains a Unity-ready project skeleton and the core gameplay scripts needed for the prototype:

- PlayerController.cs
- AfterimageRecorder.cs
- AfterimagePlayback.cs
- AfterimageInteractable.cs
- PressurePlate.cs
- DoorController.cs
- GameManager.cs
- UIManager.cs
- UnderwaterPrototypeBootstrap.cs

## Gameplay loop
The prototype records the diver for 10 seconds, creates an Afterimage, then replays the saved motion. When the ghost reaches the pressure plate, the door unlocks and the player can pass through the puzzle room.

## How to use
1. Open the folder in Unity 2022.3 LTS or newer.
2. Add the generated scripts under Assets/Scripts.
3. Enter Play Mode.
4. Move with WASD, look with the mouse, ascend with Space, descend with Left Ctrl, and boost with Shift.
5. The prototype auto-generates the scene and puzzles when the scene starts.
