using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NClassifier;
using NClassifier.Bayesian;
using HotBot.Data;
using System.Data.Objects;

namespace HotBot.GUI.Controls
{

    public partial class TwitterControl
    {
        private HotBotEntities DatabaseContext;
        public Job CurrentJob = null;

        private BayesianClassifier Classifier;
        private HotBotDbWordsDataSource ClassifierDataSource;

        private void InitializeState()
        {
            DatabaseContext = new HotBotEntities();
        }


        public void CreateJob(string name, string query, string type)
        {
            CurrentJob = new Job();

            CurrentJob.Name = name;
            CurrentJob.Query = query;
            CurrentJob.Type = type;
            CurrentJob.IsLearning = true;

            DatabaseContext.AddToJob(CurrentJob);
            DatabaseContext.SaveChanges();

            ClassifierDataSource = new HotBotDbWordsDataSource(CurrentJob.JobId);
            Classifier = new BayesianClassifier(ClassifierDataSource);

            UpdateView();
        }


        public void DeleteJob()
        {
            DatabaseContext.DeleteObject(CurrentJob);
            DatabaseContext.SaveChanges();
        }


        public void LoadJob(int jobid)
        {

            ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + jobid.ToString(), DatabaseContext);
            CurrentJob = jobsquery.ToList().First();

            CurrentJob.Account.Load();
            CurrentJob.Response.Load();
            
            ClassifierDataSource = new HotBotDbWordsDataSource(CurrentJob.JobId);
            Classifier = new BayesianClassifier(ClassifierDataSource);

            UpdateView();
        }


        public void SaveJob()
        {
            CurrentJob.Name = textJobName.Text;
            CurrentJob.Type = textJobType.Text;
            CurrentJob.Query = textJobQuery.Text;
            CurrentJob.IsLearning = checkIsLearning.IsChecked.Value;
            CurrentJob.MinMatchScore = double.Parse(textMinMatchScore.Text);
            CurrentJob.MaxNonMatchScore = double.Parse(textMaxNonMatchScore.Text);

            DatabaseContext.SaveChanges();

            UpdateView();
        }


        public void UpdateView()
        {
            textJobId.Text = CurrentJob.JobId.ToString();
            textJobName.Text = CurrentJob.Name;
            textJobQuery.Text = CurrentJob.Query;
            textJobType.Text = CurrentJob.Type;
            checkIsLearning.IsChecked = CurrentJob.IsLearning;

            textMinMatchScore.Text = CurrentJob.MinMatchScore.ToString();
            textMaxNonMatchScore.Text = CurrentJob.MaxNonMatchScore.ToString();
        }
    }
}
