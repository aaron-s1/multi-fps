using UnityEngine;

public class TestFloorModification : MonoBehaviour
{
    public GameObject targetObject; // Reference to the object with the mesh you want to carve
    public Vector3 holePosition; // Position of the hole in local space
    public float holeRadius = 1f; // Radius of the hole

    private void Start()
    {
        // Ensure the target object has a MeshFilter component
        MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();
        if (targetMeshFilter == null)
        {
            Debug.LogError("Target object does not have a MeshFilter component!");
            return;
        }

        // Create a new mesh instance based on the target object's mesh
        Mesh carvedMesh = Instantiate(targetMeshFilter.sharedMesh);

        // Get the position of the hole in world space
        Vector3 worldHolePosition = targetObject.transform.TransformPoint(holePosition);

        // Modify the vertices and triangles to carve out a hole
        Vector3[] vertices = carvedMesh.vertices;
        int[] triangles = carvedMesh.triangles;

        for (int i = 0; i < vertices.Length; i++)
        {
            // Check if the vertex is inside the hole radius
            if (Vector3.Distance(vertices[i], worldHolePosition) <= holeRadius)
            {
                // Move the vertex to a position inside the hole
                vertices[i] = worldHolePosition;
            }
        }

        // Update the vertices and triangles of the carved mesh
        carvedMesh.vertices = vertices;
        carvedMesh.triangles = triangles;
        carvedMesh.RecalculateNormals();
        carvedMesh.RecalculateBounds();

        // Assign the carved mesh to the target object's MeshFilter
        targetMeshFilter.mesh = carvedMesh;
    }
}