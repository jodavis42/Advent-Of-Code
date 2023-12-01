namespace AdventOfCode2015
{
  internal class Day04
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
      {
        for (uint i = 0; i < uint.MaxValue; ++i)
        {
          var text = lines[0] + i.ToString();
          byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
          byte[] hashBytes = md5.ComputeHash(inputBytes);
          string result = Convert.ToHexString(hashBytes); // .NET 5 +
          var leadingZeroes = CountLeadingZeros(result);
          if(leadingZeroes >= 5)
          {
            Console.WriteLine(i);
            break;
          }
        }
      }
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath); 
      using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
      {
        for (uint i = 0; i < uint.MaxValue; ++i)
        {
          var text = lines[0] + i.ToString();
          byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
          byte[] hashBytes = md5.ComputeHash(inputBytes);
          string result = Convert.ToHexString(hashBytes); // .NET 5 +
          var leadingZeroes = CountLeadingZeros(result);
          if (leadingZeroes >= 6)
          {
            Console.WriteLine(i);
            break;
          }
        }
      }
    }

    public uint CountLeadingZeros(string hash)
  {
    var leadingZeroes = 0u;
    for (var i = 0; i < hash.Length; ++i)
    {
      if (hash[i] != '0')
        break;
      ++leadingZeroes;
    }
    return leadingZeroes;
  }
  }
}
