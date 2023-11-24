using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            resource = Ressource.resscoureD;
        }
        noiseDistanceFactor = Mathf.Abs(worldNoise) * (float)distance;
        valid = true;
    }

    public int CompareTo(ProceduralTileInfo other)
    {
        return this.noiseDistanceFactor.CompareTo(other.noiseDistanceFactor);
    }
}

[CreateAssetMenu(fileName = "MapSettings", menuName = "Procedural")]
public class MapSettings : ScriptableObject
{
    FastNoiseLite noise1 = new FastNoiseLite();
    FastNoiseLite noise2 = new FastNoiseLite();
    FastNoiseLite worldNoise = new FastNoiseLite();
    FastNoiseLite hillNoise = new FastNoiseLite();

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
        List<ProceduralTileInfo> tiles = MakeTiles();
        tiles = FixTileCount(tiles);

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
        //    Debug.Log("Coordinate: " + pti.coord + ", Distance: " + pti.distance + ", Noise: " + pti.noiseValue + ", NoiseDistanceFactor: " + pti.noiseDistanceFactor);
        //}
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
    }

    public List<ProceduralTileInfo> FixTileCount(List<ProceduralTileInfo> tiles)
    {
        List<ProceduralTileInfo> fixedList = tiles;
        //Debug.Log("TileCount: " + fixedList.Count);
        for (int i = fixedList.Count - 1; i > myTileCount - 1; i--)
        {
            fixedList[i].valid = false;
        }
        //Debug.Log("TileCount: " + fixedList.Count);
        return fixedList;
    }
    public List<ProceduralTileInfo> FixUnreachableTiles(List<ProceduralTileInfo> tiles)
    {
        List<ProceduralTileInfo> reachableTiles = new List<ProceduralTileInfo>();

        List<Vector2Int> tileCoords = new List<Vector2Int>();
        foreach (ProceduralTileInfo pti in tiles)
        {
            tileCoords.Add(pti.coord);
        }

        List<Vector2Int> unreachableCoords = new List<Vector2Int>();
        List<Vector2Int> reachableCoords = HexGridUtil.CubeToAxialCoord(HexGridUtil.CoordinatesReachable(Vector3Int.zero, noiseDataSize.x, HexGridUtil.AxialToCubeCoord(tileCoords)));
        foreach (Vector2Int vv in reachableCoords)
        {
            unreachableCoords.Remove(vv);
        }

        for (int i = 0; i < unreachableCoords.Count; i++)
        {
            Vector2Int currentCoord = unreachableCoords[i];
            if (GetTileFromCoord(currentCoord, tiles, out ProceduralTileInfo result))
            {

            }
        }


        //take every tilecoord in a List
        //get every reachable coordinate from the List
        //form a list from every coordinate that is not in reachable coordinates 



        //foreach(Vector2Int c in unreachableCoords)
        //{
        //    List<Vector3Int> distLine = HexGridUtil.CubeLineDraw(HexGridUtil.AxialToCubeCoord(c), Vector3Int.zero);
        //    if(GetTileFromCoord(c,reachableTiles,out ProceduralTileInfo reult))
        //    {

        //    }
        //}


        return reachableTiles;
    }

    public List<ProceduralTileInfo> reachableTiles(List<ProceduralTileInfo> input)
    {
        List<ProceduralTileInfo> reachableTiles = new List<ProceduralTileInfo>();



        return reachableTiles;
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
}
