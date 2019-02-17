using TSCScraper.Services;

namespace TSCScraper
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      //change this when debugging
      const string url = "http://www.soniccenter.org/rankings/sonic_1/times/green_hill_1";

      var driver = new InitDriverService().InitChromeDriver();
      var listOfRecords = new GetListOfRecordsService(url, driver).GetListOfRecords();
      driver.Dispose();
      var csvContent = new GetCsvFromRecordListService(listOfRecords).GetCsvFromRecordList();
      new SaveCsvToFileService(csvContent, url).SaveCsvToFile();
    }
  }
}
