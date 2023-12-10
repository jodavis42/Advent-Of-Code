using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AdventOfCode2023
{
  internal class Day08
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      Print();
      var stepCount = Walk();
      Console.WriteLine(stepCount);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      Print();
      var stepCount = Walk2();
      Console.WriteLine(stepCount);
    }
    List<char> Directions = new List<char>();
    Map map = new Map();
    public class Map
    {
      public class Entry
      {
        public string Key;
        public List<string> Directions = new List<string>();
        public bool IsStart => Key[Key.Length - 1] == 'A';
        public bool IsEnd => Key[Key.Length - 1] == 'Z';
        public void Parse(string text)
        {
          var regex = new Regex(@"(\w+)\s*=\s*\((\w+),\s*(\w+)\)");
          var collection = regex.Matches(text);
          var groups = collection[0].Groups;
          Key = groups[1].Value;
          Directions.Add(groups[2].Value);
          Directions.Add(groups[3].Value);
        }
      }
      public void BuildLookup()
      {
        foreach (var entry in Entries)
          LookupMap[entry.Key] = entry;
      }
      public List<Entry> Entries = new List<Entry>();
      public Dictionary<string, Entry> LookupMap = new Dictionary<string, Entry>();
    }
    void Parse(string[] lines)
    {
      foreach (var c in lines[0])
        Directions.Add(c);
      for (var i = 2; i < lines.Count(); ++i)
      {
        var entry = new Map.Entry();
        entry.Parse(lines[i]);
        map.Entries.Add(entry);
      }
      map.BuildLookup();
    }
    void Print()
    {
      foreach (var d in Directions)
        Console.Write(d);
      Console.WriteLine();
      foreach(var entry in map.Entries)
        Console.WriteLine($"{entry.Key} = ({entry.Directions[0]}, {entry.Directions[1]})");
    }
    public uint Walk()
    {
      var start = map.LookupMap["AAA"];
      var end = map.LookupMap["ZZZ"];
      uint steps = 0;
      int index = 0;
      while(start != end)
      {
        var d = Directions[index % Directions.Count];
        ++index;
        var lrIndex = d == 'L' ? 0 : 1;
        var nextNodeName = start.Directions[lrIndex];
        Console.WriteLine($"{start.Key} {d}: {nextNodeName}");
        start = map.LookupMap[nextNodeName];
        ++steps;
      }
      return steps;
    }
    public Map.Entry GetNext(Map.Entry node, ref int index)
    {
      var d = Directions[index % Directions.Count];
      var lrIndex = d == 'L' ? 0 : 1;
      ++index;
      var nextNodeName = node.Directions[lrIndex];
      return map.LookupMap[nextNodeName];
    }
    public uint Walk2(Map.Entry node, uint timesToCount)
    {
      uint steps = 0;
      int index = 0;
      uint times = 0;
      while (true)
      {
        if (node.IsEnd)
        {
          ++times;
          Console.Write(steps + " ");
          if(times == timesToCount)
            break;
          steps = 0;
        }
        node = GetNext(node, ref index);
        ++steps;
      }
      return steps;
    }
    static UInt64 gcd(UInt64 n1, UInt64 n2)
    {
      if (n2 == 0)
      {
        return n1;
      }
      else
      {
        return gcd(n2, n1 % n2);
      }
    }

    public static UInt64 lcm(UInt64[] numbers)
    {
      return numbers.Aggregate((S, val) => S * val / gcd(S, val));
    }
    public UInt64 Walk2()
    {
      var nodes = new List<Map.Entry>();
      foreach(var entry in map.Entries)
      {
        if (entry.IsStart)
          nodes.Add(entry);
      }

      var stepsList = new List<UInt64>();
      foreach(var node in nodes)
      {
        uint s= Walk2(node, 5);
        Console.WriteLine();
        stepsList.Add(s);
      }
      return lcm(stepsList.ToArray());
    }
  }
}
