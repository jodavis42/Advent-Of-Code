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

      var graph = new SearchGraph();
      var sum = 0;
      foreach (var machine in machines)
      {
        var node = graph.Run(machine);
        Console.WriteLine("-----");
        Console.WriteLine(node.Depth);
        Console.WriteLine(graph.LogPath(node));
        sum += node.Depth;
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var machines = Parse(lines);
      var graph = new SearchGraph();
      var sum = 0;
      foreach (var machine in machines)
      {
        var count = graph.RunJoltage(machine);
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
    class SearchGraph
    {
      public class Node : IComparable<Node>
      {
        public int Depth = 0;
        public UInt32 State = 0;
        public Node? PreviousNode = null;
        public Machine.ButtonWiringSchematic? Path = null;

        int IComparable<Node>.CompareTo(Node? other)
        {
          return -Depth.CompareTo(other!.Depth);
        }
      }
      public class JoltageNode : IComparable<JoltageNode>, IEquatable<JoltageNode>
      {
        public int Depth = 0;
        public List<int> Joltages = new List<int>();
        public JoltageNode? PreviousNode = null;
        public Machine.ButtonWiringSchematic? Path = null;
        Machine Machine;
        public JoltageNode(Machine machine, int depth)
        {
          Initialize(machine);
          Depth = depth;
          Machine = machine;
        }
        public JoltageNode(Machine machine, JoltageNode previousNode, Machine.ButtonWiringSchematic path)
        {
          Machine = machine;
          PreviousNode = previousNode;
          Path = path;
          Depth = previousNode.Depth + 1;

          foreach (var joltage in previousNode.Joltages)
            Joltages.Add(joltage);
          foreach(var button in path.Buttons)
          {
            ++Joltages[button];
          }
        }
        public void Initialize(Machine machine)
        {
          foreach (var l in machine.LightDiagram)
            Joltages.Add(0);
        }
        public bool IsComplete()
        {
          for(var i = 0; i < Joltages.Count; ++i)
          {
            if (Joltages[i] != Machine.JoltageRequirements[i])
              return false;
          }
          return true;
        }

        int IComparable<JoltageNode>.CompareTo(JoltageNode? other)
        {
          return -Depth.CompareTo(other!.Depth);
        }

        bool IEquatable<JoltageNode>.Equals(JoltageNode? other)
        {
          return other != null ? Equals(other) : false;
        }
        public override bool Equals(object? obj)
        {
          return obj != null && obj is JoltageNode p && Equals(p);
        }

        public bool Equals(JoltageNode other)
        {
          if (other == null)
            return false;
          if (other.Joltages.Count != Joltages.Count)
            return false;
          for (var i = 0; i < Joltages.Count; ++i)
          {
            if (Joltages[i] != other.Joltages[i])
              return false;
          }
          return true;
        }
        public override int GetHashCode()
        {
          var code = 0;
          foreach (var joltage in Joltages)
            code = HashCode.Combine(code, joltage);
          return code;
        }
      }

      public Node? Run(Machine machine)
      {
        var nodeMap = new Dictionary<UInt32, Node>();
        var nodeList = new List<Node>();

        var root = new Node { Depth = 0, State = 0 };
        nodeMap.Add(root.State, root);
        nodeList.Add(root);

        while (nodeList.Count > 0)
        {
          var node = nodeList[nodeList.Count - 1];
          nodeList.RemoveAt(nodeList.Count - 1);

          if (node.State == machine.LightDiagramBits)
            return node;

          var newNodes = new List<Node>();
          foreach (var schematic in machine.ButtonWiringSchematics)
          {
            var newState = node.State ^ schematic.ButtonsMask;
            if (nodeMap.ContainsKey(newState))
              continue;

            var newNode = new Node { Depth = node.Depth + 1, State = newState, PreviousNode = node, Path = schematic};
            if (newNode.State == machine.LightDiagramBits)
              return newNode;

            nodeMap.Add(newNode.State, newNode);
            newNodes.Add(newNode);
          }
          nodeList.AddRange(newNodes);
          nodeList.Sort();
        }
        return null;
      }
      class JoltageSolver
      {
        List<int> CurrentJoltages = new List<int>();
        List<int> ButtonPresses = new List<int>();
        List<Machine.ButtonWiringSchematic> ButtonWiringSchematics;
        Machine Machine;
        public int Run(Machine machine)
        {
          Machine = machine;
          foreach (var joltage in machine.JoltageRequirements)
            CurrentJoltages.Add(0);
          foreach (var schematic in machine.ButtonWiringSchematics)
            ButtonPresses.Add(0);
          ButtonWiringSchematics = machine.ButtonWiringSchematics;
          ButtonWiringSchematics.Sort(SortSchematics);
          Run(0);

          var count = 0;
          for(var i = 0; i < ButtonPresses.Count; ++i)
          {
            count += ButtonPresses[i];
            if(ButtonPresses[i] != 0)
            {
              //Console.WriteLine($"{ButtonWiringSchematics[i]}: {ButtonPresses[i]}");
            }
          }
          return count;
        }
        bool Run(int schematicIndex)
        {
          if (schematicIndex >= ButtonPresses.Count)
            return false;

          var schematic = ButtonWiringSchematics[schematicIndex];


          var count = ComputeMaxButtonPresses(schematic);
          ApplySchematic(schematicIndex, schematic, count);

          if (IsSolved())
            return true;

          while (ButtonPresses[schematicIndex] >= 0)
          {
            if (Run(schematicIndex + 1))
              return true;
            if (ButtonPresses[schematicIndex] == 0)
              break;
            ApplySchematic(schematicIndex, schematic, -1);
          }
          ButtonPresses[schematicIndex] = 0;

          return false;
        }
        int ComputeMaxButtonPresses(Machine.ButtonWiringSchematic schematic)
        {
          var maxValue = int.MaxValue;
          foreach (var buttonIndex in schematic.Buttons)
          {
            var delta = Machine.JoltageRequirements[buttonIndex] - CurrentJoltages[buttonIndex];
            maxValue = Math.Min(maxValue, delta);
          }
          return maxValue;
        }
        void ApplySchematic(int schematicIndex, Machine.ButtonWiringSchematic schematic, int count)
        {
          foreach (var buttonIndex in schematic.Buttons)
          {
            CurrentJoltages[buttonIndex] += count;
          }

          ButtonPresses[schematicIndex] += count;
        }
        bool IsSolved()
        {
          for(var i = 0; i < CurrentJoltages.Count; ++i)
          {
            if (Machine.JoltageRequirements[i] != CurrentJoltages[i])
              return false;
          }
          return true;
        }

        static int SortSchematics(Machine.ButtonWiringSchematic a, Machine.ButtonWiringSchematic b)
        {
          return a.Buttons.Count.CompareTo(b.Buttons.Count);
        }
      }
      public int RunJoltage(Machine machine)
      {
        var solver = new JoltageSolver();
        return solver.Run(machine);
      }

      public JoltageNode? RunJoltageBruteForce(Machine machine)
      {
        var nodeList = new List<JoltageNode>();
        var nodeMap = new HashSet<JoltageNode>();

        var root = new JoltageNode(machine, 0);
        nodeList.Add(root);

        while (nodeList.Count > 0)
        {
          var node = nodeList[nodeList.Count - 1];
          nodeList.RemoveAt(nodeList.Count - 1);

          if (node.IsComplete())
            return node;

          foreach (var schematic in machine.ButtonWiringSchematics)
          {
            var newNode = new JoltageNode(machine, node, schematic);
            if (nodeMap.Contains(newNode))
              continue;

            if (newNode.IsComplete())
              return newNode;

            nodeMap.Add(newNode);
            nodeList.Add(newNode);
          }
          nodeList.Sort();
        }
        return null;
      }
      public string ToBinary(UInt32 data, int count = 32)
      {
        var builder = new StringBuilder();
        builder.Append("0b");
        for(var i = count - 1; i >= 0; --i)
        {
          UInt32 state = data & (1u << i);
          if (state != 0)
            builder.Append("1");
          else
            builder.Append("0");
        }
        return builder.ToString();
      }
      public string LogPath(Node? node)
      {
        List<Node> list = new List<Node>();
        while(node != null)
        {
          list.Add(node);
          node = node.PreviousNode;
        }

        var builder = new StringBuilder();
        UInt32 prevoiusState = 0;
        for(var i = list.Count - 1; i >= 0; --i)
        {
          var curNode = list[i];
          if (curNode.Path == null)
            builder.AppendLine($"{ToBinary(curNode.State)}");
          else
          {
            builder.AppendLine($"{ToBinary(prevoiusState)} ^ {ToBinary(curNode.Path.ButtonsMask)} -> {ToBinary(curNode.State)}");
          }
          prevoiusState = curNode.State;
        }
        return builder.ToString();
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
