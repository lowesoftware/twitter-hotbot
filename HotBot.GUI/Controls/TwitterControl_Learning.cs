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
        private DataSet LearningTweetDataSet;

        private void ProcessMarkMessage(bool ismatch)
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


            using(HotBotEntities context = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJob.JobId.ToString(), context);
                Job job = jobsquery.ToList().First();

                job.Message.Attach(job.Message.CreateSourceQuery().Where("it.MessageID IN {" + string.Join(",", MessageIdList.ToArray()) + "}"));

                // update the database
                foreach (Message message in job.Message)
                {

                    // if we're in learning mode then process it
                    if (CurrentJob.IsLearning == true)
                    {
                        // if it's marked the same then don't do anything
                        if (message.IsMatch == ismatch)
                        {
                        }
                        // it's never been trained so go ahead
                        else if (message.IsMatch == null)
                        {
                            //train the classifier
                            if (ismatch)
                            {
                                Classifier.TeachMatch(message.Body);
                            }
                            else
                            {
                                Classifier.TeachNonMatch(message.Body);
                            }
                        }

                        // the training has been changed
                        else if (message.IsMatch != ismatch)
                        {
                            //re-train the classifier
                            if (ismatch)
                            {
                                //Classifier.UnTeachNonMatch(message.Body);
                                Classifier.TeachMatch(message.Body);
                                Classifier.TeachMatch(message.Body);
                            }
                            else
                            {
                                //Classifier.UnTeachMatch(message.Body);
                                Classifier.TeachNonMatch(message.Body);
                                Classifier.TeachNonMatch(message.Body);
                            }
                        }
                    }



                    // update the record whether we're in training mode or not
                    message.IsMatch = ismatch;
                    
                }

                context.SaveChanges();

            }

        }

        private void buttonMarkGood_Click(object sender, RoutedEventArgs e)
        {
            ProcessMarkMessage(true);
            RefreshClassificationGrid();
        }


        private void buttonMarkBad_Click(object sender, RoutedEventArgs e)
        {
            ProcessMarkMessage(false);
            RefreshClassificationGrid();
        }



        private void buttonClearLearning_Click(object sender, RoutedEventArgs e)
        {
            CurrentJob.WordProbability.Load();


            while (CurrentJob.WordProbability.Any())
            {
                DatabaseContext.DeleteObject(CurrentJob.WordProbability.First());
            }

            CurrentJob.Message.Load();

            foreach (Message message in CurrentJob.Message)
            {
                message.IsMatch = null;
                message.MatchScore = null;
            }

            DatabaseContext.SaveChanges();


            ClassifierDataSource = new HotBotDbWordsDataSource(CurrentJob.JobId);
            Classifier = new BayesianClassifier(ClassifierDataSource);

            RefreshClassificationGrid();
        }


        private void buttonReLearn_Click(object sender, RoutedEventArgs e)
        {
            Classifier.TeachMatch("This is a sentence");
            Classifier.TeachMatch("The sentence is good");
            Classifier.TeachNonMatch("Foo goes the bar a sentence is here");

            string s = "Matches = " + Classifier.Classify("This is a sentence and it also say");
        }
    }
}
