using FTN.Common;
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

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestGda testGda = null;
        public string SelectedGid { get; set; }
        public MainWindow()
        {
            testGda = new TestGda();
            InitializeComponent();
            gidCmb.ItemsSource = testGda.GetGids();
        }

        private void buttonGetValues_Click(object sender, RoutedEventArgs e)
        {
            MainWindow getValuesWindow = new MainWindow();
            this.Close();
            getValuesWindow.Show();
        }

        private void buttonGetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            GetExtentValues getExtentValueswindow = new GetExtentValues();
            this.Close();
            getExtentValueswindow.Show();
        }

        private void buttonGetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            GetRelatedValues getRelatedValuesWindow = new GetRelatedValues();
            this.Close();
            getRelatedValuesWindow.Show();
        }

        private void gidCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedGid = gidCmb.SelectedItem.ToString();
            getValuesButton.Visibility = Visibility.Visible;
            selectAllCheckBox.Visibility = Visibility.Visible;
            propListBox.Visibility = Visibility.Visible;
            propListBox.ItemsSource = testGda.GetProperties(SelectedGid);
        }

        private void getValuesButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGid != String.Empty && propListBox.SelectedItems.Count > 0)
            {
                long globalId = Convert.ToInt64(Int64.Parse(SelectedGid.Remove(0, 2), System.Globalization.NumberStyles.HexNumber));
                List<ModelCode> properties = new List<ModelCode>();

                foreach (var item in propListBox.SelectedItems)
                {
                    properties.Add((ModelCode)item);
                }

                try
                {
                    resultTb.Text = testGda.GetValues(globalId, properties);
                }
                catch (Exception ex)
                {
                    string message = string.Format("GetValues failed. {0}", ex.Message);
                    Console.WriteLine(message);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                }

            }
        }

        private void selectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (selectAllCheckBox.IsChecked == true)
            {
                propListBox.SelectAll();
            }
            else
            {
                propListBox.UnselectAll();
            }
        }
    }
}
