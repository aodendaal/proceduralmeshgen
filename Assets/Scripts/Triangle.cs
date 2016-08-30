using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Simple container for the mesh indices 
/// </summary>
public struct triangle
{
    public int A;
    public int B;
    public int C;

    public triangle(int a, int b, int c)
    {
        this.A = a;
        this.B = b;
        this.C = c;
    }

    public override string ToString()
    {
        return string.Format("({0},{1},{2})", A, B, C);
    }
}