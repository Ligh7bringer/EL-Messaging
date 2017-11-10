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
            txtbox_mentions.IsReadOnly = true;
            txtbox_trending.IsReadOnly = true;

            foreach(var entry in StringHelper.HashTags.OrderByDescending(key => key.Value))
            {
                txtbox_trending.Text += "\n#" + entry.Key + " (used: " + entry.Value + " times)";
            }

            foreach(var entry in StringHelper.Mentions)
            {
                txtbox_mentions.Text += "\n@" + entry;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + @"\lists\SIRs.txt");
        }

        private void btn_quarantined_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + @"\lists\URLs.txt");
        }
    }
}
