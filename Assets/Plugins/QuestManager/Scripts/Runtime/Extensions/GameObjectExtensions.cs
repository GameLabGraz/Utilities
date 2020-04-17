using UnityEngine;

namespace GEAR.QuestManager.Extensions
{
    public static class GameObjectExtensions
    {
        public static Vector3 GetRendererSize(this GameObject gameObject)
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            return meshRenderer == null ? Vector3.zero : meshRenderer.bounds.size;
        }

        public static Vector3 GetMeshSize(this GameObject gameObject)
        {
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            return meshFilter == null ? Vector3.zero : meshFilter.sharedMesh.bounds.size;
        }
    }
}