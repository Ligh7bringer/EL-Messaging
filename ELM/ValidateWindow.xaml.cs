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
    /// Interaction logic for ValidateWindow.xaml
    /// </summary>
    public partial class ValidateWindow : Window
    {
        internal ValidateWindow(Message m)
        {
            InitializeComponent();
            txtbox_message.Text = m.CurrentState.ToString();
        }
    }
}
