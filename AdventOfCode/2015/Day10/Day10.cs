namespace AdventOfCode2015
{
  internal class Day10
  {
    double Constant = 1.303577269;

    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var input = lines[0];
      for (var i = 0; i < 40; ++i)
      {
        var output = Iterate(input);
        input = output;
      }
      Console.WriteLine(input.Length);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var input = lines[0];
      double length = (float)input.Length;
      var startLength = length;
      for(var i = 0; i < 30; ++i)
      {
        var output = Iterate(input);
        var inputLength = (double)input.Length;
        var outputLength = (double)output.Length;
        var ratio = outputLength / inputLength;
        var diff = outputLength - inputLength;
        //var calc = startLength + Math.Pow(Constant, i + 1);
        length = length * Constant;
        Console.WriteLine($"{inputLength} {outputLength} {ratio} {length}");
        input = output;
      }
      length = Math.Pow(length, (double)1.30537f);
      //for (var i = 0; i < 40; ++i)
      //{

      //  var output = Iterate(input);
      //  //Console.WriteLine($"{input}: {output}");
      //  input = output;
      //}
      Console.WriteLine(length);
    }

    string Iterate(string input)
    {
      string result = "";
      for(var i = 0; i < input.Length; ++i)
      {
        var count = 1;
        var c = input[i];
        while (i + 1 < input.Length && input[i + 1] == c)
        {
          ++i;
          ++count;
        }
        result += count.ToString() + c;
      }
      return result;
    }
  }
}
