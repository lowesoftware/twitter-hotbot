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

    public partial class TwitterControl
    {
        private DataSet ClassificationTweetDataSet;


        private void buttonRefreshClassification_Click(object sender, RoutedEventArgs e)
        {

            RefreshClassificationGrid();
        }

        private void RefreshClassificationGrid()
        {
//            listTweetsClassification.DataSource = null;

            using (HotBotEntities context = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJob.JobId.ToString(), context);
                Job job = jobsquery.ToList().First();

                if (listQueryType.SelectedItems.Count == 0)
                    return;

                switch (((ListBoxItem)listQueryType.SelectedValue).Content.ToString())
                {
                    case "Match":
                        job.Message.Attach(job.Message.CreateSourceQuery().Include("Friend").OrderBy("it.MessageId").Where(a => a.IsMatch == true || a.MatchScore >= CurrentJob.MinMatchScore));
                        break;

                    case "Non Match":
                        job.Message.Attach(job.Message.CreateSourceQuery().Include("Friend").OrderBy("it.MessageId").Where(a => a.IsMatch == false || a.MatchScore <= CurrentJob.MaxNonMatchScore));
                        break;

                    case "Unknown":
                        job.Message.Attach(job.Message.CreateSourceQuery().Include("Friend").OrderBy("it.MessageId").Where(a => a.IsMatch == null && a.MatchScore > CurrentJob.MaxNonMatchScore && a.MatchScore < CurrentJob.MinMatchScore));
                        break;

                    case "Unprocessed":
                        job.Message.Attach(job.Message.CreateSourceQuery().Include("Friend").OrderBy("it.MessageId").Where(a => a.IsMatch == null && a.MatchScore == null));
                        break;
                }

                // load up the list view
                listTweetsClassification.Items.Clear();

                foreach (Message message in job.Message)
                {
                    listTweetsClassification.Items.Add(new Controls.MessageListItemControl(message.MessageId, message.Published, message.Friend.Username, message.Body, message.IsMatch, message.MatchScore));
                }
            }
        }


        private void buttonScoreMessages_Click(object sender, RoutedEventArgs e)
        {
            if (listTweetsClassification.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select at least one message.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            List<string> MessageIdList = new List<string>();

            foreach (MessageListItemControl item in listTweetsClassification.SelectedItems)
            {
                MessageIdList.Add(item.MessageId.ToString());
            }


            using (HotBotEntities context = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJob.JobId.ToString(), context);
                Job job = jobsquery.ToList().First();

                job.Message.Attach(job.Message.CreateSourceQuery().Where("it.MessageID IN {" + string.Join(",", MessageIdList.ToArray()) + "}"));

                // update the database
                foreach (Message message in job.Message)
                {
                    message.MatchScore = Classifier.Classify(message.Body);
                }

                context.SaveChanges();
            }

            RefreshClassificationGrid();
        }

    }
}
