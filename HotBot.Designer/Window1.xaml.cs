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

namespace HotBot.Designer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            dragDrop = new DragDropHandler(button1, new System.Windows.DataObject(button1));
            button1.Drop += new DragEventHandler(rectangle1_Drop);
        }

        DragDropHandler dragDrop;

        void rectangle1_Drop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
