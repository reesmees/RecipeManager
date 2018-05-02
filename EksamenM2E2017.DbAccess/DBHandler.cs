using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamenM2E2017.Entities;

namespace EksamenM2E2017.DbAccess
{
    public class DBHandler : CommonDB
    {
        public DBHandler(string connectionString) : base(connectionString) { }

        public List<Ingredient> GetAllIngredients()
        {
            DataSet data = ExecuteQuery("SELECT * FROM Ingredients");
            List<Ingredient> ingredients = CreateIngredientsFromDataSet(data);
            return ingredients;
        }

        public List<Recipe> GetAllRecipes()
        {
            DataSet data = ExecuteQuery("SELECT * FROM Recipes");
            List<Recipe> recipes = CreateRecipesFromDataSet(data);
            return recipes;
        }

        public Ingredient GetIngredientByName(string name)
        {
            DataSet data = ExecuteQuery($"SELECT * FROM Ingredients WHERE [Name]='{name}'");
            Ingredient ingredient = CreateIngredientsFromDataSet(data)[0];
            return ingredient;
        }

        public Recipe GetRecipeByName(string name)
        {
            DataSet data = ExecuteQuery($"SELECT * FROM Recipes WHERE [Name]='{name}'");
            Recipe recipe = CreateRecipesFromDataSet(data)[0];
            return recipe;
        }

        public void InsertIngredient(Ingredient ingredient)
        {
            ExecuteNonQuery($"INSERT INTO Ingredients ([Name], Price, [Type]) VALUES ('{ingredient.Name}', {ingredient.Price}, '{ingredient.Type}')");
        }

        public void InsertRecipe(Recipe recipe)
        {
            if (recipe.Ingredients.Count() < 1)
                throw new ArgumentException("A recipe cannot have less than one ingredient.");
            ExecuteNonQuery($"INSERT INTO Recipes ([Name], Persons) VALUES ('{recipe.Name}', {recipe.Persons})");
            Recipe r = GetRecipeByName(recipe.Name);
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (Ingredient i in recipe.Ingredients)
            {
                ingredients.Add(GetIngredientByName(i.Name));
            }
            foreach (Ingredient i in ingredients)
            {
                ExecuteNonQuery($"INSERT INTO RecipeVsIngredients (RecipeID, IngredientID) VALUES ({r.id}, {i.id})");
            }
        }

        public List<Ingredient> CreateIngredientsFromDataSet(DataSet data)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                decimal price = row.Field<decimal>("Price");
                string name = row.Field<string>("Name");
                string typeString = row.Field<string>("Type");
                IngredientType type = (IngredientType)Enum.Parse(typeof(IngredientType), typeString);
                int id = row.Field<int>("ID");
                ingredients.Add(new Ingredient(price, name, type, id));
            }
            return ingredients;
        }

        public List<Recipe> CreateRecipesFromDataSet(DataSet data)
        {
            List<Recipe> recipes = new List<Recipe>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                string name = row.Field<string>("Name");
                List<Ingredient> ingredients = new List<Ingredient>();
                int persons = row.Field<int>("Persons");
                int id = row.Field<int>("id");
                Recipe r = new Recipe(name, ingredients, persons, id);
                recipes.Add(r);
            }
            DataSet ingredientData = ExecuteQuery("GetIngredientsByRecipe", CommandType.StoredProcedure);
            foreach (DataRow row in ingredientData.Tables[0].Rows)
            {
                int recipeID = row.Field<int>("RecipeID");
                if (recipes.Exists(x => x.id.Equals(recipeID)))
                {
                    string name = row.Field<string>("IngredientName");
                    decimal price = row.Field<decimal>("IngredientPrice");
                    string typeString = row.Field<string>("IngredientType");
                    IngredientType type = (IngredientType)Enum.Parse(typeof(IngredientType), typeString);
                    int ingredientID = row.Field<int>("IngredientID");
                    Ingredient ingredient = new Ingredient(price, name, type, ingredientID);
                    recipes.Find(x => x.id.Equals(recipeID)).Ingredients.Add(ingredient);
                }
            }
            foreach (Recipe r in recipes)
            {
                if (r.Ingredients.Count() < 1)
                    recipes.Remove(r);
            }
            return recipes;
        }
    }
}
