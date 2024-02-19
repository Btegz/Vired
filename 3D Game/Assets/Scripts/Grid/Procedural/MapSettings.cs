using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class ProceduralTileInfo : IComparable<ProceduralTileInfo>
{
    public Vector2Int coord;
    public Ressource resource;
    public float distance;
    public float noiseValue;
    public float noiseDistanceFactor;
    public bool valid;
    public ProceduralTileInfo(Vector2Int coord, float noise1, float noise2, float worldNoise)
    {
        this.coord = coord;
        this.distance = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(coord), Vector3Int.zero);
        this.noiseValue = worldNoise;

        if (noise1 < 0 && noise2 < 0)
        {
            resource = Ressource.ressourceA;
        }
        else if (noise1 < 0 && noise2 > 0)
        {
            resource = Ressource.ressourceB;
        }
        else if (noise1 > 0 && noise2 < 0)
        {
            resource = Ressource.ressourceC;
        }
        else
        {
            resource = Ressource.ressourceD;
        }
        noiseDistanceFactor = Mathf.Abs(worldNoise) * (float)distance;
        valid = true;
    }
    public int CompareTo(ProceduralTileInfo other)
    {
        return this.noiseDistanceFactor.CompareTo(other.noiseDistanceFactor);
    }
}
[CreateAssetMenu(fileName = "MapSettings", menuName = "MapSettings")]
public class MapSettings : ScriptableObject
{
    FastNoiseLite noise1 = new FastNoiseLite();
    FastNoiseLite noise2 = new FastNoiseLite();
    FastNoiseLite worldNoise = new FastNoiseLite();
    FastNoiseLite hillNoise = new FastNoiseLite();
    FastNoiseLite irregularityNoise1 = new FastNoiseLite();
    FastNoiseLite irregularityNoise2 = new FastNoiseLite();
    FastNoiseLite irregularityNoise3 = new FastNoiseLite();
    public float IrregularityFrequency;
    public float IrregularityFactor;
    public float constantHeighVarianceFactor;
    [Header("General")]
    [SerializeField] private int mySeed;
    public int MySeed
    {
        get { return mySeed; }
        set { mySeed = value; }
    }
    [SerializeField] private bool myGenerateRandomSeed;
    public bool MyGenerateRandomSeed
    {
        get { return myGenerateRandomSeed; }
        set { myGenerateRandomSeed = value; }
    }
    [SerializeField] private int myTileCount;
    public int MyTileCount
    {
        get { return myTileCount; }
        set { myTileCount = value; }
    }
    [Header("Ressource Noise Settings")]
    [SerializeField] private FastNoiseLite.NoiseType noiseType1;
    public FastNoiseLite.NoiseType NoiseType1
    {
        get { return noiseType1; }
        set { noiseType1 = value; }
    }
    [SerializeField] private FastNoiseLite.NoiseType noiseType2;
    public FastNoiseLite.NoiseType NoiseType2
    {
        get { return noiseType2; }
        set { noiseType2 = value; }
    }
    [SerializeField][Range(0.001f, 1f)] private float frequency;
    public float Frequency
    {
        get { return frequency; }
        set { frequency = value; }
    }
    [Header("Cellular noise Settings")]
    [SerializeField] private FastNoiseLite.CellularDistanceFunction myCellularDistanceFunction;
    public FastNoiseLite.CellularDistanceFunction MyCellularDistanceFunction
    {
        get { return myCellularDistanceFunction; }
        set { myCellularDistanceFunction = value; }
    }
    [SerializeField] private FastNoiseLite.CellularReturnType myCellularReturnType;
    public FastNoiseLite.CellularReturnType MyCellulareReturnType
    {
        get { return myCellularReturnType; }
        set { myCellularReturnType = value; }
    }
    [SerializeField] private float myJitter;
    public float MyJitter
    {
        get { return myJitter; }
        set { myJitter = value; }
    }
    [Header("World-Shape Noise Settings")]
    [SerializeField] private Vector2Int noiseDataSize;
    public Vector2Int NoiseDataSize
    {
        get { return noiseDataSize; }
        set { noiseDataSize = value; }
    }
    [SerializeField] private FastNoiseLite.NoiseType m_noiseType1;
    public FastNoiseLite.NoiseType M_NoiseType1
    {
        get { return m_noiseType1; }
        set { m_noiseType1 = value; }
    }
    [SerializeField][Range(0.001f, 1f)] private float m_frequency;
    public float M_Frequency
    {
        get { return m_frequency; }
        set { m_frequency = value; }
    }
    [SerializeField] float m_distanceThreshold;
    public float M_DistanceThreshold
    {
        get { return m_distanceThreshold; }
        set { m_distanceThreshold = value; }
    }
    [SerializeField] private FastNoiseLite.NoiseType m_HillNoiseType;
    public FastNoiseLite.NoiseType M_HillNoiseType
    {
        get { return m_HillNoiseType; }
        set { m_HillNoiseType = value; }
    }
    [SerializeField][Range(0.001f, 1f)] float m_HillFrequency;
    public float M_HillFrequency
    {
        get { return m_HillFrequency; }
        set { m_HillFrequency = value; }
    }
    [SerializeField] Vector2 m_HillnoiseThresholds;
    public Vector2 M_HillNoiseThresholds
    {
        get { return m_HillnoiseThresholds; }
        set { m_HillnoiseThresholds = value; }
    }
    public Dictionary<Vector2Int, ProceduralTileInfo> NoiseData(FastNoiseLite.NoiseType noiseType, float frequency)
    {
        List<ProceduralTileInfo> tiles = MakeTiles();
        tiles = FixTileCount(tiles);

        //if (myGenerateRandomSeed)
        //{
        //    mySeed = UnityEngine.Random.Range(1000, 2000);
        //}
        //noise1.SetSeed(mySeed);
        //noise1.SetNoiseType(noiseType);
        //noise1.SetFrequency(frequency);

        //if (noiseType == FastNoiseLite.NoiseType.Cellular)
        //{
        //    noise1.SetCellularDistanceFunction(MyCellularDistanceFunction);
        //    noise1.SetCellularReturnType(MyCellulareReturnType);
        //    noise1.SetCellularJitter(MyJitter);
        //}

        Dictionary<Vector2Int, ProceduralTileInfo> result = new Dictionary<Vector2Int, ProceduralTileInfo>();
        foreach (ProceduralTileInfo pti in tiles)
        {
            result.Add(pti.coord, pti);
        }

        //List<Vector2Int> coordinates = HexGridUtil.GenerateRectangleShapedGrid(noiseDataSize.x, noiseDataSize.y);
        //foreach (Vector2Int c in coordinates)
        //{
        //    Vector3 worldPos = HexGridUtil.AxialHexToPixel(c, 1f);
        //    result.Add(c, noise1.GetNoise(worldPos.x, worldPos.z));
        //}
        return result;
    }
    public List<ProceduralTileInfo> NoiseData()
    {
        List<ProceduralTileInfo> tiles = MakeTilesWithCorrectAmount();
        //tiles = FixTileCount(tiles);
        //FixUnreachableTiles(tiles);
        return tiles;
    }
    public Dictionary<Vector2Int, ProceduralTileInfo> NoiseData(FastNoiseLite.NoiseType noiseType, float frequency, FastNoiseLite.DomainWarpType domainWarpType, float domainWarpAmplitude)
    {
        Dictionary<Vector2Int, ProceduralTileInfo> result = NoiseData(noiseType, frequency);
        Dictionary<Vector2Int, ProceduralTileInfo> resultresult = new Dictionary<Vector2Int, ProceduralTileInfo>();
        noise1.SetDomainWarpType(domainWarpType);
        noise1.SetDomainWarpAmp(domainWarpAmplitude);
        foreach (KeyValuePair<Vector2Int, ProceduralTileInfo> kvp in result)
        {
            float x = kvp.Key.x;
            float y = kvp.Key.y;
            noise1.DomainWarp(ref x, ref y);
            resultresult.Add(kvp.Key, kvp.Value);
        }
        return resultresult;
    }
    public void GenerateIrregultarityNoiseData()
    {
        irregularityNoise1.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        irregularityNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        irregularityNoise3.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
    }
    public Vector3 GetIrregularityNoiseData(Vector3 position)
    {
        Vector3 result = new Vector3();
        result.x = irregularityNoise1.GetNoise(position.x, position.z);
        result.y = irregularityNoise2.GetNoise(position.x, position.z);
        result.z = irregularityNoise3.GetNoise(position.x, position.z);
        return result;
    }
    public List<ProceduralTileInfo> MakeTiles()
    {
        SetTheNoises();
        List<ProceduralTileInfo> result = new List<ProceduralTileInfo>();
        List<Vector2Int> coordinates = HexGridUtil.GenerateRectangleShapedGrid(noiseDataSize.x, noiseDataSize.y);
        foreach (Vector2Int c in coordinates)
        {
            Vector3 worldPos = HexGridUtil.AxialHexToPixel(c, 1f);
            ProceduralTileInfo newTileInfo = new ProceduralTileInfo(c, noise1.GetNoise(worldPos.x, worldPos.z), noise2.GetNoise(worldPos.x, worldPos.z), worldNoise.GetNoise(worldPos.x, worldPos.z));
            result.Add(newTileInfo);
        }
        result.Sort();
        //foreach(ProceduralTileInfo pti in result)
        //{
        //    Debug.Log("Coordinate: " + pti.coord + ", Distance: " + pti.distance + ", Noise: " + pti.noiseValue + ", NoiseDistanceFactor: " + pti.noiseDistanceFactor+", Validity: "+pti.valid);
        //}
        return result;
    }
    public List<ProceduralTileInfo> MakeTilesWithCorrectAmount()
    {
        SetTheNoises();
        List<ProceduralTileInfo> result = new List<ProceduralTileInfo>();
        result.Add(new ProceduralTileInfo(Vector2Int.zero, noise1.GetNoise(0, 0), noise2.GetNoise(0, 0), worldNoise.GetNoise(0, 0)));

        for (int i = 1; i <= myTileCount; i++)
        {
            List<ProceduralTileInfo> newNeighbors = neighbors(result);
            newNeighbors.Sort();
            //Debug.Log("----------------------------------------------------------------------------");
            //foreach (ProceduralTileInfo tile in newNeighbors)
            //{
            //    Debug.Log(tile.noiseDistanceFactor);
            //}
            result.Add(newNeighbors[0]);
        }

        foreach (ProceduralTileInfo tile in result)
        {
            //     Debug.Log(tile.noiseDistanceFactor);
        }


        return result;
    }
    public List<ProceduralTileInfo> neighbors(List<ProceduralTileInfo> input)
    {
        List<Vector2Int> originals = new List<Vector2Int>();
        List<ProceduralTileInfo> newNeighbors = new List<ProceduralTileInfo>();
        foreach (ProceduralTileInfo tile in input)
        {
            originals.Add(tile.coord);
        }

        List<Vector2Int> neighborCoords = HexGridUtil.AxialNeighbors(originals);
        foreach (Vector2Int neighborCoord in neighborCoords)
        {
            Vector3 worldPos = HexGridUtil.AxialHexToPixel(neighborCoord, 1f);
            ProceduralTileInfo newTileInfo = new ProceduralTileInfo(neighborCoord, noise1.GetNoise(worldPos.x, worldPos.z), noise2.GetNoise(worldPos.x, worldPos.z), worldNoise.GetNoise(worldPos.x, worldPos.z));
            newNeighbors.Add(newTileInfo);
        }
        return newNeighbors;
    }
    public List<Vector2Int> GetOuterBorder(List<ProceduralTileInfo> input)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        List<Vector2Int> init = new List<Vector2Int>();
        foreach (ProceduralTileInfo tile in input)
        {
            init.Add(tile.coord);
        }

