using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Class GexGridUtil provides functions with algorithms for Operations in a Hexagonal Grid.
/// </summary>
public static class HexGridUtil
{
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
    /// Substracts two Vectors.
    /// </summary>
    /// <param name="a">Minuent</param>
    /// <param name="b">Subtrahent</param>
    /// <returns>difference of "a" minus "b"</returns>
    public static Vector3Int CubeSubstract(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
    }

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
    public static float Lerp(int a, int b, float t)
    {
        return a + (b - a) * t;
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
        for (int i = 0; i < dist; i++)
        {
            result.Add(CubeRound(CubeLerp(a, b, 1 / dist * i)));
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
        float x = Mathf.Round(a.x);
        float y = Mathf.Round(a.y);
        float z = Mathf.Round(a.z);

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

        for (int x = centerCoord.x; -range <= x && x <= range; x++)
        {
            for (int y = centerCoord.y; -range <= y && y <= range; y++)
            {
                for (int z = centerCoord.z; -range <= z && z <= range; z++)
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

    /// <summary>
    /// Searches for every Coordinate, reachable from "startingCoord" within "range".
    /// <br>STILL NEED TO ADD A WAY OF CHECKING FOR BLOCKED TILES</br>
    /// <br>This is literal black magic.</br>
    /// </summary>
    /// <param name="startCoord">start of the search</param>
    /// <param name="range">how far do you want to go?</param>
    /// <returns>List of coordinates reachable from center within range that are not blocked</returns>
    public static List<Vector3Int> CoordinatesReachable(Vector3Int startCoord, int range)
    {
        // List of coordinates "visited" by the search
        List<Vector3Int> visited = new List<Vector3Int>();

        // starting Tile is added to "visited"
        visited.Add(startCoord);

        // List of a List of Coordinates. fringes[k] has all Tiles reachable in k steps.
        List<List<Vector3Int>> fringes = new List<List<Vector3Int>>();

        // fringes[0] gets the "visited" list containing only the starting coord. in 0 steps there is only the one Tile (starting coord) reachable.
        fringes[0] = new List<Vector3Int>();
        fringes[0].AddRange(visited);

        // loop through the range
        for (int k = 1; k < range; k++)
        {
            // add a new List in fringes[k] to be filled with reachable tiles in current k steps.
            fringes[k].AddRange(new List<Vector3Int>());

            // loop through every Coordinate in fringes[k-1]
            foreach (Vector3Int hex in fringes[k - 1])
            {
                // take every neighbor of the current tile
                foreach (Vector3Int neighbor in CubeNeighbors(hex))
                {
                    // check, whether we have it visited already and whether it is blocked.
                    if (!visited.Contains(neighbor) /*&& IS NOT BLOCKED*/)
                    {
                        //Add it to visited, since it is reachable
                        visited.Add(neighbor);
                        // Add it to fringes[k] for the next loop iteration of fringes[k-1]
                        fringes[k].Add(neighbor);
                    }
                }
            }
        }
        return visited;
    }

    /// <summary>
    /// Rotates a Coordinate around a center. 60° Clockwise.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="coord"></param>
    /// <returns>Rotated Coordinate.</returns>
    public static Vector3Int Rotate60DegClockwise(Vector3Int center,Vector3Int coord)
    {
        Vector3Int result = CubeSubstract(coord, center);
        

        return new Vector3Int(-result.y, -result.z, -result.z);

    }

    /// <summary>
    /// Rotates a Coordinate around a Center. 60° Counter-Clockwise.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="coord"></param>
    /// <returns>Rotated Coordinate</returns>
    public static Vector3Int Rotate60DegCounterClockwise(Vector3Int center,Vector3Int coord)
    {
        Vector3Int start = CubeSubstract(coord, center);
        Vector3Int rotatedResult = new Vector3Int(-start.z, -start.x, -start.y);
        Vector3Int result = CubeAdd(center, rotatedResult);

        return result;
    }
}
