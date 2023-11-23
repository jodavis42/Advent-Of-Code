namespace AdventOfCode2022
{
  public class HashGrid2d<T>
  {
    public Dictionary<Int2, T> Cells = new Dictionary<Int2, T>();
    public Int2 Min;
    public Int2 Max;
    
    public T Get(Int2 position, T defaultValue)
    {
      if (!Cells.TryGetValue(position, out var cellValue))
        return defaultValue;
      return cellValue;
    }
    public void Set(Int2 position, T cellValue)
    {
      Cells[position] = cellValue;
    }
    public void BuildAabb()
    {
      Min = new Int2(int.MaxValue, int.MaxValue);
      Max = new Int2(int.MinValue, int.MinValue);
      foreach (var pos in Cells.Keys)
      {
        Min.X = Math.Min(Min.X, pos.X);
        Max.X = Math.Max(Max.X, pos.X);
        Min.Y = Math.Min(Min.Y, pos.Y);
        Max.Y = Math.Max(Max.Y, pos.Y);
      }
    }
  }
}
