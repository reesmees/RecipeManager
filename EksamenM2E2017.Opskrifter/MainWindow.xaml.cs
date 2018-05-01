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
        public Recipe newRecipe;
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
            newRecipe = new Recipe("Placeholder", new List<Ingredient>(), 0);
            foreach (Recipe r in recipes)
            {
                recipeNames.Add(r.Name);
            }
            DtgAllIngredients.ItemsSource = ingredients;
            DtgAddIngredients.ItemsSource = ingredients;
            DtgItemsInNewRecipe.ItemsSource = newRecipe.Ingredients;
            ListBoxRecipeList.ItemsSource = recipeNames;
            CmbBoxIngredientTypes.ItemsSource = Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>();
        }

        private void ListBoxRecipeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recipe r = recipes.Find(x => x.Name.Equals(ListBoxRecipeList.SelectedItem.ToString()));
            DtgIngredientsInSelectedRecipe.ItemsSource = r.Ingredients;
            TxtBoxPersons.Text = r.Persons.ToString();
            TxtBoxPrice.Text = r.GetPrice().ToString();
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
            else if (newRecipe.Ingredients.Exists(x => x.Equals(DtgAllIngredients.SelectedItem)))
            {
                MessageBox.Show("You cannot add the same ingredient multiple times.");
            }
            else
            {
                newRecipe.Ingredients.Add((Ingredient)DtgAllIngredients.SelectedItem);
                DtgItemsInNewRecipe.Items.Refresh();
                LblTotalPrice.Content = newRecipe.GetPrice();
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
                newRecipe.Ingredients.Remove((Ingredient)DtgItemsInNewRecipe.SelectedItem);
                DtgItemsInNewRecipe.Items.Refresh();
                LblTotalPrice.Content = newRecipe.GetPrice();
            }
        }

        private void BtnAddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TxtBoxRecipeName.Text))
            {
                MessageBox.Show("Please enter the name of the recipe.");
            }
            else if (String.IsNullOrWhiteSpace(TxtBoxCountOfPersonsInRecipe.Text))
            {
                MessageBox.Show("Please enter how many people the recipe will feed.");
            }
            else
            {
                try
                {
                    newRecipe.Name = TxtBoxRecipeName.Text;
                    newRecipe.Persons = int.Parse(TxtBoxCountOfPersonsInRecipe.Text);
                    handler.InsertRecipe(newRecipe);
                    newRecipe = new Recipe("Placeholder", new List<Ingredient>(), 0);
                    DtgItemsInNewRecipe.ItemsSource = newRecipe.Ingredients;
                    TxtBoxCountOfPersonsInRecipe.Clear();
                    TxtBoxRecipeName.Clear();
                    LblTotalPrice.Content = newRecipe.GetPrice();
                    UpdateSources();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error has occured.{Environment.NewLine}{Environment.NewLine}{ex.Message}");
                }
            }
        }
    }
}
