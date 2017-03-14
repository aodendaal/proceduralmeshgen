using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Simple container for the mesh indices 
/// </summary>
public class Triangle
{
    public string Name { get; set; }
    public Vector3 A { get; set; }
    public Vector3 B { get; set; }
    public Vector3 C { get; set; }

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        Name = string.Format("({0},{1},{2})", A, B, C);
        A = a;
        B = b;
        C = c;
    }

    public Triangle(string name, Vector3 a, Vector3 b, Vector3 c)
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

    public override string ToString()
    {
        return Name;
    }
}