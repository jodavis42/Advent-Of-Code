using System.Threading;

namespace AdventOfCode2023
{
  internal class Day01
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var sum = 0;
      foreach(var line in lines)
      {
        var digits = new List<char>();
        foreach(var c in line)
        {
          if(int.TryParse(c.ToString(), out var value))
            digits.Add(c);
        }
        var digit0 = digits[0];
        var digit1 = digits[digits.Count - 1];
        var calibrationValue = int.Parse($"{digit0}{digit1}");
        Console.WriteLine(calibrationValue);
        sum += calibrationValue;
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var sum = 0;
      foreach (var line in lines)
      {
        var calibrationValue = GetCalibrationValue(line);
        sum += calibrationValue;
      }
      Console.WriteLine(sum);
    }

    public int GetCalibrationValue(string text)
    {
      var digits = new List<int>();
      var textDigits = new List<string>()
      {
        "one","two", "three", "four", "five", "six", "seven", "eight", "nine"
      };

      for(var i = 0; i < text.Length; ++i)
      {
        var c = text[i];
        if ('0' <= c && c <= '9')
          digits.Add(c - '0');
        else
        {
          var subStr = text.Substring(i);
          for(var d = 0; d < textDigits.Count; ++d)
          {
            if (subStr.StartsWith(textDigits[d]))
            {
              digits.Add(d + 1);
              break;
            }
          }
        }
      }
      var digit0 = digits[0];
      var digit1 = digits[digits.Count - 1];
      var calibrationValue = digit0 * 10 + digit1;
      Console.WriteLine(calibrationValue);
      return calibrationValue;      
    }
  }
}
