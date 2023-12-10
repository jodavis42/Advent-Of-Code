using System.Diagnostics;

namespace AdventOfCode2023
{
  internal class Day05
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines, false);

      PrintData();
      var lowestLocation = UInt64.MaxValue;
      foreach (var seed in Seeds)
      {
        var location = this.Process(seed);
        lowestLocation = Math.Min(lowestLocation, location);
        Console.WriteLine($"Seed: {seed} Location: {location}");
      }
      Console.WriteLine(lowestLocation);
    }

    public void Run2(string filePath)
    {
      var entry = new Map.Entry { sourceStart = 4, destStart = 14, length = 4 };
      List<Range> p = new List<Range>();
      List<Range> u = new List<Range>();

      var test = (UInt64 start, UInt64 length) =>
          {
            p.Clear();
            u.Clear();
            entry.Convert(new Range { Start = start, Length = length }, p, u);
          };

      test(4, 4); // 14 4
      test(5, 2); // 15 2
      test(4, 1); // 14 1

      test(0, 1); // 0 1
      test(0, 3); // 0 3
      test(0, 4); // 0 4

      test(8, 4); // 8 4
      test(8, 1); // 8 1

      test(2, 4); // 2 2, 14 2 
      test(3, 2); // 3 1, 14 1

      test(6, 4); // 16 2, 8 2
      test(7, 2); // 17 1, 8 1

      test(0, 12); // 0 4, 8 4, 14 4
      test(3, 6); // 3 1, 8 1, 14 4




      var lines = File.ReadAllLines(filePath);
      Parse(lines, true);

      //PrintData();
      var lowestLocation = UInt64.MaxValue;
      var locationRanges = this.Process(Seeds2);
      foreach (var locationRange in locationRanges)
      {

        lowestLocation = Math.Min(lowestLocation, locationRange.Start);
      }
      Console.WriteLine(lowestLocation);
    }
    void PrintData()
    {
      Console.Write("Seeds: ");
      foreach (var seed in Seeds)
        Console.Write($"{seed} ");
      Console.WriteLine();
      Console.WriteLine("SeedToSoil:");
      SeedToSoil.Print();
      Console.WriteLine("SoilToFertilizer:");
      SoilToFertilizer.Print();
      Console.WriteLine("FertilizerToWater:");
      FertilizerToWater.Print();
      Console.WriteLine("WaterToLight:");
      WaterToLight.Print();
      Console.WriteLine("LightToTemperature:");
      LightToTemperature.Print();
      Console.WriteLine("TemperatureToHumidity:");
      TemperatureToHumidity.Print();
      Console.WriteLine("HumidityToLocation:");
      HumidityToLocation.Print();
    }
    List<UInt64> Seeds = new List<UInt64>();
    List<Range> Seeds2 = new List<Range>();
    Map SeedToSoil = new Map();
    Map SoilToFertilizer = new Map();
    Map FertilizerToWater = new Map();
    Map WaterToLight = new Map();
    Map LightToTemperature = new Map();
    Map TemperatureToHumidity = new Map();
    Map HumidityToLocation = new Map();

    [DebuggerDisplay("{Start} {Length} {Last}")]
    class Range
    {
      public UInt64 Start;
      public UInt64 Length;
      public UInt64 Last
      {
        get { return Start + Length - 1; }
        set { Length = Last - Start + 1; }
      }
    }

    class Map
    {
      public class Entry
      {
        public UInt64 destStart;
        public UInt64 sourceStart;
        public UInt64 length;

        public void Convert(List<Range> ranges, List<Range> processed, List<Range> unprocessed)
        {
          foreach (var range in ranges)
            Convert(range, processed, unprocessed);
        }

        public void Convert(Range range, List<Range> processed, List<Range> unprocessed)
        {
          var rangeStart = range.Start;
          var rangeEnd = rangeStart + range.Length;
          var sourceEnd = sourceStart + length;
          var minUpperBound = Math.Min(sourceStart, rangeEnd);
          var lb = Math.Max(sourceStart, rangeStart);
          var ub = Math.Min(sourceEnd, rangeEnd);
          var maxLowerBound = Math.Max(sourceEnd, rangeStart);

          // If there's a portion below the range, it maps over as is
          if (rangeStart < sourceStart)
          {
            unprocessed.Add(new Range { Start = rangeStart, Length = minUpperBound - rangeStart });
          }
          if (rangeEnd > sourceEnd)
          {
            unprocessed.Add(new Range { Start = maxLowerBound, Length = rangeEnd - maxLowerBound });
          }
          // Non empty intersection range
          if (lb < ub)
          {
            var newDestStart = destStart + (lb - sourceStart);
            var newDestEnd = destStart + (ub - sourceStart);
            processed.Add(new Range { Start = newDestStart, Length = newDestEnd - newDestStart });
          }
        }
      }
      public List<Entry> Entries = new List<Entry>();
      public void Add(UInt64 destStart, UInt64 sourceStart, UInt64 length)
      {
        var entry = new Entry();
        entry.destStart = destStart;
        entry.sourceStart = sourceStart;
        entry.length = length;
        Entries.Add(entry);
      }
      public UInt64 Get(UInt64 source)
      {
        foreach (var entry in Entries)
        {
          if (entry.sourceStart <= source && source < entry.sourceStart + entry.length)
          {
            var offset = source - entry.sourceStart;
            var dest = entry.destStart + offset;
            return dest;
          }
        }
        return source;
      }
      public List<Range> Get(List<Range> ranges)
      {
        var processed = new List<Range>();
        var unprocessed = new List<Range>();
        unprocessed.AddRange(ranges);
        foreach (var entry in Entries)
        {
          var newUnprocessed = new List<Range>();
          foreach (var range in unprocessed)
            entry.Convert(range, processed, newUnprocessed);
          unprocessed = newUnprocessed;
        }
        processed.AddRange(unprocessed);
        return processed;
      }
      public void Print()
      {
        foreach (var entry in Entries)
        {
          Console.WriteLine($"{entry.destStart} {entry.sourceStart} {entry.length}");
        }
      }
    }

    void Parse(string[] lines, bool part2)
    {
      var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
      for (var i = 0; i < lines.Length; ++i)
      {
        var line = lines[i];
        if (line.Length == 0)
          continue;

        if (line.StartsWith("seeds:"))
        {
          var seedSplit = line.Substring(6).Split(' ', options);
          if (part2)
          {
            for (var j = 0; j < seedSplit.Length; j += 2)
            {
              var start = UInt64.Parse(seedSplit[i]);
              var length = UInt64.Parse(seedSplit[i + 1]);
              Seeds2.Add(new Range { Start = start, Length = length });
            }
          }
          else
          {
            foreach (var seed in seedSplit)
            {
              if (UInt64.TryParse(seed, out var seedValue))
                Seeds.Add(seedValue);
            }
          }
        }
        else if (line.StartsWith("seed-to-soil map:"))
        {
          ++i;
          BuildMap(lines, ref i, SeedToSoil);
        }
        else if (line.StartsWith("soil-to-fertilizer map:"))
        {
          ++i;
          BuildMap(lines, ref i, SoilToFertilizer);
        }
        else if (line.StartsWith("fertilizer-to-water map:"))
        {
          ++i;
          BuildMap(lines, ref i, FertilizerToWater);
        }
        else if (line.StartsWith("water-to-light map:"))
        {
          ++i;
          BuildMap(lines, ref i, WaterToLight);
        }
        else if (line.StartsWith("light-to-temperature map:"))
        {
          ++i;
          BuildMap(lines, ref i, LightToTemperature);
        }
        else if (line.StartsWith("temperature-to-humidity map:"))
        {
          ++i;
          BuildMap(lines, ref i, TemperatureToHumidity);
        }
        else if (line.StartsWith("humidity-to-location map:"))
        {
          ++i;
          BuildMap(lines, ref i, HumidityToLocation);
        }
      }
    }
    void BuildMap(string[] lines, ref int index, Map map)
    {
      var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
      while (index < lines.Length && lines[index].Length != 0)
      {
        var split = lines[index].Split(' ', options);
        UInt64 destStart = UInt64.Parse(split[0]);
        UInt64 sourceStart = UInt64.Parse(split[1]);
        UInt64 rangeLength = UInt64.Parse(split[2]);
        map.Add(destStart, sourceStart, rangeLength);
        ++index;
      }
    }
    List<Range> Process(List<Range> seeds)
    {
      var soil = SeedToSoil.Get(seeds);
      var ferilizer = SoilToFertilizer.Get(soil);
      var water = FertilizerToWater.Get(ferilizer);
      var light = WaterToLight.Get(water);
      var temperature = LightToTemperature.Get(light);
      var humidity = TemperatureToHumidity.Get(temperature);
      var location = HumidityToLocation.Get(humidity);
      return location;
    }
    UInt64 Process(UInt64 seed)
    {
      var soil = SeedToSoil.Get(seed);
      var ferilizer = SoilToFertilizer.Get(soil);
      var water = FertilizerToWater.Get(ferilizer);
      var light = WaterToLight.Get(water);
      var temperature = LightToTemperature.Get(light);
      var humidity = TemperatureToHumidity.Get(temperature);
      var location = HumidityToLocation.Get(humidity);
      return location;
    }
  }
}
