using System.ComponentModel;
using System.Xml.XPath;

namespace AdventOfCode2023
{
  internal class Day11
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      
      var originalGrid = Parse(lines);
      Print(originalGrid);
      Console.WriteLine();

      var expandedGrid = Expand(originalGrid);
      Print(expandedGrid);
      Console.WriteLine();

      Solve(expandedGrid);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      var originalGrid = Parse(lines);
      Print(originalGrid);
      Console.WriteLine();

      var galaxies = ExpandGalaxies(originalGrid, 1000000);

      Solve(galaxies);
    }
    Grid2d<char> Parse(string[] lines)
    {
      Grid2d<char> grid = new Grid2d<char>();
      grid.Initialize(lines[0].Length, lines.Length, '.');
      for (var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
          grid[x, y] = lines[y][x];
      }
      return grid;
    }
    List<Int2> FindGalaxyPositions(Grid2d<char> grid)
    {
      var galaxyPositions = new List<Int2>();
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          if (grid[x, y] == '#')
            galaxyPositions.Add(new Int2(x, y));
        }
      }
      return galaxyPositions;
    }
    Grid2d<char> Expand(Grid2d<char> grid, int expansionFactor = 10)
    {
      var scale = expansionFactor - 1;
      var columns = new List<int>();
      for(var x = 0; x < grid.Width; ++x)
      {
        bool isEmpty = true;
        for(var y = 0; y < grid.Height; ++y)
          isEmpty &= grid[x, y] == '.';
        if (isEmpty)
          columns.Add(x);
      }

      var rows = new List<int>();
      for (var y = 0; y < grid.Height; ++y)
      {
        bool isEmpty = true;
        for (var x = 0; x < grid.Width; ++x)
          isEmpty &= grid[x, y] == '.';
        if (isEmpty)
          rows.Add(y);
      }

      var result = new Grid2d<char>();
      result.Initialize(grid.Width + columns.Count * scale, grid.Height + rows.Count * scale, '.');

      var galaxyPositions = FindGalaxyPositions(grid);

      foreach (var galaxyPosition in galaxyPositions)
      {
        var newPos = galaxyPosition;
        foreach(var column in columns)
        {
          if (column < galaxyPosition.X)
            newPos.X += scale;
        }
        foreach (var row in rows)
        {
          if (row < galaxyPosition.Y)
            newPos.Y += scale;
        }
        result[newPos.X, newPos.Y] = '#';
      }

      return result;
    }
    struct UInt2
    {
      public UInt64 X;
      public UInt64 Y;
    }
    List<UInt2> ExpandGalaxies(Grid2d<char> grid, UInt64 expansionFactor)
    {
      var scale = expansionFactor - 1;
      var columns = new List<int>();
      for (var x = 0; x < grid.Width; ++x)
      {
        bool isEmpty = true;
        for (var y = 0; y < grid.Height; ++y)
          isEmpty &= grid[x, y] == '.';
        if (isEmpty)
          columns.Add(x);
      }

      var rows = new List<int>();
      for (var y = 0; y < grid.Height; ++y)
      {
        bool isEmpty = true;
        for (var x = 0; x < grid.Width; ++x)
          isEmpty &= grid[x, y] == '.';
        if (isEmpty)
          rows.Add(y);
      }

      var galaxyPositions = FindGalaxyPositions(grid);
      var results = new List<UInt2>();

      foreach (var galaxyPosition in galaxyPositions)
      {
        UInt2 newPos = new UInt2 { X = (UInt64)galaxyPosition.X, Y = (UInt64)galaxyPosition.Y };
        foreach (var column in columns)
        {
          if (column < galaxyPosition.X)
            newPos.X += scale;
        }
        foreach (var row in rows)
        {
          if (row < galaxyPosition.Y)
            newPos.Y += scale;
        }
        results.Add(newPos);
      }

      return results;
    }
    void Solve(Grid2d<char> grid)
    {
      UInt64 sum = 0;
      var galaxyPositions = FindGalaxyPositions(grid);
      for (var i = 0; i < galaxyPositions.Count; ++i)
      {
        for (var j = i + 1; j < galaxyPositions.Count; ++j)
        {
          var galaxyI = galaxyPositions[i];
          var galaxyJ = galaxyPositions[j];
          var diff = galaxyI - galaxyJ;
          var distance = Math.Abs(diff.X) + Math.Abs(diff.Y);
          sum += (UInt64)distance;
          Console.WriteLine($"{i} {j}: {distance}");
        }
      }
      Console.WriteLine(sum);
    }
    void Solve(List<UInt2> galaxyPositions)
    {
      UInt64 sum = 0;
      for(var i = 0; i < galaxyPositions.Count; ++i) 
      {
        for(var j = i + 1; j < galaxyPositions.Count; ++j)
        {
          var galaxyI = galaxyPositions[i];
          var galaxyJ = galaxyPositions[j];
          var diffX = galaxyI.X > galaxyJ.X ? (galaxyI.X - galaxyJ.X) : (galaxyJ.X - galaxyI.X);
          var diffY = galaxyI.Y > galaxyJ.Y ? (galaxyI.Y - galaxyJ.Y) : (galaxyJ.Y - galaxyI.Y);
          var distance = diffX + diffY;
          sum += (UInt64)distance;
          Console.WriteLine($"{i} {j}: {distance}");
        }
      }
      Console.WriteLine(sum);
    }
    void Print(Grid2d<char> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
          Console.Write(grid[x, y]);
        Console.WriteLine();
      }
    }
  }
}
