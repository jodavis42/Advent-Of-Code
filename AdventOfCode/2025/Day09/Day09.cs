namespace AdventOfCode2025
{
  internal class Day09
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var points = Parse(lines);
      foreach(var point in points)
        Console.WriteLine(point);
      var area = SolveBruteForce(points, out var P0, out var P1);
      Console.WriteLine($"{P0}, {P1}: {area}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var points = Parse(lines);
      foreach (var point in points)
        Console.WriteLine(point);
      var area = SolveBruteForce2(points, out var P0, out var P1);
      Console.WriteLine($"{P0}, {P1}: {area}");
    }
    List<Int2> Parse(string[] lines)
    {
      var result = new List<Int2>();
      foreach(var line in lines)
      {
        var split = line.Split(',');
        var v = new Int2(int.Parse(split[0]), int.Parse(split[1]));
        result.Add(v);
      }
      return result;
    }
    Int64 SolveBruteForce(List<Int2> points, out Int2 OutP0, out Int2 OutP1)
    {
      OutP0 = OutP1 = new Int2();
      Int64 bestArea = 0;
      for (var i = 0; i < points.Count; ++i)
      {
        for(var j = i + 1; j < points.Count; ++j)
        {
          var p0 = points[i];
          var p1 = points[j];
          Int64 xMin = Math.Min(p0.X, p1.X);
          Int64 xMax = Math.Max(p0.X, p1.X);
          Int64 yMin = Math.Min(p0.Y, p1.Y);
          Int64 yMax = Math.Max(p0.Y, p1.Y);
          Int64 area = (xMax - xMin + 1) * (yMax - yMin + 1);
          if(area > bestArea)
          {
            bestArea = area;
            OutP0 = p0;
            OutP1 = p1;
          }
        }
      }
      return bestArea;
    }
    struct Rect2d
    {
      public Int2 P0;
      public Int2 P1;
      public IntAabb2 Aabb;
      public Int64 Area;
      public Rect2d(Int2 p0, Int2 p1)
      {
        P0 = p0;
        P1 = p1;
        Aabb = new IntAabb2(p0, p1);
        Area = Aabb.Area();
      }
      public static int Compare(Rect2d left, Rect2d right)
      {
        return -left.Area.CompareTo(right.Area);
      }
    }

    Int64 SolveBruteForce2(List<Int2> points, out Int2 OutP0, out Int2 OutP1)
    {
      OutP0 = OutP1 = new Int2();
      // Build all possible rects
      List<Rect2d> rects = new List<Rect2d>();
      for (var i = 0; i < points.Count; ++i)
      {
        for (var j = i + 1; j < points.Count; ++j)
        {
          rects.Add(new Rect2d(points[i], points[j]));
        }
      }
      // Sort them so that the largest area is first
      rects.Sort(Rect2d.Compare);
      // Check each rect. If any crosses another line then skip that.
      foreach(var rect in rects)
      {
        bool valid = true;
        for(var i = 0; i < points.Count; ++i)
        {
          var p0 = points[i];
          var p1 = points[(i + 1) % points.Count];
          var aabb = new IntAabb2(p0, p1);
          if (rect.Aabb.IntersectsExclusive(aabb))
          {
            valid = false;
            break;
          }
        }
        // We terminate on first valid rect as we know it's the largest
        if(valid)
        {
          OutP0 = rect.P0;
          OutP1 = rect.P1;
          return rect.Area;
        }
      }
      return 0;
    }
  }
}
