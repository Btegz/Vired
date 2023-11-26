using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// The Class GexGridUtil provides functions with algorithms for Operations in a Hexagonal Grid.
/// </summary>
public static class HexGridUtil
{
    // GENERAL PURPOSE ------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Converts an cubic Coordinate into an Axial Coordinate. 
    /// </summary>
    /// <param name="coord">Coordinate to convert</param>
    /// <returns>converted coordinate</returns>
    public static Vector2Int CubeToAxialCoord(Vector3Int coord)
    {
        return new Vector2Int(coord.x, coord.y);
    }

    /// <summary>
    /// Converts a List of cubis Coordinates into a List of Axial Coordinates.
    /// </summary>
    /// <param name="coords">Coordinates to convert</param>
    /// <returns>List of Converted Coordinates.</returns>
    public static List<Vector2Int> CubeToAxialCoord(List<Vector3Int> coords)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Vector3Int c in coords)
        {
            result.Add(CubeToAxialCoord(c));
        }
        return result;
    }

    /// <summary>
    /// Converts an axial Coordinate into a cubic Coordinate.
    /// <br>This only used for Axial Coordinates. To convert between Offset and Axial, <see cref="GridToCubeCoord(Vector2Int)"/></br>
    /// </summary>
    /// <param name="coord">Coordinate to convert</param>
    /// <returns>Converted Coordinate</returns>
    public static Vector3Int AxialToCubeCoord(Vector2Int coord)
    {
        int z = -coord.x - coord.y;
        return new Vector3Int(coord.x, coord.y, z);
    }

    /// <summary>
    /// Converts a List of axial Coordinates into a List of cubic Coordinates
    /// </summary>
    /// <param name="coords">Coordinates to convert</param>
    /// <returns>Converted Coordinates</returns>
    public static List<Vector3Int> AxialToCubeCoord(List<Vector2Int> coords)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (Vector2Int c in coords)
        {
            result.Add(AxialToCubeCoord(c));
        }
        return result;
    }

    /// <summary>
    /// Converts a Offset-coordinate into a cubic Axial Coordinate. 
    /// <br>Where the Offset Coordinate is considered to be in a Grid with even offset rows</br>
    /// </summary>
    /// <param name="coord">Coordinate to convert</param>
    /// <returns>Converted Coordinate</returns>
    public static Vector3Int GridToCubeCoord(Vector2Int coord)
    {
        int x = coord.x;
        int y = coord.y - (coord.y + (coord.y & 1)) / 2;
        return new Vector3Int(x, y, -x - y);
    }

    /// <summary>
    /// Array of Direction Vectors for every Neighbor.
    /// <br>Used to find Neighbors of a tile <see cref="cubeNeighbor(Vector3Int, int)"/></br>
    /// </summary>
    public static Vector3Int[] cubeDirectionVectors = new Vector3Int[]{
        new Vector3Int(1,0,-1), new Vector3Int(1,-1,0), new Vector3Int(0,-1,1),
        new Vector3Int(-1,0,1), new Vector3Int(-1,1,0), new Vector3Int(0,1,-1)
    };

    /// <summary>
    /// Adds two Vectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Summ of Vectors "a" and "b"</returns>
    public static Vector3Int CubeAdd(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    /// <summary>
    /// Addas one Vector to every Vector in a List
    /// </summary>
    /// <param name="range">The List to be added</param>
    /// <param name="b">The Vector to add to every Vecotr in the list</param>
    /// <returns>List of the Summ of Vectors</returns>
    public static List<Vector3Int> CubeAddRange(List<Vector3Int> range, Vector3Int b)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (Vector3Int a in range)
        {
            result.Add(CubeAdd(a, b));
        }
        return result;
    }

    /// <summary>
    /// Substracts two Vectors.
    /// </summary>
    /// <param name="a">Minuent</param>
    /// <param name="b">Subtrahent</param>
    /// <returns>difference of "a" minus "b"</returns>
    public static Vector3Int CubeSubstract(Vector3Int a, Vector3Int b)
    {
        int resX = a.x - b.x;
        int resY = a.y - b.y;
        int resZ = a.z - b.z;
        return new Vector3Int(resX, resY, resZ);
    }

    // INTERACTING BETWEEN COORDS --------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Returns a List of every 6 Coordinate Vectors that are ajacent to given Coordinate Vector.
    /// <br>THIS DOES NOT TAKE IN ACCOUNT WHETHER THE COORDINATES HAVE A TILE OR NOT - beware of null reference Errors</br>
    /// </summary>
    /// <param name="coord">Coordinate to find Neighbors for</param>
    /// <returns>Vector3Int List of all 6 Neighbors</returns>
    public static List<Vector3Int> CubeNeighbors(Vector3Int coord)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        foreach (Vector3Int dir in cubeDirectionVectors)
        {
            neighbors.Add(CubeAdd(coord, dir));
        }
        return neighbors;
    }

    /// <summary>
    /// Array of Direction Vectors for every Diagonal.
    /// </summary>
    public static Vector3Int[] cubeDiagonalDirectionVectors = new Vector3Int[]
    {
        new Vector3Int(2,-1,-1),new Vector3Int(1,-2,1), new Vector3Int(-1,-1,2),
        new Vector3Int(-2,1,1), new Vector3Int(-1,2,-1),new Vector3Int(1,1,-2)
    };

    /// <summary>
    /// Returns a List of every 6 Corrdinate Vectors that are diagonally to given Coordinate Vector.
    /// <br>THIS DOES NOT TAKE IN ACCOUNT WHETHER THE COORDINATES HAVE A TILE OR NOT - beware of null reference Errors</br>
    /// </summary>
    /// <param name="coord">Coordinate to find Diagonals for</param>
    /// <returns>Vector3Int List of all 6 Diagonals.</returns>
    public static List<Vector3Int> CubeDiagonals(Vector3Int coord)
    {
        List<Vector3Int> diagonals = new List<Vector3Int>();

        foreach (Vector3Int dir in cubeDiagonalDirectionVectors)
        {
            diagonals.Add(CubeAdd(coord, dir));
        }
        return diagonals;
    }

    /// <summary>
    /// Calculates the Distance between two Coordinate Vectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Vector3Int of the Distance between points "a" and "b".</returns>
    public static int CubeDistance(Vector3Int a, Vector3Int b)
    {
        Vector3Int distance = CubeSubstract(a, b);
        return (Mathf.Abs(distance.x) + Mathf.Abs(distance.y) + Mathf.Abs(distance.z)) / 2;
    }

    /// <summary>
    /// Its just a Lerp function
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float Lerp(float a, float b, float t)
    {
        float result = a + (b - a) * t;
        return result;
    }

    /// <summary>
    /// It's just Lerps but for 2 Vector3Int's.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns>A Vector3 with floats.</returns>
    public static Vector3 CubeLerp(Vector3Int a, Vector3Int b, float t)
    {
        return new Vector3(Lerp(a.x, b.x, t),
                              Lerp(a.y, b.y, t),
                              Lerp(a.z, b.z, t)
                              );
    }

    /// <summary>
    /// Could Draw a Line between Coordinate "a" and "b".
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>A List of Vector3Int of every tile between Coordinate "a" and "b"</returns>
    public static List<Vector3Int> CubeLineDraw(Vector3Int a, Vector3Int b)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        int dist = CubeDistance(a, b);
        for (int i = 0; i <= dist; i++)
        {
            float lerpV = (1 / (float)dist) * i;
            result.Add(CubeRound(CubeLerp(a, b, lerpV)));
        }
        return result;
    }

    /// <summary>
    /// Rounds a Vector3 to Vector3Int.
    /// <br>It also takes care of rounding issues that could result in a Vector3Int thats not viable for an cubic Axial Grid.</br>
    /// </summary>
    /// <param name="a">Vector3 to round.</param>
    /// <returns>Rounded Vector3Int</returns>
    public static Vector3Int CubeRound(Vector3 a)
    {
        int x = Mathf.RoundToInt(a.x);
        int y = Mathf.RoundToInt(a.y);
        int z = Mathf.RoundToInt(a.z);

        float xDiff = Mathf.Abs(x - a.x);
        float yDiff = Mathf.Abs(y - a.y);
        float zDiff = Mathf.Abs(z - a.z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            x = -y - z;
        }
        else if (yDiff > zDiff)
        {
            y = -x - z;
        }
        else
        {
            z = -x - y;
        }
        return new Vector3Int((int)x, (int)y, (int)z);
    }

    /// <summary>
    /// Gives a List of Coordinates, reachable within a Range.
    /// <br>DOES NOT CONSIDER WHETHER THE COORDINATE ACTUALLY EXISTS - beware of Null reference Exceptions.</br>
    /// </summary>
    /// <param name="centerCoord"></param>
    /// <param name="range"></param>
    /// <returns>List of Coordinates within "range" from "centerCoord"</returns>
    public static List<Vector3Int> CoordinateRange(Vector3Int centerCoord, int range)
    {
        List<Vector3Int> result = new List<Vector3Int>();



        for (int x = centerCoord.x - range; -range <= x && x <= range; x++)
        {
            for (int y = centerCoord.y - range; -range <= y && y <= range; y++)
            {
                for (int z = centerCoord.z - range; -range <= z && z <= range; z++)
                {
                    if (x + y + z == 0)
                    {
                        result.Add(CubeAdd(centerCoord, new Vector3Int(x, y, z)));
                    }
                }
            }
        }
        return result;
    }

    // PATHFINDING ------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Searches for every Coordinate, reachable from "startingCoord" within "range".
    /// <br>STILL NEED TO ADD A WAY OF CHECKING FOR BLOCKED TILES</br>
    /// <br>This is literal black magic.</br>
    /// </summary>
    /// <param name="startCoord">start of the search</param>
    /// <param name="range">how far do you want to go?</param>
    /// <returns>List of coordinates reachable from center within range that are not blocked</returns>
    public static List<Vector3Int> CoordinatesReachable(Vector3Int startCoord, int range, List<Vector3Int> grid)
    {
        // List of coordinates "visited" by the search
        List<Vector3Int> visited = new List<Vector3Int>
        {
            // starting Tile is added to "visited"
            startCoord
        };

        // List of a List of Coordinates. fringes[k] has all Tiles reachable in k steps.
        List<List<Vector3Int>> fringes = new()
        {
            // fringes[0] gets the "visited" list containing only the starting coord. in 0 steps there is only the one Tile (starting coord) reachable.
            new List<Vector3Int> { startCoord }
        };

        // loop through the range
        for (int k = 1; k < range; k++)
        {
            // new List to hold every reachable tile to be added to visited.
            List<Vector3Int> NextIteration = new List<Vector3Int>();

            // loop through every Coordinate in fringes[k-1]
            foreach (Vector3Int hex in fringes[k - 1])
            {
                // take every neighbor of the current tile
                foreach (Vector3Int neighbor in CubeNeighbors(hex))
                {
                    // check, whether we have it visited already and whether it is blocked.
                    if (!visited.Contains(neighbor) && grid.Contains(neighbor))
                    {
                        //Add it to visited, since it is reachable
                        visited.Add(neighbor);
                        // Add it to fringes[k] for the next loop iteration of fringes[k-1]
                        NextIteration.Add(neighbor);
                    }
                }
            }
            // add a new List in fringes[k] to be filled with reachable tiles in current k steps.
            fringes.Add(NextIteration);
        }
        return visited;
    }
    public static List<Vector3Int> CoordinatesReachable(Vector3Int startCoord, int range)
    {
        // List of coordinates "visited" by the search
        List<Vector3Int> visited = new List<Vector3Int>
        {
            // starting Tile is added to "visited"
            startCoord
        };

        // List of a List of Coordinates. fringes[k] has all Tiles reachable in k steps.
        List<List<Vector3Int>> fringes = new()
        {
            // fringes[0] gets the "visited" list containing only the starting coord. in 0 steps there is only the one Tile (starting coord) reachable.
            new List<Vector3Int> { startCoord }
        };

        // loop through the range
        for (int k = 1; k < range; k++)
        {
            // new List to hold every reachable tile to be added to visited.
            List<Vector3Int> NextIteration = new List<Vector3Int>();

            // loop through every Coordinate in fringes[k-1]
            foreach (Vector3Int hex in fringes[k - 1])
            {
                // take every neighbor of the current tile
                foreach (Vector3Int neighbor in CubeNeighbors(hex))
                {
                    // check, whether we have it visited already and whether it is blocked.
                    if (!visited.Contains(neighbor))
                    {
                        //Add it to visited, since it is reachable
                        visited.Add(neighbor);
                        // Add it to fringes[k] for the next loop iteration of fringes[k-1]
                        NextIteration.Add(neighbor);
                    }
                }
            }
            // add a new List in fringes[k] to be filled with reachable tiles in current k steps.
            fringes.Add(NextIteration);
        }
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (Vector3Int hex in visited)
        {
            if (hex != Vector3Int.zero)
            {
                result.Add(hex);
            }
        }
        return result;
    }

    // ROTATION ----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Rotates a Coordinate around a center. 60� Clockwise.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="coord"></param>
    /// <returns>Rotated Coordinate.</returns>
    public static Vector3Int Rotate60DegClockwise(Vector3Int center, Vector3Int coord, int amount)
    {
        Vector3Int result = coord;
        result = CubeSubstract(result, center);
        for (int i = 0; i < amount; i++)
        {
            result = new Vector3Int(-result.z, -result.x, -result.y);
        }
        result = CubeAdd(center, result);

        return result;

    }

    /// <summary>
    /// Rotates a Coordinate around a Center. 60� Counter-Clockwise.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="coord"></param>
    /// <returns>Rotated Coordinate</returns>
    public static Vector3Int Rotate60DegCounterClockwise(Vector3Int center, Vector3Int coord, int amount)
    {
        Vector3Int result = coord;
        Vector3Int start = CubeSubstract(result, center);

        for (int i = 0; i < amount; i++)
        {
            result = new Vector3Int(-start.y, -start.z, -start.x);

        }
        result = CubeAdd(center, result);

        return result;
    }

    /// <summary>
    /// Rotates a List of Coordinates Clockwise around a given Center
    /// </summary>
    /// <param name="center">Center to rotate around</param>
    /// <param name="range">List of Coordinates</param>
    /// <returns>Rotated List of Coordiantes</returns>
    public static List<Vector3Int> RotateRangeClockwise(Vector3Int center, List<Vector3Int> range, int amount)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (Vector3Int a in range)
        {
            result.Add(Rotate60DegClockwise(center, a, amount));
        }
        return result;
    }

    /// <summary>
    /// Rotates a List of Coordinates Counter-Clockwise around a given Center
    /// </summary>
    /// <param name="center">Center to rotate around</param>
    /// <param name="range">List of Coordinates</param>
    /// <returns>Rotated List of Coordiantes</returns>
    public static List<Vector3Int> RotateRangeCounterClockwise(Vector3Int center, List<Vector3Int> range, int amount)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        foreach (Vector3Int a in range)
        {
            result.Add(Rotate60DegCounterClockwise(center, a, amount));
        }
        return result;
    }

    /// <summary>
    /// Converts a Hex Coordinate into a Worldposition with y=0
    /// </summary>
    /// <param name="hex">Coordinate</param>
    /// <param name="size">Size of the Hexagon</param>
    /// <returns>Worldlocation</returns>
    public static Vector3 AxialHexToPixel(Vector2Int hex, float size)
    {
        Vector3 result = new Vector3();

        float x = size * ((float)hex.x * 1.5f);
        float y = size * (((float)hex.x * (Mathf.Sqrt(3f) / 2f)) + (Mathf.Sqrt(3) * (float)hex.y));

        result.x = x;
        result.z = y;
        //result.z = -x -y;
        return result;
    }

    public static Vector2Int PixelToHexCoord2D(Vector2 pixel, float size)
    {
        Vector2Int result = new Vector2Int();

        float x = ((2f / 3f) * pixel.x) / size;
        float y = ((-1f / 3f) * pixel.x + ((Mathf.Sqrt(3f) / 3f) * pixel.y)) / size;

        Vector3Int roundedResult = CubeRound(new Vector3(x, y, -x - y));

        result.x = roundedResult.x;
        result.y = roundedResult.y;

        return result;
    }

    // COMBINE ----------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Combines Coordinates of two Cubic Grids into one along the given Axis.
    /// </summary>
    /// <param name="gridA">Origin Grid</param>
    /// <param name="gridB">Grid to be Attached</param>
    /// <param name="dirIndex">Index of Direction</param>
    /// <returns>Combines Grid</returns>
    public static List<Vector3Int> CombineGridsAlongAxis(List<Vector3Int> gridA, List<Vector3Int> gridB, Vector3Int dir, out List<Vector3Int> movedGrid)
    {
        List<Vector3Int> combine = gridA;

        List<Vector3Int> movedGridB = gridB;

        while (GridOverlaps(gridA, movedGridB))
        {
            movedGridB = CubeAddRange(movedGridB, dir);
        }

        combine.AddRange(movedGridB);
        movedGrid = movedGridB;
        return combine;
    }

    /// <summary>
    /// Combines Coordinates of two Axial Coordinate Grids into one along the given Axis.
    /// </summary>
    /// <param name="gridA">Origin of Grid</param>
    /// <param name="gridB">Grid to be Attached</param>
    /// <param name="dirIndex">Normalized Vector where gridB attaches to gridA</param>
    /// <returns>combined Grid</returns>
    public static List<Vector2Int> CombineGridsAlongAxis(List<Vector2Int> gridA, List<Vector2Int> gridB, Vector3Int dir, out List<Vector2Int> movedGrid)
    {
        List<Vector3Int> movedPart = new List<Vector3Int>();
        List<Vector3Int> combine = CombineGridsAlongAxis(AxialToCubeCoord(gridA), AxialToCubeCoord(gridB), dir, out movedPart);

        movedGrid = CubeToAxialCoord(movedPart);
        return CubeToAxialCoord(combine);
    }

    /// <summary>
    /// Checks whether two given Lists of coordinates have duplicates.
    /// </summary>
    /// <param name="gridA">List of Coordinates</param>
    /// <param name="gridB">List of Coordinates</param>
    /// <returns>true if at least one coordinate is in both Lists</returns>
    public static bool GridOverlaps(List<Vector3Int> gridA, List<Vector3Int> gridB)
    {
        foreach (Vector3Int a in gridA)
        {
            if (gridB.Contains(a))
            {
                return true;
            }
        }
        return false;
    }

    // SHAPES --------------------------------------------------------------------------------------------------------------------------------

    public static List<Vector2Int> GenerateRombusShapedGrid(int qSize, int rSize)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        int q1 = qSize / 2 * -1;
        int q2 = qSize / 2;
        int r1 = rSize / 2 * -1;
        int r2 = rSize / 2;


        for (int q = q1; q <= q2; q++)
        {
            for (int r = r1; r <= r2; r++)
            {
                result.Add(new Vector2Int(q, r));
            }
        }
        return result;
    }

    public static List<Vector2Int> GenerateRectangleShapedGrid(int qSize, int rSize)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        int left = qSize / 2 * -1;
        int right = qSize / 2;
        int top = rSize / 2 * -1;
        int bottom = rSize / 2;

        for (int q = left; q <= right; q++)
        {
            int qoffset = Mathf.FloorToInt((float)q / 2f);
            for (int r = top - qoffset; r <= bottom - qoffset; r++)
            {
                result.Add(new Vector2Int(q, r));
            }
        }
        return result;
    }

    public static List<Vector2Int> GenerateHexagonalShapedGrid(int radius)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                result.Add(new Vector2Int(q, r));
            }
        }
        return result;
    }

    public static List<Vector3Int> BreadthFIrstPathfinding(Vector3Int start, Vector3Int goal, List<Vector3Int> Grid)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        List<Vector3Int> reached = new List<Vector3Int>();

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        frontier.Enqueue(start);
        reached.Add(start);


        while (!frontier.Contains(goal))
        {
            Vector3Int current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }
            foreach (Vector3Int n in CubeNeighbors(current))
            {
                if (!cameFrom.ContainsKey(n) && Grid.Contains(n))
                {
                    frontier.Enqueue(n);
                    cameFrom.Add(n, current);
                }
            }
        }

        Vector3Int newCurrent = goal;
        while (newCurrent != start)
        {
            path.Add(newCurrent);
            newCurrent = cameFrom[newCurrent];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    public static List<Vector3Int> HeuristicPathfind(Vector3Int start, Vector3Int goal, List<Vector3Int> grid)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        Dictionary<Vector3Int, int> frontier = new Dictionary<Vector3Int, int>();
        List<Vector3Int> reached = new List<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        frontier.Add(start, 0);
        reached.Add(start);

        while (!frontier.ContainsKey(goal))
        {
            int minCost = int.MaxValue;

            // Priority Queue
            Vector3Int cheapestCurrent = new Vector3Int();
            foreach (KeyValuePair<Vector3Int, int> kvp in frontier)
            {

                if (kvp.Value < minCost)
                {
                    minCost = kvp.Value;
                    cheapestCurrent = kvp.Key;
                }
            }

            frontier.Remove(cheapestCurrent);
            Vector3Int current = cheapestCurrent;

            if (current == goal)
            {
                break;
            }
            foreach (Vector3Int next in CubeNeighbors(current))
            {
                if (!cameFrom.ContainsKey(next) && grid.Contains(next))
                {
                    int priority = CubeDistance(goal, next);
                    frontier.Add(next, priority);
                    cameFrom.Add(next, current);
                }
            }
        }



        Vector3Int newCurrent = goal;
        while (newCurrent != start)
        {
            path.Add(newCurrent);
            newCurrent = cameFrom[newCurrent];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

    public static List<Vector3Int> CostHeuristicPathFind(Vector3Int start, Vector3Int goal, Dictionary<Vector2Int, GridTile> gridWithCost, out int pathCost)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        Dictionary<Vector3Int, int> frontier = new Dictionary<Vector3Int, int>();
        List<Vector3Int> reached = new List<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        frontier.Add(start, 0);
        reached.Add(start);

        Dictionary<Vector3Int, int> CostSoFar = new Dictionary<Vector3Int, int>();
        CostSoFar.Add(start, 0);

        int iteration = 0;

        while (!frontier.ContainsKey(goal))
        {
            int minCost = int.MaxValue;

            // Priority Queue
            Vector3Int cheapestCurrent = new Vector3Int();
            foreach (KeyValuePair<Vector3Int, int> kvp in frontier)
            {

                if (kvp.Value < minCost)
                {
                    minCost = kvp.Value;
                    cheapestCurrent = kvp.Key;
                }
            }

            frontier.Remove(cheapestCurrent);
            Vector3Int current = cheapestCurrent;

            if (current == goal)
            {
                break;
            }


            foreach (Vector3Int n in CubeNeighbors(current))
            {
                Vector2Int next = CubeToAxialCoord(n);
                if (gridWithCost.ContainsKey(next))
                {
                    int nexcostsofar = CostSoFar[current];
                    int newnewgridCost = gridWithCost[next].currentGridState.NegativeSpreadCost;
                    int newCost = nexcostsofar + newnewgridCost;



                    if (!cameFrom.ContainsKey(n) && gridWithCost.ContainsKey(next) || newCost < CostSoFar[n])
                    {
                        //CostSoFar.Add(n, newCost);
                        CostSoFar[n] = newCost;
                        int priority = newCost;
                        frontier.Add(n, priority);
                        cameFrom.Add(n, current);
                    }
                }
            }
        }


        pathCost = 0;
        Vector3Int newCurrent = goal;
        while (newCurrent != start)
        {
            pathCost += CostSoFar[newCurrent];
            path.Add(newCurrent);
            newCurrent = cameFrom[newCurrent];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

    public static Vector3Int CubeScale(Vector3Int hex, int factor)
    {
        Vector3Int cube = new Vector3Int(hex.x * factor, hex.y * factor, hex.z * factor);
        return cube;
    }

    public static List<Vector2Int> Ring(Vector3Int center, int radius)
    {
        List<Vector2Int> cubeRing = new List<Vector2Int>();
        Vector3Int hex = CubeAdd(center, CubeScale(cubeDirectionVectors[4], radius));

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < radius; j++)
            {
              if (GridManager.Instance.Grid.Keys.Contains(CubeToAxialCoord(hex)))
                {
                    
                       cubeRing.Add(CubeToAxialCoord(hex));
                       
                }
                hex = CubeAdd(hex, cubeDirectionVectors[i]);

            }
        }
     
        return cubeRing;
    }

}
