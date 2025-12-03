using System.Text;

namespace AdventOfCode2025
{
  internal class Day03
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var banks = Parse(lines);
      var sum = 0;
      foreach(var bank in banks)
      {
        Console.WriteLine(bank);
        var joltage = ComputeJoltage(bank);
        sum += joltage;
        Console.WriteLine(joltage);
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var banks = Parse(lines);
      UInt64 sum = 0;
      foreach (var bank in banks)
      {
        Console.WriteLine(bank);
        var joltage = ComputeJoltage3(bank, 12);
        sum += joltage;
        Console.WriteLine(joltage);
      }
      Console.WriteLine(sum);
    }
    class Bank
    {
      public List<int> Values = new List<int>();
      public override string ToString()
      {
        var builder = new StringBuilder();
        foreach(var value in Values)
        {
          builder.Append(value.ToString());
        }
        return builder.ToString();
      }
    }
    List<Bank> Parse(string[] lines)
    {
      var result = new List<Bank>();
      foreach(var line in lines)
      {
        var bank = new Bank();
        result.Add(bank);

        foreach(var c in line)
        {
          bank.Values.Add(c - '0');
        }
      }
      return result;
    }
    int ComputeJoltage(Bank bank)
    {
      var bestJoltage = 0;
      var bestDigits = new List<int> { bank.Values[0], bank.Values[1] };
      bestJoltage = bestDigits[0] * 10 + bestDigits[1];
      for(var i = 0; i < bank.Values.Count; ++i)
      {
        if (bank.Values[i] < bestDigits[0])
          continue;

        for(var j = i + 1; j < bank.Values.Count; ++j)
        {
          var joltage = bank.Values[i] * 10 + bank.Values[j];
          if(joltage > bestJoltage)
          {
            bestJoltage = joltage;
            bestDigits[0] = bank.Values[i];
            bestDigits[1] = bank.Values[j];
          }
        }
      }
      return bestJoltage;
    }
    UInt64 ComputeCurrentJoltage(Bank bank, List<int> indices)
    {
      UInt64 joltage = 0;

      var count = indices.Count;
      for (var i = 0; i < indices.Count; ++i)
      {
        var index = indices[count - i - 1];
        UInt64 digit = (UInt64)bank.Values[index];
        joltage += digit * (UInt64)(Math.Pow(10, i));
      }
      return joltage;
    }
    UInt64 ComputeJoltage3(Bank bank, int digitCount = 12)
    {
      var digits = new List<int>();
      for (var i = 0; i < digitCount; ++i)
      {
        digits.Add(0);
      }
      for (var digitIndex = 0; digitIndex < digitCount; ++digitIndex)
      {
        var searchStart = digitIndex == 0 ? 0 : (digits[digitIndex - 1] + 1);
        var searchEnd = bank.Values.Count - (digitCount - digitIndex);
        digits[digitIndex] = FindHighestDigitIndexInRange(bank, searchStart, searchEnd);
      }
      return ComputeCurrentJoltage(bank, digits);
    }
    int FindHighestDigitIndexInRange(Bank bank, int searchStart, int searchEnd)
    {
      var highestDigit = -1;
      var highestDigitIndex = -1;
      for (var j = searchStart; j <= searchEnd; ++j)
      {
        if (bank.Values[j] > highestDigit)
        {
          highestDigit = bank.Values[j];
          highestDigitIndex = j;
        }
      }
      return highestDigitIndex;
    }
  }
}
