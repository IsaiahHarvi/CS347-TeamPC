using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyTerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int depth = 256;
    public int heightScale = 20;
    public float detailScale = 25.0f;


    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2.0f;
    public int seed = 0;
    public Vector2 offset;

    void Start()
    {
        //switched to editor usage
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, heightScale, depth);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int z)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (x + offset.x) / width * detailScale * frequency + seed;
            float zCoord = (z + offset.y) / depth * detailScale * frequency + seed;

            float perlinValue = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        float rawHeight = Mathf.Clamp01((noiseHeight + 1) / 2);


        float smoothStart = 0.3f;  
        float smoothEnd = 0.6f;    
        if (rawHeight > smoothStart && rawHeight < smoothEnd)
        {
            float smoothFactor = 0.5f;
            rawHeight = Mathf.Lerp(smoothStart, smoothEnd, (rawHeight - smoothStart) / (smoothEnd - smoothStart)) * smoothFactor + rawHeight * (1 - smoothFactor);
        }

        return rawHeight;
    }


    public void Generate()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
}
