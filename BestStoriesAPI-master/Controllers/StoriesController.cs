using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace SantanderBestStoriesAPI.Controllers
{
    [Route("api/[controller]")]
    public class StoriesController : Controller
    {
        private static IMemoryCache _cachedStories;
        private static readonly HttpClient httpClient = new HttpClient();

        [HttpGet]
        public ActionResult<object> GetTwentyTopStories()
        {
            if (_cachedStories == null) InsertTopTwentyStoriesInCache();
            return _cachedStories.Get(Stories.TopTwenty);
        }

        #region Stories CacheOperations
        private void InsertTopTwentyStoriesInCache()
        {
            List<TopStoriesFormat> StoriesList = RetrieveStories();
            var TopTwentyStories = StoriesList.Take(20).OrderByDescending(o => o.score).ToList();
            CreateCachedStoriesList(TopTwentyStories);
        }
        private void CreateCachedStoriesList(List<TopStoriesFormat> StoriesList)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.CacheMinutesLife));
            _cachedStories = new MemoryCache(new MemoryCacheOptions());
            _cachedStories.Set(Stories.TopTwenty, StoriesList, cacheEntryOptions);
        }
        #endregion

        #region HackerNewsAPI Readings
        private List<TopStoriesFormat> RetrieveStories()
        {            
            string url = "https://hacker-news.firebaseio.com/v0/beststories.json";
            var strStoryIdList = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            var lstStoriesById = JsonConvert.DeserializeObject<List<int>>(strStoryIdList);
            List<TopStoriesFormat> lstStoryDetail = new List<TopStoriesFormat>();
            foreach (var story in lstStoriesById)
            {
                TopStoriesFormat storyDetails = GetStoryDetailsById(story);
                lstStoryDetail.Add(storyDetails);
            }
            return lstStoryDetail;
        }
        private TopStoriesFormat GetStoryDetailsById(int StoryId)
        {
            string url = "https://hacker-news.firebaseio.com/v0/item/" + StoryId + ".json";
            var result = httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            return ConvertToModel(result);
        }
        private TopStoriesFormat ConvertToModel(string rawJson)
        {
            var jsonStory = JsonConvert.DeserializeObject<Story>(rawJson);
            return new TopStoriesFormat
            {
                title = jsonStory.title,
                uri = jsonStory.url,
                postedBy = jsonStory.by,
                time = UnixTimeStampToDateTime(jsonStory.time),
                score = jsonStory.score,
                comments = jsonStory.kids.Count()
            }; 
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }
        #endregion
    }
}
