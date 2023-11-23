namespace AdventOfCode2021
{
  public class BaseParser
  {
    public Tokenizer? Tokenizer;
    public int TokenIndex = 0;

    public void Parse(string text)
    {
      TokenIndex = 0;
      Tokenizer = new Tokenizer();
      Tokenizer.Tokenize(text);
      Run();
    }

    public virtual void Run() { }

    public bool Accept(TokenType tokenType, out Token? token)
    {
      SkipWhitespaceOrNewline();
      if (!TryGetToken(out token))
        return false;

      if (token != null && token.Type == tokenType)
      {
        ++TokenIndex;
        return true;
      }
      return false;
    }
    public bool Accept(TokenType tokenType) => Accept(tokenType, out var token);
    public bool Accept(TokenType tokenType, string text, out Token? token)
    {
      SkipWhitespaceOrNewline();
      if (!TryGetToken(out token))
        return false;

      if (token != null && token.Type == tokenType && token.Text == text)
      {
        ++TokenIndex;
        return true;
      }
      return false;
    }
    public bool Accept(TokenType tokenType, string text) => Accept(tokenType, text, out var token);


    public void Expect(bool result)
    {
      if (result == false)
        throw new Exception();
    }

    public void Expect(TokenType tokenType, out Token? token)
    {
      Expect(Accept(tokenType, out token));
    }
    public void Expect(TokenType tokenType) => Expect(Accept(tokenType));
    public void Expect(TokenType tokenType, string text, out Token? token) => Expect(Accept(tokenType, text, out token));
    public void Expect(TokenType tokenType, string text) => Expect(Accept(tokenType, text));

    public void SkipWhitespaceOrNewline()
    {
      while (TryGetToken(out var token))
      {
        if (token!.Type == TokenType.Newline || token.Type == TokenType.Whitespace)
        {
          ++TokenIndex;
          continue;
        }
        break;
      }
    }
    public bool CanRead() => TokenIndex < Tokenizer!.Tokens.Count;
    public bool TryGetToken(out Token? token)
    {
      token = null;
      if (CanRead())
      {
        token = Tokenizer!.Tokens[TokenIndex];
        return true;
      }
      return false;
    }
  }
}
