using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Repoitory;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyScheduler.App.Tools.Email;

namespace Schedule.WebApiCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IWebsiteRepository websiteRepository;
        private readonly IEmail email;
        private ILogger logger;

        public ValuesController(IWebsiteRepository websiteRepository, IEmail email, Microsoft.Extensions.Logging.ILoggerFactory DepLoggerFactory)
        {
            this.websiteRepository = websiteRepository;
            this.email = email;
            this.logger = DepLoggerFactory.CreateLogger("Controllers.TestController");
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                this.email.SendEmail("testowe", "TEST TEST");
                this.logger.LogInformation("Testowow");
                var values = await this.websiteRepository.GetWebites();

                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Websites website)
        {
            try
            {
                 await websiteRepository.EditWebsite(website);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Websites website)
        {
            try
            {
                await this.websiteRepository.AddWebsite(website);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                 await this.websiteRepository.DeleteWebsite(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
