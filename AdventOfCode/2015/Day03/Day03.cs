using AdventOfCode2022;

namespace AdventOfCode2015
{
  internal class Day03
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      
      foreach(var line in lines)
      {
        HashGrid2d<bool> grid = new HashGrid2d<bool>();
        var pos = new Int2(0, 0);
        uint count = 1u;
        grid.Set(pos, true);
        foreach(var c in line)
        {
          if (c == '<')
            pos.X -= 1;
          else if (c == '>')
            pos.X += 1;
          else if (c == '^')
            pos.Y += 1;
          else if (c == 'v')
            pos.Y -= 1;
          if(grid.Get(pos, false) == false)
          {
            ++count;
            grid.Set(pos, true);
          }
        }
        Console.WriteLine(count);
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      foreach (var line in lines)
      {
        HashGrid2d<bool> grid = new HashGrid2d<bool>();
        var positions = new List<Int2>{ new Int2(0, 0), new Int2(0, 0) };
        uint count = 1u;
        grid.Set(positions[0], true);
        for(var i = 0; i < line.Count(); ++i)
        {
          var position = positions[i % 2];
          Update(line[i], ref position);
          positions[i % 2] = position;

          if (grid.Get(position, false) == false)
          {
            ++count;
            grid.Set(position, true);
          }
        }
        Console.WriteLine(count);
      }
    }
    void Update(char c, ref Int2 pos)
    {
      if (c == '<')
        pos.X -= 1;
      else if (c == '>')
        pos.X += 1;
      else if (c == '^')
        pos.Y += 1;
      else if (c == 'v')
        pos.Y -= 1;
    }
  }
}
