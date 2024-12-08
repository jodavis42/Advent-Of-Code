using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode2024
{
  internal class Day06
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      Print(grid);
      int count = Simulate(grid);

      Print(grid);
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      int count = Simulate2(grid);
      Console.WriteLine(count);
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
    public void FindStartPosition(Grid2d<char> grid, out int outX, out int outY)
    {
      outX = 0;
      outY = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var c = grid[x, y];
          if (c == '^' || c == '<' || c == '>' || c == 'v')
          {
            outX = x;
            outY = y;
            return;
          }
        }
      }
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
    void Print(Grid2d<DirectionFlags> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var g = grid[x, y];
          if (g == DirectionFlags.Up)
            Console.Write('^');
          else if (g == (DirectionFlags.Up | DirectionFlags.Right))
            Console.Write('L');
          else if (g == (DirectionFlags.Up | DirectionFlags.Down))
            Console.Write('|');
          else if (g == (DirectionFlags.Up | DirectionFlags.Left))
            Console.Write('7');
          else if (g == DirectionFlags.Right)
            Console.Write('>');
          else if (g == (DirectionFlags.Right | DirectionFlags.Down))
            Console.Write('F');
          else if (g == (DirectionFlags.Right | DirectionFlags.Left))
            Console.Write('-');
          else if (g == (DirectionFlags.Down))
            Console.Write('v');
          else if (g == (DirectionFlags.Down | DirectionFlags.Left))
            Console.Write('J');
          else if (g == (DirectionFlags.Up | DirectionFlags.Right | DirectionFlags.Down))
            Console.Write('E');
          else if (g == (DirectionFlags.Up | DirectionFlags.Right | DirectionFlags.Left))
            Console.Write('W');
          else if (g == (DirectionFlags.Up | DirectionFlags.Down | DirectionFlags.Left))
            Console.Write('3');

          ///......
          Console.Write(grid[x, y]);
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
    enum Direction { Up, Right, Down, Left }
    enum DirectionFlags { None, Up = 1 << 0, Right = 1 << 1, Down = 1 << 2, Left = 1 << 3 }
    public int Simulate(Grid2d<char> grid)
    {
      int x, y;
      FindStartPosition(grid, out x, out y);

      var s = grid[x, y];
      Direction direction = Direction.Down;
      if (s == '^')
        direction = Direction.Up;
      else if (s == '>')
        direction = Direction.Right;
      else if (s == 'v')
        direction = Direction.Down;
      else if (s == '<')
        direction = Direction.Left;

      while (0 <= x && x < grid.Width && 0 <= y && y < grid.Height)
      {
        var c = grid[x, y];
        grid[x, y] = 'X';
        if (direction == Direction.Up)
        {
          if (grid.TryGetValue(x, y - 1, '.') == '#')
            direction = (Direction)(((int)direction + 1) % 4);
          else
            --y;
        }
        else if (direction == Direction.Right)
        {
          if (grid.TryGetValue(x + 1, y, '.') == '#')
            direction = (Direction)(((int)direction + 1) % 4);
          else
            ++x;
        }
        if (direction == Direction.Down)
        {
          if (grid.TryGetValue(x, y + 1, '.') == '#')
            direction = (Direction)(((int)direction + 1) % 4);
          else
            ++y;
        }
        if (direction == Direction.Left)
        {
          if (grid.TryGetValue(x - 1, y, '.') == '#')
            direction = (Direction)(((int)direction + 1) % 4);
          else
            --x;
        }
      }

      return CountVisited(grid);
    }

    public int CountVisited(Grid2d<char> grid)
    {
      var result = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          if (grid[x, y] == 'X')
            ++result;
        }
      }
      return result;
    }

    public int Simulate2(Grid2d<char> grid)
    {
      var result = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
          result += Simulate2(grid, x, y);
      }
      return result;
    }
    public Grid2d<char> Clone(Grid2d<char> grid)
    {
      var result = new Grid2d<char>();
      result.Initialize(grid.Width, grid.Height, '.');
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
          result[x, y] = grid[x, y];
      }
      return result;
    }
    DirectionFlags ToFlags(Direction dir)
    {
      if (dir == Direction.Up)
        return DirectionFlags.Up;
      if (dir == Direction.Right)
        return DirectionFlags.Right;
      if (dir == Direction.Down)
        return DirectionFlags.Down;
      return DirectionFlags.Left;
    }
    public int Simulate2(Grid2d<char> grid, int x0, int y0)
    {
      var c = grid[x0, y0];
      if (c == '#')
        return 0;
      int x, y;
      FindStartPosition(grid, out x, out y);
      if (x0 == x && y0 == y)
        return 0;

      grid[x0, y0] = '#';

      var dirGrid = new Grid2d<DirectionFlags>();
      dirGrid.Initialize(grid.Width, grid.Height, DirectionFlags.None);

      Direction direction = Direction.Down;
      c = grid[x, y];
      if (c == '^')
        direction = Direction.Up;
      else if (c == '>')
        direction = Direction.Right;
      else if (c == 'v')
        direction = Direction.Down;
      else if (c == '<')
        direction = Direction.Left;

      int result = 0;
      while (0 <= x && x < grid.Width && 0 <= y && y < grid.Height)
      {
        c = grid[x, y];
        var d = dirGrid[x, y];
        if ((d & ToFlags(direction)) != 0)
        {
          result = 1;
          break;
        }
        dirGrid[x, y] |= ToFlags(direction);

        Move(grid, ref x, ref y, ref direction);
      }
      grid[x0, y0] = '.';
      return result;
    }

    void Move(Grid2d<char> grid, ref int x, ref int y, ref Direction direction)
    {
      if (direction == Direction.Up)
      {
        if (grid.TryGetValue(x, y - 1, '.') == '#')
          direction = (Direction)(((int)direction + 1) % 4);
        else
          --y;
      }
      else if (direction == Direction.Right)
      {
        if (grid.TryGetValue(x + 1, y, '.') == '#')
          direction = (Direction)(((int)direction + 1) % 4);
        else
          ++x;
      }
      if (direction == Direction.Down)
      {
        if (grid.TryGetValue(x, y + 1, '.') == '#')
          direction = (Direction)(((int)direction + 1) % 4);
        else
          ++y;
      }
      if (direction == Direction.Left)
      {
        if (grid.TryGetValue(x - 1, y, '.') == '#')
          direction = (Direction)(((int)direction + 1) % 4);
        else
          --x;
      }
    }
  }
}
