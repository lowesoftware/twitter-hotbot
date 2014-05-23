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
using System.Threading;
using HotBot.Data;
using System.Data.Objects;

namespace HotBot.GUI
{
    /// <summary>
    /// </summary>
    public partial class WindowMain : Window
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {

            using (HotBotEntities context = new HotBotEntities())
            {
                ObjectQuery<Job> jobsquery = new ObjectQuery<Job>("SELECT VALUE c FROM HotBotEntities.Job as c", context);
                List<Job> jobs = jobsquery.ToList();

                foreach (Job job in jobs)
                {
                    Controls.TwitterControl control = new Controls.TwitterControl()
                        {
                            Visibility = Visibility.Visible,
                        };

                    control.LoadJob(job.JobId);

                    tabJobs.Items.Add(new TabItem() { 
                        Header = job.Name,
                        Content = control
                    });
                }
            }

            base.OnInitialized(e);
        }

        private void MenuJobNew_Click(object sender, RoutedEventArgs e)
        {
            Controls.TwitterControl control = new Controls.TwitterControl()
            {
                Visibility = Visibility.Visible,
            };

            control.CreateJob("New Job", "", "Twitter");

            TabItem tab = new TabItem();
            tab.Header = "New Job";
            tab.Content = control;
            tabJobs.Items.Add(tab);

            tabJobs.SelectedIndex = tabJobs.Items.Count-1;
        }

        private void MenuJobSave_Click(object sender, RoutedEventArgs e)
        {
            (tabJobs.SelectedContent as Controls.TwitterControl).SaveJob();

            (tabJobs.SelectedItem as TabItem).Header = (tabJobs.SelectedContent as Controls.TwitterControl).CurrentJob.Name;
        }

        private void MenuJobExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuJobDelete_Click(object sender, RoutedEventArgs e)
        {
            (tabJobs.SelectedContent as Controls.TwitterControl).DeleteJob();
            tabJobs.Items.Remove(tabJobs.SelectedItem);
        }

    }
}
