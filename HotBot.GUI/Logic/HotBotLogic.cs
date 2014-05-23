using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using NClassifier;
using NClassifier.Bayesian;
using HotBot.Data;
using System.Data.Objects;

namespace HotBot.GUI.Logic
{
    class HotBotLogic
    {
        int CurrentJobId;
        BayesianClassifier Classifier;

        public HotBotLogic(int currentjobid, BayesianClassifier classifier)
        {
            CurrentJobId = currentjobid;
            Classifier = classifier;
        }

        public List<TwitterService.Tweet> GetLatestTweets()
        {
            using (Data.HotBotEntities DatabaseContext = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJobId.ToString(), DatabaseContext);
                Data.Job CurrentJob = jobsquery.ToList().First();

                // get all the most recent tweets
                TwitterService.TwitterService t = new TwitterService.TwitterService();
                List<TwitterService.Tweet> tweets = t.Search(CurrentJob.Query, "");

                // get a list of the latest stored messages
                CurrentJob.Message.Attach(CurrentJob.Message.CreateSourceQuery());

                // remove any new items already stored
                foreach (Message message in CurrentJob.Message)
                {
                    for (int i = tweets.Count - 1; i >= 0; i--)
                    {
                        if (message.Hash == tweets[i].Hash)
                            tweets.RemoveAt(i);
                    }
                }



                // remove any items that are to someone or a retweet
                for (int i = tweets.Count - 1; i >= 0; i--)
                {
                    if (tweets[i].Message.Contains('@'))
                    {
                        tweets.RemoveAt(i);
                    }
                }


                // update the database
                foreach (TwitterService.Tweet tweet in tweets)
                {
                    // check for the friend
                    ObjectQuery<Friend> friendquery =
                        new ObjectQuery<Friend>(
                            "SELECT VALUE c FROM HotBotEntities.Friend as c WHERE c.Username = @username",
                            DatabaseContext);
                    friendquery.Parameters.Add(new ObjectParameter("username", tweet.Author));

                    Friend friend;
                    if (friendquery.ToList().Count == 0)
                    {
                        friend = new Friend()
                                     {
                                         Username = tweet.Author,
                                         IsFollowed = false
                                     };

                        DatabaseContext.AddToFriend(friend);
                    }
                    else
                    {
                        friend = friendquery.ToList().First();
                    }

                    // create the message
                    Message message = new Message()
                                          {
                                              Job = CurrentJob,
                                              Friend = friend,
                                              Hash = tweet.Hash,
                                              Published = tweet.Published,
                                              Body = tweet.Message
                                          };

                    DatabaseContext.AddToMessage(message);
                }

                DatabaseContext.SaveChanges();

                return tweets;
            }
        }

        public void ClassifyMessages()
        {
            using (Data.HotBotEntities DatabaseContext = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery =
                    new ObjectQuery<Job>(
                        "SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJobId.ToString(),
                        DatabaseContext);
                Data.Job CurrentJob = jobsquery.ToList().First();

                // get any jobs that are unclassified

                CurrentJob.Message.Attach(
                    CurrentJob.Message.CreateSourceQuery().Where(a => a.IsMatch == null && a.MatchScore == null));

                // update the database
                foreach (Message message in CurrentJob.Message)
                {
                    message.MatchScore = Classifier.Classify(message.Body);
                }

                DatabaseContext.SaveChanges();
            }

        }

        public void SendOneMessage()
        {
            using (Data.HotBotEntities DatabaseContext = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery =
                    new ObjectQuery<Job>(
                        "SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJobId.ToString(),
                        DatabaseContext);
                Data.Job CurrentJob = jobsquery.ToList().First();

                int sent = 0, maxsend = 1;

                TwitterService.TwitterService twitter = new TwitterService.TwitterService();

                // fetch the messages from the database            
                CurrentJob.Message.Attach(
                    CurrentJob.Message.CreateSourceQuery().Include("Friend").Include("MessageResponse").OrderBy(
                        "it.IsMatch, it.MatchScore").Where(
                            a => a.IsMatch == true || a.MatchScore >= CurrentJob.MinMatchScore));
                CurrentJob.Account.Load();
                CurrentJob.Response.Load();

                Random rand = new Random((int) DateTime.Now.Ticks);

                // get the latest unsent job (newest first)
                foreach (Data.Message message in CurrentJob.Message)
                {
                    if (message.MessageResponse.Count > 0)
                        continue;

                    if (sent >= maxsend)
                        break;

                    // get a random account
                    Data.Account account = CurrentJob.Account.OrderBy(a => rand.Next()).First();

                    string friendusername = message.Friend.Username.Substring(0, message.Friend.Username.IndexOf(' '));

                    // check if we're following the friend -- if so we will not spam them
                    if (message.Friend.IsFollowed)
                    {
                        continue;
                    }

                    // follow the user
                    twitter.Follow(
                        new TwitterService.TwitterToken() {Token = account.Token, TokenSecret = account.TokenSecret},
                        friendusername);
                    message.Friend.IsFollowed = true;

                    // select a random reply
                    if(CurrentJob.Response.Where(a => a.Active).Count()==0)
                        throw(new Exception("There are no active responses for this job."));

                    Data.Response response = CurrentJob.Response.Where(a => a.Active).OrderBy(a => rand.Next()).First();

                    // send a reply to the friend
                    twitter.Post(
                        new TwitterService.TwitterToken() {Token = account.Token, TokenSecret = account.TokenSecret},
                        "@" + friendusername + " " + response.Text);

                    // log the reply to the database
                    DatabaseContext.AddObject("MessageResponse", new Data.MessageResponse()
                                                                     {
                                                                         Message = message,
                                                                         Response = response,
                                                                         Timestamp = DateTime.Now
                                                                     });


                    sent++;
                }

                DatabaseContext.SaveChanges();
            }
        }
    }
}
