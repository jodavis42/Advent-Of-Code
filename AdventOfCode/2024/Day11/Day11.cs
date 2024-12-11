namespace AdventOfCode2024
{
  internal class Day11
  {
    int SimulationCount = 25;
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var slabs = Parse(lines);
      Print(slabs);
      Console.WriteLine();

      for(var i = 0; i < SimulationCount; ++i)
      {
        Simulate(slabs);
      }
      Console.WriteLine(slabs.Count);
    }

    public void Run2(string filePath)
    {
      SimulationCount = 75;
      var lines = File.ReadAllLines(filePath);
      var slabs = Parse(lines);
      Print(slabs);
      Console.WriteLine();

      Int64 counts = Simulate2(slabs, SimulationCount);
      Console.WriteLine(counts);
    }
    void Print(List<Slab> slabs)
    {
      foreach (var slab in slabs)
      {
        Console.Write(slab + " ");
      }
    }

    class Slab
    {
      public Int64 Value;
      public Int64 Counts;
      public override string ToString()
      {
        return Value.ToString();
      }
    }
    List<Slab> Parse(string[] lines)
    {
      var result = new List<Slab>();
      foreach(var split in lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        Int64 value = Int64.Parse(split);
        result.Add(new Slab { Value = value });
      }
      return result;
    }
    Int64 CountDigits(Int64 value)
    {
      if (value == 0)
        return 1;
      Int64 count = 0;
      while (value != 0)
      {
        value /= 10;
        ++count;
      }
      return count;
    }
    void Split(Int64 value, Int64 digits, out Int64 left, out Int64 right)
    {
      left = value / (Int64)(Math.Pow(10, digits / 2));
      right = value % (Int64)(Math.Pow(10, digits / 2));
    }
    void Simulate(List<Slab> slabs)
    {
      for(var i = 0; i  < slabs.Count; ++i)
      {
        var slab = slabs[i];
        if (slab.Value == 0)
        {
          slab.Value = 1;
          continue;
        }
        var digits = CountDigits(slab.Value);
        if(digits % 2 == 0)
        {
          Int64 leftValue, rightValue;
          Split(slab.Value, digits, out leftValue, out rightValue);
          slab.Value = leftValue;
          var newRightSlab = new Slab { Value = rightValue };
          ++i;
          slabs.Insert(i, newRightSlab);
        }
        else
        {
          slab.Value *= 2024;
        }
      }
    }
    Int64 Simulate2(List<Slab> slabs, int iterations)
    {
      void TryIncrement(Dictionary<Int64, Int64> map, Int64 value, Int64 incrementValue)
      {
        if (!map.ContainsKey(value))
          map[value] = 0;
        map[value] += incrementValue;
      }

      var countMap = new Dictionary<Int64, Int64>();
      foreach (var slab in slabs)
        TryIncrement(countMap, slab.Value, 1);

      for(var i = 0; i < iterations; ++i)
      {
        var newCounts = new Dictionary<Int64, Int64>();
        foreach(var pair in countMap)
        {
          var value = pair.Key;
          var count = pair.Value;

          if (value == 0)
            TryIncrement(newCounts, 1, count);
          else
          {
            var digits = CountDigits(value);
            if (digits % 2 == 0)
            {
              Int64 leftValue, rightValue;
              Split(value, digits, out leftValue, out rightValue);
              TryIncrement(newCounts, leftValue, count);
              TryIncrement(newCounts, rightValue, count);
            }
            else
            {
              TryIncrement(newCounts, value * 2024, count);
            }
          }
        }
        countMap = newCounts;
      }
      Int64 result = 0;
      foreach(var pair in countMap)
      {
        result += pair.Value;
      }
      return result;
    }
  }
}
