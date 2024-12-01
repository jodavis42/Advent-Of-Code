namespace AdventOfCode2024
{
  internal class Day01
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var data = new Data();
      data.Parse(lines);
      data.Sort();

      var total = 0;
      for(var i = 0; i < data.Left.Count; i++)
      {
        var l = data.Left[i];
        var r = data.Right[i];
        var d = Math.Abs(l - r);
        total += d;
      }
      Console.WriteLine(total);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var data = new Data();
      data.Parse(lines);
      data.Sort();
      var counts = new Dictionary<int, int>();
      foreach(var num in data.Right)
      {
        if (!counts.ContainsKey(num))
          counts[num] = 0;
        ++counts[num];
      }

      var total = 0;
      foreach(var num in data.Left)
      {
        if (counts.ContainsKey(num))
          total += counts[num] * num;
      }
      Console.WriteLine(total);
    }

    class Data
    {
      public List<int> Left = new List<int>();
      public List<int> Right = new List<int>();
      public void Parse(string[] lines)
      {
        foreach(var line in lines)
        {
          var split = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
          Left.Add(int.Parse(split[0]));
          Right.Add(int.Parse(split[1]));
        }
      }

      public void Sort()
      {
        Left.Sort();
        Right.Sort();
      }
    }
  }
}
