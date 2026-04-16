# RITA: Nuclear Option Russian Voice Warning System
A Nuclear Option mod that replaces the English Voice Warning System with Russian equivalents. Voice lines sourced from War Thunder.

Updated for **Nuclear Option 0.33** | **[Download RITA RVWS here](https://github.com/muhimin01/no-rita-rvws/releases/latest)**

[![Video Demonstration](https://markdown-videos-api.jorgenkh.no/youtube/6ttqqmxeVXk)](https://www.youtube.com/watch?v=6ttqqmxeVXk)

## Installation Methods

### 1. BepInEx method

#### Dependencies
[BepInEx](https://github.com/BepInEx/BepInEx)   	

#### Conflicts
Any mods that replace any Voice Warning System lines.

#### Installation
1. Install [BepInEx](https://github.com/BepInEx/BepInEx) if you haven't already.
2. Place the `RITA_RVWS` folder from the mod into your BepInEx `plugins` folder.

#### Uninstallation
Delete the `RITA_RVWS` folder from your BepInEx `plugins` folder.

---

### 2. Asset replacement method

#### Dependencies
None

#### Conflicts
Any mods that replace `resources.assets`

#### Installation
1. Go to your Nuclear Option game directory, and open the `NuclearOption_Data` folder.
2. Create a backup of your original `resource.assets` file found within the `NuclearOption_Data` folder.
3. From the mod folder, place both `resource.assets` and `RITA_RVWS.resource` into the `NuclearOption_Data` folder, and replace relevant files.

#### Uninstallation
1. Replace the modded `resource.assets` file with the original backup.
2. Delete `RITA_RVWS.resource`.

## DISCLAIMERS
- Some voice lines were generated using AI to accommodate for Nuclear Option specific warnings.
- I am not a native Russian speaker. My Russian proficiency is currently very limited. For any Russian speakers or those who are much more familiar with Russian aircraft Voice Warning Systems, you are welcome to provide any corrections or suggestions.

## Credits
War Thunder - Original voice lines

## Included voice lines | RITA RVWS 1.1.0 | NO 0.33
| English | Русский |
| ----------- | ----------- |
| Eject | Катапультируйся |
| Engine Fire | Пожар двигателя |
| Engine 1 Fire | Пожар первого двигателя
| Engine 2 Fire | Пожар второго двигателя
| Engine 3 Fire	| Пожар третьего двигателя
| Engine 4 Fire	| Пожар четвёртого двигателя
| Front Left Engine Failure | Отказ переднего левого двигателя |
| Front Right Engine Failure | Отказ переднего правого двигателя |
| Left Engine Failure | Отказ левого двигателя |
| Left Engine Fire | Пожар левого двигателя |
| Left Fan Failure | Отказ левого винта |
| Left Prop Strike | Отказ левого винта |
| Lower Gear | Выпусти шасси |
| Main Rotor Damage | Отказ несущего винта |
| Overspeed | Скорость предельная |
| Raise Gear | Проверь шасси |
| Rear Left Engine Failure | Отказ заднего левого двигателя |
| Rear Right Engine Failure | Отказ заднего правого двигателя |
| Right Engine Failure | Отказ правого двигателя |
| Right Engine Fire | Пожар правого двигателя |
| Right Fan Failure | Отказ правого винта |
| Right Prop Strike | Отказ правого винта |
| Stall | Предельный угол атаки |
| Tail Rotor Failure | Отказ хвостового винта |
| VRS | СВК |
