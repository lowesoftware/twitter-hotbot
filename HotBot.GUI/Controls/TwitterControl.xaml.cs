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

namespace HotBot.GUI.Controls
{
    /// <summary>
    /// Interaction logic for TwitterControl.xaml
    /// </summary>
    public partial class TwitterControl : UserControl
    {
        public TwitterControl()
        {
            InitializeComponent();

            InitializeState();

            SizeChanged += new SizeChangedEventHandler(Control_SizeChanged);
        }

        void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            listTweets.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            //listTweetsLearning.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            listTweetsClassification.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            listTweetsClassification.Width = tabDetails.ActualWidth - 110 > 0 ? tabDetails.ActualWidth - 110 : 0;
            listAccounts.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            listResponses.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            listMessageReplies.Height = tabDetails.ActualHeight - 60 > 0 ? tabDetails.ActualHeight - 60 : 0;
            
        }

        private DataSet NewTweetDataSet;

        private void buttonRefreshNewMessages_Click(object sender, RoutedEventArgs e)
        {
            Logic.HotBotLogic logic = new HotBot.GUI.Logic.HotBotLogic(CurrentJob.JobId, Classifier);

            List<TwitterService.Tweet> tweets = logic.GetLatestTweets();

            // load up the list view
            ListViewHelper lvh = new ListViewHelper();

            foreach (TwitterService.Tweet tweet in tweets)
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("Published", tweet.Published.ToString());
                row.Add("Author", tweet.Author);
                row.Add("Title", tweet.Message);

                lvh.AddRow(row);
            }

            if (lvh.Count == 0)
            {
                listTweets.DataSource = null;
            }
            else
            {
                NewTweetDataSet = lvh.GetDataSet();
                listTweets.DataSource = NewTweetDataSet.Tables[0].DefaultView;
            }
        }



    }
}
