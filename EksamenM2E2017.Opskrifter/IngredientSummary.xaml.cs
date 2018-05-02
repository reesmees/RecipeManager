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
using EksamenM2E2017.Services;

namespace EksamenM2E2017.Opskrifter
{
    /// <summary>
    /// Interaction logic for IngredientSummary.xaml
    /// </summary>
    public partial class IngredientSummary : Window
    {
        public ApiAccess Api = new ApiAccess();
        public IngredientSummary(string searchTerm)
        {
            InitializeComponent();
            try
            {
                TxtBlkSummary.Text = Api.GetSummary(searchTerm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurd.{Environment.NewLine}{Environment.NewLine}{ex.Message}");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
