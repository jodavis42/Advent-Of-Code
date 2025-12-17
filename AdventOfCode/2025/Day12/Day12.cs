using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2025
{
  internal class Day12
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var problem = Parse(lines);
      BuildPresentSets(problem);
      Console.WriteLine(problem.ToString());
      
      var count = 0;
      for (var i = 0; i < problem.Regions.Count; ++i)
      {
        var solver = new Solver();
        var solved = solver.Solve(problem, i);
        Console.WriteLine(solved);
        if (solved)
          ++count;
      }
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
    }
    class Present : Grid2d<char>
    {
      public int Index = -1;
      public override string ToString()
      {
        var builder = new StringBuilder();
        builder.AppendLine($"{Index}:");
        for (var y = 0; y < Height; ++y)
        {
          for (var x = 0; x < Width; ++x)
            builder.Append(this[x, y]);
          builder.AppendLine();
        }
        return builder.ToString();
      }
      public bool Equals(Present other)
      {
        if (Width != other.Width || Height != other.Height)
          return false;

        for (var y = 0; y < Height; ++y)
        {
          for (var x = 0; x < Width; ++x)
          {
            if (this[x, y] != other[x, y])
              return false;
          }
        }
        return true;
      }
    }
    class PresentSet
    {
      public List<Present> Presents = new List<Present>();
      public override string ToString()
      {
        var builder = new StringBuilder();
        builder.AppendLine($"{Presents[0].Index}:");
        for (var y = 0; y < Presents[0].Height; ++y)
        {
          foreach (var present in Presents)
          {
            for (var x = 0; x < present.Width; ++x)
              builder.Append(present[x, y]);
            builder.Append(' ');
          }
          builder.AppendLine();
        }
        return builder.ToString();
      }
      public bool TryAdd(Present newPresent)
      {
        foreach (var present in Presents)
        {
          if (present.Equals(newPresent))
            return false;
        }
        Presents.Add(newPresent);
        return true;
      }
    }
    class Region
    {
      public Int2 Size = new Int2();
      public List<int> Counts = new List<int>();
      public override string ToString()
      {
        var builder = new StringBuilder();
        builder.Append($"{Size.X}x{Size.Y}:");
        foreach (var count in Counts)
          builder.Append($" {count}");
        return builder.ToString();
      }
    }
    class Problem
    {
      public List<Present> Presents = new List<Present>();
      public List<PresentSet> PresentSets = new List<PresentSet>();
      public List<Region> Regions = new List<Region>();
      public override string ToString()
      {
        var builder = new StringBuilder();
        foreach (var present in Presents)
          builder.AppendLine(present.ToString());
        builder.AppendLine("-----------------------------");
        foreach (var presentSet in PresentSets)
          builder.AppendLine(presentSet.ToString());
        builder.AppendLine("-----------------------------");
        foreach (var region in Regions)
          builder.AppendLine(region.ToString());
        return builder.ToString();
      }
    }

    Problem Parse(string[] lines)
    {
      var presentHeaderRegex = new Regex("(\\d+):");
      var regionHeaderRegex = new Regex("(\\d+)x(\\d+):");

      var problem = new Problem();
      var i = 0;
      while (i < lines.Length)
      {
        if (regionHeaderRegex.IsMatch(lines[i]))
          break;

        var match = presentHeaderRegex.Match(lines[i]);
        if (!match.Success)
          break;
        ++i;

        var present = new Present();
        problem.Presents.Add(present);

        present.Index = int.Parse(match.Groups[1].Value);
        var start = i;
        var end = i;
        while (!string.IsNullOrEmpty(lines[end]))
          ++end;

        var width = lines[start].Length;
        var height = end - start;
        present.Initialize(width, height, ' ');
        for (var y = 0; y < height; ++y)
        {
          for (var x = 0; x < width; ++x)
            present[x, y] = lines[y + start][x];
        }
        i = end + 1;
      }

      while (i < lines.Length)
      {
        var match = regionHeaderRegex.Match(lines[i]);
        if (!match.Success)
          break;

        var region = new Region();
        problem.Regions.Add(region);
        region.Size.X = int.Parse(match.Groups[1].Value);
        region.Size.Y = int.Parse(match.Groups[2].Value);

        var split = lines[i].Split(' ');
        for (var j = 1; j < split.Length; ++j)
          region.Counts.Add(int.Parse(split[j]));
        ++i;
      }

      return problem;
    }
    void BuildPresentSets(Problem problem)
    {
      foreach (var present in problem.Presents)
      {
        var presentSet = new PresentSet();
        problem.PresentSets.Add(presentSet);
        BuildPresentSet(present, presentSet);
      }
    }
    void BuildPresentSet(Present present, PresentSet presentSet)
    {
      presentSet.Presents.Add(present);

      var present90 = new Present { Index = present.Index };
      present90.Initialize(present.Width, present.Height, ' ');
      for (var y = 0; y < present.Height; ++y)
      {
        for (var x = 0; x < present.Width; ++x)
          present90[present.Width - y - 1, x] = present[x, y];
      }
      presentSet.TryAdd(present90);

      var present180 = new Present { Index = present.Index };
      present180.Initialize(present.Width, present.Height, ' ');
      for (var y = 0; y < present.Height; ++y)
      {
        for (var x = 0; x < present.Width; ++x)
          present180[present.Width - x - 1, present.Height - y - 1] = present[x, y];
      }
      presentSet.TryAdd(present180);

      var present270 = new Present { Index = present.Index };
      present270.Initialize(present.Width, present.Height, ' ');
      for (var y = 0; y < present.Height; ++y)
      {
        for (var x = 0; x < present.Width; ++x)
          present270[y, present.Height - x - 1] = present[x, y];
      }
      presentSet.TryAdd(present270);

      var count = presentSet.Presents.Count;
      for (var i = 0; i < count; ++i)
        TryAddFlipped(presentSet.Presents[i], presentSet);
    }
    void TryAddFlipped(Present present, PresentSet presentSet)
    {
      var flippedX = new Present { Index = present.Index };
      flippedX.Initialize(present.Width, present.Height, ' ');
      for (var y = 0; y < present.Height; ++y)
      {
        for (var x = 0; x < present.Width; ++x)
          flippedX[present.Width - x - 1, y] = present[x, y];
      }
      presentSet.TryAdd(flippedX);


      var flippedY = new Present { Index = present.Index };
      flippedY.Initialize(present.Width, present.Height, ' ');
      for (var y = 0; y < present.Height; ++y)
      {
        for (var x = 0; x < present.Width; ++x)
          flippedY[x, present.Height - y - 1] = present[x, y];
      }
      presentSet.TryAdd(flippedY);
    }

    class Solver
    {
      Problem Problem;
      Region Region;
      List<int> Counts = new List<int>();
      Grid2d<char> Grid = new Grid2d<char>();
      List<int> TestIndices = new List<int>();
      int Placed = 0;
      public bool Solve(Problem problem, int regionIndex)
      {
        Problem = problem;
        Region = problem.Regions[regionIndex];
        Grid.Initialize(Region.Size.X, Region.Size.Y, '.');

        foreach (var c in Region.Counts)
          Counts.Add(0);

        for (var i = 0; i < Problem.Presents.Count; ++i)
          TestIndices.Add(i);

        TestIndices.Sort(CompareIndices);

        if (!IsPossibleToSolve())
          return false;
        if (IsTrivialToSolve())
          return true;

        Console.WriteLine($"Solving: {Region.ToString()}");
        return Solve2(0, new Int2(0, 0));
      }
      int CompareIndices(int a, int b)
      {
        var setA = Problem.PresentSets[a];
        var setB = Problem.PresentSets[b];
        return setA.Presents.Count.CompareTo(setB.Presents.Count);
      }
      void PrintCounts()
      {
        Console.Write("Target: ");
        foreach (var c in Region.Counts)
          Console.Write($" {c}");
        Console.WriteLine();
        Console.Write("Current: ");
        foreach (var c in Counts)
          Console.Write($" {c}");
        Console.WriteLine();
      }
      class StackItem
      {
        public int PresentIndex;
        public int SubIndex;
        public Int2 Pos;
      }
      bool IsPossibleToSolve()
      {
        // See if the total used area for all the presents exceed the region area.
        Int64 totalArea = Region.Size.X * Region.Size.Y;
        Int64 neededArea = 0;
        for(var i = 0; i < Region.Counts.Count; ++i)
        {
          Int64 presentCount = Region.Counts[i];
          Int64 presentArea = 0;
          var present = Problem.Presents[i];
          for(var y = 0; y < present.Height; ++y)
          {
            for(var x = 0; x < present.Width; ++x)
            {
              if(present[x, y] != '.')
                presentArea++;
            }
          }
          neededArea += presentArea * presentCount;
        }
        return neededArea < totalArea; ;
      }
      bool IsTrivialToSolve()
      {
        // See if the trivial layout (each present uses its full area) is possible to fit
        Int64 totalArea = Region.Size.X * Region.Size.Y;
        Int64 trivialArea = 0;
        for (var i = 0; i < Region.Counts.Count; ++i)
        {
          var present = Problem.Presents[i];
          Int64 presentCount = Region.Counts[i];
          Int64 presentArea = present.Width * present.Height;
          trivialArea += presentArea * presentCount;
        }
        return trivialArea <= totalArea;
      }
      bool Solve2(int presentIndex, Int2 pos)
      {
        if(presentIndex >= Problem.Presents.Count)
        {
          Console.WriteLine("Solved");
          return true;
        }
        var targetCount = Region.Counts[presentIndex];
        //PrintCounts();
        //Console.WriteLine($"PresentIndex {presentIndex}: Count({Counts[presentIndex]}) Target({targetCount})");
        if (Counts[presentIndex] == targetCount)
        {
          //Console.WriteLine("TargetCount hit");
          return Solve2(presentIndex + 1, new Int2());
        }

        var presentSet = Problem.PresentSets[presentIndex];
        var firstPresent = presentSet.Presents[0];
        var xEnd = Grid.Width - firstPresent.Width;
        var yEnd = Grid.Height - firstPresent.Height;

        for (var y = pos.Y; y <= yEnd; ++y)
        {
          for(var x = pos.X; x <= xEnd; ++x)
          {
            for (var i = 0; i < presentSet.Presents.Count; ++i)
            {
              var present = presentSet.Presents[i];
              bool canPlace = CanPlace(presentSet.Presents[i], x, y);
              if(canPlace)
              {
                Place(present, x, y);
                Counts[presentIndex]++;
                ++Placed;

                //Console.WriteLine("Placing into grid:");
                //PrintGrid();

                if (Solve2(presentIndex, new Int2(x, y)))
                  return true;

                UnPlace(present, x, y);
                Counts[presentIndex]--;
                --Placed;

                //Console.WriteLine("Unplacing from grid:");
                //PrintGrid();
              }
            }
          }
        }
        return false;
      }
      bool Solve(int testIndex)
      {
        if (testIndex >= TestIndices.Count)
        {
          Console.WriteLine("Solved");
          PrintGrid();
          return true;
        }
        int presentIndex = TestIndices[testIndex];

        if (presentIndex >= Problem.PresentSets.Count)
        {
          Console.WriteLine("Solved");
          PrintGrid();
          return true;
        }


        //Console.WriteLine($"PresentIndex {presentIndex}:");
        //PrintCounts();

        var targetCount = Region.Counts[presentIndex];
        //Console.WriteLine($"PresentIndex {presentIndex}: Count({Counts[presentIndex]}) Target({targetCount})");
        if (Counts[presentIndex] == targetCount)
        {
          //Console.WriteLine("TargetCount hit");
          return Solve(presentIndex + 1);
        }

        var presentSet = Problem.PresentSets[presentIndex];
        foreach(var present in presentSet.Presents)
        {
          var xStart = 0;
          var yStart = 0;
          var xEnd = Grid.Width - present.Width;
          var yEnd = Grid.Height - present.Height;
          for(var y = yStart; y <= yEnd; ++y)
          {
            for (var x = xStart; x <= xEnd; ++x)
            {
              bool canPlace = CanPlace(present, x, y);
              //PrintTestPlacement(present, x, y, canPlace);

              if (canPlace)
              {
                Place(present, x, y);
                Counts[presentIndex]++;
                ++Placed;

                //Console.WriteLine("Placing into grid:");
                //PrintGrid();

                if (Solve(testIndex))
                  return true;

                UnPlace(present, x, y);
                Counts[presentIndex]--;
                --Placed;

                //Console.WriteLine("Unplacing from grid:");
                //PrintGrid();
              }
            }
          }
        }
        return false;
      }
      void PrintGrid()
      {
        Console.WriteLine("CurrentGrid:");
        PrintGrid(Grid);
      }
      void PrintGrid(Grid2d<char> grid)
      {
        for(var y = 0; y < grid.Height; ++y)
        {
          for (var x = 0; x < grid.Width; ++x)
            Console.Write(grid[x, y]);
          Console.WriteLine();
        }
      }
      void PrintTestPlacement(Present present, int x, int y, bool canPlace)
      {
        var grid = new Grid2d<char>();
        grid.Initialize(Grid.Width, Grid.Height, '.');
        Place(grid, present, x, y);

        Console.WriteLine($"Testing ({x} {y}):");
        PrintGrid(grid);
        Console.WriteLine($"CanPlace: {canPlace}");
      }
      bool CanPlace(Present present, int xStart, int yStart)
      {
        if (xStart + present.Width > Grid.Width)
          return false;
        if (yStart + present.Height > Grid.Height)
          return false;

        for(var y0 = 0; y0 < present.Height; ++y0)
        {
          for(var x0 = 0; x0 < present.Width; ++x0)
          {
            if (present[x0, y0] == '.')
              continue;
            var x = x0 + xStart;
            var y = y0 + yStart;
            if (Grid[x, y] != '.')
              return false;
          }
        }
        return true;
      }
      void Place(Present present, int xStart, int yStart) => Place(Grid, present, xStart, yStart);
      void Place(Grid2d<char> grid, Present present, int xStart, int yStart)
      {
        for (var y0 = 0; y0 < present.Height; ++y0)
        {
          for (var x0 = 0; x0 < present.Width; ++x0)
          {
            var x = x0 + xStart;
            var y = y0 + yStart;
            if (present[x0, y0] != '.')
              grid[x, y] = (char)('A' + Placed);
          }
        }
      }
      void UnPlace(Present present, int xStart, int yStart)
      {
        for (var y0 = 0; y0 < present.Height; ++y0)
        {
          for (var x0 = 0; x0 < present.Width; ++x0)
          {
            var x = x0 + xStart;
            var y = y0 + yStart;
            if (present[x0, y0] != '.')
                Grid[x, y] = '.';
          }
        }
      }
    }
  }
}
