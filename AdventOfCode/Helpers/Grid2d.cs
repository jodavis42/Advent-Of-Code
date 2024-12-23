﻿public class Grid2d<T>
{
  public int Width;
  public int Height;
  public List<T> Cells = new List<T>();

  public void Initialize(int width, int height, T defaultValue)
  {
    Width = width;
    Height = height;
    for (var i = 0; i < width * height; ++i)
      Cells.Add(defaultValue);
  }

  public int GetIndex(int x, int y)
  {
    return y * Width + x;
  }
  public T Get(int x, int y) => Cells[GetIndex(x, y)];
  public void Set(int x, int y, T value) => Cells[GetIndex(x, y)] = value;
  public bool IsValidPosition(int x, int y)
  {
    return 0 <= x && x < Width && 0 <= y && y < Height;
  }
  public T TryGetValue(int x, int y, T defaultValue)
  {
    if (IsValidPosition(x, y))
      return Get(x, y);
    return defaultValue;
  }
  public T this[int x, int y]
  {
    get => Get(x, y);
    set => Set(x, y, value);
  }

  public void Print()
  {
    for (var y = 0; y < Height; ++y)
    {
      for (var x = 0; x < Width; ++x)
        Console.Write(this[x, y]);
      Console.WriteLine();
    }
  }
}

public static class GridUtils
{
  public static void Parse(this Grid2d<char> self, string[] lines, char defaultValue = '.')
  {
    self.Initialize(lines[0].Length, lines.Length, defaultValue);
    for (var y = 0; y < self.Height; ++y)
    {
      for (var x = 0; x < self.Width; ++x)
        self[x, y] = lines[y][x];
    }
  }
  public static int DefaultCharToInt(char c)
  {
    return c - '0';
  }
  public static void Parse(this Grid2d<int> self, string[] lines, int defaultValue = -1)
  {
    self.Parse(lines, DefaultCharToInt, defaultValue);
  }
  public static void Parse(this Grid2d<int> self, string[] lines, Func<char, int> ToIntFunc, int defaultValue = -1)
  {
    self.Initialize(lines[0].Length, lines.Length, defaultValue);
    for (var y = 0; y < self.Height; ++y)
    {
      for (var x = 0; x < self.Width; ++x)
      {
        var c = lines[y][x];
        self[x, y] = DefaultCharToInt(c);
      }
    }
  }
}
