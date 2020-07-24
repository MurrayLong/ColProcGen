using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class TerrainGeometry : MonoBehaviour
{
    [Header("Settings")]
    public int resolution = 255;
    public float scale = 100;
    public float elevationScale = 10;
    public float frequency = 5;
    public Material groundMaterial;
    public int trees = 50;

    System.Random rng;

    public void Generate() {
        rng = new System.Random();
        gameObject.transform.DeleteChildren();

        CreateGround();
        CreateForest();
    }

    float HeightAt(Vector2 position) 
    {
        var scaled = position/scale * frequency;
        return elevationScale *
            (0.5f +
                (float)(Math.Cos(scaled.x) * Math.Sin(scaled.y))/2.0f
            );
    }

    float HeightAt(Vector3 position) => HeightAt(new Vector2(position.x, position.z));

    void CreateGround()
    {
        var terrain =  new GameObject("terrain").transform;
        terrain.transform.parent = transform;
        terrain.transform.localScale = Vector3.one;
        terrain.transform.localPosition = Vector3.zero;
        terrain.transform.localRotation = Quaternion.identity;


        var mesh = CreateGroundMesh();
        terrain.gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
        terrain.gameObject.AddComponent<MeshRenderer>().sharedMaterial = groundMaterial;
        terrain.gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }
    void CreateTree(Vector2 position, Transform parent)
    {
        var tree = new GameObject();
        tree.name = "Tree";
        tree.transform.parent = parent;
        tree.transform.localPosition = new Vector3(position.x, HeightAt(position), position.y);
        tree.transform.localScale = Vector3.one * 0.5f;

        var leaves = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leaves.transform.parent = tree.transform;
        leaves.transform.localPosition = new Vector3(0, 10, 0);
        leaves.transform.localScale = 5 * Vector3.one + 4 * Vector3.up;
        leaves.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Leaves");

        var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        trunk.transform.parent = tree.transform;
        trunk.transform.localPosition = Vector3.zero + 3 * Vector3.up;
        trunk.transform.localScale = new Vector3(1, 3, 1);
        trunk.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Bark");
    }

    Mesh CreateGroundMesh()
    {
        var mesh = Geometry.Plane(resolution);

        mesh.vertices = mesh.vertices
            .Select(v => v * scale)
            .Select(v => v + Vector3.up * HeightAt(v))
            .ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    void CreateForest()
    {
        var forest = new GameObject();
        forest.transform.parent = transform;
        forest.transform.localScale = Vector3.one;
        forest.transform.localPosition = Vector3.zero;
        forest.name = "Forest";

        for (int i = 0; i < trees; i++)
        {
            var position = scale * (new Vector2(rng.Next(100) / 100f, rng.Next(100) / 100f) - Vector2.one * 0.5f);
            CreateTree(position, forest.transform);
        }
    }
}
