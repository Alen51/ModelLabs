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
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for GetExtentValues.xaml
    /// </summary>
    public partial class GetExtentValues : Window
    {
        TestGda testGda;
        public ModelCode SelectedModelCode { get; set; }
        public GetExtentValues()
        {
            testGda = new TestGda();
            InitializeComponent();
            modelCodeCmb.ItemsSource = testGda.GetModelCodes();
        }

        private void buttonGetValues_Click(object sender, RoutedEventArgs e)
        {
            MainWindow getValuesWindow = new MainWindow();
            this.Close();
            getValuesWindow.Show();
        }

        private void buttonGetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            GetExtentValues getExtentValuesWindow = new GetExtentValues();
            this.Close();
            getExtentValuesWindow.Show();
        }

        private void buttonGetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            GetRelatedValues getRelatedValuesWindow = new GetRelatedValues();
            this.Close();
            getRelatedValuesWindow.Show();
        }

        private void modelCodeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectAllCheckBox.IsChecked = false;
            propListBox.UnselectAll();
            SelectedModelCode = (ModelCode)modelCodeCmb.SelectedItem;
            getExtentValuesButton.Visibility = Visibility.Visible;
            selectAllCheckBox.Visibility = Visibility.Visible;
            propListBox.ItemsSource = testGda.GetProperties(SelectedModelCode);
            propListBox.Visibility = Visibility.Visible;
        }

        private void getExtentValuesButton_Click(object sender, RoutedEventArgs e)
        {
            if (propListBox.SelectedItems.Count > 0)
            {
                List<ModelCode> properties = new List<ModelCode>();
                foreach (var item in propListBox.SelectedItems)
                {
                    properties.Add((ModelCode)item);
                }

                try
                {
                    resultTb.Text = testGda.GetExtentValues(SelectedModelCode, properties);
                }
                catch (Exception ex)
                {
                    string message = string.Format("GetExtentValues failed. {0}", ex.Message);
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
