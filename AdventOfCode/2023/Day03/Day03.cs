using System.Threading;
using System.Xml;

namespace AdventOfCode2023
{
  internal class Day03
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      //Print(grid);
      var partNumbers = GetPartNumbers(grid);
      var sum = 0;
      foreach(var partNumber in partNumbers)
      {
        sum += partNumber;
        Console.WriteLine(partNumber);
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      var gearValues = GetGearNumbers(grid);
      var gearRatioSum = 0;
      foreach(var gear in gearValues) 
      {
        Console.WriteLine($"{gear.X} {gear.Y}");
        gearRatioSum += gear.X * gear.Y;
      }
      Console.Write(gearRatioSum);
    }
    public Grid2d<char> Parse(string[] lines)
    {
      var result = new Grid2d<char>();
      result.Initialize(lines[0].Length, lines.Length, '.');
      for (var y = 0; y < result.Height; ++y)
      {
        for (var x = 0; x < result.Width; ++x)
          result.Set(x, y, lines[y][x]);
      }
      return result;
    }
    public void Print(Grid2d<char> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
          Console.Write(grid[x, y]);
        Console.WriteLine();
      }
    }
    bool IsNumber(char c)
    {
      return '0' <= c && c <= '9';
    }
    public List<int> GetPartNumbers(Grid2d<char> grid)
    {
      var results = new List<int>();
      for(var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
        {
          var maxLength = grid.Width - x;
          var length = 0;
          for (; length < maxLength; ++length)
          {
            if (!IsNumber(grid[x + length, y]))
              break;
          }

          if (length == 0)
            continue;

          bool isValidPartNumber = IsValidPartNumber(grid, x, y, length);
          if (!isValidPartNumber)
            continue;

          int partNumber = GetPartNumber(grid, x, y, length);
          results.Add(partNumber);
          x += length;
        }
      }
      return results;
    }
    int GetPartNumber(Grid2d<char> grid, int x, int y, int length)
    {
      int partNumber = 0;
      for (var i = 0; i < length; ++i)
      {
        partNumber = partNumber * 10 + (grid[x + i, y] - '0');
      }
      return partNumber;
    }
    List<Int2> GetGearNumbers(Grid2d<char> grid)
    {
      var results = new List<Int2>();
      var gearMap = new Dictionary<Int2, List<int>>();

      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var maxLength = grid.Width - x;
          var length = 0;
          for (; length < maxLength; ++length)
          {
            if (!IsNumber(grid[x + length, y]))
              break;
          }

          if (length == 0)
            continue;

          var gearPositions = GetAdjacentGearPositions(grid, x, y, length);
          if (gearPositions.Count == 0)
            continue;
          var partNumber = GetPartNumber(grid, x, y, length);
          foreach (var gearPosition in gearPositions)
          {
            if(!gearMap.TryGetValue(gearPosition, out var list))
            {
              list = new List<int>();
              gearMap.Add(gearPosition, list);
            }
            list.Add(partNumber);
          }
          x += length;
        }
      }
      foreach(var gearPosition in gearMap.Values)
      {
        if (gearPosition.Count == 2)
          results.Add(new Int2(gearPosition[0], gearPosition[1]));
      }
      return results;
    }
    bool IsValidPartNumber(Grid2d<char> grid, int x, int y, int length)
    {
      var minY = y - 1;
      var maxY = y + 1;
      var minX = x - 1;
      var maxX = x + length;
      for (var cY = minY; cY <= maxY; ++cY)
      {
        for (var cX = minX; cX <= maxX; ++cX)
        {
          if (!grid.IsValidPosition(cX, cY))
            continue;

          var c = grid[cX, cY];
          if (IsNumber(c) || c == '.')
            continue;
          return true;
        }
      }
      return false;
    }
    List<Int2> GetAdjacentGearPositions(Grid2d<char> grid, int x, int y, int length)
    {
      var minY = y - 1;
      var maxY = y + 1;
      var minX = x - 1;
      var maxX = x + length;
      var result =new List<Int2>();
      for (var cY = minY; cY <= maxY; ++cY)
      {
        for (var cX = minX; cX <= maxX; ++cX)
        {
          if (!grid.IsValidPosition(cX, cY))
            continue;

          var c = grid[cX, cY];
          if (c == '*')
            result.Add(new Int2(cX, cY));
        }
      }
      return result;
    }
  }
}
