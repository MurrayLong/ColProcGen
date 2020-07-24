using UnityEngine;

public static class TransformExtensions
{
    public static Transform DeleteChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }
        return transform;
    }
}
