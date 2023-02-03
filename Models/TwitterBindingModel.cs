using System;
using System.Collections.Generic;
using TwitterChallenge.Data.Twitter;

namespace TwitterChallenge.Models
{
    public class TwitterBindingModel
    {
        public string       Author_id              { get; set; }
        public DateTime     Created_at             { get; set; }
        public List<string> Edit_history_tweet_ids { get; set; }
        public string       Id                     { get; set; }
        public string       Text                   { get; set; }

        //Define this function to be 
        public static Func<Data.Twitter.Data, TwitterBindingModel> TwitterFunc = (item) =>
            new TwitterBindingModel
            {
                Author_id              = item.author_id,
                Created_at             = item.created_at,
                Edit_history_tweet_ids = item.edit_history_tweet_ids,
                Id                     = item.id,
                Text                   = item.text
            };
    }
}
