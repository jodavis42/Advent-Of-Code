using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace AdventOfCode2025
{
  internal class Day02
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var idRanges = Parse(lines);
      UInt64 totalInvalidIds = 0;
      foreach(var idRange in idRanges)
      {
        for(var id = idRange.Min; id <= idRange.Max; ++id)
        {
          bool isValid = IsValid(id);
          if(!isValid)
          {
            totalInvalidIds += id;
            Console.WriteLine($"Invalid: {id}");
          }
        }
        Console.WriteLine(idRange);
      }
      Console.WriteLine(totalInvalidIds);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var idRanges = Parse(lines);
      UInt64 totalInvalidIds = 0;
      foreach (var idRange in idRanges)
      {
        for (var id = idRange.Min; id <= idRange.Max; ++id)
        {
          bool isValid = IsValid2(id);
          if (!isValid)
          {
            totalInvalidIds += id;
            Console.WriteLine($"Invalid: {id}");
          }
        }
        Console.WriteLine(idRange);
      }
      Console.WriteLine(totalInvalidIds);
    }

    List<IdRange> Parse(string[] lines)
    {
      var line = lines[0];
      List<IdRange> results = new List<IdRange>();
      foreach (var range in line.Split(','))
      {
        var rangeSplit = range.Split("-");

        var idRange = new IdRange();
        idRange.Min = UInt64.Parse(rangeSplit[0]);
        idRange.Max = UInt64.Parse(rangeSplit[1]);
        results.Add(idRange);
      }
      return results;
    }
    bool IsValid(UInt64 id)
    {
      var idString = id.ToString();
      if (idString.Length % 2 == 1)
        return true;

      var halfLength = (idString.Length / 2);
      for(var i = 0; i < halfLength; ++i)
      {
        var c0 = idString[i];
        var c1 = idString[i + halfLength];
        if(c0 != c1)
        {
          return true;
        }
      }
      return false;
    }
    bool IsValid2(UInt64 id)
    {
      var idString = id.ToString();
      var halfLength = (idString.Length / 2);
      for(var length = 1; length <= halfLength; ++length)
      {
        if(CheckInvalid(idString, length))
        {
          return false;
        }
      }
      return true;
    }
    bool CheckInvalid(string id, int length)
    {
      if (id.Length % length != 0)
        return false;

      // Note: Could start at length to skip the start...
      for (var i = 0; i < id.Length; ++i)
      {
        var c0 = id[i];
        var c1 = id[i % length];
        if (c0 != c1)
        {
          return false;
        }
      }
      return true;
    }

    class IdRange
    {
      public UInt64 Min = 0;
      public UInt64 Max = 0;
      public override string ToString()
      {
        return $"{Min}-{Max}";
      }
    }
  }
}
