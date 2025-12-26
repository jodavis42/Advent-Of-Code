using System.Numerics;

class Matrix<T> where T : struct, INumber<T>
{
  public int CountX { get; private set; }
  public int CountY { get; private set; }
  public Matrix(int countX, int countY)
  {
    CountX = countX;
    CountY = countY;
    T defaultValue = default;
    for (var y = 0; y < countY; y++)
    {
      for (var x = 0; x < countX; ++x)
        Values.Add(defaultValue);
    }
  }
  public int GetIndex(int y, int x) => y * CountX + x;
  public T Get(int y, int x) => Values[GetIndex(y, x)];
  public void Set(int y, int x, T value)
  {
    Values[GetIndex(y, x)] = value;
  }
  public T this[int y, int x]
  {
    get => Get(y, x);
    set => Set(y, x, value);
  }
  public Matrix<T> Transpose()
  {
    var result = new Matrix<T>(CountY, CountX);
    for (var y = 0; y < CountY; ++y)
    {
      for (var x = 0; x < CountX; ++x)
        result[x, y] = this[y, x];
    }
    return result;
  }
  public static Matrix<T> Multiply(Matrix<T> M0, Matrix<T> M1)
  {
    var result = new Matrix<T>(M1.CountX, M0.CountY);
    for (var y = 0; y < result.CountY; ++y)
    {
      for (var x = 0; x < result.CountX; ++x)
      {
        T value = default;
        for (var j = 0; j < M0.CountX; ++j)
        {
          value += M0[y, j] * M1[j, x];
        }
        result[y, x] = value;
      }
    }
    return result;
  }
  public static Vector<T> Multiply(Matrix<T> M, Vector<T> V)
  {
    var result = new Vector<T>(M.CountY);
    for (var y = 0; y < M.CountY; ++y)
    {
      T value = default;
      for (var x = 0; x < M.CountX; ++x)
        value += M[y, x] * V[x];
      result[y] = value;
    }
    return result;
  }
  List<T> Values = new List<T>();
}