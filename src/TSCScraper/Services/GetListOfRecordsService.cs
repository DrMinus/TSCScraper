using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TSCScraper.Models;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetListOfRecordsService : IGetListOfRecordsService
  {
    private const int START_INDEX_FOR_COMMENT = 3;
    private const int URL_PLAYER_INDEX = 4;
    private const int RECORD_ROW_STAT_INDEX = 0;
    private const int RECORD_ROW_DATE_INDEX = 2;
    private const int ROWS_TO_COUNT_AHEAD_WHEN_GETTING_XPATH = 2;

    private readonly string _url;
    private readonly ChromeDriver _driver;

    public GetListOfRecordsService(string urlP, ChromeDriver driverP)
    {
      _url = urlP;
      _driver = driverP;
    }

    public List<Record> GetListOfRecords()
    {
      var listOfRecords = new List<Record>();

      _driver.Navigate().GoToUrl(_url);

      var recordsCount = GetRecordCount(_driver.FindElementsByTagName("tr"));

      for (var i = 0; i < recordsCount; i++)
      {
        _driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i + ROWS_TO_COUNT_AHEAD_WHEN_GETTING_XPATH}]/td[3]/a").Click();

        var historyRecordsAsList = GetListOfRecordText(_driver.FindElementsByTagName("tr"));

        for (var j = 0; j < historyRecordsAsList.Length; j++)
        {
          var historyRowData = historyRecordsAsList[j].Split(" ");
          var comment = GetCommentFromHistoryRowData(historyRowData);

          listOfRecords.Add(new Record
          {
            Player = _driver.Url.Split("/")[URL_PLAYER_INDEX],
            //stat and date require some validation from bad data (yay)
            Stat = !historyRowData[RECORD_ROW_STAT_INDEX].Contains("/") ? historyRowData[RECORD_ROW_STAT_INDEX] : listOfRecords[listOfRecords.Count-1].Stat,
            Date = historyRowData[RECORD_ROW_DATE_INDEX].Contains("-") ? historyRowData[RECORD_ROW_DATE_INDEX] : historyRowData[RECORD_ROW_DATE_INDEX-1],
            Comment = comment,
            HasProof = comment.Contains("youtube.com") || comment.Contains("twitch.tv") || comment.Contains("youtu.be") 
          });
        }

        listOfRecords[listOfRecords.Count - 1].IsMostRecent = true;
        _driver.Navigate().Back();
      }

      return listOfRecords;
    }

    private static int GetRecordCount(IReadOnlyList<IWebElement> webElementsP)
    {
      var numberOfRecords = 0;

      for (var i = 0; i < webElementsP.Count; i++)
      {
        var webElementText = webElementsP[i].Text;
        if (webElementText == "" || webElementText.StartsWith("Rank Player Stat Date"))
        {
          continue;
        }
        numberOfRecords++;
      }

      return numberOfRecords;
    }

    private static string[] GetListOfRecordText(IReadOnlyList<IWebElement> webElementsP)
    {
      var list = new List<string>();

      for (var i = 0; i < webElementsP.Count; i++)
      {
        var webElementText = webElementsP[i].Text;
        if (webElementText == "" || webElementText.StartsWith("Stat") || webElementText.Split(" ").Length < 3)
        {
          continue;
        }
        list.Add(webElementText);
      }

      return list.ToArray();
    }

    private static string GetCommentFromHistoryRowData(IReadOnlyList<string> historyRowDataP)
    {
      var commentStringBuilder = new StringBuilder();

      for (var i = START_INDEX_FOR_COMMENT; i < historyRowDataP.Count; i++)
      {
        commentStringBuilder.Append(historyRowDataP[i] + " ");
      }

      return commentStringBuilder.ToString();
    }
  }
}
