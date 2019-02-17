namespace TSCScraper.Models
{
  public class Record
  {
    public string Player { get; set; }
    public string Stat { get; set; }
    public string Date { get; set; }
    public string Comment { get; set; }
    public bool HasProof { get; set; }
    public bool IsMostRecent { get; set; }
  }
}
