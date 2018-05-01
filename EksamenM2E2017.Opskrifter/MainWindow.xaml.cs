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
using EksamenM2E2017.DbAccess;
using EksamenM2E2017.Entities;
using EksamenM2E2017.Services;

namespace EksamenM2E2017.Opskrifter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DBHandler handler;
        public List<Ingredient> ingredients;
        public List<Recipe> recipes;
        public List<string> recipeNames;
        public MainWindow()
        {
            InitializeComponent();
            handler = new DBHandler(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = EksamenM2E2017.OpskrifterDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            ingredients = handler.GetAllIngredients();
            recipes = handler.GetAllRecipes();
            recipeNames = new List<string>();
            foreach (Recipe r in recipes)
            {
                recipeNames.Add(r.Name);
            }
            DtgAllIngredients.ItemsSource = ingredients;

            ListBoxRecipeList.ItemsSource = recipes;
        }

        private void BtnAddNewRecipe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxRecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recipe r = (Recipe)ListBoxRecipeList.SelectedItem;
            DtgIngredientsInSelectedRecipe.ItemsSource = r.Ingredients;
            TxtBoxPersons.Text = r.Persons.ToString();
            TxtBoxPrice.Text = r.GetPrice().ToString();
        }
    }
}
