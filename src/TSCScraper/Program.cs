using TSCScraper.Services;

namespace TSCScraper
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      //change this when debugging
      var url = "http://www.soniccenter.org/rankings/sonic_1/times/green_hill_1";

      var listOfRecords = new GetListOfRecordsService(url).GetListOfRecords();
      var csvContent = new GetCsvFromRecordListService(listOfRecords).GetCsvFromRecordList();
      new SaveCsvToFileService(csvContent, url).SaveCsvToFile();
    }
  }
}
