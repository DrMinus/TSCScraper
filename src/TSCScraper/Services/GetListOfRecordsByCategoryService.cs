using OpenQA.Selenium.Chrome;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetListOfRecordsByCategoryService : IGetListOfRecordsByCategoryService
  {
    private readonly ChromeDriver _driver;
    private readonly string _url;
    private const int INDEX_TO_START_LEVELS_FROM = 4;

    public GetListOfRecordsByCategoryService(ChromeDriver driverP, string urlP)
    {
      _driver = driverP;
      _url = urlP;
    }

    public void GetListOfRecordsByCategory()
    {
      _driver.Navigate().GoToUrl(_url);

      var levels = _driver.FindElementsByTagName("tr");
      var levelCount = 0;
      foreach (var level in levels)
      {
        var levelData = level.Text;
        if (levelData == "" || levelData.StartsWith("Level Division") || levelData.StartsWith("Record Total") || levelData.StartsWith("Total"))
        {
          continue;
        }

        levelCount++;
      }

      for (var i = 0; i < 5; i++)
      {
        _driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i + INDEX_TO_START_LEVELS_FROM}]/td[2]/a").Click();
        var url = _driver.Url;
        var listOfRecords = new GetListOfRecordsForLevelService(url, _driver).GetListOfRecords();
        var csvContent = new GetCsvFromRecordListService(listOfRecords).GetCsvFromRecordList();
        new SaveCsvToFileService(csvContent, url).SaveCsvToFile();
        _driver.Navigate().Back();
      }
    }
  }
}
