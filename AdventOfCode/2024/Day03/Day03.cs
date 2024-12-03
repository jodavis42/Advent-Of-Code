using AdventOfCode2021;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
  internal class Day03
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var regex = new Regex("mul\\((\\d+),(\\d+)\\)");

      int sum = 0;
      foreach (var line in lines)
      {
        var matches = regex.Matches(line);
        foreach (Match match in matches)
        {
          var lhs = int.Parse(match.Groups[1].Value);
          var rhs = int.Parse(match.Groups[2].Value);
          sum += lhs * rhs;
          Console.WriteLine($"{lhs} {rhs}");
        }
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var regex = new Regex("mul\\((\\d+),(\\d+)\\)|do\\(\\)|don't\\(\\)");

      int sum = 0;
      bool skip = false;
      foreach (var line in lines)
      {
        var matches = regex.Matches(line);
        foreach (Match match in matches)
        {
          if(match.Value == "do()")
          {
            skip = false;
            continue;
          }
          if(match.Value == "don't()")
          {
            skip = true;
            continue;
          }

          if(skip)
          {
            continue;
          }

          var lhs = int.Parse(match.Groups[1].Value);
          var rhs = int.Parse(match.Groups[2].Value);
          sum += lhs * rhs;
          Console.WriteLine($"{lhs} {rhs}");
        }
      }
      Console.WriteLine(sum);
    }
  }
}
