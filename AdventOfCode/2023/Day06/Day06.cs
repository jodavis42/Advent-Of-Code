using System.Text;

namespace AdventOfCode2023
{
  internal class Day06
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      Print();

      var product = 1u;
      for (var i = 0; i < Times.Count(); ++i)
        product *= Solve(Times[i], Distances[i]);
      Console.WriteLine($"{product}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse2(lines);
      Print();

      var product = 1u;
      for (var i = 0; i < Times.Count(); ++i)
        product *= Solve2(Times[i], Distances[i]);
      Console.WriteLine($"{product}");
    }

    List<UInt64> Times = new List<UInt64>();
    List<UInt64> Distances = new List<UInt64>();
    void Parse(string[] lines)
    {
      var timesText = lines[0];
      var distancesText = lines[1];
      var times = timesText.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      var distances = distancesText.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      for (var i = 1; i < times.Length; ++i)
      {
        Times.Add(UInt64.Parse(times[i]));
        Distances.Add(UInt64.Parse(distances[i]));
      }
    }
    void Parse2(string[] lines)
    {
      var timesText = lines[0].Substring(5);
      var distancesText = lines[1].Substring(9);
      var times = timesText.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      var distances = distancesText.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
      var timesBuilder = new StringBuilder();
      var distancesBuilder = new StringBuilder();
      for (var i = 0; i < times.Length; ++i)
      {
        timesBuilder.Append(times[i]);
        distancesBuilder.Append(distances[i]);
      }
      Times.Add(UInt64.Parse(timesBuilder.ToString()));
      Distances.Add(UInt64.Parse(distancesBuilder.ToString()));
    }
    void Print()
    {
      Console.Write("Times: ");
      foreach (var time in Times)
        Console.Write($"{time} ");
      Console.WriteLine();
      Console.Write("Distances: ");
      foreach (var distance in Distances)
        Console.Write($"{distance} ");
      Console.WriteLine();
    }
    uint Solve(UInt64 time, UInt64 distance)
    {
      Console.WriteLine($"Solve {time} {distance}:");
      var count = 0u;
      for (var i = 0u; i < time; ++i)
      {
        var speed = i;
        var timeRemaining = time - i;
        var distanceTraveled = timeRemaining * speed;
        if (distanceTraveled > distance)
        {
          Console.WriteLine($" {time} {distanceTraveled}");
          ++count;
        }
      }
      Console.WriteLine($"Total: {count}");
      return count;
    }
    uint Solve2(UInt64 time, UInt64 distance)
    {
      var count = 0u;
      for (var i = 0u; i < time; ++i)
      {
        var speed = i;
        var timeRemaining = time - i;
        var distanceTraveled = timeRemaining * speed;
        if (distanceTraveled > distance)
        {
          ++count;
        }
      }
      Console.WriteLine($"Total: {count}");
      return count;
    }
  }
}
