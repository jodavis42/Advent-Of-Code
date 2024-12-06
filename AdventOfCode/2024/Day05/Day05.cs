namespace AdventOfCode2024
{
  internal class Day05
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var context = new Context();
      context.Parse(lines);
      //context.Print();
      context.BuildGraph();
      var sum = context.SumValidated();
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var context = new Context();
      context.Parse(lines);
      //context.Print();
      context.BuildGraph();
      var sum = context.SumFixedInvalidated();
      Console.WriteLine(sum);
    }

    class Context
    {
      public class Node
      {
        public List<Node> Edges = new List<Node>();
        public int Value;
      }
      public class Graph
      {
        public Dictionary<int, Node> Nodes = new Dictionary<int, Node>();
        public Node GetOrCreateNode(int value)
        {
          if(!Nodes.TryGetValue(value, out var result))
          {
            result = new Node { Value = value };
            Nodes.Add(result.Value, result);
          }
          return result;
        }
      }
      public class Rule
      {
        public int First;
        public int Second;
      }
      public class Update
      {
        public List<int> Pages = new List<int>();
      }

      public List<Rule> Rules = new List<Rule>();
      public List<Update> Updates = new List<Update>();
      public Graph mGraph = new Graph();

      public void Parse(string[] lines)
      {
        var count = lines.Length;
        var i = 0;
        for(; i < count; ++i)
        {
          var line = lines[i];
          if (line.Length == 0)
            break;
          
          var split = line.Split('|');
          var rule = new Rule();
          rule.First = int.Parse(split[0]);
          rule.Second= int.Parse(split[1]);
          Rules.Add(rule);
        }
        ++i;
        for(; i < count; ++i)
        {
          var line = lines[i];
          var split = line.Split(',');
          var update = new Update();
          foreach(var c in split)
            update.Pages.Add(int.Parse(c));
          Updates.Add(update);
        }
      }
      public void Print()
      {
        foreach(var rule in Rules)
          Console.WriteLine($"{rule.First}|{rule.Second}");
        foreach(var update in Updates)
        {
          foreach (var page in update.Pages)
            Console.Write($"{page},");
          Console.WriteLine();
        }
      }
      public void BuildGraph()
      {
        foreach (var rule in Rules)
        {
          var first = mGraph.GetOrCreateNode(rule.First);
          var second = mGraph.GetOrCreateNode(rule.Second);
          second.Edges.Add(first);
        }
      }
      public int SumValidated()
      {
        int result = 0;
        foreach(var update in Updates)
        {
          if(Validate(update))
          {
            var m = update.Pages.Count / 2;
            result += update.Pages[m];
          }
        }
        return result;
      }
      public int SumFixedInvalidated()
      {
        int result = 0;
        foreach (var update in Updates)
        {
          if (Validate(update))
            continue;

          var fixedUpdate = FixUpdate(update);
          var m = fixedUpdate.Pages.Count / 2;
          result += fixedUpdate.Pages[m];
        }
        return result;
      }
      public bool Validate(Update update)
      {
        for (var i = 1; i < update.Pages.Count; ++i)
        {
          if (!TestValid(update, i - 1, i))
            return false;
        }
        return true;
      }
      public bool TestValid(Update update, int i0, int i1)
      {
        var p0 = update.Pages[i0];
        var p1 = update.Pages[i1];

        mGraph.Nodes.TryGetValue(p0, out var n0);
        mGraph.Nodes.TryGetValue(p1, out var n1);

        if (n0 != null)
        {
          foreach (var node in n0.Edges)
          {
            if (node.Value == p1)
              return false;
          }
        }
        return true;
      }
      public Update FixUpdate(Update updateToFix)
      {
        var result = new Update();
        foreach(var p in updateToFix.Pages)
          result.Pages.Add(p);

        for(var i = 1; i < result.Pages.Count; ++i)
        {
          var index = i;
          while(index > 0 && !TestValid(result, index - 1, index))
          {
            var temp = result.Pages[index];
            result.Pages[index] = result.Pages[index - 1];
            result.Pages[index - 1] = temp;
            --index;
          }
        }
        return result;
      }
    }
  }
}
