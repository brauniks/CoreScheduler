using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repoitory
{
    public interface IWebsiteRepository
    {
        Task<IEnumerable<Websites>> GetWebites();
        Task DeleteWebsite(int id);
        Task AddWebsite(Websites website);
        Task EditWebsite(Websites website);
    }
}