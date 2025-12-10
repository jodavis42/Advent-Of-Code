using System.Diagnostics;

[DebuggerDisplay("({X}, {Y})")]
public struct Int2 : IEquatable<Int2>
{
  public int X, Y;
  public Int2(int x, int y)
  {
    X = x;
    Y = y;
  }
  public override string ToString()
  {
    return $"({X}, {Y})";
  }
  public override bool Equals(object? obj)
  {
    return obj != null && obj is Int2 p && Equals(p);
  }

  public int Get(int i) => i == 0 ? X : Y;
  public void Set(int i, int value)
  {
    if (i == 0)
      X = value;
    else
      Y = value;
  }
  public int this[int i]
  {
    get => Get(i);
    set => Set(i, value);
  }
  public bool Equals(Int2 other) => X == other.X && Y == other.Y;
  public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
  public static Int2 operator +(Int2 lhs, Int2 rhs)
  {
    return new Int2 { X = lhs.X + rhs.X, Y = lhs.Y + rhs.Y };
  }
  public static Int2 operator -(Int2 lhs, Int2 rhs)
  {
    return new Int2 { X = lhs.X - rhs.X, Y = lhs.Y - rhs.Y };
  }
  public static Int2 operator -(Int2 self)
  {
    return new Int2 { X = -self.X, Y = -self.Y };
  }
  public static Int2 operator *(Int2 lhs, int rhs)
  {
    return new Int2 { X = lhs.X * rhs, Y = lhs.Y * rhs };
  }
  public static Int2 operator *(int lhs, Int2 rhs)
  {
    return rhs * lhs;
  }
  public int LengthSq() => X * X + Y * Y;
  public void Normalize()
  {
    int lengthSq = LengthSq();
    int length = (int)Math.Sqrt((float)lengthSq);
    X /= length;
    Y /= length;
  }
  public static bool operator==(Int2 lhs, Int2 rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
  public static bool operator!=(Int2 lhs, Int2 rhs) => !(lhs == rhs);
  public static Int2 Min(Int2 lhs, Int2 rhs) => new Int2(Math.Min(lhs.X, rhs.X), Math.Min(lhs.Y, rhs.Y));
  public static Int2 Max(Int2 lhs, Int2 rhs) => new Int2(Math.Max(lhs.X, rhs.X), Math.Max(lhs.Y, rhs.Y));
  public static int ManhattanDistance(Int2 lhs, Int2 rhs)
  {
    var dir = lhs - rhs;
    return Math.Abs(dir.X) + Math.Abs(dir.Y);
  }
}