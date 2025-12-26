using System.Numerics;
using System.Text;

class Vector<T> where T : struct, INumber<T>
{
  public int Count => Values.Count;
  public Vector(int count)
  {
    T defaultValue = default;
    for (var i = 0; i < count; i++)
      Values.Add(defaultValue);
  }
  public T Get(int i) => Values[i];
  public void Set(int i, T value)
  {
    Values[i] = value;
  }
  public T this[int i]
  {
    get => Get(i);
    set => Set(i, value);
  }

  public override string ToString()
  {
    var builder = new StringBuilder();
    builder.AppendJoin(" ", Values);
    return builder.ToString();
  }
  public override bool Equals(object? obj)
  {
    return obj != null && obj is Vector<T> p && Equals(p);
  }

  public bool Equals(Vector<T> other)
  {
    if (other.Count != Count)
      return false;

    for(int i = 0; i < Count; ++i)
    {
      if (this[i] != other[i])
        return false;
    }
    return true;
  }
  public override int GetHashCode()
  {
    var result = this[0].GetHashCode();
    for(var i = 1; i < Count; ++i)
      result = HashCode.Combine(result, this[i].GetHashCode());
    return result;
  }
  public static Vector<T> operator +(Vector<T> lhs, Vector<T> rhs)
  {
    ValidateDimensions(lhs, rhs);
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = lhs[i] + rhs[i];
    return result;
  }
  public static Vector<T> operator -(Vector<T> lhs, Vector<T> rhs)
  {
    ValidateDimensions(lhs, rhs);
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = lhs[i] - rhs[i];
    return result;
  }
  public static Vector<T> operator -(Vector<T> self)
  {
    var result = new Vector<T>(self.Count);
    for (var i = 0; i < self.Count; ++i)
      result[i] = -self[i];
    return result;
  }
  public static Vector<T> operator *(Vector<T> lhs, Vector<T> rhs)
  {
    ValidateDimensions(lhs, rhs);
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = lhs[i] * rhs[i];
    return result;
  }
  public static Vector<T> operator *(Vector<T> lhs, T rhs)
  {
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = lhs[i] * rhs;
    return result;
  }
  public static Vector<T> operator /(Vector<T> lhs, T rhs)
  {
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = lhs[i] / rhs;
    return result;
  }

  public static Vector<T> operator *(T lhs, Vector<T> rhs) => rhs * lhs;
  public static bool operator ==(Vector<T> lhs, Vector<T> rhs) => lhs.Equals(rhs);
  public static bool operator !=(Vector<T> lhs, Vector<T> rhs) => !lhs.Equals(rhs);
  public static Vector<T> Min(Vector<T> lhs, Vector<T> rhs)
  {
    ValidateDimensions(lhs, rhs);
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = T.Min(lhs[i], rhs[i]);
    return result;
  }
  public static Vector<T> Max(Vector<T> lhs, Vector<T> rhs)
  {
    ValidateDimensions(lhs, rhs);
    var result = new Vector<T>(lhs.Count);
    for (var i = 0; i < lhs.Count; ++i)
      result[i] = T.Max(lhs[i], rhs[i]);
    return result;
  }

  static void ValidateDimensions(Vector<T> lhs, Vector<T> rhs)
  {
    if (lhs.Count != rhs.Count)
      throw new Exception();
  }

  List<T> Values = new List<T>();
}