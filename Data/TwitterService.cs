using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;
using Newtonsoft.Json;

using TwitterChallenge.Models;
using System.Net.Http;
using Syncfusion.Blazor.PivotView;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TwitterChallenge.Data.Twitter
{
    public class TwitterService
    {
        private readonly IConfigurationRoot _configuration;
        //private static string UriSearch = "https://api.twitter.com/2/tweets/search/recent?query=from:twitterdev";
        private static string UriSearch;
        public  static string token;
        private List<TwitterBindingModel>  twitterData = new List<TwitterBindingModel>();
        private List<TrendingBindingModel> trendData   = new List<TrendingBindingModel>();

        public TwitterService()
        {
            var config     = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _configuration = config.Build();

            UriSearch      = _configuration.GetValue<string>("APIKeys:UriSearch");
            token          = _configuration.GetValue<string>("APIKeys:BearerToken");
        }

        //This makes the Rest call to retrieve tweets info from the Twitter API
        public Task<List<Twitter.Data>> GetTwitterInfoAsync()
        {
            var result                    = new List<Twitter.Data>();
            
            try
            {
                var client                = new HttpClient();
                client.BaseAddress        = new Uri(UriSearch);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("backfill_minutes", "1");

                var stream                = client.GetStreamAsync(UriSearch).ConfigureAwait(false);
                using (var memoryStream   = new StreamReader(stream.GetAwaiter().GetResult(), System.Text.Encoding.ASCII, false))
                {
                    var item              = memoryStream.ReadLine();
                    while (item          != null)
                    {
                        if (!String.IsNullOrEmpty(item))
                            result.Add(JsonConvert.DeserializeObject<Root>(item).data);

                        if (result.Count >= 50)
                            break;

                        item              = memoryStream.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                //Any exception here is passed along to the calling method to work its way back to the UI where it will be handled
                throw ex;
            }

            if (result        != null)
                return Task.FromResult(result);
            else
                return Task.FromResult(new List<Twitter.Data>());
        }

        //This makes the Rest call to retrieve tweets info from the Twitter API
        public Task<List<Trend.Trend>> GetTrendingInfoAsync()
        {
            var result                    = new List<Trend.Trend>();
            
            try
            {
                //var trendClient = new RestClient("https://api.twitter.com/1.1/trends/place.json?id=2442047 ");
                var trendClient = new RestClient("https://api.twitter.com/1.1/trends/place.json?id=1 ");
                var request     = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + token);

                var response    = trendClient.Execute(request);

                //response is an array with a single Root entry. Deserialize and use the single entry "[0]"
                var trends      = JsonConvert.DeserializeObject<List<Trend.Root>>(response.Content)[0];
                result          = trends.trends;
            }
            catch (Exception ex)
            {
                //Any exception here is passed along to the calling method to work its way back to the UI where it will be handled
                throw ex;
            }

            if (result        != null)
                return Task.FromResult(result);
            else
                return Task.FromResult(new List<Trend.Trend>());
        }

        //This method calls the indo method (above) and projects the data into the model
        public async Task<List<TwitterBindingModel>> GetTwitterAsync()
        {
            try
            {
                //Check for nulls with '?'
                twitterData = (await GetTwitterInfoAsync())?
                    .Select(TwitterBindingModel.TwitterFunc)?
                    .ToList();
            }
            catch (Exception ex)
            {
                //Any exception here is passed back to the UI where it will be handled
                throw ex;
            }

            return twitterData.ToList();
        }

        public async Task<List<TrendingBindingModel>> GetTrendingAsync()
        {
            try
            {
                //Check for nulls with '?'
                trendData = (await GetTrendingInfoAsync())?
                    .Select(TrendingBindingModel.TwitterFunc)?
                    .ToList();
            }
            catch (Exception ex)
            {
                //Any exception here is passed back to the UI where it will be handled
                throw ex;
            }

            return trendData.ToList();
        }
    }
}
