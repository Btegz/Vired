using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct ProceduralTileInfo
{
    public Vector2Int coord;
    public Ressource resource;
    public float distanceValue;
    public float noiseValue;
    public ProceduralTileInfo(Vector2Int coord, float noise1, float noise2, float worldNoise)
    {
        this.coord = coord;
        this.distanceValue = HexGridUtil.CubeDistance(HexGridUtil.AxialToCubeCoord(coord), Vector3Int.zero);
        this.noiseValue = worldNoise;

        if (noise1 < 0 && noise2 < 0)
        {
            resource =  Ressource.ressourceA;
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

    [SerializeField] private FastNoiseLite.DomainWarpType m_DomainWarpType;

    public FastNoiseLite.DomainWarpType M_DomainWarpType
    {
        get { return m_DomainWarpType; }
        set { m_DomainWarpType = value; }
    }

    [SerializeField] private float m_DomainWarpAmplitude;

    public float M_DomainWarpAmplitude
    {
        get { return m_DomainWarpAmplitude; }
        set { m_DomainWarpAmplitude = value; }
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

    public Dictionary<Vector2Int, float> NoiseData(FastNoiseLite.NoiseType noiseType, float frequency)
    {
        if (myGenerateRandomSeed)
        {
            mySeed = Random.Range(1000, 2000);
        }
        noise1.SetSeed(mySeed);
        noise1.SetNoiseType(noiseType);
        noise1.SetFrequency(frequency);

        if (noiseType == FastNoiseLite.NoiseType.Cellular)
        {
            noise1.SetCellularDistanceFunction(MyCellularDistanceFunction);
            noise1.SetCellularReturnType(MyCellulareReturnType);
            noise1.SetCellularJitter(MyJitter);
        }

        Dictionary<Vector2Int, float> result = new Dictionary<Vector2Int, float>();

        List<Vector2Int> coordinates = HexGridUtil.GenerateRectangleShapedGrid(noiseDataSize.x, noiseDataSize.y);
        foreach (Vector2Int c in coordinates)
        {
            Vector3 worldPos = HexGridUtil.AxialHexToPixel(c, 1f);
            result.Add(c, noise1.GetNoise(worldPos.x, worldPos.z));
        }
        return result;
    }

    public Dictionary<Vector2Int, float> NoiseData(FastNoiseLite.NoiseType noiseType, float frequency, FastNoiseLite.DomainWarpType domainWarpType, float domainWarpAmplitude)
    {
        Dictionary<Vector2Int, float> result = NoiseData(noiseType, frequency);
        Dictionary<Vector2Int, float> resultresult = new Dictionary<Vector2Int, float>();
        noise1.SetDomainWarpType(domainWarpType);
        noise1.SetDomainWarpAmp(domainWarpAmplitude);
        foreach (KeyValuePair<Vector2Int, float> kvp in result)
        {
            float x = kvp.Key.x;
            float y = kvp.Key.y;
            noise1.DomainWarp(ref x, ref y);
            resultresult.Add(kvp.Key, noise1.GetNoise(x, y));
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
            ProceduralTileInfo newTileInfo = new ProceduralTileInfo(c, noise1.GetNoise(worldPos.x, worldPos.z), noise2.GetNoise(worldPos.x, worldPos.z),worldNoise.GetNoise(worldPos.x, worldPos.z));
            result.Add(newTileInfo);
        }
        return result;
    }

    public void SetTheNoises()
    {
        noise1.SetNoiseType(noiseType1);
        noise2.SetNoiseType(noiseType2);
        noise1.SetFrequency(frequency);
        noise2.SetFrequency(frequency);
        if(noiseType1 == FastNoiseLite.NoiseType.Cellular)
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
            MySeed = Random.Range(1000,2000);
        }
        noise1.SetSeed(MySeed);
        noise2.SetSeed(MySeed + 1);

        worldNoise.SetNoiseType(m_HillNoiseType);
        worldNoise.SetFrequency(m_frequency);
        worldNoise.SetDomainWarpType(m_DomainWarpType);
        worldNoise.SetDomainWarpAmp(m_DomainWarpAmplitude);

        hillNoise.SetNoiseType(m_HillNoiseType);
        hillNoise.SetFrequency(m_HillFrequency);
    }

    public void SortTilesByDistanceNoise(List<ProceduralTileInfo> tiles)
    {
        //tiles.Sort(()=>)
    }

    public void FixTileCount()
    {

    }
}
