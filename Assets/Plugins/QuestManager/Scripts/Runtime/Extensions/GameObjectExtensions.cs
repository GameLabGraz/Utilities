using UnityEngine;

public static class GameObjectExtensions
{
    public static Vector3 GetMeshSize(this GameObject gameObject)
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        return meshRenderer == null ? Vector3.zero : meshRenderer.bounds.size;
    }
}
