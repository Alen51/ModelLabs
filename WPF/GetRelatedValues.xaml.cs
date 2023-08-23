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
    /// Interaction logic for GetRelatedValues.xaml
    /// </summary>
    public partial class GetRelatedValues : Window
    {
        TestGda testGda;
        public string SelectedGid { get; set; }
        public long ConvertedGid { get; set; }
        public ModelCode SelectedAssociation { get; set; }
        public ModelCode SelectedType { get; set; }
        public GetRelatedValues()
        {
            testGda = new TestGda();
            InitializeComponent();
            gidCmb.ItemsSource = testGda.GetGids();
            SelectedType = 0;
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

        private void gidCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resultTb.Text = "";
            selectAllCheckBox.IsChecked = false;
            propListBox.UnselectAll();
            propListBox.ItemsSource = new List<ModelCode>();
            associationCmb.ItemsSource = new List<ModelCode>();
            typeCmb.ItemsSource = new List<ModelCode>();
            SelectedGid = gidCmb.SelectedItem.ToString();
            ConvertedGid = Convert.ToInt64(Int64.Parse(SelectedGid.Remove(0, 2), System.Globalization.NumberStyles.HexNumber));
            associationCmb.ItemsSource = testGda.GetReferencePropertyIds((DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(ConvertedGid));
        }

        private void associationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (associationCmb.SelectedItem == null)
            {
                return;
            }

            resultTb.Text = "";
            selectAllCheckBox.IsChecked = false;
            propListBox.UnselectAll();
            propListBox.ItemsSource = new List<ModelCode>();
            SelectedAssociation = (ModelCode)associationCmb.SelectedItem;
            List<ModelCode> relatedEntities = testGda.GetReferencedEntities(ConvertedGid, SelectedAssociation);
            List<ModelCode> properties = new List<ModelCode>();

            if (relatedEntities.Count == 0)
            {
                resultTb.Text = $"There are no related values for GID {ConvertedGid} and association {SelectedAssociation}.";
                return;
            }

            ModelCode x = relatedEntities[0];
            int i = 0;
            List<ModelCode> entities = new List<ModelCode>();
            foreach (var entity in relatedEntities)
            {
                if (i != 0 && x == entity)
                {
                    continue;
                }
                var res = testGda.GetProperties(entity);
                properties = properties.Concat(res).ToList();
                entities.Add(entity);
                x = entity;
                i++;
            }

            typeCmb.ItemsSource = entities;
            propListBox.Visibility = Visibility.Visible;
            selectAllCheckBox.Visibility = Visibility.Visible;
            getRelatedValuesButton.Visibility = Visibility.Visible;
            propListBox.ItemsSource = properties;
        }

        private void typeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeCmb.SelectedItem != null)
            {
                SelectedType = (ModelCode)typeCmb.SelectedItem;
            }

            selectAllCheckBox.IsChecked = false;
            propListBox.ItemsSource = testGda.GetProperties(SelectedType);
        }

        private void getRelatedValuesButton_Click(object sender, RoutedEventArgs e)
        {
            if (propListBox.SelectedItems.Count > 0)
            {
                Association association = new Association();
                association.PropertyId = SelectedAssociation;
                association.Type = SelectedType;

                List<ModelCode> properties = new List<ModelCode>();
                foreach (var item in propListBox.SelectedItems)
                {
                    properties.Add((ModelCode)item);
                }

                try
                {
                    resultTb.Text = testGda.GetRelatedValues(ConvertedGid, properties, association);
                }
                catch (Exception ex)
                {
                    string message = string.Format("GetRelatedValues failed. {0}", ex.Message);
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
