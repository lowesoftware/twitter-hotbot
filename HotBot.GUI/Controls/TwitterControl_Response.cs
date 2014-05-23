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

        private void buttonAddResponse_Click(object sender, RoutedEventArgs e)
        {
            DatabaseContext.AddToResponse(new Response()
            {
                 Job = CurrentJob,
                 Text = textBody.Text,
                 Weight = int.Parse(textWeight.Text),
                 Active = true
            });

            DatabaseContext.SaveChanges();

            textBody.Text = "";
            textBody.Focus();

            RefreshResponseGrid();
        }


        private void textBody_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBody.Text = textBody.Text.Trim();
                buttonAddResponse_Click(null,null);
            }
        }



        private void RefreshResponseGrid()
        {
            CurrentJob.Response.Load();

            // load up the list view
            listResponses.Items.Clear();

            foreach (Response response in CurrentJob.Response)
            {
                listResponses.Items.Add(new TextBlock() { Text = response.Text + " " + response.Weight });
            }
        }



        private void buttonRefreshResponse_Click(object sender, RoutedEventArgs e)
        {
            RefreshResponseGrid();
        }
    }
}
