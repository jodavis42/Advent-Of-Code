using System.Text;

namespace AdventOfCode2025
{
  internal class Day04
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      grid.Print();
      var grid2 = new Grid2d<char>();
      grid2.Initialize(grid.Width, grid.Height, '.');
      var totalCount = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
        {
          var value = grid[x, y];
          if(value == '@')
          {
            var count = CountAdjacent(grid, x, y);
            if(count <= 4)
            {
              value = 'x';
              ++totalCount;
            }
          }
          grid2[x, y] = value;
        }
      }
      Console.WriteLine($"Count: {totalCount}");
      grid2.Print();
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      var totalRemoved = 0;
      while(true)
      {
        var removed = RemoveIteration(grid);
        if (removed == 0)
          break;
        totalRemoved += removed;
      }
      Console.WriteLine(totalRemoved);
    }
    int CountAdjacent(Grid2d<char> grid, int x0, int y0)
    {
      int count = 0;
      for(int y = y0 - 1; y <= y0 + 1; ++y)
      {
        for (int x = x0 - 1; x <= x0 + 1; ++x)
        {
          var value = grid.TryGetValue(x, y, '.');
          if (value == '@')
            ++count;
        }
      }
      return count;
    }
    int RemoveIteration(Grid2d<char> grid)
    {
      var removed = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var value = grid[x, y];
          if (value == '@')
          {
            var count = CountAdjacent(grid, x, y);
            if (count <= 4)
            {
              grid[x, y] = '.';
              ++removed;
            }
          }
        }
      }
      return removed;
    }
    Grid2d<char> Parse(string[] lines)
    {
      var result = new Grid2d<char>();
      GridUtils.Parse(result, lines);
      return result;
    }
  }
}
