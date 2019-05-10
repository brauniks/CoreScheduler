using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repoitory
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly PostgreDbContext context;

        public WebsiteRepository(PostgreDbContext context)
        {
            this.context = context;
        }

        public async Task AddWebsite(Websites website)
        {
            this.context.Websites.Add(website);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteWebsite(int id)
        {
            var website = await this.context.Websites.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (website != null)
            {
                this.context.Websites.Remove(website);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task EditWebsite(Websites website)
        {
            var web = await this.context.Websites.Where(x => x.Id == website.Id).FirstOrDefaultAsync();
            if (website != null)
            {
                web.Link = website.Link;
                web.Cost = website.Cost;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Websites>> GetWebites()
        {
            return await this.context.Websites.ToListAsync();
        }
    }
}
