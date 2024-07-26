# V;red - DE:HIVE 2023/24
![Header](https://github.com/Btegz/Vired/blob/13fdfd4549c4803b9cc13512fb4f62a690aa0fc7/3D%20Game/Assets/Sprites/Git/V%3BRED.png)



## Description 
V;red was developed as a third semester student project at the DE:HIVE. It presents a turn-based strategy game wherein players navigate three drones through a hazardous environment plagued by lethal parasites decimating the terrain. Players maneuver their drones across the world to collect resources, which are essential for using unique modules to combat parasites and revitalize the lands. As the players progress they can enhance their abilities. The game's distinctive appeal lies in its procedural world generation, offering a fresh experience with each playthrough, paired with the ability to modify every module.

<br />

## Procedural World
The procedural generation of the world involves the utilization of **'FastNoise Lite'** (https://github.com/Auburn/FastNoiseLite), an open-source noise generation library. The values used for the noise generation are provided by a Scriptable Object (SO) of the MapSettings class, which contains noise types, frequency, a tile count, and other parameters. These values are subsequently used for grid generation and resource distribution.
For each hexagonal **'GridTile'** of the predetermined amount specified in the **'MapSettings'**, the **'ProceduralTileInfo'** is an information container. Holding information such as coordinates and the two noise values, the container was used to assign a numerical value to each **'GridTile'** according to its predefined noises. According to this value, the resource distribution will then be determined.

<br />

The **'GridManager'** class is responsible for transferring data and generating the grid based on the active **'MapSettings'** SO, depending on the generated noises the grid will vary in its shape.

>![World](https://github.com/Btegz/Vired/blob/97e59d2684727033326f75f47df021ddc29f3190/3D%20Game/Assets/Sprites/Git/World1.png) 




<br />

## Hexagon Grid
The Class **'HexGridUtil'** provides functions with algorithms for operations in a hexagonal grid
<br />

### Most important functions

<br />

  - **CubeToAxialCoord:**     	  Conversion Cubic to Axial Coord
  >
  - **AxialToCubeCoord:**         Conversion Axial to Cubic Coord
  >
  - **CubeNeighbors:**            Function to return all six adjacent neighbors of a **'GridTile'**
  >
  - **CoordinatesReachable:**     Searches for every Coordinate reachable from "startingCoord" within "range"
  >
  - **Rotation:**                 Rotation around the center (60 degrees)
  >
  - **HeuristicPathfinding:**     Finds the shortest path between to coordinates
>
  <br />
  
  > ![Pathfinding](https://github.com/Btegz/Vired/blob/167b9b0ec111ec8ff99472bcbfd0f61d00c6c35b/3D%20Game/Assets/Sprites/Git/Pathfinding.png)
  **_Pathfinding_**

<br />

## Player character
The **'Playermanager'** class is responsible for overseeing various aspects of player behavior, including selection, movement, and the use of abilities. It operates by interpreting user inputs to control player actions.
>
For player movement, several conditions must be met. Firstly, the mouse position must be on the grid and the left mouse button must be clicked. Additionally, the player must have available movement points, which are represented as an integer value. The **'GridTile'** clicked by the player must be a neighbor of the tile the currently selected player is positioned on and the neighbor tile must be walkable, which includes **'GridTile'** with an assigned positive or neutral state. To determine the **'GridTile'** at the mouse position, the **'Playermanager'** uses a ray cast from the camera through the current mouse position. If the ray cast intersects with a tile and not with the UI, the coordinates of the **'GridTile'** are returned. This allows the system to identify which tile the player intends to interact with. When it comes to movement, the system checks whether the selected tile's coordinates match any of the neighboring tile coordinates. 
>
Furthermore, the player cannot be in the process of using an ability, as indicated by a boolean value, or in the state of crafting a new one, indicated by an enum
In terms of ability usage, the **'Playermanager'** class manages the selection and activation of abilities. When an ability is chosen, the class assesses the costs associated with its use and checks the player's resources to ensure they can afford it. It also handles the effects of the ability, including any potential cancelation procedures and the execution of the ability's effects, ensuring that the player's actions align with the game's mechanics and rules.


> ![Player](https://github.com/Btegz/Vired/blob/167b9b0ec111ec8ff99472bcbfd0f61d00c6c35b/3D%20Game/Assets/Sprites/Git/Player.png)
 **_Player Movement_**
>
> ![Ability](https://github.com/Btegz/Vired/blob/167b9b0ec111ec8ff99472bcbfd0f61d00c6c35b/3D%20Game/Assets/Sprites/Git/Ability.png)
 **_Ability Preview_**

