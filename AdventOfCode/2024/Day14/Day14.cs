using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
  internal class Day14
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var robots = Parse(lines);

      int width = 101;
      int height = 103;
      for (var i = 0; i < 100; ++i)
      {
        Simulate(robots, width, height);
      }
      Console.WriteLine(Score(robots, width, height));
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var robots = Parse(lines);

      int width = 101;
      int height = 103;
      for (var i = 0; i < 10000; ++i)
      {
        Simulate(robots, width, height);
        DumpToFile(robots, width, height, $"Step{i + 1}.txt");
      }
    }

    class Robot
    {
      public int Px, Py;
      public int Vx, Vy;
      public override string ToString()
      {
        return $"({Px}, {Py}) ({Vx}, {Vy})";
      }
    }
    List<Robot> Parse(string[] lines)
    {
      var results = new List<Robot>();
      foreach(var line in lines)
      {
        var split0 = line.Split(' ');
        var pSplit = split0[0].Split('=')[1].Split(',');
        var vSplit = split0[1].Split('=')[1].Split(',');
        var robot = new Robot();
        robot.Px = int.Parse(pSplit[0]);
        robot.Py = int.Parse(pSplit[1]);
        robot.Vx = int.Parse(vSplit[0]);
        robot.Vy = int.Parse(vSplit[1]);
        results.Add(robot);
      }
      return results;
    }
    void Print(List<Robot> robots, int width, int height)
    {
      var grid = BuildGrid(robots, width, height);
      Print(grid);
    }
    int Mod(int value, int modValue)
    {
      if (value < 0)
        return value + modValue;
      return value % modValue; ;
    }
    void Simulate(Robot robot, int width, int height)
    {
      var px = robot.Px + robot.Vx;
      var py = robot.Py + robot.Vy;
      robot.Px = Mod(px, width);
      robot.Py = Mod(py, height);
    }
    void Simulate(List<Robot> robots, int width, int height)
    {
      foreach(var robot in robots)
        Simulate(robot, width, height);
    }
    int Score(List<Robot> robots, int width, int height)
    {
      int lu = 0;
      int ld = 0;
      int ru = 0;
      int rd = 0;
      int halfW = width / 2;
      int halfH = height / 2;
      foreach(var robot in robots)
      {
        if(robot.Px <  halfW)
        {
          if (robot.Py < halfH)
            ++lu;
          else if (robot.Py > halfH)
            ++ld;
        }
        else if(robot.Px > halfW)
        {
          if (robot.Py < halfH)
            ++ru;
          else if (robot.Py > halfH)
            ++rd;
        }
      }
      return lu * ld * ru * rd;
    }
    Grid2d<int> BuildGrid(List<Robot> robots, int width, int height)
    {
      var grid = new Grid2d<int>();
      grid.Initialize(width, height, 0);
      for (var i = 0; i < robots.Count; ++i)
      {
        var robot = robots[i];
        ++grid[robot.Px, robot.Py];
      }
      return grid;
    }
    void Print(Grid2d<int> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var count = grid[x, y];
          if (count == 0)
            Console.Write('.');
          else
            Console.Write(count);
        }
        Console.WriteLine();
      }
    }
    bool CheckPart2(Grid2d<int> grid)
    {
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var c = grid[x, y];
          if (c > 1)
            return false;
        }
      }
      return true;
    }
    void DumpToFile(List<Robot> robots, int width, int height, string name)
    {
      var grid = BuildGrid(robots, width, height);
      if (!CheckPart2(grid))
        return;

      var file = File.OpenWrite(name);
      for (var y = 0; y < grid.Height; ++y)
      {
        for (var x = 0; x < grid.Width; ++x)
        {
          var count = grid[x, y];
          if (count == 0)
            file.WriteByte((byte)'.');
          else
            file.WriteByte((byte)(count + '1'));
        }
        file.WriteByte((byte)'\n');
      }
    }
  }
}
