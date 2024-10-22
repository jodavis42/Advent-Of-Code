using AdventOfCode2021;
using System.Collections.Generic;

namespace AdventOfCode2015
{
  internal class Day12
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      foreach(var line in lines)
      {
        var node = Parse(line);
        int count = Count(node);
        Console.WriteLine($"{line}: {count}");
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      foreach (var line in lines)
      {
        var node = Parse(line);
        int count = Count2(node);
        Console.WriteLine($"{line}: {count}");
      }
    }
    class Object
    {
      public Node Name;
      public Node Value;
    }
    class Node
    {
      public enum NodeKind { Number, String, Array, Object}
      public NodeKind Kind;
      public int NumberValue;
      public string StringValue;
      public List<Node> ArrayValue = null;
      public List<Object> ObjectValues = null;
    }
    Node Parse(string text)
    {
      var tokenizer = new Tokenizer();
      tokenizer.Tokenize(text);
      var tokens = tokenizer.Tokens;
      int index = 0;
      return Parse(tokens, ref index);
    }
    Node Parse(List<Token> tokens, ref int index)
    {
      if (index >= tokens.Count)
        return null;
      var token = tokens[index];
      if (token.Type == TokenType.Number || token.Type == TokenType.Subtract)
        return ParseNumber(tokens, ref index);
      else if (token.Type == TokenType.StringLiteral)
        return ParseString(tokens, ref index);
      else if (token.Type == TokenType.BeginBracket)
        return ParseArray(tokens, ref index);
      else if (token.Type == TokenType.BeginBrace)
        return ParseObject(tokens, ref index);
      return null;
    }

    Node ParseNumber(List<Token> tokens, ref int index)
    {
      var text = "";
      while (index < tokens.Count && tokens[index].Type == TokenType.Subtract || tokens[index].Type == TokenType.Number)
      {
        text += tokens[index].Text;
        ++index;
      }
      var node = new Node
      {
        Kind = Node.NodeKind.Number,
        NumberValue = int.Parse(text),
      };
      return node;
    }
    Node ParseString(List<Token> tokens, ref int index)
    {
      var node = new Node
      {
        Kind = Node.NodeKind.String,
        StringValue = tokens[index].Text,
      };
      ++index;
      return node;
    }
    Node ParseArray(List<Token> tokens, ref int index)
    {
      if (tokens[index].Type != TokenType.BeginBracket)
        return null;
      ++index;

      var node = new Node();
      node.Kind = Node.NodeKind.Array;
      node.ArrayValue = new List<Node>();

      while (tokens[index].Type != TokenType.EndBracket)
      {
        node.ArrayValue.Add(Parse(tokens, ref index));
        if (tokens[index].Type == TokenType.Comma)
          ++index;
      }
      if (tokens[index].Type != TokenType.EndBracket)
        throw new Exception("Unexpected token");
      ++index;
      return node;
    }
    Node ParseObject(List<Token> tokens, ref int index)
    {
      Expect(tokens, ref index, TokenType.BeginBrace);

      var node = new Node();
      node.Kind = Node.NodeKind.Object;
      node.ObjectValues = new List<Object>();

      while (tokens[index].Type != TokenType.EndBrace)
      {
        var obj = new Object();
        obj.Name = Parse(tokens, ref index);
        Expect(tokens, ref index, TokenType.Colon);
        obj.Value = Parse(tokens, ref index);
        node.ObjectValues.Add(obj);

        Accept(tokens, ref index, TokenType.Comma);
      }
      Expect(tokens, ref index, TokenType.EndBrace);
      return node;
    }

    bool Accept(List<Token> tokens, ref int index, TokenType expectedType)
    {
      if (tokens[index].Type == expectedType)
      {
        ++index;
        return true;
      }
      return false;
    }
    void Expect(List<Token> tokens, ref int index, TokenType expectedType)
    {
      if(!Accept(tokens, ref index, expectedType))
        throw new Exception("Unexpected token type");
    }

    int Count2(Node node)
    {
      if (node.Kind == Node.NodeKind.Number)
        return node.NumberValue;
      else if(node.Kind == Node.NodeKind.Array)
      {
        int sum = 0;
        foreach (var child in node.ArrayValue)
          sum += Count2(child);
        return sum;
      }
      else if (node.Kind == Node.NodeKind.Object)
      {
        foreach(var child in node.ObjectValues)
        {
          if (child.Value.Kind == Node.NodeKind.String && child.Value.StringValue == "\"red\"")
            return 0;
        }
        int sum = 0;
        foreach (var child in node.ObjectValues)
        {
          sum += Count2(child.Name);
          sum += Count2(child.Value);
        }
        return sum;
      }
      return 0;
    }

    int Count(Node node)
    {
      if (node.Kind == Node.NodeKind.Number)
        return node.NumberValue;
      else if(node.Kind == Node.NodeKind.Array)
      {
        int sum = 0;
        foreach (var child in node.ArrayValue)
          sum += Count2(child);
        return sum;
      }
      else if (node.Kind == Node.NodeKind.Object)
      {
        int sum = 0;
        foreach (var child in node.ObjectValues)
        {
          sum += Count2(child.Name);
          sum += Count2(child.Value);
        }
        return sum;
      }
      return 0;
    }

    void Print(Node node, int level)
    {
      //for (var i = 0; i < level; ++i)
      //  Console.Write("\t");
      
      if (node.Kind == Node.NodeKind.Number)
        Console.Write(node.NumberValue);
      else if (node.Kind == Node.NodeKind.String)
        Console.Write(node.StringValue);
      else if (node.Kind == Node.NodeKind.Array)
      {
        Console.Write('[');
        var first = false;
        foreach (var valueNode in node.ArrayValue)
        {
          if(!first)
            Console.Write(',');
          first = true;
          Print(valueNode, level + 1);
        }
        Console.Write(']');
      }
      else if (node.Kind == Node.NodeKind.Object)
      {
        Console.Write('{');
        bool first = false;
        foreach (var objValue in node.ObjectValues)
        {
          if (!first)
            Console.Write(',');
          first = true;
          Print(objValue.Name, level + 1);
          Console.Write(':');
          Print(objValue.Value, level + 1);
        }
        Console.Write('}');
      }
    }
  }
}
