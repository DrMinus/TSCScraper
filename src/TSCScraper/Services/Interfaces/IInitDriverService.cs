using OpenQA.Selenium.Chrome;

namespace TSCScraper.Services.Interfaces
{
  public interface IInitDriverService
  {
    ChromeDriver InitChromeDriver();
  }
}
