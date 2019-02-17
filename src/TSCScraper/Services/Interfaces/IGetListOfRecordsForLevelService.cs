using System.Collections.Generic;
using TSCScraper.Models;

namespace TSCScraper.Services.Interfaces
{
  public interface IGetListOfRecordsForLevelService
  {
    List<Record> GetListOfRecords();
  }
}
