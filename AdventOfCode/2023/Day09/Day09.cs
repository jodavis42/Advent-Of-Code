namespace AdventOfCode2023
{
  internal class Day09
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      Print();
      Int64 sum = 0;
      foreach(var value in Values)
      {
        Console.WriteLine();
        var sequences = ComputeSequences(value);
        Solve(sequences);
        Print(sequences);
        sum += sequences[0][sequences[0].Count - 1];
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      Print();
      Int64 sum = 0;
      foreach (var value in Values)
      {
        Console.WriteLine();
        var sequences = ComputeSequences(value);
        Solve2(sequences);
        Print(sequences);
        sum += sequences[0][0];
      }
      Console.WriteLine(sum);
    }
    class Value
    {
      public List<Int64> History = new List<Int64>();
    }
    List<Value> Values = new List<Value>();
    public void Parse(string[] lines)
    {
      foreach(var line in lines)
      {
        var split = line.Split(' ');
        var value = new Value();
        foreach(var s in split)
          value.History.Add(Int64.Parse(s));
        Values.Add(value);
      }
    }
    public void Print() 
    { 
      foreach(var value in Values)
      {
        foreach(var h in value.History)
          Console.Write(h + " ");
        Console.WriteLine();
      }
    }
    List<List<Int64>> ComputeSequences(Value value)
    {
      var sequences = new List<List<Int64>>();
      sequences.Add(value.History);
      while(true)
      {
        var prev = sequences[sequences.Count - 1];
        var next = new List<Int64>();
        sequences.Add(next);
        bool isAllZero = true;
        for(var i = 1; i < prev.Count; ++i)
        {
          var delta = prev[i] - prev[i - 1];
          next.Add(delta);
          if (delta != 0)
            isAllZero = false;
        }
        if (isAllZero)
          break;
      }
      return sequences;
    }
    void Solve(List<List<Int64>> sequences)
    {
      foreach(var s in sequences)
      {
        s.Add(0);
      }
      for(var i = sequences.Count - 2; i >= 0; --i)
      {
        var prev = sequences[i + 1];
        var next = sequences[i];
        var b = prev[prev.Count - 1];
        var l = next[next.Count - 2];
        // b = r - l
        // r = b + l
        var r = b + l;
        next[next.Count - 1] = r;
      }
    }
    void Solve2(List<List<Int64>> sequences)
    {
      foreach (var s in sequences)
      {
        s.Insert(0, 0);
      }
      for (var i = sequences.Count - 2; i >= 0; --i)
      {
        var prev = sequences[i + 1];
        var next = sequences[i];
        var b = prev[0];
        var r = next[1];
        // b = r - l
        // l = r - b
        var l = r - b;
        next[0] = l;
      }
    }
    void Print(List<List<Int64>> sequences)
    {
      foreach(var s in sequences)
      {
        foreach(var v in s)
          Console.Write(v + " ");
        Console.WriteLine();
      }
    }
  }
}
