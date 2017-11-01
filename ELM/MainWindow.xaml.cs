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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ELM.Model;

namespace ELM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Message m;
            try
            {
                m = new Message(txtbox_header.Text.ToString(), txtbox_body.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            ValidateWindow validate = new ValidateWindow(m);
            validate.ShowDialog();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            txtbox_header.Clear();
            txtbox_body.Clear();
        }
    }
}
