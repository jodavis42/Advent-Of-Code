namespace AdventOfCode2015
{
  internal class Day15
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var propertyList = Parse(lines);
      foreach (var cookieProperites in propertyList)
        cookieProperites.Print();

      Int64 bestScore = Solve(propertyList, 100);
      Console.WriteLine(bestScore);
    }

    public void Run2(string filePath)
    {
      Part2 = true;

      var lines = File.ReadAllLines(filePath);
      var propertyList = Parse(lines);
      foreach (var cookieProperites in propertyList)
        cookieProperites.Print();

      Int64 bestScore = Solve(propertyList, 100);
      Console.WriteLine(bestScore);
    }

    bool Part2 = false;

    class Properties
    {
      public string Name;
      public Int64 Capacity;
      public Int64 Durability;
      public Int64 Flavor;
      public Int64 Texture;
      public Int64 Calories;

      public void Print()
      {
        Console.WriteLine($"{Name}: capacity {Capacity}, durability {Durability}, flavor {Flavor}, texture {Texture}, colories {Calories}");
      }
    }

    List<Properties> Parse(string[] lines)
    {
      var results = new List<Properties>();
      foreach(var line in lines)
      {
        var split = line.Split(new char[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
        var split2 = line.Split();
        var cookieProperties = new Properties
        {
          Name = split[0],
          Capacity = Int64.Parse(split[2]),
          Durability = Int64.Parse(split[4]),
          Flavor = Int64.Parse(split[6]),
          Texture = Int64.Parse(split[8]),
          Calories = Int64.Parse(split[10]),
        };
        results.Add(cookieProperties);
      }
      return results;
    }

    Int64 Solve(List<Properties> Properties, Int64 teaspoons)
    {
      var counts = new List<Int64>();
      for (var i = 0; i < Properties.Count; ++i)
        counts.Add(0);

      Int64 bestScore = 0;
      Solve(Properties, teaspoons, counts, 0, ref bestScore);
      return bestScore;
    }

    void Solve(List<Properties> Properties, Int64 teaspoons, List<Int64> counts, int index, ref Int64 bestScore)
    {
      var usedTeaspoons = (Int64)0;
      for(var i = 0; i < index; ++i)
        usedTeaspoons += counts[i];
      var remainingTeaspoons = teaspoons - usedTeaspoons;

      if (index == counts.Count - 1)
      {
        
        counts[counts.Count - 1] = remainingTeaspoons;

        Int64 score = ComputeScore(Properties, teaspoons, counts);
        if (score > bestScore)
          bestScore = score;
        return;
      }

      for(var i = 0; i < remainingTeaspoons; ++i)
      {
        counts[index] = i;
        Solve(Properties, teaspoons, counts, index + 1, ref bestScore);
      }
    }

    Int64 ComputeScore(List<Properties> Properties, Int64 teaspoons, List<Int64> counts)
    {
      var scores = new List<Int64>();
      for (var i = 0; i < 5; ++i)
        scores.Add(0);

      for (var i = 0; i < Properties.Count; ++i)
      {
        var cookieProperties = Properties[i];
        var c = counts[i];
        scores[0] += c * cookieProperties.Capacity;
        scores[1] += c * cookieProperties.Durability;
        scores[2] += c * cookieProperties.Flavor;
        scores[3] += c * cookieProperties.Texture;
        scores[4] += c * cookieProperties.Calories;
      }
      if(Part2)
      {
        if (scores[4] != 500)
          return 0;
      }
      Int64 totalScore = 1;
      for(var i = 0; i < 4; ++i)
        totalScore *= Math.Max(scores[i], 0);
      return totalScore;
    }
  }
}
