using System.Collections.Generic;
using TSCScraper.Models;

namespace TSCScraper.Services.Interfaces
{
  public interface IGetListOfRecordsService
  {
    IEnumerable<Record> GetListOfRecords();
  }
}
