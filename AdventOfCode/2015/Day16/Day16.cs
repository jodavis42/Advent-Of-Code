namespace AdventOfCode2015
{
  internal class Day16
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var auntCompounds = Parse(lines);
      var expectedCompounds = new Compounds
      {
        Children = 3,
        Cats = 7,
        Samoyeds = 2,
        Pomeranians = 3,
        Akitas = 0,
        Vizslas = 0,
        Goldfish = 5,
        Trees = 3,
        Cars = 2,
        Perfumes = 1,
      };
      for(var i = 0; i < auntCompounds.Count; i++)
      {
        bool isValid = Check(expectedCompounds, auntCompounds[i]);
        if (isValid)
          Console.WriteLine($"Found Aunt {i + 1}");
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var auntCompounds = Parse(lines);
      var expectedCompounds = new Compounds
      {
        Children = 3,
        Cats = 7,
        Samoyeds = 2,
        Pomeranians = 3,
        Akitas = 0,
        Vizslas = 0,
        Goldfish = 5,
        Trees = 3,
        Cars = 2,
        Perfumes = 1,
      };
      for (var i = 0; i < auntCompounds.Count; i++)
      {
        bool isValid = Check2(expectedCompounds, auntCompounds[i]);
        if (isValid)
          Console.WriteLine($"Found Aunt {i + 1}");
      }
    }

    class Compounds
    {
      public int? Children;
      public int? Cats;
      public int? Samoyeds;
      public int? Pomeranians;
      public int? Akitas;
      public int? Vizslas;
      public int? Goldfish;
      public int? Trees;
      public int? Cars;
      public int? Perfumes;
    }

    List<Compounds> Parse(string[] lines)
    {
      var results = new List<Compounds>();
      foreach (var line in lines)
      {
        var compounds = new Compounds();
        results.Add(compounds);
        var firstColon = line.IndexOf(':');
        var data = line.Substring(firstColon + 1);
        var compoundsSplit = data.Split(",", StringSplitOptions.TrimEntries);
        foreach (var split in compoundsSplit)
        {
          var split1 = split.Split(':', StringSplitOptions.TrimEntries);
          var name = split1[0];
          var value = int.Parse(split1[1]);
          if (name == "children")
            compounds.Children = value;
          else if (name == "cats")
            compounds.Cats = value;
          else if (name == "samoyeds")
            compounds.Samoyeds = value;
          else if (name == "pomeranians")
            compounds.Pomeranians = value;
          else if (name == "akitas")
            compounds.Akitas = value;
          else if (name == "vizslas")
            compounds.Vizslas = value;
          else if (name == "goldfish")
            compounds.Goldfish = value;
          else if (name == "trees")
            compounds.Trees = value;
          else if (name == "cars")
            compounds.Cars = value;
          else if (name == "perfumes")
            compounds.Perfumes = value;
          else
            throw new Exception($"Unexpected name {name}");
        }
      }
      return results;
    }

    bool Check(Compounds expectedCompounds, Compounds actualCompounds)
    {
      if(actualCompounds.Children.HasValue && actualCompounds.Children != expectedCompounds.Children)
        return false;
      if (actualCompounds.Cats.HasValue && actualCompounds.Cats != expectedCompounds.Cats)
        return false;
      if (actualCompounds.Samoyeds.HasValue && actualCompounds.Samoyeds != expectedCompounds.Samoyeds)
        return false;
      if (actualCompounds.Pomeranians.HasValue && actualCompounds.Pomeranians != expectedCompounds.Pomeranians)
        return false;
      if (actualCompounds.Akitas.HasValue && actualCompounds.Akitas != expectedCompounds.Akitas)
        return false;
      if (actualCompounds.Vizslas.HasValue && actualCompounds.Vizslas != expectedCompounds.Vizslas)
        return false;
      if (actualCompounds.Goldfish.HasValue && actualCompounds.Goldfish != expectedCompounds.Goldfish)
        return false;
      if (actualCompounds.Trees.HasValue && actualCompounds.Trees != expectedCompounds.Trees)
        return false;
      if (actualCompounds.Cars.HasValue && actualCompounds.Cars != expectedCompounds.Cars)
        return false;
      if (actualCompounds.Perfumes.HasValue && actualCompounds.Perfumes != expectedCompounds.Perfumes)
        return false;
      return true;
    }

    bool Check2(Compounds expectedCompounds, Compounds actualCompounds)
    {
      if (actualCompounds.Children.HasValue && (actualCompounds.Children != expectedCompounds.Children))
        return false;
      if (actualCompounds.Cats.HasValue && !(actualCompounds.Cats > expectedCompounds.Cats))
        return false;
      if (actualCompounds.Samoyeds.HasValue && (actualCompounds.Samoyeds != expectedCompounds.Samoyeds ))
        return false;
      if (actualCompounds.Pomeranians.HasValue && !(actualCompounds.Pomeranians < expectedCompounds.Pomeranians))
        return false;
      if (actualCompounds.Akitas.HasValue && (actualCompounds.Akitas != expectedCompounds.Akitas))
        return false;
      if (actualCompounds.Vizslas.HasValue && (actualCompounds.Vizslas != expectedCompounds.Vizslas))
        return false;
      if (actualCompounds.Goldfish.HasValue && !(actualCompounds.Goldfish < expectedCompounds.Goldfish))
        return false;
      if (actualCompounds.Trees.HasValue && !(actualCompounds.Trees > expectedCompounds.Trees))
        return false;
      if (actualCompounds.Cars.HasValue && (actualCompounds.Cars != expectedCompounds.Cars))
        return false;
      if (actualCompounds.Perfumes.HasValue && (actualCompounds.Perfumes != expectedCompounds.Perfumes))
        return false;
      return true;
    }
  }
}
