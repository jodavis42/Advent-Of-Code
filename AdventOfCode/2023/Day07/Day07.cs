using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2023
{
  internal class Day07
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var hands = Parse(lines);
      hands.Sort();
      UInt64 score = 0;
      for (var i = 0; i < hands.Count; i++)
      {
        var h = hands[i];
        var handScore = h.bid * (UInt64)(i + 1);
        h.Print();
        Console.WriteLine($"{handScore} = {h.bid} * {i + 1}");
        score += handScore;
      }
      Console.WriteLine(score);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      var hands = Parse(lines, false);
      hands.Sort();
      UInt64 score = 0;
      for (var i = 0; i < hands.Count; i++)
      {
        var h = hands[i];
        var handScore = h.bid * (UInt64)(i + 1);
        h.Print();
        Console.WriteLine($"{handScore} = {h.bid} * {i + 1}");
        score += handScore;
      }
      Console.WriteLine(score);
    }
    public enum HandType
    {
      HighCard,
      OnePair,
      TwoPair,
      ThreeOfAKind,
      FullHouse,
      FourOfAKind,
      FiveOfKind,
    }
    public enum Card
    {
      Joker,
      Two,
      Three,
      Four,
      Five,
      Six,
      Seven,
      Eight,
      Nine,
      Ten,
      Jack,
      Queen,
      King,
      Ace,
      Count,
    }
    class Hand : IComparable<Hand>
    {
      public List<Card> cards = new List<Card>();
      public List<Card> cardsSorted = new List<Card>();
      public List<int> cardCounts = new List<int>();
      public HandType hand;
      public UInt64 bid;
      public void Parse(string text, bool part1 = true)
      {
        var split = text.Split(' ');
        bid = UInt64.Parse(split[1]);
        foreach (var c in split[0])
        {
          if (c == 'A')
            cards.Add(Card.Ace);
          else if (c == 'K')
            cards.Add(Card.King);
          else if (c == 'Q')
            cards.Add(Card.Queen);
          else if (c == 'J')
          {
            if (part1)
              cards.Add(Card.Jack);
            else
              cards.Add(Card.Joker);
          }
          else if (c == 'T')
            cards.Add(Card.Ten);
          else
          {
            cards.Add((Card)(c - '2' + (int)Card.Two));
          }
        }

        BuildSorted();
        CountCards(part1);
        Score(part1);
      }
      public void BuildSorted()
      {
        foreach (var card in cards)
          cardsSorted.Add(card);
        cardsSorted.Sort();
      }
      public void CountCards(bool part1)
      {
        for (var i = 0; i < (int)Card.Count; ++i)
          cardCounts.Add(0);

        foreach (var c in cards)
          ++cardCounts[(int)c];
      }
      public void Score(bool part1)
      {
        var numCounts = new List<int>();
        for (var i = 0; i < 5; ++i)
          numCounts.Add(0);

        var jokerCount = cardCounts[(int)Card.Joker];
        if (jokerCount != 0)
        {
          var maxValue = 0;
          var maxIndex = 0;
          for (var i = (int)Card.Two; i < (int)Card.Count; ++i)
          {
            if (cardCounts[i] > maxValue)
            {
              maxValue = cardCounts[i];
              maxIndex = i;
            }
          }
          cardCounts[(int)Card.Joker] -= jokerCount;
          cardCounts[maxIndex] += jokerCount;
        }
        for (var i = 0; i < cardCounts.Count; ++i)
        {
          var count = cardCounts[i];

          if (count == 0)
            continue;
          ++numCounts[(int)count - 1];
        }

        if (numCounts[4] >= 1)
          hand = HandType.FiveOfKind;
        else if (numCounts[3] >= 1)
          hand = HandType.FourOfAKind;
        else if (numCounts[2] >= 1)
        {
          if (numCounts[1] >= 1 || numCounts[2] >= 2)
            hand = HandType.FullHouse;
          else
            hand = HandType.ThreeOfAKind;
        }
        else if (numCounts[1] >= 2)
          hand = HandType.TwoPair;
        else if (numCounts[1] >= 1)
          hand = HandType.OnePair;
        else
          hand = HandType.HighCard;
      }
      public void Print()
      {
        foreach (var c in cards)
          Console.Write(c.ToString() + " ");
        Console.Write(bid);
        Console.WriteLine(" " + hand.ToString());
        Console.WriteLine();
      }

      public int CompareTo(Hand? other)
      {
        int h = hand.CompareTo(other!.hand);
        if (h != 0)
          return h;
        for (var i = 0; i < 5; ++i)
        {
          if (cards[i] > other.cards[i])
            return 1;
          else if (cards[i] < other.cards[i])
            return -1;
        }
        return 0;
      }
    }
    List<Hand> Parse(string[] lines, bool part1 = true)
    {
      var hands = new List<Hand>();
      foreach (var line in lines)
      {
        var hand = new Hand();
        hand.Parse(line, part1);
        hands.Add(hand);
      }
      return hands;

    }
  }
}
