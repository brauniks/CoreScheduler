using DAL.Model;
using HtmlAgilityPack; 

namespace BLL.Interfaces
{
  public  interface IWebsiteConfig
    {
        string Website { get; set; } 

         decimal MaximalCost { get; set; }

        HtmlDocument htmlDoc { get; set; }
        LimangoProduct processingProduct { get; set; } 

    }
}
