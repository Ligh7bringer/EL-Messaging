using ELM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ELM
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public StatsWindow()
        {
            InitializeComponent();
            //set the textboxes to not editable
            txtbox_mentions.IsReadOnly = true;
            txtbox_trending.IsReadOnly = true;

            //display hashtags and mentions stored in StringHelper
            foreach(var entry in StringHelper.HashTags.OrderByDescending(key => key.Value))
            {
                txtbox_trending.Text += "\n#" + entry.Key + " (used: " + entry.Value + " times)";
            }

            foreach(var entry in StringHelper.Mentions)
            {
                txtbox_mentions.Text += "\n@" + entry;
            }
        }

        //open the file where SIRs are stored
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + @"\lists\SIRs.txt");
        }

        //open the file where quarantined URLs are stored
        private void btn_quarantined_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + @"\lists\URLs.txt");
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
