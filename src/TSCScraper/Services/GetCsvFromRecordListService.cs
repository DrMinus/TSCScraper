using System;
using System.Collections.Generic;
using System.Text;
using TSCScraper.Extensions;
using TSCScraper.Models;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class GetCsvFromRecordListService : IGetCsvFromRecordList
  {
    private readonly List<Record> _listOfRecords;
    public GetCsvFromRecordListService(List<Record> listOfRecordsP)
    {
      _listOfRecords = listOfRecordsP;
    }
    
    public string GetCsvFromRecordList()
    {
      var csvStringBuilder = new StringBuilder();
      csvStringBuilder.Append($"Player,Stat,Date,Comment,Has Proof,Is Most Recent{Environment.NewLine}");

      for (var i = 0; i < _listOfRecords.Count; i++)
      {
        var record = _listOfRecords[i];
        csvStringBuilder.Append($"{record.Player.ToCsvCell()},{record.Stat.ToCsvCell()},{record.Date.ToCsvCell()},{record.Comment.ToCsvCell() ?? ""},{record.HasProof.ToString().ToCsvCell()},{record.IsMostRecent.ToString().ToCsvCell()}{Environment.NewLine}");
      }

      return csvStringBuilder.ToString();
    }
  }
}
