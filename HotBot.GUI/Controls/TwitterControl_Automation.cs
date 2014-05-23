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

        private void buttonAutomationStart_Click(object sender, RoutedEventArgs e)
        {
            StartThread();
        }

        private void buttonAutomationStop_Click(object sender, RoutedEventArgs e)
        {
            CancelThread();
        }

        Thread AutomationThread;
        bool CancelAutomationThread = false;

        [STAThread]
        public void AutomationThreadBody()
        {
            try
            {
                Logic.HotBotLogic logic = new HotBot.GUI.Logic.HotBotLogic(CurrentJob.JobId, Classifier);


                while (!CancelAutomationThread)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        // get the latest from twitter
                        UpdateStatus("Retrieving latest tweets: " + CurrentJob.Query);
                        List<TwitterService.Tweet> tweets = logic.GetLatestTweets();
                        UpdateStatus("Added " + tweets.Count.ToString() + " new tweets to db");
                        RefreshAutomationCounters();
                    }
                    catch (Exception ex)
                    {
                        UpdateStatus(ex.ToString());
                        UpdateStatus("Continuing anyway...");
                    }

                    // classify
                    UpdateStatus("Classifying unclassified messages in db");
                    logic.ClassifyMessages();
                    RefreshAutomationCounters();

                    // send messages
                    UpdateStatus("Sending a message");
                    logic.SendOneMessage();
                    RefreshAutomationCounters();

                    // delay to ensure api isn't pounded too hard
                    int delay = new Random().Next(30, 60);
                    UpdateStatus("Waiting " + delay.ToString() + " seconds");
                    for (int i = 0; i < delay; i++)
                    {
                        UpdateStatus("Waiting " + (delay-i).ToString() + " seconds", true);

                        Thread.Sleep(1000);
                        if (CancelAutomationThread)
                            break;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                UpdateStatus("Automation thread was aborted");
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }

            EndAutomation();
        }

        public void StartThread()
        {
            if (AutomationThread != null)
                return;

            if (CurrentJob.IsLearning)
            {
                UpdateStatus("Cannot start automation when job is in learning mode");
                return;
            }

            CancelAutomationThread = false;

            buttonAutomationStart.Visibility = Visibility.Collapsed;
            buttonAutomationStop.Visibility = Visibility.Visible;
            progress.Visibility = Visibility.Visible;
            textAutomation.Margin = new Thickness(25, 0, 0, 0);

            UpdateStatus("Starting automation thread");

            AutomationThread = new Thread(new ThreadStart(AutomationThreadBody));
            AutomationThread.Name = "AutomationThread";
            AutomationThread.Start();
        }

        public void CancelThread()
        {
            if (AutomationThread == null)
                return;

            UpdateStatus("Automation cancel request sent");
            CancelAutomationThread = true;
        }

        public void UpdateStatus(string text)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                textAutomationStatus.Text += DateTime.Now.ToString() + " - " + text;
                textAutomationStatus.Text += "\n";
                textAutomationStatus.Focus();
                textAutomationStatus.SelectionStart = textAutomationStatus.Text.Length;
            }));
        }

        public void RefreshAutomationCounters()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                using (Data.HotBotEntities DatabaseContext = new HotBotEntities())
                {
                    ObjectQuery<Job> jobsquery =
                        new ObjectQuery<Job>(
                            "SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + CurrentJob.JobId.ToString(),
                            DatabaseContext);
                    Data.Job job = jobsquery.ToList().First();
                    job.Message.Attach(job.Message.CreateSourceQuery().Include("MessageResponse"));

                    textAutomationTotalReplies.Text = job.Message.Where(a => a.MessageResponse.Count() > 0).Count().ToString();
                    textAutomationTotalMessages.Text = job.Message.Count().ToString();
                    textAutomationTotalMatches.Text = job.Message.Where(a => a.IsMatch == true || a.MatchScore >= CurrentJob.MinMatchScore).Count().ToString();
                }

            }));
        }

        public void UpdateStatus(string text, bool overwriteline)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (overwriteline)
                {
                    textAutomationStatus.Text = textAutomationStatus.Text.Substring(0, textAutomationStatus.Text.LastIndexOf("\n", textAutomationStatus.Text.Length - 2)+1);
                }

                UpdateStatus(text);
            }));
        }



        public void EndAutomation()
        {
            Dispatcher.BeginInvoke(new Action(delegate
                {
                    UpdateStatus("Automation ended");

                    buttonAutomationStart.Visibility = Visibility.Visible;
                    buttonAutomationStop.Visibility = Visibility.Collapsed;
                    progress.Visibility = Visibility.Collapsed;
                    textAutomation.Margin = new Thickness(0, 0, 0, 0);

                    AutomationThread = null;
                }));
        }



    }
}
