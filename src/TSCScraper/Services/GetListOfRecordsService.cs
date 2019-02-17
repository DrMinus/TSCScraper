using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TSCScraper.Models;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetListOfRecordsService : IGetListOfRecordsService
  {
    private const int MAIN_PAGE_PLAYER_INDEX = 1;
    private const int MAIN_PAGE_STAT_INDEX = 2;
    private const int MAIN_PAGE_DATE_INDEX = 3;

    private const int START_TABLE_FROM_INDEX = 2;

    private const int PLAYER_STAT_INDEX = 0;
    private const int PLAYER_DATE_INDEX = 2;
    private const int PLAYER_COMMENT_INDEX = 3;

    private const int URL_PLAYER_NAME_INDEX = 4;

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
        //driver.Manage().Window.Minimize();
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
          //          var rowData = records[i].Text.Split(Environment.NewLine);
          //
          //          var currentRecord = new Record
          //          {
          //            Player = rowData[MAIN_PAGE_PLAYER_INDEX],
          //            Stat = rowData[MAIN_PAGE_STAT_INDEX],
          //            Date = rowData[MAIN_PAGE_DATE_INDEX]
          //          };
          //
          //          try
          //          {
          //            var commentElement = driver.FindElementByXPath($"//*[@id=\"content\"]/table/tbody/tr[2]/td[2]/table/tbody/tr[{i + START_TABLE_FROM_INDEX}]/td[3]/a/span");
          //            if (commentElement.GetAttribute("title") != "")
          //            {
          //              currentRecord.Comment = commentElement.GetAttribute("title");
          //
          //              if (currentRecord.Comment.Contains("youtu") || currentRecord.Comment.Contains("twitch"))
          //              {
          //                currentRecord.HasProof = true;
          //              }
          //            }
          //          }
          //          catch (NoSuchElementException)
          //          {
          //          }
          //
          //          listOfRecords.Add(currentRecord);

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

            for (var a = 3; a < historyRowData.Length; a++)
            {
              comment += historyRowData[a] + " ";
            }

            if (historyRowData.Length < 3)
            {
              continue;
            }

            listOfRecords.Add(new Record
            {
              Player = driver.Url.Split("/")[4],
              Stat = historyRowData[0],
              Date = historyRowData[2],
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
