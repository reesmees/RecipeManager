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
using EksamenM2E2017.Entities;
using EksamenM2E2017.DbAccess;

namespace EksamenM2E2017.Opskrifter
{
    /// <summary>
    /// Interaction logic for UpdateIngredient.xaml
    /// </summary>
    public partial class UpdateIngredient : Window
    {
        Ingredient oldIngredient;
        DBHandler handler;
        public UpdateIngredient(Ingredient ingredient, DBHandler handler)
        {
            InitializeComponent();
            this.handler = handler;
            TxtBoxIngredientName.Text = ingredient.Name;
            TxtBoxIngredientPrice.Text = ingredient.Price.ToString();
            CmbBoxIngredientTypes.ItemsSource = Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>();
            CmbBoxIngredientTypes.SelectedItem = ingredient.Type;
            oldIngredient = ingredient;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string updatedName = TxtBoxIngredientName.Text;
            decimal updatedPrice = decimal.Parse(TxtBoxIngredientPrice.Text);
            IngredientType updatedType = (IngredientType)CmbBoxIngredientTypes.SelectedItem;
            try
            {
                Ingredient updatedIngredient = new Ingredient(updatedPrice, updatedName, updatedType, oldIngredient.id);
                handler.UpdateIngredient(updatedIngredient);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occured.{Environment.NewLine}{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