        List<Vector2Int> outerNeighbors = HexGridUtil.AxialNeighbors(init);
        List<Vector2Int> neighborsNeighbors = HexGridUtil.AxialNeighbors(outerNeighbors);
        foreach (Vector2Int neighborCoord in neighborsNeighbors)
        {
            if (init.Contains(neighborCoord))
            {
                result.Add(neighborCoord);
            }
        }
        return result;
    }
    public void SetTheNoises()
    {
        noise1.SetNoiseType(noiseType1);
        noise2.SetNoiseType(noiseType2);
        noise1.SetFrequency(frequency);
        noise2.SetFrequency(frequency);
        if (noiseType1 == FastNoiseLite.NoiseType.Cellular)
        {
            noise1.SetCellularDistanceFunction(MyCellularDistanceFunction);
            noise1.SetCellularReturnType(MyCellulareReturnType);
            noise1.SetCellularJitter(MyJitter);
        }
        if (noiseType2 == FastNoiseLite.NoiseType.Cellular)
        {
            noise2.SetCellularDistanceFunction(MyCellularDistanceFunction);
            noise2.SetCellularReturnType(MyCellulareReturnType);
            noise2.SetCellularJitter(MyJitter);
        }
        if (myGenerateRandomSeed)
        {
            MySeed = UnityEngine.Random.Range(1000, 2000);
        }
        noise1.SetSeed(MySeed);
        noise2.SetSeed(MySeed + 1);
        worldNoise.SetSeed(MySeed);

        worldNoise.SetNoiseType(M_NoiseType1);
        worldNoise.SetFrequency(m_frequency);

        if (M_NoiseType1 == FastNoiseLite.NoiseType.Cellular)
        {
            worldNoise.SetCellularDistanceFunction(MyCellularDistanceFunction);
            worldNoise.SetCellularReturnType(MyCellulareReturnType);
            worldNoise.SetCellularJitter(MyJitter);
        }

        hillNoise.SetNoiseType(m_HillNoiseType);
        hillNoise.SetFrequency(m_HillFrequency);


        irregularityNoise1.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        irregularityNoise2.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        irregularityNoise3.SetNoiseType(FastNoiseLite.NoiseType.Perlin);

        irregularityNoise1.SetFrequency(IrregularityFrequency);
        irregularityNoise2.SetFrequency(IrregularityFrequency);
        irregularityNoise3.SetFrequency(IrregularityFrequency);
    }
    public List<ProceduralTileInfo> FixTileCount(List<ProceduralTileInfo> tiles)
    {
        List<ProceduralTileInfo> fixedList = tiles;
        //Debug.Log("TileCount: " + fixedList.Count);
        for (int i = fixedList.Count - 1; i > myTileCount - 1; i--)
        {
            //fixedList.RemoveAt(i);
            //Debug.Log("I INVALIDATE A TILE. i = " + i + ", myTileCount: " + myTileCount);
            fixedList[i].valid = false;
        }
        //Debug.Log("TileCount: " + fixedList.Count);
        return fixedList;
    }
    public void FixUnreachableTiles(List<ProceduralTileInfo> tiles)
    {
        // takes every valid tiles coordinate into "tileCoords" List.
        List<Vector2Int> tileCoords = new List<Vector2Int>();
        foreach (ProceduralTileInfo pti in tiles)
        {
            if (pti.valid)
            {
                tileCoords.Add(pti.coord);
            }
        }

        // takes every reachable coord in "tileCoords" List into "reachableCoords" List.
        // takes every unreachable coord into "unreachableCoords" List.
        List<Vector2Int> unreachableCoords = new List<Vector2Int>();
        List<Vector2Int> reachableCoords = HexGridUtil.CubeToAxialCoord(HexGridUtil.CoordinatesReachable(Vector3Int.zero, noiseDataSize.x, HexGridUtil.AxialToCubeCoord(tileCoords)));
        //Debug.Log("reachable Coords is now: " + reachableCoords.Count);
        //Debug.Log("This is how many reachable coords i have: " + reachableCoords.Count + " this is how many should be unreachable" + (myTileCount - reachableCoords.Count)+".");
        foreach (Vector2Int coord in tileCoords)
        {
            if (GetTileFromCoord(coord, tiles, out ProceduralTileInfo resultTest))
            {
                if (!reachableCoords.Contains(coord))
                {
                    unreachableCoords.Add(coord);
                }
            }
        }
        List<List<ProceduralTileInfo>> islands = new List<List<ProceduralTileInfo>>();

        for (int i = 0; i < unreachableCoords.Count;)
        {
            //Debug.Log("unreachableCoords is now " + unreachableCoords.Count + " big.");
            if (GetTileFromCoord(unreachableCoords[i], tiles, out ProceduralTileInfo resultTest))
            {
                List<ProceduralTileInfo> island = FindIslandOfTile(resultTest, tiles);
                //Debug.Log("found an Island its " + island.Count + " big");
                islands.Add(island);
                foreach (ProceduralTileInfo pti in island)
                {
                    //if (unreachableCoords.Contains(pti.coord))
                    //{
                    //    Debug.Log("everything fine");
                    //}
                    //else
                    //{
                    //    Debug.Log("wtfffffffffff????");
                    //}
                    pti.valid = false;
                    unreachableCoords.Remove(pti.coord);
                }
            }
            //Debug.Log("unreachableCoords is now " + unreachableCoords.Count + " big.");
        }

        foreach (List<ProceduralTileInfo> island in islands)
        {
            //Debug.Log("Time to move an Island of the size of " + island.Count);
            List<Vector2Int> islandCoords = new List<Vector2Int>();
            foreach (ProceduralTileInfo pti in island)
            {
                pti.valid = false;
                islandCoords.Add(pti.coord);
            }
            List<Vector2Int> zeroedIsland = HexGridUtil.AxialAddRange(islandCoords, islandCoords[0] * -1);
            //Debug.Log("i have zeroed out the Island on its first Index. The Index now has the coordinate of " + zeroedIsland[0]);
            //Debug.Log("reachable Coords is now" + reachableCoords.Count + " big.");
            reachableCoords = HexGridUtil.CombineGridsAlongAxis(reachableCoords, zeroedIsland, HexGridUtil.cubeDirectionVectors[UnityEngine.Random.Range(0, HexGridUtil.cubeDirectionVectors.Length)], out List<Vector2Int> movedGrid);
            //Debug.Log("After moving Island reachable Coords is now" + reachableCoords.Count + " big.");
            //foreach (Vector2Int coord in movedGrid)
            //{
            //    Debug.Log("moved islandcoordinate now is " + coord);
            //}
        }

        //Debug.Log("reachable Tiles now has" + reachableCoords.Count);

        foreach (Vector2Int coordinate in reachableCoords)
        {
            if (GetTileFromCoord(coordinate, tiles, out ProceduralTileInfo result))
            {
                result.valid = true;
            }
        }

        //Debug.Log("reachable Tiles now has" + reachableCoords.Count);
        //if (unreachableCoords.Count > 0)
        //{
        //    if (GetTileFromCoord(unreachableCoords[0], tiles, out ProceduralTileInfo resultTest))
        //    {
        //        FindIslandOfTile(resultTest, tiles);
        //    }
        //}

        // Takes all invalid Neighbors of "reachableCoords" List into "reachableNeighbors" List.
        //List<Vector2Int> reachableNeighbors = HexGridUtil.AxialNeighbors(reachableCoords);


        //for (int i = 0; i < unreachableCoords.Count; i++)
        //{
        //    Debug.Log("i relocate a Tile that was unreachable");
        //    if (GetTileFromCoord(unreachableCoords[i], tiles, out ProceduralTileInfo result))
        //    {
        //        result.valid = false;
        //        Vector2Int newValidCoord = reachableNeighbors[UnityEngine.Random.Range(0, reachableNeighbors.Count)];
        //        if (GetTileFromCoord(newValidCoord, tiles, out ProceduralTileInfo result2))
        //        {
        //            result2.valid = true;
        //            reachableNeighbors.Remove(newValidCoord);
        //        }
        //    }
        //}

        int validNumber = 0;
        foreach (ProceduralTileInfo pti in tiles)
        {
            if (reachableCoords.Contains(pti.coord))
            {
                pti.valid = true;
                validNumber++;
            }
            else
            {
                pti.valid = false;
            }
        }
        //Debug.Log("I now have " + validNumber + " valid Tiles that should be attached to each other");
    }
    public bool GetTileFromCoord(Vector2Int coord, List<ProceduralTileInfo> input, out ProceduralTileInfo result)
    {
        foreach (ProceduralTileInfo pti in input)
        {
            if (pti.coord == coord)
            {
                result = pti;
                return true;
            }
        }
        result = input[0];
        return false;
    }
    public List<ProceduralTileInfo> FindIslandOfTile(ProceduralTileInfo tile, List<ProceduralTileInfo> input)
    {
        // List of Coordinates starting with the "tile" the function is called with.
        List<Vector2Int> island = new List<Vector2Int>() { tile.coord };

        // form a "newfrontier" of every valid neighbor from former "frontier" items.
        // do as long as newfrontier gets new items

        List<Vector2Int> frontier = new List<Vector2Int>() { tile.coord };
        List<Vector2Int> newFrontier = new List<Vector2Int>();
        int i = 0;
        //Debug.Log("-----------------------------------------------");
        while (i < 50)
        {
            //Debug.Log("i expand my Frontier currently consisting of " + frontier.Count + " items.");
            //Debug.Log("-----------------------------------------------");
            foreach (Vector2Int frontierItem in frontier)
            {
                //Debug.Log("I loop through frontier. currently at: " + frontierItem);
                //Debug.Log("-----------------------------------------------");
                List<Vector2Int> neighbors = HexGridUtil.AxialNeighbors(frontierItem);
                foreach (Vector2Int neighbor in neighbors)
                {
                    //Debug.Log("I loop through the neighbors of current frontierItem " + frontierItem + ", neighbor: " + neighbor);
                    //Debug.Log("-----------------------------------------------");
                    if (GetTileFromCoord(neighbor, input, out ProceduralTileInfo result))
                    {
                        if (!newFrontier.Contains(neighbor) && result.valid && !island.Contains(neighbor) && !frontier.Contains(neighbor))
                        {
                            //Debug.Log("I found a new valid tile in the frontier Neighbors " + neighbor);
                            //Debug.Log("-----------------------------------------------");
                            island.Add(neighbor);
                            newFrontier.Add(neighbor);
                        }
                    }
                }
                //Debug.Log("I am done with frontierItem " + frontierItem + ".");
            }
            if (newFrontier.Count > 0)
            {
                //Debug.Log("i have found Items so i form a new frontier");
                frontier = newFrontier;
                newFrontier.Clear();
            }
            else
            {
                //Debug.Log("i have not found any new items in the Frontier Neighbors so i am Done");
                break;
            }
        }

        List<ProceduralTileInfo> islandTiles = new List<ProceduralTileInfo>();
        foreach (Vector2Int coordinate in island)
        {
            if (GetTileFromCoord(coordinate, input, out ProceduralTileInfo result))
            {
                islandTiles.Add(result);
            }
        }
        //Debug.Log("My Search for the Island is done. The Island is " + islandTiles.Count + " tiles big:");
        foreach (ProceduralTileInfo coordinate in islandTiles)
        {
            Debug.Log(coordinate.coord);
        }
        //Debug.Log("--------------------------------------------------------------------");
        return islandTiles;
    }
}