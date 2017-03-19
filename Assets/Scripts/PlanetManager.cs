using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject pointMarker;
    [SerializeField]
    private GameObject trianglePrefab;
    [SerializeField]
    private GameObject pointPrefab;

    [Header("Materials")]
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material centerMaterial;

    [Header("Planet Details")]
    [SerializeField]
    [Range(0,2)]
    private int recursion;
    private int size = 3;

    private List<Vector3> points;
    private List<Triangle> triangles;

    private GameObject marker = null;

    private bool canTurn = true;
    private float turnRate = 40f;

    // Use this for initialization
    private void Start()
    {
        BuildInitialPoints();
        BuildTriangles();
        PlaceTriangles_Click();
        PlacePoints();

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 2f).setEase(LeanTweenType.easeOutBounce).setDelay(0.5f);
    }

    // Update is called once per frame
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1 << 8))
        {
            canTurn = false;           

            HighlightCircle(hitInfo.point);
        }
        else
        {
            DestroyImmediate(marker);
            marker = null;
            canTurn = true;
        }

        RotatePlanet();
    }

    private void HighlightCircle(Vector3 hitPoint)
    {
        Vector3 closestPoint = Vector3.zero;
        var distance = Mathf.Infinity;

        foreach (Vector3 point in points)
        {
            var adjusted = transform.TransformPoint(point);

            var pointDistance = Vector3.Distance(hitPoint, adjusted);

            if (pointDistance < distance)
            {
                distance = pointDistance;
                closestPoint = point;
            }
        }

        var heading = transform.TransformPoint(closestPoint) - transform.position;
        var dist = heading.sqrMagnitude;
        var direction = heading / dist;


        if (marker == null)
        {
            marker = Instantiate(pointMarker, transform.TransformPoint(closestPoint), Quaternion.Euler(direction));
        }

        marker.transform.position = transform.TransformPoint(closestPoint);
        marker.transform.rotation = Quaternion.LookRotation(heading);
    }

    private void RotatePlanet()
    {
        if (canTurn)
        {
            // rotate the the object around the Y-axis
            transform.Rotate(Vector3.up, turnRate * Time.deltaTime);
        }
    }

    #region Build

    private void BuildInitialPoints()
    {
        points = new List<Vector3>();

        // golden ratio
        var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        // Y rectangle
        points.Add(new Vector3(-1f, t, 0f).normalized * Scale());
        points.Add(new Vector3(1f, t, 0f).normalized * Scale());
        points.Add(new Vector3(-1f, -t, 0f).normalized * Scale());
        points.Add(new Vector3(1f, -t, 0f).normalized * Scale());

        // Z rectan
        points.Add(new Vector3(0f, -1f, t).normalized * Scale());
        points.Add(new Vector3(0f, 1f, t).normalized * Scale());
        points.Add(new Vector3(0f, -1f, -t).normalized * Scale());
        points.Add(new Vector3(0f, 1f, -t).normalized * Scale());

        // X rectan
        points.Add(new Vector3(t, 0f, -1f).normalized * Scale());
        points.Add(new Vector3(t, 0f, 1f).normalized * Scale());
        points.Add(new Vector3(-t, 0f, -1f).normalized * Scale());
        points.Add(new Vector3(-t, 0f, 1f).normalized * Scale());
    }

    private void BuildTriangles()
    {
        triangles = new List<Triangle>();

        // 5 faces around point 0
        triangles.Add(new Triangle("(0,5,11)", points[0], points[5], points[11]));
        triangles.Add(new Triangle(points[0], points[5], points[1]));
        triangles.Add(new Triangle(points[0], points[1], points[7]));
        triangles.Add(new Triangle(points[0], points[7], points[10]));
        triangles.Add(new Triangle(points[0], points[10], points[11]));

        // 5 adjacent faces
        triangles.Add(new Triangle(points[1], points[5], points[9]));
        triangles.Add(new Triangle(points[5], points[11], points[4]));
        triangles.Add(new Triangle(points[11], points[10], points[2]));
        triangles.Add(new Triangle(points[10], points[7], points[6]));
        triangles.Add(new Triangle(points[7], points[1], points[8]));

        // 5 faces around point 3
        triangles.Add(new Triangle(points[3], points[9], points[4]));
        triangles.Add(new Triangle(points[3], points[4], points[2]));
        triangles.Add(new Triangle(points[3], points[2], points[6]));
        triangles.Add(new Triangle(points[3], points[6], points[8]));
        triangles.Add(new Triangle(points[3], points[8], points[9]));

        // 5 adjacent faces
        triangles.Add(new Triangle(points[4], points[9], points[5]));
        triangles.Add(new Triangle(points[2], points[4], points[11]));
        triangles.Add(new Triangle(points[6], points[2], points[10]));
        triangles.Add(new Triangle(points[8], points[6], points[7]));
        triangles.Add(new Triangle(points[9], points[8], points[1]));


        var triangleCount = 0;

        for (var index = 0; index < recursion; index++)
        {
            var newTriangles = new List<Triangle>();

            foreach (Triangle oldTriangle in triangles)
            {
                var a = new Vector3(oldTriangle.A.x, oldTriangle.A.y, oldTriangle.A.z);
                var b = new Vector3(oldTriangle.B.x, oldTriangle.B.y, oldTriangle.B.z);
                var c = new Vector3(oldTriangle.C.x, oldTriangle.C.y, oldTriangle.C.z);

                var AB = Subdivide(a, b);
                var BC = Subdivide(b, c);
                var CA = Subdivide(c, a);

                if (!points.Contains(AB))
                {
                    points.Add(AB);
                }

                if (!points.Contains(BC))
                {
                    points.Add(BC);
                }

                if (!points.Contains(CA))
                {
                    points.Add(CA);
                }

                newTriangles.Add(new Triangle((triangleCount++).ToString(), a, AB, CA));
                newTriangles.Add(new Triangle((triangleCount++).ToString(), b, BC, AB));
                newTriangles.Add(new Triangle((triangleCount++).ToString(), c, CA, BC));
                newTriangles.Add(new Triangle((triangleCount++).ToString(), AB, BC, CA));
            }

            triangles = newTriangles;
        }
    }

    private Vector3 Subdivide(Vector3 a, Vector3 b)
    {
        // Find mid-point
        var p = new Vector3((a.x + b.x) / 2f,
                            (a.y + b.y) / 2f,
                            (a.z + b.z) / 2f);

        // Align to sphere
        var v = p.normalized * Scale();

        return v;
    }

    private float Scale()
    {
        return Mathf.Clamp(Mathf.Pow(size, recursion), 2, Mathf.Infinity);
    }

    public void PlaceTriangles_Click()
    {
        foreach (var triangle in triangles)
        {
            var center = triangle.GetCenter();   
            
            var heading = center - Vector3.zero;
            var distance = heading.magnitude;
            var direction = heading.normalized;

            var tri = Instantiate(trianglePrefab, center, Quaternion.LookRotation(direction, triangle.A - center));
            tri.transform.parent = transform;
            tri.name = "Triangle " + triangle.Name;
        }        
    }

    private void PlacePoints()
    {
        foreach (Vector3 point in points)
        {
            var go = Instantiate(pointPrefab, point, Quaternion.identity);
            go.name = point.ToString();
            go.transform.parent = transform;
        }
    }

    #endregion
}