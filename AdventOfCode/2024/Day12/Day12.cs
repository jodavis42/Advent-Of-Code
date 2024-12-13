namespace AdventOfCode2024
{
  internal class Day12
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = new Grid2d<char>();
      grid.Parse(lines);
      var context = new Context();
      FloodFill(grid, context);
      context.Print();
      int price = context.ComputePrice(grid);
      Console.WriteLine(price);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
    }
    class Region
    {
      public List<Int2> Positions = new List<Int2>();
      public void Print()
      {
        Console.Write($"\t");
        foreach (var pos in Positions)
        {
          Console.Write($"({pos.X}, {pos.Y}), ");
        }
        Console.WriteLine();
      }
      public int ComputeArea(Grid2d<char> grid)
      {
        return Positions.Count;
      }
      public int ComputePerimeter(Grid2d<char> grid)
      {
        int result = 0;
        foreach(var pos in Positions)
        {
          if (!grid.IsValidPosition(pos.X - 1, pos.Y) || grid[pos.X - 1, pos.Y] != grid[pos.X, pos.Y])
            ++result;
          if (!grid.IsValidPosition(pos.X + 1, pos.Y) || grid[pos.X + 1, pos.Y] != grid[pos.X, pos.Y])
            ++result;
          if (!grid.IsValidPosition(pos.X, pos.Y - 1) || grid[pos.X, pos.Y - 1] != grid[pos.X, pos.Y])
            ++result;
          if (!grid.IsValidPosition(pos.X, pos.Y + 1) || grid[pos.X, pos.Y + 1] != grid[pos.X, pos.Y])
            ++result;
        }
        return result;
      }
      public int ComputePrice(Grid2d<char> grid)
      {
        int area = ComputeArea(grid);
        int perimeter = ComputePerimeter(grid);
        return area * perimeter;
      }
    }
    class RegionList
    {
      public List<Region> Regions = new List<Region>();
      public int ComputePrice(Grid2d<char> grid)
      {
        int sum = 0;
        foreach(var region in Regions)
          sum += region.ComputePrice(grid);
        return sum;
      }
    }

    class Context
    {
      public Dictionary<char, RegionList> RegionMap = new Dictionary<char, RegionList>();
      public void Print()
      {
        foreach(var pair in RegionMap)
        {
          Console.WriteLine($"{pair.Key}:");
          foreach (var list in pair.Value.Regions)
            list.Print();
        }
      }
      public int ComputePrice(Grid2d<char> grid)
      {
        int sum = 0;
        foreach (var regions in RegionMap.Values)
          sum += regions.ComputePrice(grid);
        return sum;
      }
    }
    void FloodFill(Grid2d<char> grid, Context context)
    {
      Grid2d<bool> visitedGrid = new Grid2d<bool>();
      visitedGrid.Initialize(grid.Width, grid.Height, false);

      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          FloodFill(grid, visitedGrid, context, new Int2(x, y));
        }
      }
    }
    void FloodFill(Grid2d<char> grid, Grid2d<bool> visitedGrid, Context context, Int2 position)
    {
      if (visitedGrid[position.X, position.Y] == true)
          return;

      visitedGrid[position.X, position.Y] = true;

      var stack = new Stack<Int2>();
      stack.Push(position);
      var c = grid[position.X, position.Y];

      if(!context.RegionMap.TryGetValue(c, out var regionList))
      {
        regionList = new RegionList();
        context.RegionMap.Add(c, regionList);
      }

      var region = new Region();
      regionList.Regions.Add(region);
      region.Positions.Add(position);

      var TryAdd = (int x, int y) =>
      {
        if (visitedGrid.TryGetValue(x, y, true) == true)
          return;
        if (grid[x, y] != c)
          return;

        visitedGrid[x, y] = true;
        stack.Push(new Int2(x, y));
        region.Positions.Add(new Int2(x, y));
      };

      while(stack.Count != 0)
      {
        Int2 pos = stack.Pop();
        TryAdd(pos.X - 1, pos.Y + 0);
        TryAdd(pos.X + 1, pos.Y + 0);
        TryAdd(pos.X + 0, pos.Y - 1);
        TryAdd(pos.X + 0, pos.Y + 1);
      }
    }
  }
}
