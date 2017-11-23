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
using Microsoft.Win32;
using System.IO;

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
            txtbox_body.AcceptsReturn = true; //allow the user to move to a new line in the text box
            FileParser.Initialise(); //initalise the text speak dictionary
        }

        //when the add message button is clicked
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Message m = null;
            string[] body = new string[30];

            try
            {
                // For each line in the text box
                for (int i = 0; i < txtbox_body.LineCount; i++)
                {
                    //store it in an array
                    body[i] = txtbox_body.GetLineText(i);
                }
                m = new Message(txtbox_header.Text.ToString(), body); //create a message
            }
           catch (Exception ex) { 
               //display errors if any 
               MessageBox.Show(ex.Message);
               return;
            }

            ClearText(); 

            ValidateWindow validate = new ValidateWindow(m); //create a new validate window
            validate.ShowDialog(); //display it
        }

        //clear both text boxes when clear button is clicked
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearText();
        }

        //clears both text boxes in the window
        private void ClearText()
        {
            txtbox_header.Clear();
            txtbox_body.Clear();
        }

        //when open file button is clicked
        private void btn_openFile_Click(object sender, RoutedEventArgs e)
        {
            //allow the user to select a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileParser.ReadMessages(openFileDialog.FileName); //try reading the selected file
                }
                catch (Exception ex)
                {
                    //display errors if any
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            //if everything is ok, display each message in a text box
            foreach(var m in FileParser.Messages)
            {
                MessageBox.Show(m.ToString(), "Processed message");
            }

            //reset 
            FileParser.Reset();
        }

        //display the stats window
        private void btn_stats_Click(object sender, RoutedEventArgs e)
        {
            StatsWindow sw = new StatsWindow();
            sw.ShowDialog();
        }
    }
}
