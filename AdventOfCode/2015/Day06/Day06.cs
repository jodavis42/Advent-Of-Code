using System.Diagnostics;

namespace AdventOfCode2015
{
  internal class Day06
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      var grid = new Grid2d<bool>();
      grid.Initialize(1000, 1000, false);
      foreach(var line in lines)
      {
        var split = line.Split(' ');
        if (split[0] == "toggle")
        {
          var min = Parse(split[1]);
          var max = Parse(split[3]);
          Toggle(grid, min, max, true);
        }
        else if (split[1] == "on")
        {
          var min = Parse(split[2]);
          var max = Parse(split[4]);
          Set(grid, min, max, true);
        }
        else
        {
          var min = Parse(split[2]);
          var max = Parse(split[4]);
          Set(grid, min, max, false);
        }
      }
      Console.WriteLine(CountSet(grid));
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      var grid = new Grid2d<uint>();
      grid.Initialize(1000, 1000, 0u);
      foreach (var line in lines)
      {
        var split = line.Split(' ');
        if (split[0] == "toggle")
        {
          var min = Parse(split[1]);
          var max = Parse(split[3]);
          Toggle2(grid, min, max, true);
        }
        else if (split[1] == "on")
        {
          var min = Parse(split[2]);
          var max = Parse(split[4]);
          Set2(grid, min, max, true);
        }
        else
        {
          var min = Parse(split[2]);
          var max = Parse(split[4]);
          Set2(grid, min, max, false);
        }
      }
      Console.WriteLine(CountBrightness(grid));
    }
    Int2 Parse(string text)
    {
      var split = text.Split(',');
      var result = new Int2();
      result.X = int.Parse(split[0]);
      result.Y = int.Parse(split[1]);
      return result;
    }
    void Set(Grid2d<bool> grid, Int2 min, Int2 max, bool value)
    {
      for(var y = min.Y; y <= max.Y; ++y)
      {
        for (var x = min.X; x <= max.X; ++x)
          grid[x, y] = value;
      }
    }
    void Toggle(Grid2d<bool> grid, Int2 min, Int2 max, bool value)
    {
      for (var y = min.Y; y <= max.Y; ++y)
      {
        for (var x = min.X; x <= max.X; ++x)
          grid[x, y] = !grid[x, y];
      }
    }
    uint CountSet(Grid2d<bool> grid)
    {
      uint count = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          if (grid[x,y]) 
            count++;
        }
      }
      return count;
    }


    void Set2(Grid2d<uint> grid, Int2 min, Int2 max, bool value)
    {
      for (var y = min.Y; y <= max.Y; ++y)
      {
        for (var x = min.X; x <= max.X; ++x)
        {
          var gridValue = grid[x, y];
          if (value)
            grid[x, y] = gridValue + 1;
          else if (gridValue != 0)
            grid[x, y] = gridValue - 1;
        }
      }
    }
    void Toggle2(Grid2d<uint> grid, Int2 min, Int2 max, bool value)
    {
      for (var y = min.Y; y <= max.Y; ++y)
      {
        for (var x = min.X; x <= max.X; ++x)
          grid[x, y] = grid[x, y] + 2;
      }
    }
    uint CountBrightness(Grid2d<uint> grid)
    {
      uint brightness = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          brightness += grid[x, y];
        }
      }
      return brightness;
    }
  }
}
