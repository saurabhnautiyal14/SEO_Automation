using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SEO_Automation
{
    public class Rating
    {
        private readonly string keyword;
        private readonly string url;
        private ChromeDriver chromeDriver;
        bool disposed = false;

        public Rating(string keyword, string url)
        {
            this.keyword = keyword;
            this.url = url;
            Init("https://www.google.com");
        }

        public void Init(string searchWebsite)
        {
            var options = new ChromeOptions();
            options.AddArguments("--disable-gpu");

            chromeDriver = new ChromeDriver(options);
            chromeDriver.Navigate().GoToUrl(searchWebsite);
        }

        class JSONResult
        {
            public string searchString;
            public string url;
            public List<int> ranking;

            public JSONResult(string searchString, string url, List<int> ranking)
            {
                this.searchString = searchString;
                this.url = url;
                this.ranking = ranking;
            }
        }

        public string getRanking()
        {

            string[] words = keyword.Split(",");
            var rankingList = new List<int>();

            foreach (var word in words)
            {
                try
                {
                    // search by "q" name.
                    chromeDriver.FindElement(By.Name("q")).SendKeys(word);
                    chromeDriver.FindElement(By.Name("q")).SendKeys(OpenQA.Selenium.Keys.Enter);

                    int ranking = 1;
                    var listOfLinks = new List<string>();
                    bool searchResult = false;
                    do
                    {
                        searchResult = GetLink(ref ranking, ref listOfLinks);
                        chromeDriver.FindElementByXPath("//*[@id=\"pnnext\"]/span[2]").Click();
                    } while (!searchResult);

                    // Not Found case : 
                    ranking = (ranking == -1) ? 0 : ranking;
                    rankingList.Add(ranking);

                    //Clean text area to search for next element. 
                    chromeDriver.FindElementByXPath("//*[@id=\"tsf\"]/div[2]/div[1]/div[2]/div/div[2]/input").Clear();
                }
                catch (Exception e)
                {

                    Console.WriteLine("OOPS. BROKE MYSELF", e.Message);
                }
            }

            chromeDriver.Close();
            string jsonResult = JsonConvert.SerializeObject(new JSONResult(keyword, url, rankingList), Formatting.Indented);
            return jsonResult;
        }

        bool GetLink(ref int ranking, ref List<string> listOfLinks)
        {
            try
            {
                var searchSites = chromeDriver.FindElements(By.ClassName("TbwUpd"));
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
                    Console.WriteLine("Found the URL at rank.", ranking);
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
