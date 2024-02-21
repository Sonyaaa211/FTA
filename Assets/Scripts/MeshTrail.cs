using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;
    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;
    public Material mat;
    private bool isTrailActive;
    public SkinnedMeshRenderer[] skinnedMeshRenderers;

    public Mesh testMesh;

    private void Start()
    {
        // Start the coroutine to create the mesh trail

    }

    public IEnumerator ActiveTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            }

            GameObject obj = new GameObject();
            obj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

            MeshRenderer mr = obj.AddComponent<MeshRenderer>();
            MeshFilter mf = obj.AddComponent<MeshFilter>();

            Mesh myMesh = testMesh;
            Mesh newMesh = new Mesh();

            int[] oldTrianges = myMesh.GetTriangles(0);

            int count = 0;
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int x = 0; x < oldTrianges.Length; x++)
            {
                int current = oldTrianges[x];

                if (!dictionary.ContainsKey(current))
                {
                    dictionary.Add(current, count);
                    count = count + 1;
                }
            }

            int[] newTriangles = new int[oldTrianges.Length];
            for (int x = 0; x < oldTrianges.Length; x++)
            {
                newTriangles[x] = dictionary[oldTrianges[x]];
            }

            Vector3[] oldVerts = myMesh.vertices;
            Vector3[] newVerts = new Vector3[count];
            foreach (KeyValuePair<int, int> pair in dictionary)
            {
                int oldVertIndex = pair.Key;
                int newVertIndex = pair.Value;
                newVerts[newVertIndex] = oldVerts[oldVertIndex];
            }

            newMesh.vertices = newVerts;
            newMesh.triangles = newTriangles;
            newMesh.uv = new Vector2[newVerts.Length];
            newMesh.RecalculateNormals();
            newMesh.Optimize();

            mf.mesh = newMesh;
            mr.material = mat;

            Destroy(obj, meshDestroyDelay);

            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }
}
