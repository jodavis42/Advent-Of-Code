namespace AdventOfCode2024
{
  internal class Day02
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var reports = Parse(lines);
      Print(reports);

      int safeCount = 0;
      foreach(var report in reports)
      {
        bool isSafe = IsSafe(report);
        if (isSafe)
          ++safeCount;
        Console.WriteLine($"{isSafe}");
      }
      Console.WriteLine(safeCount);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var reports = Parse(lines);
      //Print(reports);

      int safeCount = 0;
      foreach (var report in reports)
      {
        bool isSafe = IsSafeWithDampener(report);
        if (isSafe)
          ++safeCount;
        Console.WriteLine($"{isSafe}");
      }
      Console.WriteLine(safeCount);
    }

    class Report
    {
      public List<int> Levels = new List<int>();
    }
    List<Report> Parse(string[] lines)
    {
      var results = new List<Report>();
      foreach(var line in lines)
      {
        var report = new Report();
        results.Add(report);

        var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach(var s in split)
          report.Levels.Add(int.Parse(s));
      }
      return results;
    }
    void Print(List<Report> reports)
    {
      foreach (var report in reports)
      {
        foreach(var level in report.Levels)
          Console.Write($"{level} ");
        Console.WriteLine();
      }
    }
    bool IsSafe(Report report)
    {
      var totalSign = 0;
      for(var i = 1; i < report.Levels.Count; ++i)
      {
        var prev = report.Levels[i - 1];
        var curr = report.Levels[i];
        var diff = prev - curr;
        var sign = Math.Sign(diff);
        var absDiff = Math.Abs(diff);
        if (!(1 <= absDiff && absDiff <= 3))
          return false;
        if (totalSign == 0)
          totalSign = sign;
        if (totalSign != sign)
          return false;
      }
      return true;
    }
    bool IsSafe(Report report, int levelIndexToSkip)
    {
      var newReport = new Report();
      for(var i = 0; i <  report.Levels.Count; ++i)
      {
        if (i == levelIndexToSkip)
          continue;
        newReport.Levels.Add(report.Levels[i]);
      }
      return IsSafe(newReport);
    }

    bool IsSafeWithDampener(Report report)
    {
      if (IsSafe(report))
        return true;

      for (var i = 0; i < report.Levels.Count; ++i)
      {
        if (IsSafe(report, i))
          return true;
      }
      return false;
    }
  }
}
