using TSCScraper.Services;

namespace TSCScraper
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      //change this when debugging
      const string gameUrl = "http://www.soniccenter.org/rankings/sonic_1/times";
      var driver = new InitDriverService().InitChromeDriver();
      new GetListOfRecordsByCategoryService(driver, gameUrl).GetListOfRecordsByCategory();
      driver.Dispose();
    }
  }
}
