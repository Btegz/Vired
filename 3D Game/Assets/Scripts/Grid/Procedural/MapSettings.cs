using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Procedural")]
public class MapSettings : ScriptableObject
{
    FastNoiseLite noise = new FastNoiseLite();

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


    [Header("Ressource Noise Settings")]
    [SerializeField] private Vector2Int noiseDataSize;

    public Vector2Int NoiseDataSize
    {
        get { return noiseDataSize; }
        set { noiseDataSize = value; }
    }

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


    public Dictionary<Vector2Int, float> NoiseData(FastNoiseLite.NoiseType noiseType, float frequency)
    {
        noise.SetNoiseType(noiseType);
        noise.SetFrequency(frequency);
        if (myGenerateRandomSeed)
        {
            mySeed = Random.Range(1000, 2000);
        }
        noise.SetSeed(mySeed);
        
        Dictionary<Vector2Int, float> result = new Dictionary<Vector2Int, float>();

        List<Vector2Int> coordinates = HexGridUtil.GenerateRectangleShapedGrid(noiseDataSize.x, noiseDataSize.y);
        foreach (Vector2Int c in coordinates)
        {
            Vector3 worldPos =  HexGridUtil.AxialHexToPixel(c, 1f);
            result.Add(c, noise.GetNoise(worldPos.x, worldPos.z));
        }
        return result;
    }

    public Dictionary<Vector2Int,float> NoiseData(FastNoiseLite.NoiseType noiseType,float frequency, FastNoiseLite.DomainWarpType domainWarpType, float domainWarpAmplitude)
    {
        if (myGenerateRandomSeed)
        {
            mySeed = Random.Range(1000, 2000);
        }
        noise.SetSeed(mySeed);
        Dictionary<Vector2Int, float> result = NoiseData(noiseType, frequency);
        Dictionary<Vector2Int, float> resultresult = new Dictionary<Vector2Int, float>();
        noise.SetDomainWarpType(domainWarpType);
        noise.SetDomainWarpAmp(domainWarpAmplitude);
        foreach (KeyValuePair<Vector2Int, float> kvp in result)
        {
            float x = kvp.Key.x;
            float y = kvp.Key.y;
            noise.DomainWarp(ref x,ref y);
            resultresult.Add(kvp.Key, noise.GetNoise(x, y));
        }
        return resultresult;
    }

    [Header("World-Shape Noise Settings")]

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
}
