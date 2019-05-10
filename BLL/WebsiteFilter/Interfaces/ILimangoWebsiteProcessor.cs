using HtmlAgilityPack;
using System.Threading.Tasks;

namespace  BLL.WebsiteFilter
{
    public interface ILimangoWebsiteProcessor
    {
        Task ProcessWebsite(HtmlDocument htmlDoc, decimal maximalCost);
    }
}