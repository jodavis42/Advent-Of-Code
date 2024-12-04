namespace AdventOfCode2024
{
  internal class Day04
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      int count = 0;
      for(int y = 0; y < grid.Height; ++y)
      {
        for (int x = 0; x < grid.Width; ++x)
        { 
          count += Search(grid, x, y);
        }
      }
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      int count = 0;
      for (int y = 0; y < grid.Height; ++y)
      {
        for (int x = 0; x < grid.Width; ++x)
        {
          count += SearchX_MAS(grid, x, y);
        }
      }
      Console.WriteLine(count);
    }

    Grid2d<char> Parse(string[] lines)
    {
      var result = new Grid2d<char>();
      result.Initialize(lines[0].Length, lines.Length, '.');
      for (int y = 0; y < result.Height; y++)
      {
        for(int x = 0; x < result.Width; x++)
        {
          result[x, y] = lines[y][x];
        }
      }
      return result;
    }

    string XMas = "XMAS";

    int Search(Grid2d<char> grid, int x0, int y0)
    {
      int result = 0;
      result += Search(grid, x0, y0, 1, 0);
      result += Search(grid, x0, y0, -1, 0);
      result += Search(grid, x0, y0, 0, 1);
      result += Search(grid, x0, y0, 0, -1);
      result += Search(grid, x0, y0, 1, 1);
      result += Search(grid, x0, y0, 1, -1);
      result += Search(grid, x0, y0, -1, 1);
      result += Search(grid, x0, y0, -1, -1);
      return result;
    }

    int Search(Grid2d<char> grid, int x0, int y0, int dx, int dy)
    {
      var x = x0;
      var y = y0;
      for (var i = 0; i < XMas.Length; ++i)
      {
        if (!grid.IsValidPosition(x, y))
          return 0;
        if (grid[x, y] != XMas[i])
          return 0;
        x += dx;
        y += dy;
      }
      return 1;
    }
    int SearchX_MAS(Grid2d<char> grid, int x0, int y0)
    {
      if (x0 <= 0 || y0 <= 0 || x0 >= grid.Width - 1 || y0 >= grid.Height - 1)
        return 0;
      if (grid[x0, y0] != 'A')
        return 0;

      var tl = grid[x0 - 1, y0 - 1];
      var tr = grid[x0 + 1, y0 - 1];
      var bl = grid[x0 - 1, y0 + 1];
      var br = grid[x0 + 1, y0 + 1];

      bool tlbr = false;
      bool trbl = false;
      if (tl == 'M' && br == 'S')
        tlbr = true;
      if (tl == 'S' && br == 'M')
        tlbr = true;
      if (tr == 'M' && bl == 'S')
        trbl = true;
      if (tr == 'S' && bl == 'M')
        trbl = true;
      return (tlbr && trbl) ? 1 : 0;
    }
  }
}
