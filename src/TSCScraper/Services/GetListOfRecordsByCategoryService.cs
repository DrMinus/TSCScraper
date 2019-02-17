using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetListOfRecordsByCategoryService : IGetListOfRecordsByCategoryService
  {
    private readonly ChromeDriver _driver;
    private readonly string _url;
    private int _indexToStartLevelsFrom;

    public GetListOfRecordsByCategoryService(ChromeDriver driverP, string urlP)
    {
      _driver = driverP;
      _url = urlP;
    }

    public void GetListOfRecordsByCategory()
    {
      _driver.Navigate().GoToUrl(_url);

      var levelCount = GetLevelCount(_driver.FindElementsByTagName("tr"));

      for (var i = 0; i < levelCount; i++)
      {
        //this is the worst piece of code I have written
        try
        {
          _driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i + _indexToStartLevelsFrom}]/td[2]/a").Click();
        }
        catch (NoSuchElementException)
        {
          //handle multiple game characters in the markup by using the first td tag instead of the second
          _driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i + _indexToStartLevelsFrom}]/td[1]/a").Click();
        }

        var url = _driver.Url;
        var listOfRecords = new GetListOfRecordsForLevelService(url, _driver).GetListOfRecords();
        var csvContent = new GetCsvFromRecordListService(listOfRecords).GetCsvFromRecordList();
        new SaveCsvToFileService(csvContent, url).SaveCsvToFile();
        _driver.Navigate().Back();
      }
    }

    private int GetLevelCount(IReadOnlyList<IWebElement> webElementsP)
    {
      var isPartOfTotal = true;
      var levelCount = 0;
      for (var i = 0; i < webElementsP.Count; i++)
      {
        var levelData = webElementsP[i].Text;

        if (levelData == "")
        {
          continue;
        }

        if (levelData.StartsWith("Level Division") || levelData.StartsWith("Record Total") || levelData.StartsWith("Total"))
        {
          _indexToStartLevelsFrom++;
          continue;
        }

        if (isPartOfTotal && (levelData.StartsWith("Tails") || levelData.StartsWith("Knuckles")))
        {
          _indexToStartLevelsFrom++;
          continue;
        }

        levelCount++;
        isPartOfTotal = false;
      }

      return levelCount;
    }
  }
}
