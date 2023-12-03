using System.Threading;

namespace AdventOfCode2023
{
  internal class Day02
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var games = Parse(lines);
      var sum = 0;
      foreach (var game in games)
      {
        var isPossible = game.IsPossible(12, 13, 14);
        Console.WriteLine($"Game {game.Id}: {isPossible}");
        if (isPossible)
          sum += game.Id;
      }
      Console.WriteLine(sum);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var games = Parse(lines);
      var power = 0;
      foreach (var game in games)
      {
        int redCount, greenCount, blueCount;
        game.CountMinPossible(out redCount, out greenCount, out blueCount);
        Console.WriteLine($"Game {game.Id}: {redCount} {greenCount} {blueCount}");
        power += redCount * greenCount * blueCount;
      }
      Console.WriteLine(power);
    }
    public class Game
    {
      public class Set
      {
        public int Red = 0;
        public int Green = 0;
        public int Blue = 0;

        public void Parse(string text)
        {
          var split = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
          foreach(var pullText in split)
          {
            var pullSplit = pullText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var countText = pullSplit[0];
            var count = int.Parse(countText);
            var colorText = pullSplit[1];
            if (colorText == "red")
              Red += count;
            else if (colorText == "blue")
              Blue += count;
            else if (colorText == "green")
              Green += count;
          }
        }
      }
      public int Id = 0;
      public List<Set> sets = new List<Set>();
      public void Parse(string text)
      {
        var split0 = text.Split(':', StringSplitOptions.RemoveEmptyEntries);
        Id = int.Parse(split0[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

        var setListText = split0[1].Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach(var setText in setListText)
        {
          var set = new Set();
          set.Parse(setText);
          sets.Add(set);
        }
      }
      public void Print()
      {
        Console.Write($"Game {Id}:");
        foreach(var set in sets)
        {
          if (set.Red != 0)
            Console.Write($"{set.Red} red,");
          if (set.Green != 0)
            Console.Write($"{set.Green} green,");
          if (set.Blue != 0)
            Console.Write($"{set.Blue} blue,");
          Console.Write(";");
        }
        Console.WriteLine();
      }
      public bool IsPossible(int redCount, int greenCount, int blueCount)
      {
        foreach(var set in sets)
        {
          if (set.Red > redCount)
            return false;
          if (set.Green > greenCount)
            return false;
          if (set.Blue > blueCount)
            return false;
        }
        return true;
      }
      public void CountMinPossible(out int redCount, out int greenCount, out int blueCount)
      {
        redCount = 0;
        greenCount = 0;
        blueCount = 0;
        foreach(var set in sets)
        {
          redCount = Math.Max(redCount, set.Red);
          greenCount = Math.Max(greenCount, set.Green);
          blueCount = Math.Max(blueCount, set.Blue);
        }
      }
    }
    List<Game> Parse(string[] lines)
    {
      var games = new List<Game>();
      foreach(var line in lines)
      {
        var game = new Game();
        games.Add(game);
        game.Parse(line);
      }
      return games;
    }
  }
}
