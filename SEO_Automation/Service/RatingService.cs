using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SEO_Automation.Model;

namespace SEO_Automation.Service
{
    public class RatingService : IRatingService
    {
        private ChromeDriver _chromeDriver;
        private const string HttpsWwwGoogleCom = "https://www.google.com";

        public RatingService()
        {
            Init(HttpsWwwGoogleCom);
        }

        public void Init(string searchWebsite)
        {
            var options = new ChromeOptions();
            options.AddArguments("--disable-gpu");

            _chromeDriver = new ChromeDriver(options);
            _chromeDriver.Navigate().GoToUrl(searchWebsite);
        }



        public Task<string> GetRanking(string keyword, string url)
        {
            return Task.Factory.StartNew(() =>
            {
                string[] words = keyword.Split(",");
                var rankingList = new List<int>();

                foreach (var word in words)
                {
                    try
                    {
                        // search by "q" name.
                        _chromeDriver.FindElement(By.Name("q")).SendKeys(word);
                        _chromeDriver.FindElement(By.Name("q")).SendKeys(Keys.Enter);

                        int ranking = 1;
                        bool searchResult;
                        do
                        {
                            searchResult = GetLink(ref ranking, url);
                            _chromeDriver.FindElementByXPath("//*[@id=\"pnnext\"]/span[2]").Click();
                        } while (!searchResult);

                        // Not Found case : 
                        ranking = (ranking == -1) ? 0 : ranking;
                        rankingList.Add(ranking);

                        //Clean text area to search for next element. 
                        _chromeDriver.FindElementByXPath("//*[@id=\"tsf\"]/div[2]/div[1]/div[2]/div/div[2]/input").Clear();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OOPS. BROKE MYSELF { e.Message}");
                        throw;
                    }
                }

                _chromeDriver.Close();
                string jsonResult = JsonConvert.SerializeObject(new JsonResult(keyword,
                    url,
                    rankingList),
                    Formatting.Indented);

                return jsonResult;
            });
        }

        private bool GetLink(ref int ranking, string url)
        {
            try
            {
                List<string> listOfLinks = new List<string>();
                var searchSites = _chromeDriver.FindElements(By.ClassName("TbwUpd"));
                foreach (var site in searchSites)
                {

                    try
                    {
                        if (ranking >= 100)
                        {
                            ranking = -1;
                            Console.WriteLine("Top 100 elements selected.");
                            return true;
                        }

                        var alt = site.FindElement(By.TagName("img")).GetAttribute("alt");
                        if (listOfLinks.Exists(x => string.Equals(x, alt)))
                        {
                            continue;
                        }

                        listOfLinks.Add(alt);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Continue with exception  ${e.Message}");
                    }
                }


                Console.WriteLine("***********************");
                foreach (var item in listOfLinks)
                {
                    Console.WriteLine($"Item of unique link = {item}");
                }
                Console.WriteLine("***********************");

                int distIndex = listOfLinks.FindIndex(link => link.Contains(url));

                Console.WriteLine($"Index of url {url} is {distIndex}");

                ranking = distIndex;
                if (distIndex != -1)
                {
                    //Found the ranking. Since FindIndex starts from 0 we need to increment by 1.
                    ranking++;
                    Console.WriteLine($"Found the URL at rank.  {ranking}");
                    return true;
                }
                else
                {
                    ranking = listOfLinks.Count;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Some Exception Caught ${e.Message}");
            }
            return false;
        }
    }
}
