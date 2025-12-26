using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2025
{
  internal class Day10
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var machines = Parse(lines);
      foreach(var machine in machines)
        Console.WriteLine(machine);

      var solver = new Solver();
      var sum = 0;
      foreach (var machine in machines)
      {
        var state = solver.Solve(machine);
        var minCount = int.MaxValue;
        foreach (var solution in state.Solutions)
          minCount = Math.Min(solution.Count, minCount);

        Console.WriteLine(minCount);
        sum += minCount;
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var machines = Parse(lines);
      var solver = new JoltageSolver();
      Int64 sum = 0;
      foreach (var machine in machines)
      {
        var count = solver.Solve(machine);
        Console.WriteLine(count);
        sum += count;
      }
      Console.WriteLine(sum);
    }
    
    class Machine
    {
      public class ButtonWiringSchematic
      {
        public List<int> Buttons = new List<int>();
        public UInt32 ButtonsMask = 0;
        public override string ToString()
        {
          var builder = new StringBuilder();
          builder.Append("(");
          foreach (var button in Buttons)
            builder.Append($"{button}, ");
          builder.Append(")");
          return builder.ToString();
        }
        public void Initialize()
        {
          ButtonsMask = 0;
          foreach(var  button in Buttons)
          {
            if (button > 31)
              throw new Exception();
            ButtonsMask |= 1u << button;
          }
        }
      }
      public List<char> LightDiagram = new List<char>();
      public List<ButtonWiringSchematic> ButtonWiringSchematics = new List<ButtonWiringSchematic>();
      public List<int> JoltageRequirements = new List<int>();

      public UInt32 LightDiagramBits = 0;
      public override string ToString()
      {
        var builder = new StringBuilder();

        builder.Append("[");
        foreach (var c in LightDiagram)
          builder.Append(c);
        builder.Append("]");

        foreach (var schematic in ButtonWiringSchematics)
          builder.Append(schematic);

        builder.Append("{");
        foreach (var joltage in JoltageRequirements)
          builder.Append($"{joltage}, ");
        builder.Append("}");
        builder.AppendLine();

        return builder.ToString();
      }
      public void Initialize()
      {
        foreach(var schematic in ButtonWiringSchematics)
          schematic.Initialize();

        if (LightDiagram.Count > 32)
          throw new Exception();

        LightDiagramBits = 0;
        for(var i = 0; i < LightDiagram.Count; i++)
        {
          var c = LightDiagram[i];
          if(c == '#')
            LightDiagramBits |= 1u << i;
        }
      }
    }
    class Solver
    {
      Machine Machine;
      public class State
      {
        public uint TargetState = 0;
        public uint CurrentState = 0;
        public List<Machine.ButtonWiringSchematic> SchematicStack = new List<Machine.ButtonWiringSchematic>();
        public List<List<Machine.ButtonWiringSchematic>> Solutions = new List<List<Machine.ButtonWiringSchematic>>();
        public void BuildState(List<char> lightDiagrams)
        {
          TargetState = 0;
          for(var i = 0; i < lightDiagrams.Count; ++i)
          {
            if (lightDiagrams[i] == '#')
              TargetState |= 1u << i;
          }
        }
        public void Push(Machine.ButtonWiringSchematic schematic)
        {
          CurrentState ^= schematic.ButtonsMask;
          SchematicStack.Add(schematic);
        }
        public void Pop()
        {
          var last = SchematicStack[SchematicStack.Count - 1];
          CurrentState ^= last.ButtonsMask;
          SchematicStack.RemoveAt(SchematicStack.Count - 1);
        }
        public void CopySolution()
        {
          var solution = new List<Machine.ButtonWiringSchematic>();
          foreach (var scehmatic in SchematicStack)
            solution.Add(scehmatic);
          Solutions.Add(solution);
        }
      }
      

      public State Solve(Machine machine)
      {
        return Solve(machine, Machine.LightDiagramBits);
      }

      public State Solve(Machine machine, uint target)
      {
        Machine = machine;
        var schematics = Machine.ButtonWiringSchematics;
        int Count = 1 << schematics.Count;

        //var TargetState = Machine.LightDiagramBits;
        var result = new State();

        for (var schematicsMask = 0; schematicsMask < Count; ++schematicsMask)
        {
          var state = 0u;
          for (var j = 0; j < schematics.Count; ++j)
          {
            if ((schematicsMask & (1 << j)) != 0)
            {
              state ^= schematics[j].ButtonsMask;
            }
          }

          if (state == target)
          {
            var solution = new List<Machine.ButtonWiringSchematic>();
            for (var j = 0; j < schematics.Count; ++j)
            {
              if ((schematicsMask & (1 << j)) != 0)
              {
                solution.Add(schematics[j]);
              }
            }
            result.Solutions.Add(solution);
          }
        }
        return result;
      }
    }
    class JoltageSolver
    {
      Machine Machine;
      Dictionary<uint, Solver.State> StateCache = new Dictionary<uint, Solver.State>();
      Solver Solver = new Solver();
      class Node
      {
        public string Name;
        public Vector<int> TargetJoltage;
        public Vector<int> TargetPattern;
        public Int64 ButtonCount = 0;
        public List<Edge> Edges = new List<Edge>();
        public void Dump()
        {
          Console.WriteLine($"{Name} [label=\"{{{TargetJoltage} | {TargetPattern} | {ButtonCount}}}\"]");
          foreach(var edge in Edges)
          {
            Console.WriteLine($"{Name} -> {edge.To.Name} [label=\"{edge.ButtonPresses}\"]");
          }
          //JoltageSolver.ToNodeName(TargetJoltage);
        }
      }
      class Edge
      {
        public Vector<int> ButtonPresses;
        public Vector<int> ResultJoltage;
        public Node To;
      }
      
      static string ToNodeName(Vector<int> vector)
      {
        var builder = new StringBuilder();
        builder.Append("\"");
        for (var i = 0; i < vector.Count; ++i)
          builder.Append($"{vector[i]}_");
        builder.Append("\"");
        return builder.ToString();
      }
      List<Node> Nodes = new List<Node>();

      //Dictionary<>
      public Int64 Solve(Machine machine)
      {
        Machine = machine;
        // Note: Could be optimized...
        Nodes.Clear();
        StateCache.Clear();

        uint target = 0;
        var joltages = new Vector<int>(Machine.JoltageRequirements.Count);
        for (var i = 0; i < Machine.JoltageRequirements.Count; ++i)
        {
          if (Machine.JoltageRequirements[i] % 2 == 1)
            target |= 1u << i;
          joltages[i] = Machine.JoltageRequirements[i];
        }

        

        var result = Solve(joltages);

        //Console.WriteLine("digraph {");
        //Console.WriteLine("node[shape = record]");
        //foreach (var node in Nodes)
        //  node.Dump();
        //Console.WriteLine("}");

        return result.ButtonCount;
      }
      Node Solve(Vector<int> joltages)
      {
        var node = new Node();
        node.Name = Nodes.Count.ToString();
        node.TargetJoltage = joltages;
        node.ButtonCount = 0;
        Nodes.Add(node);

        bool baseCase = true;
        for(var i = 0; i < joltages.Count; ++i)
        {
          if (joltages[i] != 0)
            baseCase = false;
        }

        Vector<int> subJoltages = new Vector<int>(joltages.Count);
        uint target = 0;
        for (var i = 0; i < joltages.Count; ++i)
        {
          if (joltages[i] < 0)
          {
            node.ButtonCount = Int64.MaxValue;
            return node;
          }
          if (joltages[i] % 2 == 1)
          {
            target |= 1u << i;
            subJoltages[i] = 1;
          }
        }

        node.TargetPattern = subJoltages;

        if (baseCase == true)
          return node;

        if (!StateCache.TryGetValue(target, out var state))
        {
          state = Solver.Solve(Machine, target);
          StateCache.Add(target, state);
        }

        foreach(var solution in state.Solutions)
        {
          var builder = new StringBuilder();
          foreach (var schematic in solution)
            builder.Append(schematic.ToString());
        }

        Int64 min = Int64.MaxValue;
        foreach (var solution in state.Solutions)
        {
          var builder = new StringBuilder();
          foreach (var schematic in solution)
            builder.Append(schematic.ToString());

          var consumedJoltages = BuildVector(solution);
          var newJoltages = (joltages - consumedJoltages) / 2;


          var edge = new Edge();
          edge.ResultJoltage = newJoltages;
          edge.ButtonPresses = BuildButtonVector(solution);
          
          node.Edges.Add(edge);

          var subNode = Solve(newJoltages);
          edge.To = subNode;

          if (subNode.ButtonCount == Int64.MaxValue)
          {
            continue;
          }
          var count = subNode.ButtonCount * 2 + solution.Count;
          min = Math.Min(count, min);
        }
        node.ButtonCount = min;
        return node;
      }
      Vector<int> BuildVector(List<Machine.ButtonWiringSchematic> schematics)
      {
        var result = new Vector<int>(Machine.JoltageRequirements.Count);
        foreach(var schematic in schematics)
        {
          foreach (var i in schematic.Buttons)
            ++result[i];
        }
        return result;
      }
      Vector<int> BuildButtonVector(List<Machine.ButtonWiringSchematic> schematics)
      {
        var result = new Vector<int>(Machine.ButtonWiringSchematics.Count);
        for(var i = 0; i < Machine.ButtonWiringSchematics.Count; ++i)
        {
          var schematic = Machine.ButtonWiringSchematics[i];
          if (schematics.IndexOf(schematic) != -1)
            result[i] = 1;
        }
        return result;
      }
    }
    List<Machine> Parse(string[] lines)
    {
      var result = new List<Machine>();
      foreach(var line in lines)
      {
        var machine = new Machine();
        result.Add(machine);

        var start = 1;
        var end = line.IndexOf(']');
        for(var i = start; i < end; ++i)
          machine.LightDiagram.Add(line[i]);

        start = end + 1;
        while(true)
        {
          start = line.IndexOf('(', end + 1);
          if (start == -1)
            break;

          ++start;
          end = line.IndexOf(')', start);
          if (end == -1)
            throw new Exception();

          var subStr = line.Substring(start, end - start);
          var split = subStr.Split(',');

          var schematic = new Machine.ButtonWiringSchematic();
          machine.ButtonWiringSchematics.Add(schematic);
          foreach (var s in split)
            schematic.Buttons.Add(int.Parse(s));
        }
        {
          start = line.IndexOf('{', end + 1);
          ++start;
          end = line.IndexOf('}', start);
          var subStr = line.Substring(start, end - start);
          var split = subStr.Split(',');
          foreach (var s in split)
            machine.JoltageRequirements.Add(int.Parse(s));
        }
        machine.Initialize();
      }
      return result;
    }
  }
}
