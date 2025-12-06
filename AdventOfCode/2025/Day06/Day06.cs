using System.Text;

namespace AdventOfCode2025
{
  internal class Day06
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var problems = Parse(lines);

      foreach (var problem in problems)
        Console.WriteLine(problem);

      Int64 sum = 0;
      foreach (var problem in problems)
      {
        sum += Compute(problem);
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var problems = Parse2(lines);
      foreach (var problem in problems)
      {
        Console.WriteLine(problem);
      }

      Int64 sum = 0;
      foreach (var problem in problems)
      {
        sum += Compute(problem);
      }
      Console.WriteLine(sum);
    }
    class Problem
    {
      public List<Int64> Numbers = new List<Int64>();
      public char Symbol = '\0';
      public override string ToString()
      {
        var builder = new StringBuilder();
        foreach(var number in Numbers)
          builder.Append($"{number.ToString()} ");
        builder.Append(Symbol);
        return builder.ToString();
      }
    }
    List<Problem> Parse(string[] lines)
    {
      var result = new List<Problem>();

      var splits = new List<string[]>();
      foreach(var line in lines)
      {
        splits.Add(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
      }

      var problemLength = splits.Count - 1;
      var problemCount = splits[0].Length;
      
      for (var i = 0; i < problemCount; ++i)
        result.Add(new Problem());

      for(var i = 0; i < splits.Count - 1; ++i)
      {
        var split = splits[i];
        for(var j = 0; j < split.Length; ++j)
        {
          result[j].Numbers.Add(Int64.Parse(split[j]));
        }
      }

      var signsSplit = splits[splits.Count - 1];
      for(var i = 0; i < signsSplit.Length; ++i)
      {
        result[i].Symbol = signsSplit[i][0];
      }
      return result;
    }
    List<Problem> Parse2(string[] lines)
    {
      var result = new List<Problem>();

      var lastLine = lines.Length - 1;
      var i = 0;
      while(i != -1)
      {
        var problem = new Problem();
        result.Add(problem);

        problem.Symbol = lines[lastLine][i];
        var next = lines[lastLine].IndexOfAny(new char[] { '+', '*' }, i + 1);
        var end = next == -1 ? lines[lastLine].Length : next - 1;

        for (var x = i; x < end; ++x)
        {
          Int64 Number = 0;
          for (var y = 0; y < lastLine; ++y)
          {
            var c = lines[y][x];
            if (c != ' ')
              Number = Number * 10 + (c - '0');
          }
          problem.Numbers.Add(Number);
        }

        i = next;
      }
      return result;
    }
    Int64 Compute(Problem problem)
    {
      Int64 result = 0;
      if (problem.Symbol == '*')
        result = 1;

      foreach(var number in problem.Numbers)
      {
        if (problem.Symbol == '+')
          result += number;
        else if (problem.Symbol == '-')
          result -= number;
        else if (problem.Symbol == '*')
          result *= number;
        else if (problem.Symbol == '/')
          result /= number;
      }
      return result;
    }
  }
}
