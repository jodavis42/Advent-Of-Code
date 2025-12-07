using System.Text;

namespace AdventOfCode2025
{
  internal class Day07
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      //grid.Print();
      var start = FindStart(grid);
      var count = Simulate(grid, start);
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      //grid.Print();
      var start = FindStart(grid);
      var count = SimulateTimelines(grid, start);
      Console.WriteLine(count);
    }
    Grid2d<char> Parse(string[] lines)
    {
      var result = new Grid2d<char>();
      result.Initialize(lines[0].Length, lines.Length, '.');
      for (var y = 0; y < lines.Length; ++y)
      {
        for (var x = 0; x < lines[y].Length; ++x)
        {
          result[x, y] = lines[y][x];
        }
      }
      return result;
    }
    Int2 FindStart(Grid2d<char> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          if (grid[x, y] == 'S')
            return new Int2(x, y);
        }
      }
      return new Int2(-1, -1);
    }
    int Simulate(Grid2d<char> grid, Int2 start)
    {
      var beams = new List<Int2>();
      beams.Add(start);
      var splits = 0;

      while (beams.Count != 0)
      {
        var beams2 = new List<Int2>();
        foreach (var beam in beams)
        {
          var next = new Int2(beam.X, beam.Y + 1);
          if (!grid.IsValidPosition(next.X, next.Y))
            continue;

          var nextVal = grid[next.X, next.Y];
          if (nextVal == '|')
            continue;
          if (nextVal == '.')
          {
            beams2.Add(next);
            grid[next.X, next.Y] = '|';
          }
          else if (nextVal == '^')
          {
            var left = new Int2(next.X - 1, next.Y);
            var right = new Int2(next.X + 1, next.Y);
            ++splits;
            if (grid[left.X, left.Y] == '.')
            {
              grid[left.X, left.Y] = '|';
              beams2.Add(left);
            }
            if (grid[right.X, right.Y] == '.')
            {
              grid[right.X, right.Y] = '|';
              beams2.Add(right);
            }
          }
        }

        beams = beams2;
      }
      return splits;
    }
    Int64 SimulateTimelines(Grid2d<char> grid, Int2 start)
    {
      var gridCounts = new Grid2d<Int64>();
      gridCounts.Initialize(grid.Width, grid.Height, 0);

      var beams = new List<Int2>();
      beams.Add(start);
      gridCounts[start.X, start.Y] = 1;

      while (beams.Count != 0)
      {
        var beams2 = new List<Int2>();
        foreach (var beam in beams)
        {
          var beamPos = beam;
          var next = new Int2(beamPos.X, beamPos.Y + 1);
          var curCount = gridCounts[beamPos.X, beamPos.Y];
          if (!grid.IsValidPosition(next.X, next.Y))
          {
            continue;
          }

          var nextVal = grid[next.X, next.Y];
          if (nextVal == '|')
          {
            gridCounts[next.X, next.Y] += curCount;
            continue;
          }
          if (nextVal == '.')
          {
            beams2.Add(next);
            grid[next.X, next.Y] = '|';
            gridCounts[next.X, next.Y] += curCount;
          }
          else if (nextVal == '^')
          {
            var left = new Int2(next.X - 1, next.Y);
            var right = new Int2(next.X + 1, next.Y);
            gridCounts[left.X, left.Y] += curCount;
            gridCounts[right.X, right.Y] += curCount;

            if (grid[left.X, left.Y] == '.')
            {
              grid[left.X, left.Y] = '|';
              beams2.Add(left);
            }
            if (grid[right.X, right.Y] == '.')
            {
              grid[right.X, right.Y] = '|';
              beams2.Add(right);
            }
          }
        }

        beams = beams2;
      }

      Int64 total = 0;
      for (var x = 0; x < gridCounts.Width; ++x)
      {
        total += gridCounts[x, gridCounts.Height - 1];
      }
      return total;
    }
  }
}
