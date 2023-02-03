using System;
using System.Collections.Generic;
using TwitterChallenge.Data.Twitter;

namespace TwitterChallenge.Models
{
    public class TrendingBindingModel
    {
        public string Name            { get; set; }
        public string Url             { get; set; }
        public object PromotedContent { get; set; }
        public string Query           { get; set; }
        public int?   TweetVolume     { get; set; }


        //Define this function to be 
        public static Func<Data.Twitter.Trend.Trend, TrendingBindingModel> TwitterFunc = (item) =>
            new TrendingBindingModel
            {
                Name            = item.name,
                Url             = item.url,
                PromotedContent = item.promoted_content,
                Query           = item.query,
                TweetVolume     = item.tweet_volume
            };
    }
}
