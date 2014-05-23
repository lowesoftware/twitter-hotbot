using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NClassifier;
using NClassifier.Bayesian;
using HotBot.Data;
using System.Data.Objects;

namespace NClassifier.Bayesian
{
	public class HotBotDbWordsDataSource : ICategorizedWordsDataSource
	{
        Job CurrentJob;
        HotBotEntities DatabaseContext;

		/// <summary>
		/// Create a SqlWordsDataSource using the DEFAULT_CATEGORY ("DEFAULT")
		/// </summary>
		public HotBotDbWordsDataSource(int jobid)
		{
            DatabaseContext = new HotBotEntities();

            ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c WHERE c.JobId = " + jobid.ToString(), DatabaseContext);
            CurrentJob = jobsquery.ToList().First();

            CurrentJob.WordProbability.Load();
		}

        public WordProbability GetWordProbability(string category, string word)
		{
            List<HotBot.Data.WordProbability> WPList = CurrentJob.WordProbability.Where(a => a.Category == category && a.Word == word).ToList();

            if (WPList.Count() == 0)
            {
                return null;
            }
            else
            {
                return new WordProbability(word, WPList.First().Matches, WPList.First().NonMatches);
            }
		}

		public WordProbability GetWordProbability(string word)
		{
			return GetWordProbability(ICategorizedClassifierConstants.DEFAULT_CATEGORY, word);
		}

		private void UpdateWordProbability(string category, string word, bool isMatch)
		{
            HotBot.Data.WordProbability wp = null;

            if (CurrentJob.WordProbability.Where(a => a.Category == category && a.Word == word).Count() > 0)
            {
                wp = CurrentJob.WordProbability.Where(a => a.Category == category && a.Word == word).First();
            }


			// truncate word at 255 characters
			if (word.Length > 255)
				word = word.Substring(0, 255);


            // wp doesn't exist
            if (wp == null)
            {
                if (isMatch)
                {
                    CurrentJob.WordProbability.Add(new HotBot.Data.WordProbability() { Category = category, Word = word, Matches = 1 });
                }
                else
                {
                    CurrentJob.WordProbability.Add(new HotBot.Data.WordProbability() { Category = category, Word = word, NonMatches = 1 });
                }
            }
            else
            {

                if (isMatch)
                {
                    wp.Matches++;
                }
                else
                {
                    wp.NonMatches++;
                }
            }

            DatabaseContext.SaveChanges();
            
		}

        public void AddMatch(string category, string word)
		{
			if (category == null)
				throw new ArgumentNullException("Category cannot be null.");
			UpdateWordProbability(category, word, true);
		}

		public void AddMatch(string word)
		{
			UpdateWordProbability(ICategorizedClassifierConstants.DEFAULT_CATEGORY, word, true);
		}

		public void AddNonMatch(string category, string word)
		{
			if (category == null)
				throw new ArgumentNullException("Category cannot be null.");
			UpdateWordProbability(category, word, false);
		}

		public void AddNonMatch(string word)
		{
			UpdateWordProbability(ICategorizedClassifierConstants.DEFAULT_CATEGORY, word, false);
		}

	}
}