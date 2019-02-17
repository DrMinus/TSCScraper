using System.IO;
using OpenQA.Selenium.Chrome;
using TSCScraper.Services.Interfaces;

namespace TSCScraper.Services
{
  public class InitDriverService : IInitDriverService
  {
    public ChromeDriver InitChromeDriver()
    {
      return new ChromeDriver($"{Directory.GetCurrentDirectory()}", new ChromeOptions());
    }
  }
}
