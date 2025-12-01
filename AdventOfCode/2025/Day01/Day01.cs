namespace AdventOfCode2025
{
  internal class Day01
  {
    class Instruction
    {
      public int Value;
      public override string ToString()
      {
        return Value.ToString();
      }
    }
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var instructions = Parse(lines);
      int CurrentValue = 50;
      int ZeroCount = 0;
      foreach (var instruction in instructions)
      {
        var newValue = Evaluate(CurrentValue, instruction);
        Console.WriteLine($"{CurrentValue} + {instruction.Value} = {newValue}");
        CurrentValue = newValue;
        if(CurrentValue == 0)
        {
          ++ZeroCount;
        }
        //Console.WriteLine(instruction);
      }
      Console.WriteLine(ZeroCount);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var instructions = Parse(lines);
      int CurrentValue = 50;
      int ZeroCount = 0;
      foreach (var instruction in instructions)
      {
        var newValue = Evaluate2(CurrentValue, instruction);
        newValue = Reduce(newValue, ref ZeroCount);
        Console.WriteLine($"{CurrentValue} + {instruction.Value} = {newValue}; ZeroCount {ZeroCount}");
        CurrentValue = newValue;
      }
      Console.WriteLine(ZeroCount);
    }

    int Evaluate(int Value, Instruction instruction)
    {
      Value += instruction.Value;
      Value = (Value + 100) % 100;
      return Value;
    }
    int Evaluate2(int Value, Instruction instruction)
    {
      Value += instruction.Value;
      return Value;
    }
    int Reduce(int Value, ref int ZeroCount)
    {
      while(Value < 0)
      {
        Value += 100;
        ++ZeroCount;
      }
      while(Value >= 100)
      {
        Value -= 100;
        ++ZeroCount;
      }
      return Value;
    }

    List<Instruction> Parse(string[] lines)
    {
      var instructions = new List<Instruction>();
      foreach(var line in lines)
      {
        var instruction = new Instruction();
        instruction.Value = int.Parse(line.Substring(1));
        if (line[0] == 'L')
          instruction.Value *= -1;
        instructions.Add(instruction);
      }
      return instructions;
    }
  }
}
