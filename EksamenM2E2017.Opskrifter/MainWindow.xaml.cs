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
        public List<Ingredient> ingredientsInNewRecipe;
        public List<Recipe> recipes;
        public List<string> recipeNames;
        public MainWindow()
        {
            InitializeComponent();
            handler = new DBHandler(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = EksamenM2E2017.OpskrifterDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            ingredients = handler.GetAllIngredients();
            recipes = handler.GetAllRecipes();
            recipeNames = new List<string>();
            ingredientsInNewRecipe = new List<Ingredient>();
            foreach (Recipe r in recipes)
            {
                recipeNames.Add(r.Name);
            }
            DtgAllIngredients.ItemsSource = ingredients;
            DtgAddIngredients.ItemsSource = ingredients;
            DtgItemsInNewRecipe.ItemsSource = ingredientsInNewRecipe;
            ListBoxRecipeList.ItemsSource = recipeNames;
            CmbBoxIngredientTypes.ItemsSource = Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>();
        }

        private void ListBoxRecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recipe r = recipes.Find(x => x.Name.Equals(ListBoxRecipeList.SelectedItem.ToString()));
            DtgIngredientsInSelectedRecipe.ItemsSource = r.Ingredients;
            TxtBlkPersons.Text = r.Persons.ToString();
            TxtBlkPrice.Text = r.GetPrice().ToString();
        }

        private void BtnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ingredient ingredient = new Ingredient(decimal.Parse(TxtBoxIngredientPrice.Text), TxtBoxIngredientName.Text, (IngredientType)CmbBoxIngredientTypes.SelectedItem);
                handler.InsertIngredient(ingredient);
                UpdateSources();
                TxtBoxIngredientName.Clear();
                TxtBoxIngredientPrice.Clear();
                CmbBoxIngredientTypes.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occured.{Environment.NewLine}{Environment.NewLine}{ex.Message}");
            }
        }

        public void UpdateSources()
        {
            ingredients = handler.GetAllIngredients();
            recipes = handler.GetAllRecipes();
            recipeNames = new List<string>();
            foreach (Recipe r in recipes)
            {
                recipeNames.Add(r.Name);
            }
            DtgAllIngredients.ItemsSource = ingredients;
            DtgAddIngredients.ItemsSource = ingredients;
            ListBoxRecipeList.ItemsSource = recipeNames;
        }

        private void BtnMoveItemRight_Click(object sender, RoutedEventArgs e)
        {
            if (DtgAllIngredients.SelectedItem == null)
            {
                MessageBox.Show("Please select an ingredient to add to the recipe.");
            }
            else if (ingredientsInNewRecipe.Exists(x => x.Equals(DtgAllIngredients.SelectedItem)))
            {
                MessageBox.Show("You cannot add the same ingredient multiple times.");
            }
            else
            {
                ingredientsInNewRecipe.Add((Ingredient)DtgAllIngredients.SelectedItem);
                DtgItemsInNewRecipe.Items.Refresh();
                LblTotalPrice.Content = CalculatePrice(ingredientsInNewRecipe);
            }
        }

        private void BtnMoveItemLeft_Click(object sender, RoutedEventArgs e)
        {
            if (DtgItemsInNewRecipe.SelectedItem == null)
            {
                MessageBox.Show("Please select an ingredient to remove from the recipe.");
            }
            else
            {
                ingredientsInNewRecipe.Remove((Ingredient)DtgItemsInNewRecipe.SelectedItem);
                DtgItemsInNewRecipe.Items.Refresh();
                LblTotalPrice.Content = CalculatePrice(ingredientsInNewRecipe);
            }
        }

        private void BtnAddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string newRecipeName = TxtBoxRecipeName.Text;
                int newRecipePersons = int.Parse(TxtBoxCountOfPersonsInRecipe.Text);
                Recipe newRecipe = new Recipe(newRecipeName, ingredientsInNewRecipe, newRecipePersons);
                handler.InsertRecipe(newRecipe);
                ingredientsInNewRecipe.Clear();
                DtgItemsInNewRecipe.Items.Refresh();
                TxtBoxCountOfPersonsInRecipe.Clear();
                TxtBoxRecipeName.Clear();
                LblTotalPrice.Content = CalculatePrice(ingredientsInNewRecipe);
                UpdateSources();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occured.{Environment.NewLine}{Environment.NewLine}{ex.Message}");
            }
        }

        public decimal CalculatePrice(List<Ingredient> ingredients)
        {
            decimal price = 0;
            foreach (Ingredient i in ingredients)
            {
                price += i.Price;
            }
            return price;
        }
    }
}
