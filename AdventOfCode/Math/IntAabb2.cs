
using System.Diagnostics;

[DebuggerDisplay("({Min}, {Max})")]
struct IntAabb2 : IEquatable<IntAabb2>
{
  public Int2 Min = new Int2();
  public Int2 Max = new Int2();
  public IntAabb2() { }
  public IntAabb2(Int2 p0, Int2 p1)
  {
    Min = Int2.Min(p0, p1);
    Max = Int2.Max(p0, p1);
  }
  public Int64 Area()
  {
    Int64 xMin = Min.X;
    Int64 xMax = Max.X;
    Int64 yMin = Min.Y;
    Int64 yMax = Max.Y;
    Int64 area = (xMax - xMin + 1) * (yMax - yMin + 1);
    return area;
  }
  public bool IntersectsExclusive(IntAabb2 other)
  {
    return !(Max.X <= other.Min.X || other.Max.X <= Min.X ||
      Max.Y <= other.Min.Y || other.Max.Y <= Min.Y);
  }

  public bool Equals(IntAabb2 other)
  {
    return Min == other.Min && Max == other.Max;
  }
}