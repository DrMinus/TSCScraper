using System;
using System.Collections.Generic;
using System.Linq;
using TSCScraper.Models;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetCsvFromRecordListService : IGetCsvFromRecordList
  {
    private readonly IEnumerable<Record> _listOfRecords;
    public GetCsvFromRecordListService(IEnumerable<Record> listOfRecordsP)
    {
      _listOfRecords = listOfRecordsP;
    }
    
    public string GetCsvFromRecordList()
    {
      var csv = $"Player,Stat,Date,Comment,Has Proof,Is Most Recent{Environment.NewLine}";

      return _listOfRecords
        .Aggregate(csv,
          (current,
              record) =>
            current +
            $"\"{record.Player}\",\"{record.Stat}\",\"{record.Date}\",\"{record.Comment.Replace("\"", "\"\"") ?? ""}\",\"{record.HasProof}\",\"{record.IsMostRecent}\"{Environment.NewLine}");
    }
  }
}
