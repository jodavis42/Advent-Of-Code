namespace AdventOfCode2024
{
  internal class Day08
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      Print(grid);
      var cacheMap = BuildFrequencyCache(grid);
      var counts = new Grid2d<int>();
      counts.Initialize(grid.Width, grid.Height, 0);
      foreach(var cache in cacheMap.Values)
      {
        Test(grid, counts, cache);
      }
      var result = Count(counts);
      Console.WriteLine(result);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      Print(grid);
      var cacheMap = BuildFrequencyCache(grid);
      var counts = new Grid2d<int>();
      counts.Initialize(grid.Width, grid.Height, 0);
      foreach (var cache in cacheMap.Values)
      {
        Test2(grid, counts, cache);
      }
      var result = Count(counts);
      Console.WriteLine(result);
    }
    Grid2d<char> Parse(string[] lines)
    {
      var result = new Grid2d<char>();
      result.Initialize(lines[0].Length, lines.Length, '.');
      for (var y = 0; y < result.Height; ++y)
      {
        for (var x = 0; x < result.Width; ++x)
          result[x, y] = lines[y][x];
      }
      return result;
    }
    void Print(Grid2d<char> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
          Console.Write(grid[x, y]);
        Console.WriteLine();
      }
      Console.WriteLine();
    }
    class FrequencyCache
    {
      public char Id;
      public List<Int2> Positions = new List<Int2>();
    }
    Dictionary<char, FrequencyCache> BuildFrequencyCache(Grid2d<char> grid)
    {
      var result = new Dictionary<char, FrequencyCache>();
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var c = grid[x, y];
          if (c == '.')
            continue;
          if(!result.TryGetValue(c, out var cache))
          {
            cache = new FrequencyCache { Id = c };
            result.Add(cache.Id, cache);
          }
          cache.Positions.Add(new Int2(x, y));
        }
      }
      return result;
    }
    void Test(Grid2d<char> grid, Grid2d<int> counts, FrequencyCache cache)
    {
      for(int i = 1; i < cache.Positions.Count; ++i)
      {
        for(int j = 0; j < i; ++j)
        {
          var p0 = cache.Positions[i];
          var p1 = cache.Positions[j];
          var testP0 = p0 + (p0 - p1);
          var testP1 = p1 + (p1 - p0);
          if(grid.IsValidPosition(testP0.X, testP0.Y))
          {
            ++counts[testP0.X, testP0.Y];
          }
          if(grid.IsValidPosition(testP1.X, testP1.Y))
          {
            ++counts[testP1.X, testP1.Y];
          }
        }
      }
    }
    void Test2(Grid2d<char> grid, Grid2d<int> counts, FrequencyCache cache)
    {
      for (int i = 1; i < cache.Positions.Count; ++i)
      {
        for (int j = 0; j < i; ++j)
        {
          var p0 = cache.Positions[i];
          var p1 = cache.Positions[j];

          int multiplier = 0;
          while (true)
          {
            var testP0 = p0 + (p0 - p1) * multiplier;
            if (!grid.IsValidPosition(testP0.X, testP0.Y))
              break;
            ++counts[testP0.X, testP0.Y];
            ++multiplier;
          }
          multiplier = 0;
          while (true)
          {
            var testP1 = p1 + (p1 - p0) * multiplier;
            if (!grid.IsValidPosition(testP1.X, testP1.Y))
              break;
            ++counts[testP1.X, testP1.Y];
            ++multiplier;
          }
        }
      }
    }
    int Count(Grid2d<int> counts)
    {
      var result = 0;
      for (var y = 0; y < counts.Height; ++y)
      {
        for (var x = 0; x < counts.Width; ++x)
        {
          if (counts[x, y] != 0)
            ++result;
        }
      }
      return result;
    }
  }
}
