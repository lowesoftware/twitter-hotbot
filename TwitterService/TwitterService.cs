using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using TweetSharp.Twitter;
using TweetSharp.Twitter.Fluent;
using TweetSharp.Twitter.Model;
using TweetSharp.Twitter.Extensions;
using TweetSharp.Fluent;
using TweetSharp.Model;
using TweetSharp.Extensions;


namespace TwitterService
{
    public class Tweet
    {
        public string Id;
        public string Author;
        public string Message;
        public DateTime Published;
        public string Hash;
    }

    public class TwitterToken
    {
        public string Token;
        public string TokenSecret;
    }

    public class TwitterService
    {

        private string OAUTH_CONSUMER_KEY = "wJLxct1tqM59h15PC0YRQw";
        private string OAUTH_CONSUMER_SECRET = "i5tGNRSTBGVDIh7z9agqqdQ8T5f1MpedvrK7Ymbbg";


        public TwitterToken Authenticate(string username, string password)
        {
            var untwitter = FluentTwitter.CreateRequest().Configuration.UseHttps().Authentication.GetRequestToken(OAUTH_CONSUMER_KEY, OAUTH_CONSUMER_SECRET);

            var response = untwitter.Request();
            var unauthorizedToken = response.AsToken();

            // Calling this method will launch the authorization page in a browser
            var twitter = FluentTwitter.CreateRequest()
                .Authentication.AuthorizeDesktop(
                    OAUTH_CONSUMER_KEY,
                    OAUTH_CONSUMER_SECRET,
                    unauthorizedToken.Token);

            //After authorizing your app, the user will be given a PIN that you'll have
            //to collect from them as you need to pass it back to the server
            string verifier = Microsoft.VisualBasic.Interaction.InputBox("Twitter token for " + username + " - " + password, "Authenticate", "", 0, 0);


            //if the user approved your app and provided the correct PIN
            //submitting the request will turn your unauthorized request token into
            //an authorized token needed for the next step
            twitter = FluentTwitter.CreateRequest().Configuration.UseHttps().Authentication.GetAccessToken(OAUTH_CONSUMER_KEY, OAUTH_CONSUMER_SECRET, unauthorizedToken.Token, verifier);

            response = twitter.Request();
            var accessToken = response.AsToken();

            if (accessToken == null)
            {
                return null;
            }

            return new TwitterToken()
            {
                Token = accessToken.Token,
                TokenSecret = accessToken.TokenSecret
            };
        }

        public void Post(TwitterToken token, string message)
        {
            var response = FluentTwitter.CreateRequest().Configuration.UseHttps().AuthenticateWith(OAUTH_CONSUMER_KEY, OAUTH_CONSUMER_SECRET, token.Token, token.TokenSecret).Statuses().Update(message).AsJson().Request();
        }

        public void Follow(TwitterToken token, string followuser)
        {
            var response = FluentTwitter.CreateRequest().Configuration.UseHttps().AuthenticateWith(OAUTH_CONSUMER_KEY, OAUTH_CONSUMER_SECRET, token.Token, token.TokenSecret).Friendships().Befriend(followuser).AsJson().Request();
        }

        public List<Tweet> Search(string query, string since_id)
        {
            string scrubbed = HttpUtility.UrlEncode(query);
            var reader = XmlReader.Create(string.Format("http://search.twitter.com/search.atom?lang=en&q={0}&rpp=100&result_type=recent&since_id={1}", scrubbed, since_id));
            var feed = SyndicationFeed.Load(reader);

            List<Tweet> Tweets = new List<Tweet>();

            foreach (SyndicationItem item in feed.Items)
            {
                Tweets.Add(new Tweet() 
                    { 
                        Id = item.Id, 
                        Author = item.Authors[0].Name, 
                        Message = item.Title.Text, 
                        Published = item.PublishDate.LocalDateTime,
                        Hash = HotBot.Data.Md5.GetMd5Sum(item.Authors[0].Name + item.Title.Text)
                    });
            }

            return Tweets;
        }
    }
}
