namespace AdventOfCode2023
{
  internal class Day04
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var cards = Parse(lines);
      var scoreTotal = 0u;
      foreach (var card in cards)
      {
        card.Print();
        var score = card.Score();
        scoreTotal += score;
        Console.WriteLine($"Score: {score}");
      }

      Console.WriteLine($"ScoreTotal: {scoreTotal}");
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var cards = Parse(lines);
      var cardSets = new Dictionary<uint, uint>();

      foreach (var card in cards)
        cardSets[card.CardId] = 0;
      foreach (var card in cards)
      {
        var score = card.ScoreCount();
        cardSets[card.CardId] += 1;
        var currentCardCount = cardSets[card.CardId];
        for (var i = 1u; i <= score; ++i)
        {
          var cardId = card.CardId + i;
          if (cardSets.ContainsKey(cardId))
            cardSets[cardId] += currentCardCount;
        }
      }
      var total = 0u;
      foreach (var pair in cardSets)
      {
        total += pair.Value;
        Console.WriteLine($"Card {pair.Key}: {pair.Value}");
      }
      Console.WriteLine("Cards: " + total);

    }
    List<Card> Parse(string[] lines)
    {
      var cards = new List<Card>();
      foreach (var line in lines)
        cards.Add(new Card(line));
      return cards;

    }
    class Card
    {
      public uint CardId = 0;
      public List<int> WinningNumbers = new List<int>();
      public HashSet<int> WinningNumberSet = new HashSet<int>();
      public List<int> Numbers = new List<int>();
      public Card(string text)
      {
        Parse(text);
      }

      public void Parse(string text)
      {
        var options = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var split0 = text.Split(":", options);
        CardId = uint.Parse(split0[0].Split(' ', options)[1]);

        var split1 = split0[1].Split('|', options);
        foreach (var numberText in split1[0].Split(' ', options))
        {
          var number = int.Parse(numberText);
          WinningNumbers.Add(number);
          WinningNumberSet.Add(number);
        }
        foreach (var numberText in split1[1].Split(' ', options))
          Numbers.Add(int.Parse(numberText));
      }
      public void Print()
      {
        Console.Write($"Card {CardId}: ");
        foreach (var winningNumber in WinningNumbers)
          Console.Write($"{winningNumber} ");
        Console.Write(" | ");
        foreach (var number in Numbers)
          Console.Write($"{number} ");
        Console.WriteLine();
      }
      public uint Score()
      {
        var score = 0u;
        foreach (var number in Numbers)
        {
          if (WinningNumberSet.Contains(number))
          {
            if (score == 0)
              score = 1;
            else
              score *= 2;
          }
        }
        return score;
      }
      public uint ScoreCount()
      {
        var score = 0u;
        foreach (var number in Numbers)
        {
          if (WinningNumberSet.Contains(number))
          {
            ++score;
          }
        }
        return score;
      }
    }
  }
}
