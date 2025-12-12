using System.Diagnostics;
using System.Text;

namespace AdventOfCode2025
{
  internal class Day11
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var devices = Parse(lines);
      foreach(var device in devices)
        Console.WriteLine(device);

      var solver = new Solver();
      var count = solver.Run(devices);
      Console.WriteLine(count);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var devices = Parse(lines);

      var solver = new Solver();
      solver.Initialize(devices);
      var count = solver.SolveCounts("svr", "out");
      Console.WriteLine(count);
    }
    void DumpGraphviz(List<Device> devices)
    {
      Console.WriteLine("digraph G {");
      foreach (var device in devices)
      {
        foreach(var output in device.Outputs)
          Console.WriteLine($"  {device.Name} -> {output}");
      }
      Console.WriteLine("}");
    }
    class Device
    {
      public string Name;
      public List<string> Outputs = new List<string>();
      public override string ToString()
      {
        var builder = new StringBuilder();
        builder.Append($"{Name}:");
        foreach (var output in Outputs)
          builder.Append($" {output}");
        return builder.ToString();
      }
    }
    class Solver
    {
      Device You;
      List<Device> Outputs;
      List<Device> DeviceList;
      Dictionary<string, Device> DeviceMap = new Dictionary<string, Device>();
      Dictionary<string, Node> NodeMap = new Dictionary<string, Node>();
      class Edge
      {
        public Node From = null;
        public Node To = null;
      }
      class Node
      {
        public List<Edge> Edges = new List<Edge>();
        public List<Edge> BackEdges = new List<Edge>();
        public Device Device;
        public string DeviceName;
      }
      public void Initialize(List<Device> devices)
      {
        DeviceList = devices;
        Initialize();
        BuildGraph();
        SimplifyGraph();
      }
      public int Run(List<Device> devices)
      {
        Initialize();
        SolveRecursiveForward(NodeMap["you"]);
        return Count;
      }
      void Initialize()
      {
        You = null;
        Outputs = new List<Device>();
        foreach (var device in DeviceList)
        {
          DeviceMap.Add(device.Name, device);
          if (device.Name == "you")
            You = device;
          foreach (var output in device.Outputs)
          {
            if (output == "out")
              Outputs.Add(device);
          }
        }
      }
      void BuildGraph()
      {
        foreach (var device in DeviceList)
          NodeMap.Add(device.Name, new Node { Device = device, DeviceName = device.Name });
        NodeMap.Add("out", new Node { Device = null, DeviceName = "out"});

        foreach (var device in DeviceList)
        {
          var node = NodeMap[device.Name];
          foreach (var output in device.Outputs)
          {
            var outputNode = NodeMap[output];
            var edge = new Edge { From = node, To = outputNode };
            node.Edges.Add(edge);
            outputNode.BackEdges.Add(edge);
          }
        }
      }
      void SimplifyGraph()
      {
        // Recursively remove all nodes with no back or forward edges...
        foreach(var node in NodeMap.Values)
          Simplify(node);
      }
      void Simplify(Node node)
      {
        if (node.DeviceName == "you" || node.DeviceName == "out")
          return;

        if (node.BackEdges.Count == 0)
        {
          var edges = node.BackEdges;
          node.BackEdges = new List<Edge>();
          foreach(var edge in edges)
          {
            var to = edge.To;
            to.Edges.Remove(edge);
            Simplify(to);
          }
        }
        if (node.Edges.Count == 0)
        {
          var edges = node.Edges;
          node.Edges = new List<Edge>();
          foreach (var edge in edges)
          {
            var from = edge.From;
            from.Edges.Remove(edge);
            Simplify(from);
          }
        }
      }

      int Count = 0;
      List<Node> Stack = new List<Node>();
      void SolveRecursiveForward(Node node)
      {
        // Cycle
        if (Stack.Contains(node))
          return;

        if (node.DeviceName == "out")
        {
          ++Count;
          return;
        }
        Stack.Add(node);

        foreach (var edge in node.Edges)
        {
          SolveRecursiveForward(edge.To);
        }
        Stack.RemoveAt(Stack.Count - 1);
      }
      [DebuggerDisplay("({Node.DeviceName}: Depth({Depth}), Count({Count}), FFT({FFTCount}), DAC({DACCount})")]
      class SearchNode
      {
        public Int64 Depth = 0;
        public Int64 Count = 0;
        public Int64 FFTCount = 0;
        public Int64 DACCount = 0;
        public Node Node;

        public void Accumulate(SearchNode from)
        {
          Count += from.Count;
          FFTCount += from.FFTCount;
          DACCount += from.DACCount;
          if(Node.DeviceName == "fft")
          {
            FFTCount += from.Count;
          }
          if(Node.DeviceName == "dac")
          {
            DACCount += from.FFTCount;
          }
        }
        public static int Compare(SearchNode A, SearchNode B)
        {
          return -A.Depth.CompareTo(B.Depth);
        }
      }
      Dictionary<string, SearchNode> SearchNodeMap = new Dictionary<string, SearchNode>();

      List<SearchNode> SearchNodeQueue = new List<SearchNode>();
      HashSet<SearchNode> SearchNodeQueueSet = new HashSet<SearchNode>();
      public Int64 SolveCounts(string startNode, string endNode)
      {
        foreach (var node in NodeMap.Values)
          SearchNodeMap.Add(node.DeviceName, new SearchNode { Node = node });
        SearchNodeMap[startNode].Count = 1;

        ComputeDepths(SearchNodeMap[startNode]);
        SolveCounts(SearchNodeMap[startNode], SearchNodeMap[endNode]);

        return SearchNodeMap[endNode].DACCount;
      }
      void ComputeDepths(SearchNode node)
      {
        foreach(var edge in node.Node.Edges)
        {
          var toSearchNode = SearchNodeMap[edge.To.DeviceName];
          if (toSearchNode.Depth < node.Depth + 1)
          {
            toSearchNode.Depth = node.Depth + 1;
            ComputeDepths(toSearchNode);
          }
        }
      }
      void SolveCounts(SearchNode startNode, SearchNode endNode)
      {
        SearchNodeQueue.Add(startNode);

        while(SearchNodeQueue.Count != 0)
        {
          SearchNodeQueue.Sort(SearchNode.Compare);

          var node = SearchNodeQueue[SearchNodeQueue.Count - 1];
          SearchNodeQueue.RemoveAt(SearchNodeQueue.Count - 1);

          if (node == endNode)
            break;

          foreach (var edge in node.Node.Edges)
          {
            var toSearchNode = SearchNodeMap[edge.To.DeviceName];
            toSearchNode.Accumulate(node);
            
            if(!SearchNodeQueueSet.Contains(toSearchNode))
            {
              SearchNodeQueue.Add(toSearchNode);
              SearchNodeQueueSet.Add(toSearchNode);
            }
          }
        }
      }
    }

    List<Device> Parse(string[] lines)
    {
      var result = new List<Device>();
      foreach(var line in lines)
      {
        var split0 = line.Split(":");
        var split1 = split0[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var device = new Device();
        device.Name = split0[0];
        foreach (var output in split1)
          device.Outputs.Add(output);
        result.Add(device);
      }
      return result;
    }
  }
}
