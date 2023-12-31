﻿class Program
{
  static void Main(string[] args)
  {
    if (args.Length >= 3)
    {
      int.TryParse(args[0], out var dayNumber);
      int.TryParse(args[1], out var testNumber);
      string filePath = args[2];

      var typeName = $"AdventOfCode2021.Day{dayNumber}";
      var type = Type.GetType(typeName);
      if (type == null)
        throw new Exception();

      var day = Activator.CreateInstance(type) as AdventOfCode.IDay;
      if (day == null)
        throw new Exception();

      if (testNumber == 0)
        day.UnitTest();
      else if (testNumber == 1)
        day.Run1(filePath);
      else if (testNumber == 2)
        day.Run2(filePath);
      return;
    }


    var test = new AdventOfCode2023.Day03();
    var name = Path.Combine("2023", test.GetType().Name);
    string testFile;
    testFile = Path.Combine(name, "Example.txt");
    //testFile = Path.Combine(name, "input1");
    //testFile = Path.Combine(name, "input2");

    test.Run1(testFile);
    //test.Run2(testFile);
  }
}