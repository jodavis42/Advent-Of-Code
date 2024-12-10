using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2024
{
  internal class Day10
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      grid.Print();
      Context context = Initialize(grid);
      int count = Solve(context, grid);
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var grid = Parse(lines);
      grid.Print();
      Context context = Initialize(grid);
      int count = Solve2(context, grid);
      Console.WriteLine(count);
    }

    Grid2d<int> Parse(string[] lines)
    {
      var result = new Grid2d<int>();
      result.Initialize(lines[0].Length, lines.Length, -1);
      for (var y = 0; y < result.Height; ++y)
      {
        for (var x = 0; x < result.Width; ++x)
        {
          var c = lines[y][x];
          if (c == '.')
            result[x, y] = -1;
          else
            result[x, y] = c - '0';
        }
      }
      return result;
    }

    class Context
    {
      [DebuggerDisplay("{ToString()}")]
      public class Entry
      {
        public int Value;
        public int X, Y;
        public int Count = 0;
        public override string ToString()
        {
          return $"Value({Value}) Count({Count}) {X} {Y}";
        }
      }
      public List<Entry> Stack = new List<Entry>();
    }
    Context Initialize(Grid2d<int> grid)
    {
      var result = new Context();
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var i = grid[x, y];
          if (i == 0)
            result.Stack.Add(new Context.Entry { Value = i, X = x, Y = y });
        }
      }
      return result;
    }
    int ScoreTrailhead(Context context, Grid2d<int> grid, int x0, int y0)
    {
      int count = 0;
      var set = new HashSet<Int2>();
      context.Stack.Clear();
      context.Stack.Add(new Context.Entry { Value = 0, X = x0, Y = y0 });
      while (context.Stack.Count > 0)
      {
        var entry = context.Stack.Last();
        context.Stack.RemoveAt(context.Stack.Count - 1);
        set.Add(new Int2(entry.X, entry.Y));

        if (entry.Value == 9)
        {
          ++count;
          continue;
        }

        var TryAdd = (int x, int y) =>
        {
          if (set.Contains(new Int2(x, y)))
            return;
          if (grid.TryGetValue(x, y, -1) == entry.Value + 1)
            context.Stack.Add(new Context.Entry { X = x, Y = y, Value = entry.Value + 1 });
        };
        TryAdd(entry.X - 1, entry.Y);
        TryAdd(entry.X + 1, entry.Y);
        TryAdd(entry.X, entry.Y - 1);
        TryAdd(entry.X, entry.Y + 1);
      }
      return count;
    }
    int Solve(Context context, Grid2d<int> grid)
    {
      List<Int2> starts = new List<Int2>();
      foreach(var entry in context.Stack)
      {
        starts.Add(new Int2(entry.X, entry.Y));
      }
      context.Stack.Clear();
      int sum = 0;
      foreach (var start in starts)
        sum += ScoreTrailhead(context, grid, start.X, start.Y);
      return sum;
    }
    int Solve2(Context context, Grid2d<int> grid)
    {
      List<Int2> starts = new List<Int2>();

      foreach(var entry in context.Stack)
      {
        starts.Add(new Int2(entry.X, entry.Y));
      }
      context.Stack.Clear();

      int sum = 0;
      foreach (var start in starts)
        sum += Solve2(grid, start.X, start.Y);
      return sum;
    }
    int Solve2(Grid2d<int> grid, int x0, int y0)
    {
      var entryGrid = new Grid2d<Context.Entry>();
      entryGrid.Initialize(grid.Width, grid.Height, null);
      for(var y = 0; y < grid.Height; ++y)
      {
        for(var x = 0; x < grid.Width; ++x)
          entryGrid[x,y] = new Context.Entry { Value = grid[x,y], X = x, Y = y };
      }
      var queue = new PriorityQueue<Context.Entry, int>();
      var set = new HashSet<Context.Entry>();
      entryGrid[x0, y0].Count = 1;
      queue.Enqueue(entryGrid[x0, y0], 0);
      set.Add(entryGrid[x0, y0]);

      while(queue.Count != 0)
      {
        var TryAdd = (int x, int y, Context.Entry sourceEntry) =>
        {
          if (!grid.IsValidPosition(x, y))
            return;

          var entry = entryGrid[x, y];
          var value = grid.TryGetValue(x, y, -1);
          if (value != sourceEntry.Value + 1)
            return;

          entry.Count += sourceEntry.Count;
          if (set.Contains(entry))
            return;

          set.Add(entry);
          queue.Enqueue(entry, entry.Value);
        };
        var entry = queue.Dequeue();
        
        TryAdd(entry.X - 1, entry.Y, entry);
        TryAdd(entry.X + 1, entry.Y, entry);
        TryAdd(entry.X, entry.Y - 1, entry);
        TryAdd(entry.X, entry.Y + 1, entry);
      }

      var sum = 0;
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var entry = entryGrid[x, y];
          if (entry.Value == 9)
            sum += entry.Count;
        }
      }
      return sum;
    }
  }
}
