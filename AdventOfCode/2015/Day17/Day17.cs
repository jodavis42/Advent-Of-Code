namespace AdventOfCode2015
{
  internal class Day17
  {
    int TargetLiters = 150;

    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var containers = Parse(lines);
      
      var solutionCache = new SolutionCache();
      Solve(TargetLiters, containers, 0, TargetLiters, solutionCache);
      Console.WriteLine(solutionCache.SolutionCount);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var containers = Parse(lines);

      var solutionCache = new SolutionCache();
      Solve(TargetLiters, containers, 0, TargetLiters, solutionCache);
      var minSize = int.MaxValue;
      foreach(var pair in solutionCache.CountsPerSize)
      {
        if(pair.Key <  minSize)
          minSize = pair.Key;
      }
      Console.WriteLine(solutionCache.CountsPerSize[minSize]);
    }

    static int Compare(int a, int b)
    {
      return -a.CompareTo(b);
    }

    List<int> Parse(string[] lines)
    {
      var results = new List<int>();
      foreach(var line in lines)
        results.Add(int.Parse(line));
      results.Sort(Compare);
      return results;
    }

    class Context
    {
      public int TargetLiters = 0;
      public List<int> Containers = new List<int>();
      public int Index = 0;
      public List<int> UsedContainerIndices = new List<int>();
      public int RemainingLiters = 0;

      public int SolutionCount = 0;
    }
    class SolutionCache
    {
      public int SolutionCount;
      public List<int> UsedContainerIndices = new List<int>();
      public Dictionary<int, int> CountsPerSize = new Dictionary<int, int>();
    }

    void Solve(int targetLiters, List<int> containers, int currentIndex, int remainingLiters, SolutionCache solutionCache)
    {
      if(remainingLiters == 0)
      {
        ++solutionCache.SolutionCount;
        var count = solutionCache.UsedContainerIndices.Count;
        if (!solutionCache.CountsPerSize.ContainsKey(count))
          solutionCache.CountsPerSize[count] = 1;
        else
          solutionCache.CountsPerSize[count]++;
        Print(containers, solutionCache);
        return;
      }

      if (currentIndex >= containers.Count)
        return;

      for (var i = currentIndex; i < containers.Count; ++i)
      {
        var containerSize = containers[i];
        if (containerSize > remainingLiters)
          continue;

        solutionCache.UsedContainerIndices.Add(i);
        Solve(targetLiters, containers, i + 1, remainingLiters - containerSize, solutionCache);
        solutionCache.UsedContainerIndices.RemoveAt(solutionCache.UsedContainerIndices.Count - 1);
      }
    }
    void Print(List<int> containers, SolutionCache solutionCache)
    {
      for(var i = 0; i < containers.Count; ++i)
      {
        bool isFilled = solutionCache.UsedContainerIndices.IndexOf(i) != -1;
        string expression = isFilled ? "x" : " ";
        Console.Write($"{containers[i]}[{expression}] ");
      }
      Console.WriteLine();
    }
  }
}
