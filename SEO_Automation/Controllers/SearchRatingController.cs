using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SEO_Automation.Service;

namespace SEO_Automation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchRatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public SearchRatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        //https://localhost:44324/api/searchRating?searchString=iron&url=www.google.com
        [HttpGet]
        public async Task<ActionResult> GetAsync(string searchString, string url)
        {
            if (ValidateArgs(searchString, url, out var badRequest))
            {
                return badRequest;
            }


            string result = await _ratingService.GetRanking(searchString, url);

            return new OkObjectResult(result);
        }

        private bool ValidateArgs(string searchString, string url, out ActionResult badRequest)
        {
            badRequest = null;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                {
                    badRequest = BadRequest("Search String can not be null");
                    return true;
                }
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                {
                    badRequest = BadRequest("URL String can not be null");
                    return true;
                }
            }


            return false;
        }
    }
}