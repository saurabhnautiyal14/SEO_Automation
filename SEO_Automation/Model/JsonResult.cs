using System.Collections.Generic;

namespace SEO_Automation.Model
{
    class JsonResult
    {
        public string SearchString;
        public string Url;
        public List<int> Ranking;

        public JsonResult(string searchString, string url, List<int> ranking)
        {
            SearchString = searchString;
            Url = url;
            Ranking = ranking;
        }
    }
}
