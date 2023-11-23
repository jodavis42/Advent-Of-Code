public class Logger
{
  public int Indentation = 0;
  public void Write(string text)
  {
    WriteIndentation();
    Console.Write(text);
  }
  public void WriteLine(string text)
  {
    WriteIndentation();
    Console.WriteLine(text);
  }
  public void WriteLine() => Console.WriteLine();
  public void Indent() => ++Indentation;
  public void Deindent() => --Indentation;

  public void WriteIndentation()
  {
    for (var i = 0; i < Indentation; i++)
      Console.Write("  ");
  }
}
