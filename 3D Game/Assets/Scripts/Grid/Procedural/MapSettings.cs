using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Procedural")]
public class MapSettings : ScriptableObject
{
    FastNoiseLite noise = new FastNoiseLite();

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

    [SerializeField] private float frequency; 
    public float Frequency
    {
        get { return frequency; }
        set { frequency = value; }
    }

    [SerializeField] Vector2 noiseThresholds;

    public Vector2 NoiseThresholds
    {
        get { return noiseThresholds; }
        set { noiseThresholds = value; }
    }

    [SerializeField] float distanceThreshold;

    public float DistanceThreshold
    {
        get { return distanceThreshold; }
        set { distanceThreshold = value; }
    }


    public Dictionary<Vector2Int, float> NoiseData(FastNoiseLite.NoiseType noiseType,float frequency)
    {
        noise.SetNoiseType(noiseType);
        noise.SetFrequency(frequency);
        noise.SetSeed(Random.Range(1000, 2000));
        Dictionary<Vector2Int, float> result = new Dictionary<Vector2Int, float>();

        List<Vector2Int> coordinates = HexGridUtil.GenerateRectangleShapedGrid(noiseDataSize.x, noiseDataSize.y);
        foreach (Vector2Int c in coordinates)
        {
            result.Add(c, noise.GetNoise(c.x, c.y));
        }

        //for (int x = 0; x < NoiseDataSize.x; x++)
        //{
        //    for (int y = 0; y < NoiseDataSize.y; y++)
        //    {
        //        result.Add(new Vector2Int(x, y), noise.GetNoise(x, y));
        //    }
        //}
        return result;
    }

    [Header("World-Shape Noise Settings")]

    [SerializeField] private Vector2Int m_noiseDataSize;

    public Vector2Int M_NoiseDataSize
    {
        get { return m_noiseDataSize; }
        set { m_noiseDataSize = value; }
    }

    [SerializeField] private FastNoiseLite.NoiseType m_noiseType1;

    public FastNoiseLite.NoiseType M_NoiseType1
    {
        get { return m_noiseType1; }
        set { m_noiseType1 = value; }
    }

    [SerializeField] private FastNoiseLite.NoiseType m_noiseType2;

    public FastNoiseLite.NoiseType M_NoiseType2
    {
        get { return m_noiseType2; }
        set { m_noiseType2 = value; }
    }

    [SerializeField] private float m_frequency;

    public float M_Frequency
    {
        get { return m_frequency; }
        set { m_frequency = value; }
    }

    [SerializeField] Vector2 m_noiseThresholds;

    public Vector2 M_NoiseThresholds
    {
        get { return m_noiseThresholds; }
        set { m_noiseThresholds = value; }
    }

    [SerializeField] float m_distanceThreshold;

    public float M_DistanceThreshold
    {
        get { return m_distanceThreshold; }
        set { m_distanceThreshold = value; }
    }

}
