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
    /// Interaction logic for MessageReplyListItemControl.xaml
    /// </summary>
    public partial class MessageReplyListItemControl : UserControl
    {
        public int MessageResponseId;

        public MessageReplyListItemControl(Nullable<int> messageresponseid, Nullable<DateTime> sent, string author, string original, string reply)
        {
            InitializeComponent();

            if(messageresponseid.HasValue)
                MessageResponseId = messageresponseid.Value;

            textMessageResponseId.Text = messageresponseid.ToString();
            
            if(sent.HasValue)
                textPublished.Text = sent.Value.ToString();

            textAuthor.Text = author;
            textOriginal.Text = original;
            textReply.Text = reply;
        }
    }
}
