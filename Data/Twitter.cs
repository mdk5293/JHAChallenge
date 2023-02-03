using System;
using System.Collections.Generic;

namespace TwitterChallenge.Data.Twitter
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string       author_id              { get; set; }
        public DateTime     created_at             { get; set; }
        public List<string> edit_history_tweet_ids { get; set; }
        public string       id                     { get; set; }
        public string       text                   { get; set; }
    }

    public class Includes
    {
        public List<User>   users                  { get; set; }
    }

    public class Root
    {
        public Data         data                   { get; set; }
        public Includes     includes               { get; set; }
    }

    public class User
    {
        public DateTime     created_at             { get; set; }
        public string       id                     { get; set; }
        public string       name                   { get; set; }
        public string       username               { get; set; }
    }
}
