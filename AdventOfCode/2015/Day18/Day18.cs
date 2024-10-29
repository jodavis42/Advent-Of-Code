namespace AdventOfCode2015
{
  internal class Day18
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);

      for (var i = 0; i < 100; ++i)
      {
        grid = Update(grid);
      }
      var onCount = Count(grid, CellState.On);
      Console.WriteLine($"Count: {onCount}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      SetCorners(grid);
      //Console.WriteLine("Initial state:");
      //Print(grid);
      for (var i = 0; i < 100; ++i)
      {
        Console.WriteLine();
        grid = Update(grid);
        SetCorners(grid);
        //Console.WriteLine($"After {i + 1} step(s):");
        //Print(grid);
      }
      var onCount = Count(grid, CellState.On);
      Console.WriteLine($"Count: {onCount}");
    }

    enum CellState
    {
      Off, On
    }
    Grid2d<CellState> Parse(string[] lines)
    {
      var grid = new Grid2d<CellState>();
      grid.Initialize(lines[0].Length, lines.Length, CellState.Off);
      for(var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
        {
          var c = lines[y][x];
          var state = CellState.Off;
          if (c == '.')
            state = CellState.Off;
          else if (c == '#')
            state = CellState.On;
          grid[x, y] = state;
        }
      }
      return grid;
    }

    void Print(Grid2d<CellState> grid)
    {
      for(var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Height; ++x)
        {
          var state = grid[x, y];
          var c = state == CellState.Off ? '.' : '#';
          Console.Write(c);
        }
        Console.WriteLine();
      }
    }

    Grid2d<CellState> Update(Grid2d<CellState> grid)
    {
      var countGrid = new Grid2d<int>();
      countGrid.Initialize(grid.Width, grid.Height, 0);
      var newGrid = new Grid2d<CellState>();
      newGrid.Initialize(grid.Width, grid.Height, CellState.Off);

      for(var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
        {
          var currentState = grid[x, y];
          int offCount, onCount;
          Count(grid, x, y, out offCount, out onCount);
          if (currentState == CellState.Off && onCount == 3)
            currentState = CellState.On;
          else if(currentState == CellState.On)
          {
            if (onCount == 2 || onCount == 3)
              currentState = CellState.On;
            else
              currentState = CellState.Off;
          }
          newGrid[x, y] = currentState;
        }
      }
      return newGrid;
    }
    void Count(Grid2d<CellState> grid, int targetX, int targetY, out int offCount, out int onCount)
    {
      int minX = Math.Max(targetX - 1, 0);
      int maxX = Math.Min(targetX + 1, grid.Width - 1);
      int minY = Math.Max(targetY - 1, 0);
      int maxY = Math.Min(targetY + 1, grid.Height - 1);

      onCount = 0;
      offCount = 0;
      for(var y = minY; y <= maxY; ++y)
      {
        for(var x = minX; x <= maxX; ++x)
        {
          var state = grid[x, y];
          if (x == targetX && y == targetY)
            continue;

          if (state == CellState.Off)
            ++offCount;
          else if (state == CellState.On)
            ++onCount;
        }
      }
    }
    int Count(Grid2d<CellState> grid, CellState searchState)
    {
      var count = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
        {
          if (grid[x, y] == searchState)
            ++count;
        }
      }
      return count;
    }
    void SetCorners(Grid2d<CellState> grid)
    {
      grid[0, 0] = CellState.On;
      grid[0, grid.Height - 1] = CellState.On;
      grid[grid.Width - 1, grid.Height - 1] = CellState.On;
      grid[grid.Width - 1, 0] = CellState.On;
    }
  }
}
