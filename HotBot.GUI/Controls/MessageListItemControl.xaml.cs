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

namespace HotBot.GUI.Controls
{
    /// <summary>
    /// Interaction logic for MessageListItemControl.xaml
    /// </summary>
    public partial class MessageListItemControl : UserControl
    {
        public int MessageId;

        public MessageListItemControl(int messageid, DateTime published, string author, string body, Nullable<bool> ismatch, Nullable<double> matchscore)
        {
            InitializeComponent();

            MessageId = messageid;

            textMessageId.Text = messageid.ToString();
            textPublished.Text = published.ToString();
            textAuthor.Text = author;
            textBody.Text = body;

            if (ismatch == null)
                textIsMatch.Text = "none";
            else
                textIsMatch.Text = ismatch.Value.ToString();


            if (matchscore == null)
                textMatchScore.Text = "none";
            else
                textMatchScore.Text = matchscore.Value.ToString();
        }
    }
}
