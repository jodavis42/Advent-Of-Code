using System.Diagnostics;

namespace AdventOfCode2025
{
  internal class Day08
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var points = Parse(lines);
      foreach (var point in points)
      {
        Console.WriteLine($"{point.X}, {point.Y}, {point.Z}");
      }
      var graph = Initialize(points);
      for(var i = 0; i < 1000; ++i)
        Step(graph);
      var circuitCounts = GetCircuitCounts(graph);
      foreach (var circuit in circuitCounts)
        Console.Write($"{circuit}, ");

      Console.WriteLine();
      var product = 1;
      for (var i = 0; i < 3; ++i)
      {
        product *= circuitCounts[circuitCounts.Count - 1 - i];
      }
      Console.WriteLine(product);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var points = Parse(lines);
      foreach (var point in points)
      {
        Console.WriteLine($"{point.X}, {point.Y}, {point.Z}");
      }
      var graph = Initialize(points);
      int Node0Index = 0, Node1Index = 0;
      while(Step2(graph, out Node0Index, out Node1Index))
      {

      }
      var pos0 = graph.Nodes[Node0Index].Position;
      var pos1 = graph.Nodes[Node1Index].Position;
      Console.WriteLine($"{pos0} {pos1}");
      Int64 X0 = pos0.X;
      Int64 X1 = pos1.X;
      Console.WriteLine(X0 * X1);
    }
    List<Int3> Parse(string[] lines)
    {
      var result = new List<Int3>();
      foreach (var line in lines)
      {
        var split = line.Split(',');
        var int3 = new Int3();
        int3.X = int.Parse(split[0]);
        int3.Y = int.Parse(split[1]);
        int3.Z = int.Parse(split[2]);
        result.Add(int3);
      }
      return result;
    }
    public class Graph
    {
      [DebuggerDisplay("({Node0}, {Node1}): {Distance}")]
      public class Edge : IComparable<Edge>
      {
        public int Node0 = -1;
        public int Node1 = -1;
        public Int64 Distance = 0;

        int IComparable<Edge>.CompareTo(Edge? other)
        {
          return -Distance.CompareTo(other!.Distance);
        }
      }
      public class Node
      {
        public Int3 Position;
        public int Circuit = -1;
      }
      public class Circuit
      {
        public List<int> Nodes = new List<int>();
      }

      public List<Node> Nodes = new List<Node>();
      public List<Circuit> Circuits = new List<Circuit>();
      public Grid2d<Edge> PossibleEdges = new Grid2d<Edge>();
      public List<Edge> SortedEdges = new List<Edge>();
      public Grid2d<Edge> ActualEdges = new Grid2d<Edge>();
      public List<Edge> ActualEdgesList = new List<Edge>();
    }
    Graph Initialize(List<Int3> positions)
    {
      var nodeCount = positions.Count;
      var graph = new Graph();
      graph.PossibleEdges.Initialize(nodeCount, nodeCount, null);
      graph.ActualEdges.Initialize(nodeCount, nodeCount, null);

      foreach (var position in positions)
      {
        var node = new Graph.Node
        {
          Position = position
        };
        graph.Nodes.Add(node);
      }
      for (var i = 0; i < graph.Nodes.Count; ++i)
      {
        for (var j = i + 1; j < graph.Nodes.Count; ++j)
        {
          var pos0 = graph.Nodes[i].Position;
          var pos1 = graph.Nodes[j].Position;
          var edge = new Graph.Edge { Node0 = i, Node1 = j };
          edge.Distance = (pos0 - pos1).LengthSq();

          Int64 x = pos0.X - pos1.X;
          Int64 y = pos0.Y - pos1.Y;
          Int64 z = pos0.Z - pos1.Z;
          edge.Distance = x * x + y * y + z * z;

          graph.PossibleEdges[i, j] = graph.PossibleEdges[j, i] = edge;
          graph.SortedEdges.Add(edge);
        }
      }
      graph.SortedEdges.Sort();
      return graph;
    }
    Graph.Edge GetNextEdge(Graph graph)
    {
      var bestIndex = graph.SortedEdges.Count - 1;
      Graph.Edge bestEdge = graph.SortedEdges[bestIndex];
      graph.SortedEdges.RemoveAt(bestIndex);
      return bestEdge;
    }
    void Step(Graph graph)
    {
      var edge = GetNextEdge(graph);
      graph.PossibleEdges[edge.Node0, edge.Node1] = null;
      graph.PossibleEdges[edge.Node1, edge.Node0] = null;
      graph.ActualEdges[edge.Node0, edge.Node1] = edge;
      graph.ActualEdges[edge.Node1, edge.Node0] = edge;
      graph.ActualEdgesList.Add(edge);

      MergeCircuits(graph, edge.Node0, edge.Node1);
    }
    bool Step2(Graph graph, out int Node0Index, out int Node1Index)
    {
      var edge = GetNextEdge(graph);
      Node0Index = edge.Node0;
      Node1Index = edge.Node1;
      graph.PossibleEdges[edge.Node0, edge.Node1] = null;
      graph.PossibleEdges[edge.Node1, edge.Node0] = null;
      graph.ActualEdges[edge.Node0, edge.Node1] = edge;
      graph.ActualEdges[edge.Node1, edge.Node0] = edge;
      graph.ActualEdgesList.Add(edge);

      int count = MergeCircuits(graph, edge.Node0, edge.Node1);
      bool finished = count == graph.Nodes.Count;
      return !finished;
    }
    int MergeCircuits(Graph graph, int node0Index, int node1Index)
    {
      var node0 = graph.Nodes[node0Index];
      var node1 = graph.Nodes[node1Index];
     
      if (node0.Circuit == -1 && node1.Circuit == -1)
      {
        var circuit = new Graph.Circuit();
        var circuitIndex = graph.Circuits.Count;

        circuit.Nodes.Add(node0Index);
        circuit.Nodes.Add(node1Index);
        graph.Circuits.Add(circuit);
        node0.Circuit = circuitIndex;
        node1.Circuit = circuitIndex;
        return circuit.Nodes.Count;
      }
      else if (node0.Circuit == -1)
      {
        node0.Circuit = node1.Circuit;
        var circuit = graph.Circuits[node1.Circuit];
        circuit.Nodes.Add(node0Index);
        return circuit.Nodes.Count;
      }
      else if(node1.Circuit == -1)
      {
        node1.Circuit = node0.Circuit;
        var circuit = graph.Circuits[node0.Circuit];
        circuit.Nodes.Add(node1Index);
        return circuit.Nodes.Count;
      }
      else
      {
        if (node0.Circuit == node1.Circuit)
          return 0;

        var circuit0Index = node0.Circuit;
        var circuit1Index = node1.Circuit;
        var circuit0 = graph.Circuits[circuit0Index];
        var circuit1 = graph.Circuits[circuit1Index];
        circuit0.Nodes.AddRange(circuit1.Nodes);
        circuit1.Nodes.Clear();
        foreach (var nodeIndex in circuit0.Nodes)
          graph.Nodes[nodeIndex].Circuit = circuit0Index;
        return circuit0.Nodes.Count;
      }
    }
    List<int> GetCircuitCounts(Graph graph)
    {
      var result = new List<int>();
      foreach (var circuit in graph.Circuits)
        result.Add(circuit.Nodes.Count);
      result.Sort();
      return result;
    }
  }
}
