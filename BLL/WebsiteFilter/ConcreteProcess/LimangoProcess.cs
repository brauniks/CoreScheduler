namespace MyScheduler.App.BLL.WebsiteFilter
{
    using DAL.Model;
    using global::BLL.Interfaces;
    using global::BLL.WebsiteFilter;
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="LimangoProcess" />
    /// </summary>
    public partial class LimangoProcess : IWebsiteConfig, ILimangoProcess
    {
        private readonly ILimangoWebsiteProcessor websiteProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimangoProcess"/> class.
        /// </summary>
        public LimangoProcess(HtmlWeb htmlWeb, ILogger<LimangoProcess> logger, ILimangoWebsiteProcessor websiteProcessor)
        { 
            Logger = logger;
            this.websiteProcessor = websiteProcessor;
            web = htmlWeb; 
        }

        /// <summary>
        /// Gets or sets the MyProductCostList
        /// </summary>
        public  decimal MaximalCost { get; set; } 

        /// <summary>
        /// Gets or sets the Website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the web
        /// </summary>
        public HtmlWeb web { get; set; }

        /// <summary>
        /// Gets or sets the htmlDoc
        /// </summary>
        public HtmlDocument htmlDoc { get; set; }
         
        public ILogger Logger { get; } 

        public LimangoProduct processingProduct { get; set; }

        public void Process(string link, decimal cost)
        {
            this.Logger.LogInformation("poczatek");
            this.Website = link;
            this.MaximalCost = cost;

            htmlDoc = web.Load(Website);

            this.websiteProcessor.ProcessWebsite(htmlDoc, this.MaximalCost);
        } 
    }
}
