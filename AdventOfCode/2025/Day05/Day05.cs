using System.Text;

namespace AdventOfCode2025
{
  internal class Day05
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines, out var ranges, out var ids);
      Print(ranges, ids);

      var freshCount = 0;
      foreach(var id in ids)
      {
        bool fresh = false;
        foreach (var range in ranges)
        {
          if(range.Start <= id && id <= range.End)
          {
            fresh = true;
            break;
          }
        }
        if(fresh)
        {
          Console.WriteLine($"Fresh: {id}");
          ++freshCount;
        }
      }
      Console.WriteLine($"FreshCount: {freshCount}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines, out var ranges, out var ids);

      var mergedRanges = new  List<Range>();
      Merge(ranges, mergedRanges);

      UInt64 freshCount = 0;
      foreach (var range in mergedRanges)
      {
        Console.WriteLine(range);
        freshCount += range.End - range.Start + 1;
      }
      Console.WriteLine(freshCount);
    }
    void Print(List<Range> ranges, List<UInt64> ids)
    {
      Print(ranges);
      Print(ids);
    }
    void Print(List<Range> ranges)
    {
      foreach (var range in ranges)
        Console.WriteLine(range);
    }
    void Print(List<UInt64> ids)
    {
      foreach (var id in ids)
        Console.WriteLine(id);
    }
    void Merge(List<Range> ranges, List<Range> mergedRanges)
    {
      // Reverse sort by start. Now we can always pop off the back and check the next item,
      // if the ranges overlap or are adjacent we pop and merge the ranges.
      ranges.Sort();
      while (ranges.Count > 0)
      {
        var index = ranges.Count - 1;
        var range = ranges[index];
        ranges.RemoveAt(index);
        mergedRanges.Add(range);

        for (var i = index - 1; i >= 0; --i)
        {
          // Note: check end + 1 so that we merge ranges likes [0-2][3-5] together
          if (ranges[i].Start <= range.End + 1)
          {
            // Take the max of the ranges, as since we sorted by start only, we could have the ranges:
            // [0-10], [2-5]
            range.End = Math.Max(ranges[i].End, range.End);
            ranges.RemoveAt(i);
          }
          else
            break;
        }
      }
    }
    class Range : IComparable<Range>
    {
      public UInt64 Start = 0;
      public UInt64 End = 0;

      public override string ToString()
      {
        return $"{Start}-{End}";
      }
      public int CompareTo(Range? other)
      {
        return -Start.CompareTo(other!.Start);
      }
      public bool Contains(Range other)
      {
        if (End < other.Start || other.End < Start)
          return false;
        return true;
      }
    }

    void Parse(string[] lines, out List<Range> ranges, out List<UInt64> ids)
    {
      ranges = new List<Range>();
      ids = new List<UInt64>();
      var i = 0;
      for(; lines[i].Length > 0; i++)
      {
        var split = lines[i].Split('-');
        var range = new Range();
        range.Start = UInt64.Parse(split[0]);
        range.End = UInt64.Parse(split[1]);
        ranges.Add(range);
      }
      ++i;
      for (; i < lines.Length; i++)
      {
        ids.Add(UInt64.Parse(lines[i]));
      }
    }
  }
}
