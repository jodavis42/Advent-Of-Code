using System.Diagnostics;

[DebuggerDisplay("({X}, {Y}, {Z})")]
public struct Int3 : IEquatable<Int3>
{
  public int X, Y, Z;
  public Int3(int x, int y, int z)
  {
    X = x;
    Y = y;
    Z = z;
  }
  public override string ToString()
  {
    return $"({X}, {Y}, {Z})";
  }
  public override bool Equals(object? obj)
  {
    return obj != null && obj is Int3 p && Equals(p);
  }

  public bool Equals(Int3 other) => X == other.X && Y == other.Y && Z == other.Z;
  public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
  public static Int3 operator +(Int3 lhs, Int3 rhs)
  {
    return new Int3 { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y, Z = lhs.Z + rhs.Z};
  }
  public static Int3 operator -(Int3 lhs, Int3 rhs)
  {
    return new Int3 { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y, Z = lhs.Z - rhs.Z };
  }
  public static Int3 operator -(Int3 self)
  {
    return new Int3 { X = -self.X, Y = -self.Y, Z = -self.Z };
  }
  public static Int3 operator *(Int3 lhs, int rhs)
  {
    return new Int3 { X = lhs.X * rhs, Y = lhs.Y * rhs, Z = lhs.Z * rhs };
  }
  public static Int3 operator *(int lhs, Int3 rhs)
  {
    return rhs * lhs;
  }
  public int LengthSq() => X * X + Y * Y + Z * Z;
  public void Normalize()
  {
    int lengthSq = LengthSq();
    int length = (int)Math.Sqrt((float)lengthSq);
    X /= length;
    Y /= length;
    Z /= length;
  }
  public static bool operator==(Int3 lhs, Int3 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
  public static bool operator!=(Int3 lhs, Int3 rhs) => !(lhs == rhs);
  public static Int3 Min(Int3 lhs, Int3 rhs) => new Int3(Math.Min(lhs.X, rhs.X), Math.Min(lhs.Y, rhs.Y), Math.Min(lhs.Z, rhs.Z));
  public static Int3 Max(Int3 lhs, Int3 rhs) => new Int3(Math.Max(lhs.X, rhs.X), Math.Max(lhs.Y, rhs.Y), Math.Max(lhs.Z, rhs.Z));
  public static int ManhattanDistance(Int3 lhs, Int3 rhs)
  {
    var dir = lhs - rhs;
    return Math.Abs(dir.X) + Math.Abs(dir.Y);
  }
}