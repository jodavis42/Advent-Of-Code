using System.Diagnostics;

namespace AdventOfCode2015
{
  internal class Day08
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      var totalMemSize = 0;
      var totalStringSize = 0;
      foreach(var line in lines)
      {
        Count(line, out var memSize, out var stringSize);
        Console.WriteLine(line);
        Console.WriteLine($"  ({memSize}, {stringSize})");
        totalMemSize += memSize;
        totalStringSize += stringSize;
      }
      Console.WriteLine($"{totalMemSize - totalStringSize}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      var totalMemSize = 0;
      var totalEncodedSize = 0;
      foreach (var line in lines)
      {
        CountEncoded(line, out var memSize, out var encodedSize);
        Console.WriteLine(line);
        Console.WriteLine($"  ({memSize}, {encodedSize})");
        totalMemSize += memSize;
        totalEncodedSize += encodedSize;
      }
      Console.WriteLine($"{totalEncodedSize - totalMemSize}");
    }

    void Count(string text, out int memorySize, out int stringLiteralSize)
    {
      memorySize = 0;
      stringLiteralSize = 0;
      memorySize = text.Length;

      bool inString = false;
      for (int i = 0; i < text.Length; ++i)
      {
        char curr = text[i];
        if (curr == '\"')
        {
          inString = !inString; ;
          continue;
        }

        if (curr == '\\')
        {
          char next = text[i + 1];
          if (next == '\\')
          {
            ++stringLiteralSize;
            i += 1;
            continue;
          }
          else if (next == '\"')
          {
            ++stringLiteralSize;
            i += 1;
            continue;
          }
          else if (next == 'x')
          {
            ++stringLiteralSize;
            i += 3;
            continue;
          }
        }
        else
          ++stringLiteralSize;
      }
    }

    void CountEncoded(string text, out int memorySize, out int encodedStringSize)
    {
      memorySize = 0;
      encodedStringSize = 0;
      memorySize = text.Length;

      encodedStringSize += 2; // add 2 for the new quotes
      for (int i = 0; i < text.Length; ++i)
      {
        char curr = text[i];
        if (curr == '\\')
          ++encodedStringSize;
        else if (curr == '"')
          ++encodedStringSize;
        ++encodedStringSize;
      }
    }
  }
}
