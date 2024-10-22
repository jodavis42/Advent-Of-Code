namespace AdventOfCode2015
{
  internal class Day11
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      
      List<Rule> rules = new List<Rule>();
      rules.Add(new Rule1());
      rules.Add(new Rule2());
      rules.Add(new Rule3());

      foreach(var line in lines)
      {
        var password = line.ToList();
        Print(password);
        Console.Write(" ");
        Increment(password, rules);
        Print(password);
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);

      List<Rule> rules = new List<Rule>();
      rules.Add(new Rule1());
      rules.Add(new Rule2());
      rules.Add(new Rule3());

      foreach (var line in lines)
      {
        var password = line.ToList();
        for(var i = 0; i < 2; ++i)
        {
          Print(password);
          Console.Write(" ");
          Increment(password, rules);
          Print(password);
        }
      }
    }

    interface Rule
    {
      bool IsValid(List<char> password);
    }
    class Rule1 : Rule
    {
      bool Rule.IsValid(List<char> password)
      {
        for (var i = 2; i < password.Count; ++i)
        {
          var c0 = password[i - 2];
          var c1 = password[i - 1];
          var c2 = password[i - 0];
          if (c0 + 1 == c1 && c1 + 1 == c2)
            return true;
        }
        return false;
      }
    }
    class Rule2 : Rule
    {
      bool Rule.IsValid(List<char> password)
      {
        foreach (var c in password)
        {
          if (c == 'i' || c == 'o' || 'c' == 'l')
            return false;
        }
        return true;
      }
    }
    class Rule3 : Rule
    {
      bool Rule.IsValid(List<char> password)
      {
        int count = 0;
        for (var i = 1; i < password.Count; ++i)
        {
          if (password[i] == password[i - 1])
          {
            ++count;
            ++i;
          }
          if (count >= 2)
            return true;
        }
        return false;
      }
    }

    void Increment(List<char> password, List<Rule> rules)
    {
      while(true)
      {
        Increment(password);
        if (IsValid(password, rules))
          break;
      }
    }

    bool IsValid(List<char> password, List<Rule> rules)
    {
      foreach (var rule in rules)
      {
        if (!rule.IsValid(password))
          return false;
      }
      return true;
    }


    void Increment(List<char> password)
    {
      for(var i = password.Count - 1; i >= 0; i--)
      {
        password[i] += (char)1;
        if (password[i] > 'z')
          password[i] = 'a';
        else
          break;
      }
    }

    void Print(List<char> password)
    {
      foreach (var c in password)
        Console.Write(c);
      Console.WriteLine();
    }
  }
}
