using System.Collections.Generic;

namespace AdventOfCode2015
{
  internal class Day14
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var reindeirs = Parse(lines);
      foreach (var reindeir in reindeirs)
        reindeir.Print();

      int maxDistance = 0;
      Reindeir bestReindeir = null;
      for(var i = 0; i < reindeirs.Count; ++i)
      {
        var reindeir = reindeirs[i];
        int distance = reindeir.CalculateDistance(2503);
        Console.WriteLine($"{reindeir.Name}: {distance}");
        if(distance > maxDistance)
        {
          maxDistance = distance;
          bestReindeir = reindeir;
        }
      }
      Console.WriteLine($"Best reindeir is {bestReindeir.Name} with a distance of {maxDistance}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var reindeirs = Parse(lines);
      foreach (var reindeir in reindeirs)
        reindeir.Print();

      var distances = new List<int>();
      var scores = new List<int>();
      foreach (var reindeir in reindeirs)
      {
        distances.Add(0);
        scores.Add(0);
      }

      for(var t = 0; t < 2503; ++t)
      {
        for(var i = 0; i < reindeirs.Count; ++i)
        {
          var reindeir = reindeirs[i];
          var totalCycleLength = reindeir.TravelTime + reindeir.RestTime;
          var remainder = t % totalCycleLength;
          if (remainder < reindeir.TravelTime)
            distances[i] += reindeir.Speed;
        }

        var maxDistance = 0;
        foreach (var distance in distances)
          maxDistance = Math.Max(maxDistance, distance);

        for(var i = 0; i < distances.Count; ++i)
        {
          if (distances[i] == maxDistance)
            scores[i] += 1;
        }
      }

      var bestScore = 0;
      for(var i = 0; i < reindeirs.Count; ++i)
      {
        var reindeir = reindeirs[i];
        Console.WriteLine($"{reindeir.Name} traveled {distances[i]} and got a score of {scores[i]}");
        bestScore = Math.Max(bestScore, scores[i]);
      }
      Console.WriteLine(bestScore);
    }

    class Reindeir
    {
      public string Name;
      public int Speed;
      public int TravelTime;
      public int RestTime;

      public void Print()
      {
        Console.WriteLine($"{Name}: {Speed} km/s for {TravelTime}, but then must rest for {RestTime}");
      }

      public int CalculateDistance(int time)
      {
        int totalCylcleTime = TravelTime + RestTime;
        int fullCycles = time / totalCylcleTime;
        int remainder = time % totalCylcleTime;
        int distance = fullCycles * Speed * TravelTime;
        if (remainder < TravelTime)
          distance += Speed * remainder;
        else
          distance += Speed * TravelTime;
        return distance;
      }
    }

    List<Reindeir> Parse(string[] lines)
    {
      var results = new  List<Reindeir>();
      foreach (var line in lines)
      {
        var split = line.Split();
        var reindeir = new Reindeir();
        reindeir.Name = split[0];
        reindeir.Speed = int.Parse(split[3]);
        reindeir.TravelTime = int.Parse(split[6]);
        reindeir.RestTime = int.Parse(split[13]);
        results.Add(reindeir);
      }
      return results;
    }
  }
}
