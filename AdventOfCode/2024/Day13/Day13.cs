using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
  internal class Day13
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var machines = Parse(lines);

      Int64 totalCost = 0;
      foreach (var machine in machines)
      {
        Console.WriteLine(machine);

        Int64 x = 0, y = 0;
        if(machine.TrySolve(ref x, ref y))
        {
          totalCost += x * 3 + y;
          Console.WriteLine($"{x} {y}");
        }
      }
      Console.WriteLine(totalCost);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var machines = Parse(lines);

      Int64 offset = 10000000000000;
      Int64 totalCost = 0;
      foreach (var machine in machines)
      {
        //Console.WriteLine(machine);
        machine.Px += offset;
        machine.Py += offset;

        Int64 x = 0, y = 0;
        if (machine.TrySolve(ref x, ref y))
        {
          totalCost += x * 3 + y;
        }
      }
      Console.WriteLine(totalCost);
    }

    class Machine
    {
      public Int64 Ax, Ay;
      public Int64 Bx, By;
      public Int64 Px, Py;
      public override string ToString()
      {
        return $"({Ax}, {Ay}) ({Bx}, {By}) ({Px}, {Py})";
      }
      public bool TrySolve(ref Int64 outX, ref Int64 outY)
      {
        Int64 denom = Ay * Bx - Ax * By;
        if (denom == 0)
          return false;
        var num = Ay * Px - Ax * Py;
        outY = num / denom;
        outX = (Px - Bx * outY) / Ax;

        if (Px != outX * Ax + outY * Bx)
          return false;
        if (Py != outX * Ay + outY * By)
          return false;
        return true;
      }
    }

    List<Machine> Parse(string[] lines)
    {
      var buttonRegex = new Regex(":\\sX\\+(\\d+),\\sY\\+(\\d+)");
      var prizeRegex = new Regex("X=(\\d+),\\sY=(\\d+)");

      var result = new List<Machine>();
      for(var i = 0; i < lines.Length; i += 4)
      {
        
        var aMatch = buttonRegex.Match(lines[i + 0]);
        var bMatch = buttonRegex.Match(lines[i + 1]);
        var prizeMatch = prizeRegex.Match(lines[i + 2]);

        var machine = new Machine();
        machine.Ax = Int64.Parse(aMatch.Groups[1].Value);
        machine.Ay = Int64.Parse(aMatch.Groups[2].Value);
        machine.Bx = Int64.Parse(bMatch.Groups[1].Value);
        machine.By = Int64.Parse(bMatch.Groups[2].Value);

        machine.Px = Int64.Parse(prizeMatch.Groups[1].Value);
        machine.Py = Int64.Parse(prizeMatch.Groups[2].Value);
        result.Add(machine);
      }
      return result;
    }
  }
}
