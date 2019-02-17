using System.Text;

namespace TSCScraper.Extensions
{
  public static class StringExtensions
  {
    public static string ToCsvCell(this string stringToConvertP)
    {
      var mustQuote = (stringToConvertP.Contains(",") || stringToConvertP.Contains("\"") || stringToConvertP.Contains("\r") || stringToConvertP.Contains("\n"));
      if (!mustQuote)
      {
        return stringToConvertP;
      }

      var sb = new StringBuilder();
      sb.Append("\"");
      foreach (var nextChar in stringToConvertP)
      {
        sb.Append(nextChar);
        if (nextChar != '"')
        {
          continue;
        }

        sb.Append("\"");
      }
      sb.Append("\"");
      return sb.ToString();
    }
  }
}
