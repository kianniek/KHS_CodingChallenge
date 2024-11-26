# Equipment System made as a coding challenge for **Knuckle Head Studios**

> [Play on Itch.io](https://kosmo-gd.itch.io/khs-codingchallenge)

## Overview
This repository contains a Unity-based equipment system developed using C# for **Knuckle Head Studios**. The system allows players to equip and interact with various items, including weapons, static objects, and wearable items. The equipment system demonstrates how items can be handled, equipped, and used within a player's hands and head.

## Features
- **Gun**: A gun that can be equipped and fired in both single and full automatic modes. It can hold an ammo-clip which contains bullets to be shot when the gun is used.
- **Ammo-clip**: The ammo-clip contains bullets and can be equipped in one hand to reload the gun in the other hand, consuming bullets from the clip.
- **Flashlight**: A flashlight that can be equipped and toggled on or off.
- **Rock**: A rock that can be equipped and thrown, which will then be unequipped after use.
- **Hat**: A hat that can be equipped to the player’s head. This item cannot be used interactively.
- **Bonus Task**: Implemented a system to interact with objects that cannot be equipped, such as opening doors, pressing buttons, turning keys, and switching levers.

## Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/KnuckleHeadStudios/EquipmentSystem.git
   ```

2. Open the project in Unity.

3. Ensure that the project has the necessary Unity version to run the equipment system (Unity 2020.3 or newer is recommended).

4. Play the scene to test the equipment system.

## Usage

- **Equipping Items**: Players can equip items in their hands or head using the in-game inventory system.
- **Gun**: Equip the gun in one hand and the ammo-clip in the other to reload. Press the designated button to fire the gun in either single or full automatic mode.
- **Ammo-clip**: Equip the ammo-clip to reload the gun. The bullets in the clip will be consumed during the reloading process.
- **Flashlight**: Equip the flashlight to toggle its state (on/off).
- **Rock**: Equip the rock and press the interaction button to throw it. The rock will be unequipped once thrown.
- **Hat**: Equip the hat to wear it on the player’s head. This item cannot be used interactively.

## Bonus Features

- **Object Interaction**: The player can interact with various non-equippable objects in the environment. For example:
  - **Doors**: Open or close doors using the interaction system.
  - **Levers**: Switch levers on or off.
  - **Keys**: Turn keys in locks.
  - **Buttons**: Press buttons to trigger actions.

## Project Structure

Here is a simplified view of the project structure:

```
├───Animations                # Animator controllers and animation assets
├───Materials                 # Materials for objects (weapons, environment, etc.)
├───Misc                       # Miscellaneous resources (fonts, shaders, etc.)
├───Models                     # 3D models for the game (weapons, flashlight, etc.)
├───Prefabs                    # Prefabs for effects, interactables, and UI elements
├───Scenes                     # Unity scenes to test and demonstrate the system
├───Script                     # C# scripts for gameplay mechanics, equipment system, etc.
├───Settings                   # Unity settings files (input, quality, etc.)
├───Sounds                     # Audio files for various in-game sounds
├───Sprites                    # 2D sprites for effects (muzzle flash, smoke, etc.)
└───TextMesh Pro               # TextMesh Pro resources (fonts, materials, shaders)
```
## Requirements

- Unity 2020.3 or newer
- C# scripting knowledge for customization or enhancement of the system

## Acknowledgements

- **Knuckle Head Studios** for supporting this equipment system project.
- Unity Technologies for providing Unity and making game development accessible.
- C# programming community for inspiration and solutions.
