using System.Diagnostics;

namespace AdventOfCode2021
{
  public enum TokenType { Word, Number, Whitespace, Newline, BeginBrace, EndBrace, BeginBracket, EndBracket, Colon, Semicolon, Comma, Equal, Add, Subtract, Multiply, Divide, Arrow, Less, Greater}
  [DebuggerDisplay("{Type}: {Text}")]
  public class Token
  {
    public TokenType Type;
    public string Text = string.Empty;
    public int Start;
    public int End;
  }

  public class Tokenizer
  {
    public List<Token> Tokens = new List<Token>();
    public string mData = string.Empty;
    public Dictionary<char, TokenType> SingleTokens = new Dictionary<char, TokenType>
    {
      { ',', TokenType.Comma },
      { ':', TokenType.Colon },
      { ';', TokenType.Semicolon },
      { '=', TokenType.Equal },
      { '+', TokenType.Add },
      { '-', TokenType.Subtract },
      { '*', TokenType.Multiply },
      { '/', TokenType.Divide },
      { '{', TokenType.BeginBrace },
      { '}', TokenType.EndBrace },
      { '[', TokenType.BeginBracket },
      { ']', TokenType.EndBracket },
      { '<', TokenType.Less },
      { '>', TokenType.Greater },
    };
    public Dictionary<string, TokenType> MultiTokens = new Dictionary<string, TokenType>
    {
      { "->", TokenType.Arrow},
    };

    public void Tokenize(string data)
    {
      mData = data;
      Parse();
    }
    void Parse()
    {
      int start = 0;
      int end = 0;
      while (end < mData.Length)
      {
        end = ParseNext(start);
        start = end;
      }
    }

    int ParseNext(int start)
    {
      int end = start;
      var token = new Token();
      if (ParseWhitespace(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }
      else if(ParseNewline(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }
      else if (ParseMultiToken(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }
      else if(ParseSingleToken(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }
      else if (ParseNumber(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }
      else if (ParseWord(start, ref end, ref token))
      {
        Tokens.Add(token);
        return end;
      }

      return end;
    }

    bool ParseWhitespace(int start, ref int end, ref Token token)
    {
      end = start;
      while (CanRead(end) && IsWhitespace(mData[end]))
        ++end;

      bool success = (end > start);
      if (success)
        token = BuildToken(start, end, TokenType.Whitespace);
      return success;
    }
    bool ParseNewline(int start, ref int end, ref Token token)
    {
      end = start;
      while (CanRead(end) && IsNewline(mData[end]))
        ++end;

      bool success = (end > start);
      if (success)
        token = BuildToken(start, end, TokenType.Newline);
      return success;
    }

    bool ParseSingleToken(int start, ref int end, ref Token token)
    {
      if (!CanRead(start) || !SingleTokens.TryGetValue(mData[start], out var tokenType))
        return false;

      end = start + 1;
      token = BuildToken(start, end, tokenType);
      return true;
    }

    bool ParseMultiToken(int start, ref int end, ref Token token)
    {
      foreach(var pair in MultiTokens)
      {
        if (ParseMultiToken(pair.Key, pair.Value, start, ref end, ref token))
          return true;
      }
      return false;
    }

    bool ParseMultiToken(string text, TokenType tokenType, int start, ref int end, ref Token token)
    {
      for(var i = 0; i < text.Length; ++i)
      {
        var index = i + start;
        if (!CanRead(end))
          return false;

        if (mData[index] != text[i])
          return false;
      }
      end = start + text.Length;
      token = BuildToken(start, end, tokenType);
      return true;
    }

    bool ParseNumber(int start, ref int end, ref Token token)
    {
      if (!CanRead(start) || !IsNumeric(mData[start]))
        return false;

      end = start + 1;
      while (CanRead(end) && IsNumeric(mData[end]))
        ++end;

      token = BuildToken(start, end, TokenType.Number);
      return true;
    }

    bool ParseWord(int start, ref int end, ref Token token)
    {
      if (!CanRead(start) || !IsAlpha(mData[start]))
        return false;

      end = start + 1;
      while (CanRead(end) && IsAlphaNumeric(mData[end]))
        ++end;

      token = BuildToken(start, end, TokenType.Word);
      return true;
    }

    Token BuildToken(int start, int end, TokenType type)
    {
      return new Token
      {
        Text = mData.Substring(start, end - start),
        Type = type,
        Start = start,
        End = end,
      };
    }

    bool IsWhitespace(char value)
    {
      return value == ' ' ||
             value == '\t';
    }
    bool IsNewline(char value)
    {
      return value == '\n' ||
             value == '\r';
    }

    bool IsAlpha(char value)
    {
      return ('a' <= value && value <= 'z') || ('A' <= value && value <= 'Z');
    }

    bool IsNumeric(char value)
    {
      return ('0' <= value && value <= '9');
    }

    bool IsAlphaNumeric(char value)
    {
      return IsAlpha(value) || IsNumeric(value);
    }

    bool CanRead(int position)
    {
      return position < mData.Length;
    }
  }
}
