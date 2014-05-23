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
using System.Threading;

namespace HotBot.GUI.Controls
{

    public partial class TwitterControl
    {

        private void buttonSendReplies_Click(object sender, RoutedEventArgs e)
        {
            int sent = 0, maxsend = 5;

            TwitterService.TwitterService twitter = new TwitterService.TwitterService();

            // fetch the messages from the database
            CurrentJob.Message.Attach(CurrentJob.Message.CreateSourceQuery().Include("Friend").Include("MessageResponse").OrderBy("it.MessageId").Where(a => a.IsMatch == true || a.MatchScore >= CurrentJob.MinMatchScore));
            CurrentJob.Account.Load();
            CurrentJob.Response.Load();

            Random rand = new Random((int)DateTime.Now.Ticks);

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

                // check if we're following the friend
                if (!message.Friend.IsFollowed)
                {
                    twitter.Follow(new TwitterService.TwitterToken() { Token = account.Token, TokenSecret = account.TokenSecret }, friendusername);
                    message.Friend.IsFollowed = true;
                }

                // select a random reply based on weight
                Data.Response response = CurrentJob.Response.Where(a=>a.Active==true).OrderBy(a => rand.Next()).First();

                // send a reply to the friend
                twitter.Post(new TwitterService.TwitterToken() { Token = account.Token, TokenSecret = account.TokenSecret }, "@" + friendusername + " " + response.Text);

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
            

            RefreshMessageRepliesGrid();
        }



        private void RefreshMessageRepliesGrid()
        {
            CurrentJob.Message.Attach(CurrentJob.Message.CreateSourceQuery().Include("Friend").Include("MessageResponse").OrderBy("it.MessageId").Where(a => a.IsMatch == true || a.MatchScore >= CurrentJob.MinMatchScore));

            // load up the list view
            listMessageReplies.Items.Clear();

            foreach (Message message in CurrentJob.Message)
            {
                if (message.MessageResponse.Count > 0)
                {
                    listMessageReplies.Items.Add(new Controls.MessageReplyListItemControl(
                        message.MessageResponse.First().MessageResponseId,
                        message.MessageResponse.First().Timestamp,
                        message.Friend.Username,
                        message.Body,
                        message.MessageResponse.First().Response.Text));
                }
                else
                {
                    listMessageReplies.Items.Add(new Controls.MessageReplyListItemControl(
                        null,
                        null,
                        message.Friend.Username ?? "",
                        message.Body,
                        ""));
                }
            }
        }

        private void buttonRefreshMessageReply_Click(object sender, RoutedEventArgs e)
        {
            RefreshMessageRepliesGrid();
        }
    }
}
