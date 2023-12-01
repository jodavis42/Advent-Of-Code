namespace AdventOfCode2015
{
  internal class Day02
  {
    public void Run1(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      uint total = 0u;
      foreach(var line in lines) 
      {
        var split = line.Split('x');
        uint length = uint.Parse(split[0]);
        uint width = uint.Parse(split[1]);
        uint height = uint.Parse(split[2]);
        uint lw = length * width;
        uint lh = length * height;
        uint wh = width * height;
        uint surfaceArea = 2 * (lw + lh + wh);
        uint minSide = Math.Min(lw, Math.Min(lh, wh));
        total += surfaceArea + minSide;
      }
      Console.WriteLine(total);
    }

    public void Run2(string filePath)
    {
      var lines = File.ReadAllLines(filePath);
      uint total = 0u;
      foreach (var line in lines)
      {
        var split = line.Split('x');
        uint length = uint.Parse(split[0]);
        uint width = uint.Parse(split[1]);
        uint height = uint.Parse(split[2]);
        total += length * width *height;
        if (length >= width && length >= height)
          total += 2u * (width + height);
        else if (width >= length && width >= height)
          total += 2u * (length + height);
        else if (height >= length && height >= width)
          total += 2u * (length + width);
        else
          throw new Exception();
      }
      Console.WriteLine(total);
    }
  }
}
