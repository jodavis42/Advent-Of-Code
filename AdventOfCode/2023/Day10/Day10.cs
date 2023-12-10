using System.Dynamic;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2023
{
  internal class Day10
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      Parse(lines);
      BuildMasks();
      var start = ComputeStart();
      Console.WriteLine($"{start.X} {start.Y}");
      ComputeStartTile(start);
      Print();
      Console.WriteLine();
      Solve(start);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
    }
    Grid2d<char> Grid = new Grid2d<char>();
    void Parse(string[] lines)
    {
      Grid.Initialize(lines[0].Length, lines.Length, '.');
      for(var y = 0; y < Grid.Height; ++y) 
      {
        for (var x = 0; x < Grid.Width; ++x)
          Grid[x, y] = lines[y][x];
      }
    }
    Int2 ComputeStart()
    {
      for (var y = 0; y < Grid.Height; ++y)
      {
        for (var x = 0; x < Grid.Width; ++x)
        {
          if (Grid[x, y] == 'S')
            return new Int2(x, y);
        }
      }
      return new Int2();
    }
    void Print()
    {
      for (var y = 0; y < Grid.Height; ++y)
      {
        for (var x = 0; x < Grid.Width; ++x)
        {
          Console.Write(Grid[x, y]);
        }
        Console.WriteLine();
      }
    }
    enum Mask : uint
    {
      None,
      East = 1 << 1,
      West = 1 << 2,
      North = 1 << 3,
      South = 1 << 4,
    }
    Dictionary<char, Mask> TileToMask = new Dictionary<char, Mask>();
    Dictionary<Mask, char> MaskToTile = new Dictionary<Mask, char>();
    void SetMask(char c, Mask mask)
    {
      TileToMask[c] = mask;
      MaskToTile[mask] = c;
    }
    void BuildMasks()
    {
      SetMask('|', Mask.North | Mask.South);
      SetMask('-', Mask.East | Mask.West);
      SetMask('L', Mask.North | Mask.East);
      SetMask('J', Mask.North | Mask.West);
      SetMask('7', Mask.South | Mask.West);
      SetMask('F', Mask.South | Mask.East);
      SetMask('.', Mask.None);
    }
    Mask GetMask(char c)
    {
      return TileToMask.GetValueOrDefault(c, Mask.None);
    }
    Int2 GetDirection(Mask mask)
    {
      if (mask == Mask.East)
        return new Int2(1, 0);
      else if (mask == Mask.West)
        return new Int2(-1, 0);
      else if (mask == Mask.North)
        return new Int2(0, -1); // flipped due to grid indexing
      else if (mask == Mask.South)
        return new Int2(0, 1); // flipped due to grid indexing
      throw new Exception();
    }
    Mask GetMask(Int2 direction)
    {
      Mask result = Mask.None;
      if (direction.X == 1)
        result |= Mask.East;
      if (direction.X == -1)
        result |= Mask.West;
      if (direction.Y == 1)
        result |= Mask.South;
      if (direction.Y == -1)
        result |= Mask.North;
      return result;
    }
    char GetTile(Mask mask)
    {
      return MaskToTile.GetValueOrDefault(mask, '.');
    }
    void ComputeStartTile(Int2 start)
    {
      var directions = new List<Mask>()
      {
        Mask.East,
        Mask.West,
        Mask.North,
        Mask.South,
      };
      var masks = new List<Mask>
      {
        Mask.West,
        Mask.East,
        Mask.South,
        Mask.North,
      };

      Mask connectedMask = Mask.None;
      for(var i = 0; i < directions.Count; ++i)
      {
        var offset = GetDirection(directions[i]);
        var pos = start + offset;
        if (!Grid.IsValidPosition(pos.X, pos.Y))
          continue;
        var tile = Grid[pos.X, pos.Y];
        var mask = GetMask(tile);
        if((mask & masks[i]) != 0)
        {
          connectedMask |= directions[i];
        }
      }
      Console.WriteLine($"{connectedMask} {GetTile(connectedMask)}");
      Grid[start.X, start.Y] = GetTile(connectedMask);
    }
    bool IsConnected(Int2 p0, Int2 p1)
    {
      var t0 = Grid[p0.X, p0.Y];
      var t1 = Grid[p1.X, p1.Y];
      var dir = p1 - p0;
      var dirMask = GetMask(dir);
      var negDirMask = GetMask(-dir);
      var t0Connected = (GetMask(t0) & dirMask) != 0;
      var t1Connected = (GetMask(t1) & negDirMask) != 0;
      return t0Connected && t1Connected;
    }
    void Solve(Int2 start)
    {
      var dGrid = new Grid2d<uint>();
      dGrid.Initialize(Grid.Width, Grid.Height, uint.MaxValue);
      dGrid[start.X, start.Y] = 0;
      var stack = new Stack<(Int2 Position, uint Distance)>();
      stack.Push((start, 0));
      var offsets = new List<Int2>()
      {
        new Int2(1, 0),
        new Int2(-1, 0),
        new Int2(0, 1),
        new Int2(0, -1),
      };
      while (stack.Count != 0)
      {
        var top = stack.Pop();
        var tile = Grid[top.Position.X, top.Position.Y];
        for(var i = 0; i < offsets.Count; ++i)
        {
          var offset = offsets[i];
          var nextPos = top.Position + offset;
          if (!Grid.IsValidPosition(nextPos.X, nextPos.Y))
            continue;
          var newDistance = top.Distance + 1;
          var currDistance = dGrid[nextPos.X, nextPos.Y];
          if (currDistance <= newDistance)
            continue;
          if (!IsConnected(top.Position, nextPos))
            continue;
          dGrid[nextPos.X, nextPos.Y] = newDistance;
          stack.Push((nextPos, newDistance));
        }
      }
      uint maxDistance = 0;
      for (var y = 0; y < dGrid.Height; ++y)
      {
        for (var x = 0; x < dGrid.Width; ++x)
        {
          var d = dGrid[x, y];
          if (d == uint.MaxValue)
            Console.Write(". ");
          else
          {
            maxDistance = Math.Max(maxDistance, d);
            Console.Write(d + " ");
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine(maxDistance);
    }
  }
}
