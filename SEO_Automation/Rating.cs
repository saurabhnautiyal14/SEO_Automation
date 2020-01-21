using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace SEO_Automation
{
    public class Rating
    {
        private readonly string keyword;
        private readonly string url;

        public Rating(string keyword, string url)
        {
            this.keyword = keyword;
            this.url = url;
        }

        public IDictionary<string, int> getRanking()
        {
            Console.WriteLine("Entered");
            var options = new ChromeOptions();
            options.AddArguments("--disable-gpu");

            var chromeDriver = new ChromeDriver(options);
            chromeDriver.Navigate().GoToUrl("https://www.google.com");

            string[] words = keyword.Split(",");
            var rankingDict = new Dictionary<string, int>();

            try
            {
                foreach (var word in words)
                {
                    // search by "q" name.
                    chromeDriver.FindElement(By.Name("q")).SendKeys(word);
                    chromeDriver.FindElement(By.Name("q")).SendKeys(OpenQA.Selenium.Keys.Enter);

                    int ranking = 1;
                    var listOfLinks = new List<string>();
                    bool searchResult = false;
                    do
                    {
                        searchResult = GetLink(ref chromeDriver, ref ranking, ref listOfLinks);
                        chromeDriver.FindElementByXPath("//*[@id=\"pnnext\"]/span[2]").Click();
                    } while (!searchResult);

                    // Not Found case : 
                    ranking = (ranking == -1) ? 0 : ranking;
                    rankingDict.Add(word, ranking);

                    //Clean text area to search for next element. 
                    chromeDriver.FindElementByXPath("//*[@id=\"tsf\"]/div[2]/div[1]/div[2]/div/div[2]/input").Clear();

                }
            }
            catch (Exception e)
            {

                Console.WriteLine("OOPS. BROKE MYSELF", e.Message);
                Console.WriteLine(System.Environment.StackTrace);
            }

            return rankingDict;
        }

        bool GetLink(ref ChromeDriver chromeDriver, ref int ranking, ref List<string> listOfLinks)
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


                // Console.WriteLine("***********************");
                // foreach (var item in listOfLinks)
                // {
                //     Console.WriteLine($"Item of unique link = {item}");
                // }
                // Console.WriteLine("***********************");

                int distIndex = listOfLinks.FindIndex(link => link.Contains(url));

                Console.WriteLine($"Index of url {url} is {distIndex}");

                ranking = distIndex;
                if (distIndex != -1)
                {
                    //Found the ranking.
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
