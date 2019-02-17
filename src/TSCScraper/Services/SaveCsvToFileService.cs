using System.IO;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class SaveCsvToFileService : ISaveCsvToFileService
  {
    private const int LEVEL_TITLE_INDEX = 6;
    private const int GAME_CHARACTER_INDEX = 7;
    private const int RECORD_CATEGORY_INDEX = 5;
    private const int GAME_NAME_INDEX = 4;
    private readonly string _csvContent;
    private readonly string _url;
    
    public SaveCsvToFileService(string csvContentP, string urlP)
    {
      _csvContent = csvContentP;
      _url = urlP;
    }

    public void SaveCsvToFile()
    {
      var splitUrl = _url.Split("/");
      var filePath = $@"{Directory.GetCurrentDirectory()}\{splitUrl[GAME_NAME_INDEX]}\{splitUrl[RECORD_CATEGORY_INDEX]}\{splitUrl[GAME_CHARACTER_INDEX]}_{splitUrl[LEVEL_TITLE_INDEX]}.csv";
      var file = new FileInfo(filePath);
      file.Directory?.Create();
      File.WriteAllText(filePath, _csvContent);
    }
  }
}
