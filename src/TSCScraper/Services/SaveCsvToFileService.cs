using System.IO;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class SaveCsvToFileService : ISaveCsvToFileService
  {
    private const int LEVEL_TITLE_INDEX = 6;
    private readonly string _csvContent;
    private readonly string _url;
    
    public SaveCsvToFileService(string csvContentP, string urlP)
    {
      _csvContent = csvContentP;
      _url = urlP;
    }

    public void SaveCsvToFile()
    {
      File.WriteAllText($@"{Directory.GetCurrentDirectory()}\{_url.Split("/")[LEVEL_TITLE_INDEX]}.csv", _csvContent);
    }
  }
}
