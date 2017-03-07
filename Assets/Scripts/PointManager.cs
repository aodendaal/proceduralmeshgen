using System;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    private class Triangle
    {
        public string Name { get; set; }
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }

        public Triangle()
        {

        }

        public Triangle (string name, Vector3 a, Vector3 b, Vector3 c)
        {
            Name = name;
            A = a;
            B = b;
            C = c;
        }

        public Vector3 GetCenter()
        {
            return new Vector3((A.x + B.x + C.x) / 3f, (A.y + B.y + C.y) / 3f, (A.z + B.z + C.z) / 3f);
        }
    }

    [SerializeField]
    private GameObject pointMarker;

    [SerializeField]
    public GameObject trianglePrefab;

    [Header("Materials")]
    [SerializeField]
    public Material highlightMaterial;
    [SerializeField]
    public Material centerMaterial;

    private List<Vector3> points;
    private List<Triangle> triangles;

    // Use this for initialization
    private void Start()
    {
        BuildPoints();
        BuildTriangles();
    }

    private void BuildPoints()
    {
        points = new List<Vector3>();

        // golden ratio
        var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        // Y rectangle
        points.Add(new Vector3(-1, t, 0));
        points.Add(new Vector3(1, t, 0));
        points.Add(new Vector3(-1, -t, 0));
        points.Add(new Vector3(1, -t, 0));

        // Z rectangle
        points.Add(new Vector3(0, -1, t));
        points.Add(new Vector3(0, 1, t));
        points.Add(new Vector3(0, -1, -t));
        points.Add(new Vector3(0, 1, -t));

        // X rectangle
        points.Add(new Vector3(t, 0, -1));
        points.Add(new Vector3(t, 0, 1));
        points.Add(new Vector3(-t, 0, -1));
        points.Add(new Vector3(-t, 0, 1));
    }

    private void BuildTriangles()
    {
        triangles = new List<Triangle>();

        triangles.Add(new Triangle("(0,11,5)", points[0], points[5], points[11]));
        triangles.Add(new Triangle("(0,5,1)", points[0], points[5], points[1]));
        triangles.Add(new Triangle("(0,1,7)", points[0], points[1], points[7]));
        triangles.Add(new Triangle("(0,7,10)", points[0], points[7], points[10]));
        triangles.Add(new Triangle("(0,10,11)", points[0], points[10], points[11]));
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        if (points == null || points.Count == 0)
            return;

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(points[0], points[5]);
        Gizmos.DrawLine(points[5], points[11]);
        Gizmos.DrawLine(points[11], points[0]);
    }

    public void PlacePointMarkers_Click()
    {
        var count = 0;
        foreach (var point in points)
        {
            var go = Instantiate(pointMarker, point, Quaternion.identity);
            go.name = "Point " + count.ToString() + " " + point.ToString();

            if (count == 0 || count == 5 || count == 11)
            {
                go.GetComponent<Renderer>().material = highlightMaterial;
            }
            count++;
        }
    }

    public void PlaceTriangles_Click()
    {

        foreach (var triangle in triangles)
        {
            var center = triangle.GetCenter();            
            var heading = center - Vector3.zero;
            var distance = heading.magnitude;
            var direction = heading / distance;
            var tri = Instantiate(trianglePrefab, center, Quaternion.LookRotation(direction, Vector3.up));
            tri.name = "Triangle " + triangle.Name;
        }
    }
}