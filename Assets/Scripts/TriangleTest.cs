using UnityEngine;
using System.Collections;

public class TriangleTest : MonoBehaviour {

    public Material material;

    // Use this for initialization
    void Start () {
        var triangle = new GameObject("triangle", typeof(MeshFilter), typeof(MeshRenderer));

        var meshFilter = triangle.GetComponent<MeshFilter>();
        var meshRenderer = triangle.GetComponent<MeshRenderer>();

        var mesh = BuildMesh();

        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private Mesh BuildMesh()
    {
        var mesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(0f, 0f, 0f),
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f)
        };

        int[] triangles = {
            0, 1, 2
        };

        Vector2[] uvs = {
            new Vector2(0.5f, 1f),
            new Vector2(0f, 0.2f),
            new Vector2(1f, 0.2f),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }
}
