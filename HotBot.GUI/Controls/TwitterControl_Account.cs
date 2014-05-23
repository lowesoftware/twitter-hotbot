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

        private void buttonAddAccount_Click(object sender, RoutedEventArgs e)
        {

            DatabaseContext.AddToAccount(new Account()
            {
                Job = CurrentJob,
                Username = textUsername.Text,
                Password = textPassword.Text,
                Token = textAccountToken.Text,
                TokenSecret = textAccountTokenSecret.Text
            });

            DatabaseContext.SaveChanges();

            textUsername.Text = "";
            textPassword.Text = "";

            RefreshAccountGrid();
        }


        private void RefreshAccountGrid()
        {
            CurrentJob.Account.Load();

            // load up the list view
            listAccounts.Items.Clear();

            foreach (Account account in CurrentJob.Account)
            {
                listAccounts.Items.Add(new TextBlock() { Text = account.Username + " - " + account.Password + " - " + account.Token + " - " + account.TokenSecret });
            }
        }



        private void buttonRefreshAccount_Click(object sender, RoutedEventArgs e)
        {
            RefreshAccountGrid();
        }



        private void buttonAuthAccount_Click(object sender, RoutedEventArgs e)
        {
            TwitterService.TwitterService service = new TwitterService.TwitterService();

            TwitterService.TwitterToken token = service.Authenticate(textUsername.Text, textPassword.Text);

            textAccountToken.Text = token.Token;
            textAccountTokenSecret.Text = token.TokenSecret;
        }

    }
}
