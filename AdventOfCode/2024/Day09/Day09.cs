using System.Diagnostics;

namespace AdventOfCode2024
{
  internal class Day09
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var data = Parse(lines);
      var decompressed = Decompress(data);
      foreach (var d in decompressed)
        d.Print();
      Console.WriteLine();
      var expanded = Expand(decompressed);
      Compact(expanded);
      foreach (var i in expanded)
      {
        if (i == -1)
          Console.Write('.');
        else
          Console.Write(i);
      }
      Console.WriteLine();
      Console.WriteLine(ComputeChecksum(expanded));
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath); 
      var data = Parse(lines);
      var decompressed = Decompress(data);
      CompactFiles(decompressed);
      var expanded = Expand(decompressed); 
      foreach (var i in expanded)
      {
        if (i == -1)
          Console.Write('.');
        else
          Console.Write(i);
      }
      Console.WriteLine();
      Console.WriteLine(ComputeChecksum(expanded));
    }

    List<int> Parse(string[] lines)
    {
      var result = new List<int>();
      foreach (var c in lines[0])
        result.Add((int)(c - '0'));
      return result;
    }
    [DebuggerDisplay("{ToString()}")]
    class Block
    {
      public int Id;
      public bool Free;
      public int Length;
      public void Print()
      {
        var s = Id.ToString();
        if (Free)
          s = ".";
        for (var i = 0; i < Length; ++i)
          Console.Write(s);
      }
      public override string ToString()
      {
        int id = Free ? -1 : Id;
        return $"{id}: {Length}";
      }
    }
    List<Block> Decompress(List<int> compressed)
    {
      var results = new List<Block>();
      var id = 0;
      for (var i = 0; i < compressed.Count; ++i)
      {
        var block = new Block();
        if(i % 2 == 0)
        {
          block.Id = id;
          ++id;
          block.Length = compressed[i];
          block.Free = false;
        }
        else
        {
          block.Free = true;
          block.Length = compressed[i];
        }
        results.Add(block);
      }
      return results;
    }
    List<int> Expand(List<Block> blocks)
    {
      var results = new List<int>();
      foreach(var block in blocks)
      {
        int value = block.Free ? -1 : block.Id;
        for (var i = 0; i < block.Length; ++i)
          results.Add(value);
      }
      return results;
    }
    void Compact(List<int> values)
    {
      var start = 0;
      var end = values.Count - 1;
      while(true)
      {
        while (start < values.Count && values[start] != -1)
          ++start;
        while (end >= 0 && values[end] == -1)
          --end;

        if (start >= end)
          break;

        var temp = values[start];
        values[start] = values[end];
        values[end] = temp;
      }
    }
    int FindFreeBlock(List<Block> blocks, int endIndex, int length)
    {
      for(var i = 0; i < endIndex; ++i)
      {
        if (blocks[i].Free && blocks[i].Length >= length)
          return i;
      }
      return -1;
    }
    void CompactFiles(List<Block> blocks)
    {
      var curr = blocks.Count - 1;
      while(true)
      {
        while (curr > 0 && blocks[curr].Free)
          --curr;

        if (curr <= 0)
          break;

        var freeIndex = FindFreeBlock(blocks, curr, blocks[curr].Length);
        if (freeIndex == -1)
        {
          --curr;
          continue;
        }

        // Same length, just swap blocks
        var freeBlock = blocks[freeIndex];
        var currBlock = blocks[curr];
        if (blocks[curr].Length == blocks[freeIndex].Length)
        {
          blocks[freeIndex] = currBlock;
          blocks[curr] = freeBlock;
        }
        else
        {
          // have to split the free block
          freeBlock.Length -= currBlock.Length;
          currBlock.Free = true;
          var movedBlock = new Block { Free = false, Length = currBlock.Length, Id = currBlock.Id };
          blocks.Insert(freeIndex, movedBlock);
        }
      }
    }
    Int64 ComputeChecksum(List<int> values)
    {
      Int64 result = 0;
      for(var i = 0; i < values.Count; ++i)
      {
        var value = values[i];
        if (value != -1)
          result += value * i;
      }
      return result;
    }
  }
}
