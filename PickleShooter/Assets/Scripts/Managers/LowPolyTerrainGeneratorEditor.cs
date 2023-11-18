using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LowPolyTerrainGenerator))]
public class LowPolyTerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector

        LowPolyTerrainGenerator terrainGenerator = (LowPolyTerrainGenerator)target;

        if (GUILayout.Button("Generate Terrain"))
        {
            terrainGenerator.Generate();
        }
    }
}
