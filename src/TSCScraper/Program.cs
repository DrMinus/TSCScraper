using System;
using TSCScraper.Services;

namespace TSCScraper
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      //change this when debugging
      var url = "http://www.soniccenter.org/rankings/sonic_1/times/green_hill_1";

      for (var i = 0; i < args.Length; i++)
      {
        if (args[i] == "-url")
        {
          url = args[i + 1];
        }
      }

      if (!url.Contains("soniccenter.org"))
      {
        throw new ArgumentException("can only be used against soniccenter.org!");
      }

      var listOfRecords = new GetListOfRecordsService(url).GetListOfRecords();
      var csvContent = new GetCsvFromRecordListService(listOfRecords).GetCsvFromRecordList();
      new SaveCsvToFileService(csvContent, url).SaveCsvToFile();
    }
  }
}
