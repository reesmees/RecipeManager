using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamenM2E2017.Entities;
using EksamenM2E2017.DbAccess;
using EksamenM2E2017.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.Entities.Tests
{
    [TestClass]
    public class RecipeTests
    {
        [TestMethod]
        public void ConstructorValidArgumentTest()
        {
            string name = "Tomato soup";
            int persons = 2;
            List<Ingredient> ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient(4.95m, "Chopped tomatoes", IngredientType.Vegetable));
            ingredients.Add(new Ingredient(14.95m, "Red lentils", IngredientType.Legume));
            ingredients.Add(new Ingredient(3.35m, "Vegetable stock", IngredientType.Vegetable));

            Recipe recipe = new Recipe(name, ingredients, persons);

            Assert.AreEqual("Tomato soup", recipe.Name);
            Assert.AreEqual("Chopped tomatoes", recipe.Ingredients[0].Name);
            Assert.AreEqual(23.25m, recipe.GetPrice());
        }

        [TestMethod][ExpectedException(typeof(ArgumentException))]
        public void ConstructorInvalidArgumentTest()
        {
            string name = "";
            int persons = 2;
            List<Ingredient> ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient(15, "Leek", IngredientType.Vegetable));

            Recipe recipe = new Recipe(name, ingredients, persons);
        }
    }

    [TestClass]
    public class DBTest
    {
        DBHandler handler = new DBHandler(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = EksamenM2E2017.OpskrifterDB; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
        [TestMethod]
        public void GetRecipeTest()
        {
            List<Recipe> recipes = handler.GetAllRecipes();

            Assert.AreEqual(9, recipes[0].Ingredients[2].id);
        }

        [TestMethod]
        public void GetIngredientsTest()
        {
            List<Ingredient> ingredients = handler.GetAllIngredients();

            Assert.AreEqual(2002, ingredients[11].id);
        }
    }
}