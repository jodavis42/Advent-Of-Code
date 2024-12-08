namespace AdventOfCode2024
{
  internal class Day07
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var equations = Parse(lines);
      foreach (var equation in equations)
        equation.Print();

      Int64 result = 0;
      foreach (var equation in equations)
        result += Test(equation);
      Console.WriteLine(result);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var equations = Parse(lines);
      foreach (var equation in equations)
        equation.Print();

      Int64 result = 0;
      foreach (var equation in equations)
        result += Test2(equation);
      Console.WriteLine(result);
    }
    enum Operators { Add, Multiply, Concatenate, Count };
    class Equation
    {
      public Int64 Result;
      public List<Int64> Operands = new List<Int64>();

      public void Print()
      {
        Console.Write($"{Result}: ");
        foreach (var i in Operands)
          Console.Write(i + " ");
        Console.WriteLine();
      }

      public Int64 Compute(List<Operators> operators)
      {
        Int64 result = Operands[0];
        for (var i = 0; i < operators.Count; ++i)
        {
          var op = operators[i];
          var v = Operands[i + 1];
          if (op == Operators.Add)
            result += v;
          else if (op == Operators.Multiply)
            result *= v;
          else if (op == Operators.Concatenate)
          {
            int multiplier = 1;
            while (v / multiplier != 0)
              multiplier *= 10;
            result = result * multiplier + v;
          }
        }
        return result;
      }
    }

    List<Equation> Parse(string[] lines)
    {
      var results = new List<Equation>();
      foreach (var line in lines)
      {
        var split0 = line.Split(':');
        var split1 = split0[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var eq = new Equation();
        eq.Result = Int64.Parse(split0[0]);
        foreach (var s in split1)
          eq.Operands.Add(Int64.Parse(s));
        results.Add(eq);
      }
      return results;
    }
    bool Advance(List<Operators> operators)
    {
      int index = 0;
      while (index < operators.Count)
      {
        if (operators[index] == Operators.Add)
        {
          ++operators[index];
          return true;
        }
        // we need to carry...
        if (operators[index] == Operators.Multiply)
        {
          operators[index] = Operators.Add;
          ++index;
        }
      }
      return false;
    }
    bool Advance2(List<Operators> operators)
    {
      int index = 0;
      while (index < operators.Count)
      {
        if (operators[index] <= Operators.Multiply)
        {
          ++operators[index];
          return true;
        }
        // we need to carry...
        else
        {
          operators[index] = Operators.Add;
          ++index;
        }
      }
      return false;
    }
    Int64 Test(Equation equation)
    {
      List<Operators> operators = new List<Operators>();
      for (var i = 1; i < equation.Operands.Count; ++i)
        operators.Add(Operators.Add);

      int count = 0;
      while (true)
      {
        var result = equation.Compute(operators);
        if (result == equation.Result)
          ++count;
        if (!Advance(operators))
          break;
      }

      if (count != 0)
        return equation.Result;
      return 0;
    }
    Int64 Test2(Equation equation)
    {
      List<Operators> operators = new List<Operators>();
      for (var i = 1; i < equation.Operands.Count; ++i)
        operators.Add(Operators.Add);

      int count = 0;
      while (true)
      {
        var result = equation.Compute(operators);
        if (result == equation.Result)
          ++count;
        if (!Advance2(operators))
          break;
      }

      if (count != 0)
        return equation.Result;
      return 0;
    }
  }
}
