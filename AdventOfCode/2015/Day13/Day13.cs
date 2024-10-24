using AdventOfCode2021;
using System.Collections.Generic;

namespace AdventOfCode2015
{
  internal class Day13
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var graph = Parse(lines);
      int totalScore = Solve(graph);
      Console.WriteLine(totalScore);
      graph.Print();
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var graph = Parse(lines);
      var self = new Node { Name = "Josh" };
      foreach(var node in graph.Nodes.Values)
      {
        self.Edges.Add(new Edge { Name = node.Name, Value = 0 });
        node.Edges.Add(new Edge { Name = self.Name, Value = 0 });
      }
      graph.Nodes.Add(self.Name, self);
      int totalScore = Solve(graph);
      Console.WriteLine(totalScore);
      graph.Print();
    }

    class Edge
    {
      public string Name;
      public int Value;
    }
    class Node
    {
      public string Name;
      public List<Edge> Edges = new List<Edge>();
    }
    class Graph
    {
      public Dictionary<string, Node> Nodes = new Dictionary<string, Node>();

      public void Print()
      {
        foreach(var node in Nodes.Values)
        {
          foreach(var edge in node.Edges)
          {
            Console.WriteLine($"{node.Name} -> {edge.Name}: {edge.Value}");
          }
        }
      }
    }

    Graph Parse(string[] lines)
    {
      var graph = new Graph();
      foreach (var line in lines)
      {
        var split = line.Split(' ');
        var fromName = split[0];
        var gainLose = split[2];
        var valueStr = split[3];
        var toName = split[10].Trim('.');

        if (!graph.Nodes.TryGetValue(fromName, out var node))
        {
          node = new Node { Name = fromName };
          graph.Nodes.Add(node.Name, node);
        }
        var value = int.Parse(valueStr);
        if (gainLose == "lose")
          value *= -1;
        node.Edges.Add(new Edge { Name = toName, Value = value });
      }

      return graph;
    }

    class Layout
    {
      public Graph Graph;
      public List<Node> CurrentList = new List<Node>();
      public HashSet<string> AlreadySeated = new HashSet<string>();
      public int Total;
      public int Best;

      public int Score()
      {
        var count = CurrentList.Count;
        var score = 0;
        for(var i = 0; i < count; i++)
        {
          var curr = CurrentList[i];
          var prev = CurrentList[(i + count - 1) % count];
          var next = CurrentList[(i + 1) % count];
          foreach(var edge in curr.Edges)
          {
            if (edge.Name == prev.Name || edge.Name == next.Name)
              score += edge.Value;
          }
        }
        return score;
      }
    }

    int Solve(Graph graph)
    {
      Layout layout = new Layout();
      layout.Graph = graph;
      Solve(layout);
      return layout.Best;
    }

    void Solve(Layout layout)
    {
      // Base case
      if(layout.CurrentList.Count == layout.Graph.Nodes.Count)
      {
        layout.Total = layout.Score();
        if (layout.Total > layout.Best)
        {
          layout.Best = layout.Total;
        }
        return;
      }

      foreach(var node in layout.Graph.Nodes.Values)
      {
        if (layout.AlreadySeated.Contains(node.Name))
          continue;

        layout.CurrentList.Add(node);
        layout.AlreadySeated.Add(node.Name);

        Solve(layout);

        layout.CurrentList.RemoveAt(layout.CurrentList.Count - 1);
        layout.AlreadySeated.Remove(node.Name);
      }
    }
  }
}
