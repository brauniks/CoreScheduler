namespace  BLL.WebsiteFilter
{
    using DAL.Model;
    using DAL.Repoitory;
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;
    using MyScheduler.App.Tools.Email;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class LimangoWebsiteProcessor : ILimangoWebsiteProcessor
    {
        private readonly IEmail email;
        private readonly IProcessRepository hash;
        private ILogger logger;

        public LimangoWebsiteProcessor(IEmail email, IProcessRepository hash, Microsoft.Extensions.Logging.ILoggerFactory DepLoggerFactory)
        { 
            this.email = email;
            this.hash = hash; 
            this.logger = DepLoggerFactory.CreateLogger("Controllers.TestController");
        }

        public async Task ProcessWebsite(HtmlDocument htmlDoc, decimal maximalCost)
        {
            try
            {
                IEnumerable<HtmlNode> htmlDocumentNode = GetElementText(htmlDoc.DocumentNode, "product-content");

                if (htmlDocumentNode != null)
                {
                    foreach (HtmlNode item in htmlDocumentNode)
                    {
                        IEnumerable<HtmlNode> hajs = GetElementText(item, "salesPrice");
                        decimal amount = GetAmount(hajs);
                        if (IfAmountFitted(amount, maximalCost))
                        {

                            string zdjecie = GetImage(item);
                            string zdj_src = StringIsDataUri(zdjecie.ToString());
                            string hrefLink = GetHrefLink(item);
                            IEnumerable<HtmlNode> nazwa = GetElementText(item, "product-title");

                            string rozmiar = GetSize(item);
                            if (!await hash.GetHashFromDatabase(nazwa.First().InnerText, rozmiar, amount.ToString(), zdj_src, hrefLink))
                            {
                                logger.LogInformation((nazwa.First().InnerText + "  " + rozmiar + "  " + amount.ToString() + "  " + zdj_src + "  " + hrefLink));
                                ProcessSuccessResult(zdj_src, hrefLink, nazwa, rozmiar, amount);
                            }
                        }
                    }

                }
                else
                {
                    logger.LogError("Jest nullem!!!" + Environment.NewLine + htmlDoc.DocumentNode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error ! "  + ex.ToString());
                throw;
            }
        }

        public IEnumerable<HtmlNode> GetElementText(HtmlNode htmlDoc, string nodeName)
        {
            return htmlDoc.Descendants("div")
                        .Where(d =>
                           d.Attributes.Contains("class")
                           &&
                           d.Attributes["class"].Value.Contains(nodeName)
                        );
        }

        public void ProcessSuccessResult(string zdj_src, string hrefLink, IEnumerable<HtmlNode> nazwa, string rozmiar, decimal amount)
        {
            Console.WriteLine($"[{DateTime.Now}]  udalo sie!!!!");
            LimangoProduct processingProduct = new LimangoProduct() { Name = nazwa.First().InnerText, Cost = amount, Size = rozmiar };
            Console.WriteLine(processingProduct.ToString());

            string htmlBody = @"<!DOCTYPE html>
                                <html>
                                <body>
                                <h1>" + processingProduct.Name + @"</h1>
                                <p>" + rozmiar + "   za: " + processingProduct.Cost + " zł" + @"</p>
                                <img src=" + zdj_src + @"><br>
                                <a href=""" + hrefLink + @"""> Link do strony </a>
                                </ body >
                                </ html > ";

            logger.LogInformation("Sending email \n" + processingProduct.ToString());
            email.SendEmail(processingProduct.Name, htmlBody);
        }

        public bool IfAmountFitted(decimal amount, decimal maximalCost)
        {
            return amount < maximalCost;
        }

        public static decimal GetAmount(IEnumerable<HtmlNode> hajs)
        {
            return decimal.Parse(hajs.First().InnerText.Replace(" zł", "").Replace("od", "").Replace(" ", "").Replace(".", ""));
        }

        public string GetSize(HtmlNode item)
        {
            return new string(GetElementText(item, "product-availability").First().InnerText.Replace("Czy jeszcze dostępne?", "").Replace(" ", "").Where(c => !char.IsControl(c)).ToArray());
        }

        public string GetHrefLink(HtmlNode item)
        {
            return @"https://www.limango-outlet.pl" + item.Descendants("a").Select(h => h.GetAttributeValue("href", "")).Where(h => h.Length > 3).FirstOrDefault().ToString();
        }

        public string GetImage(HtmlNode item)
        {
            return item.Descendants("img")
                .Where(p => p.ParentNode.Name == "noscript").FirstOrDefault().OuterHtml;
        }

        /// <summary>
        /// The StringIsDataUri
        /// </summary>
        /// <param name="stringToTest">The <see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string StringIsDataUri(string text)
        {
            string pattern = @"(https:\/\/.*)""";
            MatchCollection matches = Regex.Matches(text, pattern);

            return matches[0].ToString();
        }

    } 
}
