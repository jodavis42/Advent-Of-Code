using System.Diagnostics;

namespace AdventOfCode2015
{
  internal class Day05
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      uint niceCount = 0;
      foreach(var line in lines)
      {
        bool isNice = IsNice(line);
        if (isNice)
        {
          ++niceCount;
          Console.WriteLine("Nice");
        }
        else
          Console.WriteLine("Naughty");
      }
      Console.WriteLine(niceCount);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      uint niceCount = 0;
      foreach (var line in lines)
      {
        bool isNice = IsNice2(line);
        if (isNice)
        {
          ++niceCount;
          Console.WriteLine($"{line}: Nice");
        }
        else
          Console.WriteLine($"{line}: Naughty");
      }
      Console.WriteLine(niceCount);
    }
    bool IsNice(string text)
    {
      uint vowelCount = 0;
      bool hasDuplicate = false;
      bool hasBadString = false;
      for(var i = 0; i < text.Count(); ++i)
      {
        var c = text[i];
        vowelCount += IsVowel(c) ? 1u : 0u;
        if (i > 0 && c == text[i - 1])
          hasDuplicate = true;
        if (i > 0)
        {
          var p = text[i - 1];
          if((p == 'a' && c == 'b') ||
            (p == 'c' && c == 'd') ||
            (p == 'p' && c == 'q') ||
            (p == 'x' && c == 'y'))
            hasBadString = true;
        }
      }
      return vowelCount >= 3 && hasDuplicate && !hasBadString;
    }
    bool IsNice2(string text)
    {
      bool hasRepeat = false;
      bool hasDouble = false;
      var set = new Dictionary<string, int>();
      for(var i = 1; i < text.Count(); ++i)
      {
        var subStr = text.Substring(i - 1, 2);
        if(set.TryGetValue(subStr, out var oldIndex))
        {
          if(oldIndex != i - 1)
            hasDouble = true;
        }
        else
          set[subStr] = i;
      }
      for (var i = 2; i < text.Count(); ++i)
      {
        if (text[i - 2] == text[i])
          hasRepeat = true;
      }
      return hasRepeat && hasDouble;
    }
    bool IsVowel(char c)
    {
      return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }
  }
}
