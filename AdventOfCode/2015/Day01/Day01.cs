namespace AdventOfCode2015
{
  internal class Day01
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      int floor = 0;
      foreach(var c in lines[0])
      {
        if(c == '(')
          floor++;
        else if(c == ')')
          floor--;
      }
      Console.WriteLine(floor);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      int floor = 0;
      for(var i = 0; i < lines[0].Length; i++)
      {
        var c = lines[0][i];
        if (c == '(')
          floor++;
        else if (c == ')')
          floor--;
        if (floor == -1)
          Console.WriteLine(i + 1);
      }
    }
  }
}
