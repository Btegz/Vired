using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MapSettings", menuName = "Procedural")]
public class MapSettings : ScriptableObject
{
    FastNoiseLite noise = new FastNoiseLite();

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

    [SerializeField] private float Frequency;

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


    public Dictionary<Vector2Int, float> NoiseData(FastNoiseLite.NoiseType noiseType)
    {
        noise.SetNoiseType(noiseType);
        noise.SetFrequency(Frequency);
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

}
