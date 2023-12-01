using System.Diagnostics;

namespace AdventOfCode2015
{
  internal class Day07
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var wires = Build(lines);
      foreach (var wire in wires)
        ComputeValue(wire);
      foreach (var wire in wires)
      {
        if(wire.Name == "a")
          Console.WriteLine($"{wire.Name}: {wire.Value}");
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var wires = Build(lines);
      foreach (var wire in wires)
        ComputeValue(wire);
      foreach (var wire in wires)
      {
        if (wire.Name == "a")
          Console.WriteLine($"{wire.Name}: {wire.Value}");
      }
    }

    void PrintWires(List<Wire> wires)
    {
      foreach (var wire in wires)
        Console.WriteLine($"{wire.Name}: {wire.Value}");
    }

    List<Wire> Build(string[] lines)
    {
      var result = new List<Wire>();
      var map = new Dictionary<string, Wire>();
      var tryBuildConstant = (string valueStr) =>
      {
        if (map.TryGetValue(valueStr, out var wire))
          return;

        if (!ushort.TryParse(valueStr, out var value))
          return;

        wire = new Wire();
        wire.Name = valueStr;
        wire.Value = value;
        //result.Add(wire);
        map.Add(wire.Name, wire);
      };

      foreach(var line in lines)
      {
        var split0 = line.Split("->", StringSplitOptions.TrimEntries);
        var split1 = split0[0].Split(' ');
        var wire = new Wire();
        wire.Name = split0[1];
        if(split1.Length == 1)
        {
          wire.kind = Wire.Kind.Set;
          wire.InputEdgeNames.Add(split1[0]);
        }
        else if(split1.Length == 2) 
        { 
          wire.kind = Wire.Kind.Not;
          wire.InputEdgeNames.Add(split1[1]);
        }
        else
        {
          var circuitKind = split1[1];
          if (circuitKind == "AND")
            wire.kind = Wire.Kind.And;
          else if(circuitKind == "OR")
            wire.kind = Wire.Kind.Or;
          else if (circuitKind == "LSHIFT")
            wire.kind = Wire.Kind.LShift;
          else if (circuitKind == "RSHIFT")
            wire.kind = Wire.Kind.RShift;
          wire.InputEdgeNames.Add(split1[0]);
          wire.InputEdgeNames.Add(split1[2]);
        }
        result.Add(wire);
        map.Add(wire.Name, wire);
      }
      foreach(var wire in result)
      {
        foreach(var name in wire.InputEdgeNames)
        {
          tryBuildConstant(name);
          map.TryGetValue(name, out var source);
          if (source == null)
            throw new Exception();
          wire.InputEdges.Add(source);
        }
      }
      return result;
    }
    void ComputeValue(Wire wire)
    {
      if (wire.Value.HasValue)
        return;

      foreach (var edge in wire.InputEdges)
        ComputeValue(edge);

      switch(wire.kind)
      {
        case Wire.Kind.Set:
          wire.Value = wire.InputEdges[0].Value;
          break;
        case Wire.Kind.And:
          wire.Value = (ushort)(wire.InputEdges[0]!.Value & wire.InputEdges[1]!.Value);
          break;
        case Wire.Kind.Or:
          wire.Value = (ushort)(wire.InputEdges[0].Value | (int)wire.InputEdges[1].Value);
          break;
        case Wire.Kind.LShift:
          wire.Value = (ushort)(wire.InputEdges[0].Value << (int)wire.InputEdges[1]!.Value);
          break;
        case Wire.Kind.RShift:
          wire.Value = (ushort)(wire.InputEdges[0]!.Value >> (int)wire.InputEdges[1]!.Value);
          break;
        case Wire.Kind.Not:
          wire.Value = (ushort)~wire.InputEdges[0]!.Value;
          break;
      }
    }

    class Wire
    {
      public enum Kind { None, Set, And, Or, Xor, LShift, RShift, Not }
      public Kind kind;
      public string Name;
      public List<string> InputEdgeNames = new List<string>();
      public List<Wire> InputEdges = new List<Wire>();
      public List<Wire> OutputEdges = new List<Wire>();
      public ushort? Value = null;
    }
  }
}
