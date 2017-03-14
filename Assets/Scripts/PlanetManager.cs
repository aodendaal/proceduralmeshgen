using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [Header("Prefabs")]
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

    private GameObject marker = null;

    private bool canTurn = true;
    private float turnRate = 40f;

    // Use this for initialization
    private void Start()
    {
        BuildPoints();
        BuildTriangles();
        PlaceTriangles_Click();
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

        if (marker == null)
        {
            marker = Instantiate(pointMarker, transform.TransformPoint(closestPoint), Quaternion.identity);
        }

        marker.transform.position = transform.TransformPoint(closestPoint);
    }

    private void RotatePlanet()
    {
        if (canTurn)
        {
            // rotate the the object around the Y-axis
            transform.Rotate(Vector3.up, turnRate * Time.deltaTime);
        }
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

        // 5 faces around point 0
        triangles.Add(new Triangle("(0,11,5)", points[0], points[5], points[11]));
        triangles.Add(new Triangle("(0,5,1)", points[0], points[5], points[1]));
        triangles.Add(new Triangle("(0,1,7)", points[0], points[1], points[7]));
        triangles.Add(new Triangle("(0,7,10)", points[0], points[7], points[10]));
        triangles.Add(new Triangle("(0,10,11)", points[0], points[10], points[11]));

        // 5 adjacent faces
        triangles.Add(new Triangle("(1,5,9)", points[1], points[5], points[9]));
        triangles.Add(new Triangle("(5,11,4)", points[5], points[11], points[4]));
        triangles.Add(new Triangle("(11,10,2)", points[11], points[10], points[2]));
        triangles.Add(new Triangle("(10,7,6)", points[10], points[7], points[6]));
        triangles.Add(new Triangle("(7,1,8)", points[7], points[1], points[8]));

        // 5 faces around point 3
        triangles.Add(new Triangle("(3,9,4)", points[3], points[9], points[4]));
        triangles.Add(new Triangle("(3,4,2)", points[3], points[4], points[2]));
        triangles.Add(new Triangle("(3,2,6)", points[3], points[2], points[6]));
        triangles.Add(new Triangle("(3,6,8)", points[3], points[6], points[8]));
        triangles.Add(new Triangle("(3,8,9)", points[3], points[8], points[9]));

        // 5 adjacent faces
        triangles.Add(new Triangle("(4,9,5)", points[4], points[9], points[5]));
        triangles.Add(new Triangle("(2,4,11)", points[2], points[4], points[11]));
        triangles.Add(new Triangle("(6,2,10)", points[6], points[2], points[10]));
        triangles.Add(new Triangle("(8,6,7)", points[8], points[6], points[7]));
        triangles.Add(new Triangle("(9,8,1)", points[9], points[8], points[1]));
    }

    public void PlaceTriangles_Click()
    {
        foreach (var triangle in triangles)
        {
            var center = triangle.GetCenter();            
            var heading = center - Vector3.zero;
            var distance = heading.magnitude;
            var direction = heading / distance;

            var tri = Instantiate(trianglePrefab, center, Quaternion.LookRotation(direction, triangle.A - center));
            tri.transform.parent = transform;
            tri.name = "Triangle " + triangle.Name;

            transform.localScale = Vector3.zero;

            LeanTween.scale(gameObject, Vector3.one, 2f).setEase(LeanTweenType.easeOutBounce).setDelay(0.5f);
        }
    }
}