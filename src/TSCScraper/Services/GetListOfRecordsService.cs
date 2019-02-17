using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium.Chrome;
using TSCScraper.Models;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetListOfRecordsService : IGetListOfRecordsService
  {
    private const int START_INDEX_FOR_RECORD_LIST = 3;
    private const int URL_PLAYER_INDEX = 4;
    private const int RECORD_ROW_STAT_INDEX = 0;
    private const int RECORD_ROW_DATE_INDEX = 2;

    private readonly string _url;
    public GetListOfRecordsService(string urlP)
    {
      _url = urlP;
    }

    public IEnumerable<Record> GetListOfRecords()
    {
      var listOfRecords = new List<Record>();
      using (var driver = new ChromeDriver($"{Directory.GetCurrentDirectory()}", new ChromeOptions()))
      {
        driver.Navigate().GoToUrl(_url);

        var records = driver.FindElementsByTagName("tr");
        var recordsAsList = new List<string>();

        foreach (var record in records)
        {
          recordsAsList.Add(record.Text);
        }

        for (var i = 0; i < recordsAsList.Count; i++)
        {
          if (recordsAsList[i] == "" || recordsAsList[i].StartsWith("Rank Player Stat Date"))
          {
            continue;
          }

          driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i - 1}]/td[3]/a").Click();
          var historyRecords = driver.FindElementsByTagName("tr").ToList();
          foreach (var record in historyRecords)
          {
            if (record.Text == "" || record.Text.StartsWith("Stat"))
            {
              continue;
            }

            var historyRowData = record.Text.Split(" ");
            var comment = "";

            for (var a = START_INDEX_FOR_RECORD_LIST; a < historyRowData.Length; a++)
            {
              comment += historyRowData[a] + " ";
            }

            if (historyRowData.Length < 3)
            {
              continue;
            }

            listOfRecords.Add(new Record
            {
              Player = driver.Url.Split("/")[URL_PLAYER_INDEX],
              Stat = historyRowData[RECORD_ROW_STAT_INDEX],
              Date = historyRowData[RECORD_ROW_DATE_INDEX],
              Comment = comment,
              HasProof = comment.Contains("youtu") || comment.Contains("twitch"),
            });
          }

          listOfRecords[listOfRecords.Count - 1].IsMostRecent = true;
          driver.Navigate().Back();
        }

        return listOfRecords;
      }
    }
  }
}
