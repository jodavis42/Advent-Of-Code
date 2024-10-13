namespace AdventOfCode2015
{
  internal class Day09
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var graph = new Graph();
      graph.Parse(lines);
      //graph.Print();
      Search(graph, SearchData.SearchMode.Shortest);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var graph = new Graph();
      graph.Parse(lines);
      //graph.Print();
      Search(graph, SearchData.SearchMode.Longest);
    }

    class Graph
    {
      public class Edge
      {
        public string To;
        public Int64 Cost;
      }
      public class Node
      {
        public string Name;
        public List<Edge> Edges = new List<Edge>();
      }
      public Dictionary<String, Node> Nodes =  new Dictionary<String, Node>();

      public void Parse(string[] lines)
      {
        foreach (var line in lines)
          Parse(line);
      }
      public void Parse(string line)
      {
        var split = line.Split(' ');
        var from = split[0];
        var to = split[2];
        var cost = split[4];
        AddEdge(from, to, cost);
        AddEdge(to, from, cost);
      }
      public void AddEdge(string from, string to, string cost)
      {
        if (!Nodes.TryGetValue(from, out var node))
        {
          node = new Node { Name = from };
          Nodes.Add(from, node);
        }
        var edge = new Edge { To = to, Cost = int.Parse(cost) };
        node.Edges.Add(edge);
      }

      public void Print()
      {
        foreach(var node in Nodes)
        {
          Console.WriteLine(node.Key);
          foreach(var edge in node.Value.Edges)
            Console.WriteLine($"  {edge.To}: {edge.Cost}");
        }
      }
    }
    class SearchData
    {
      public HashSet<string> visitedSet = new HashSet<string>();
      public List<string> visitedList = new List<string>();
      public Int64 totalCost = 0;
      public Int64 totalCount = 0;
      public Int64 bestCost = Int64.MaxValue;
      public enum SearchMode { Shortest, Longest}
      public SearchMode Mode = SearchMode.Shortest;

      public SearchData(SearchMode mode)
      {
        Mode = mode;
        if (mode == SearchMode.Shortest)
          bestCost = Int64.MaxValue;
        else if (mode == SearchMode.Longest)
          bestCost = 0;
      }

      public void Add(string name, Int64 cost)
      {
        visitedSet.Add(name);
        visitedList.Add(name);
        totalCost += cost;

        if(visitedList.Count == totalCount)
        {
          if(Mode == SearchMode.Shortest && totalCost < bestCost)
            bestCost = totalCost;
          else if (Mode == SearchMode.Longest && totalCost > bestCost)
            bestCost = totalCost;
        }
      }
      public void Remove(string name, Int64 cost)
      {
        visitedSet.Remove(name);
        visitedList.RemoveAt(visitedList.Count - 1);
        totalCost -= cost;
      }
      public void Reset()
      {
        visitedSet.Clear();
        visitedList.Clear();
        totalCost = 0;
      }

      public void Print()
      {
        for (var i = 0; i < visitedList.Count; i++)
        {
          Console.Write(visitedList[i]);
          if (i != visitedList.Count - 1)
            Console.Write(" -> ");
        }
        Console.Write(" = ");
        Console.WriteLine(totalCost);
      }
    }
    void Search(Graph graph, SearchData.SearchMode searchMode)
    {
      var searchData = new SearchData(searchMode);

      HashSet<string> allNames = new HashSet<string>();
      foreach(var nodePair in graph.Nodes)
      {
        allNames.Add(nodePair.Key);
        foreach (var edge in nodePair.Value.Edges)
          allNames.Add(edge.To);
      }
      searchData.totalCount = allNames.Count;

      foreach(var nodePair in graph.Nodes)
      {
        var node = nodePair.Value;
        searchData.Reset();

        searchData.Add(nodePair.Key, 0);
        Traverse(graph, searchData, nodePair.Key);
      }
      Console.WriteLine($"Best: {searchData.bestCost}");
    }

    void Traverse(Graph graph, SearchData searchData, string currentName)
    {
      if(searchData.totalCount == searchData.visitedList.Count)
      {
        //searchData.Print();
        return;
      }

      var node = graph.Nodes[currentName];
      foreach(var edge in node.Edges)
      {
        if (searchData.visitedSet.Contains(edge.To))
          continue;

        searchData.Add(edge.To, edge.Cost);
        Traverse(graph, searchData, edge.To);
        searchData.Remove(edge.To, edge.Cost);
      }
    }
  }
}
