using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEO_Automation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchIndexResultController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<string> Get(string searchText, string url)
        {
            Console.WriteLine($"SearchText = ${searchText} and url =${url}");
            // Perform the get action and return.
            return new List<string>();
        }

    }
}