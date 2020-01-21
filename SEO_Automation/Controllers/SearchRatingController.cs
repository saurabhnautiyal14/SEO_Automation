﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SEO_Automation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchRatingController : ControllerBase
    {

        private readonly ILogger<SearchRatingController> _logger;

        public SearchRatingController(ILogger<SearchRatingController> logger)
        {
            _logger = logger;
        }

        //https://localhost:44324/api/searchRating?searchString=iron&url=www.google.com
        [HttpGet]
        public ActionResult Get(string searchString, string url)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return BadRequest("Search String can not be null");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("URL String can not be null");
            }


            //Perform your operation here
            //Better to create service and inject to this controller

            var rating = new Rating(searchString, url);
            var findRatingDict = rating.getRanking();
            foreach (KeyValuePair<string, int> item in findRatingDict)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            }

            string json = JsonConvert.SerializeObject(findRatingDict, Formatting.Indented);

            return new OkObjectResult(json);
        }


    }
}