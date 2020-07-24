using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGeometry))]
public class TerrainUI : Editor
{
    TerrainGeometry terrainGenerator;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            terrainGenerator.Generate();
        }
    }

    void OnEnable()
    {
        terrainGenerator = (TerrainGeometry)target;
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }

}
