using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.Opskrifter
{
    public class TestRecipeClass
    {
        private List<TestIngredientClass> ingredientList;
        private string recipeName;




        public TestRecipeClass(List<TestIngredientClass> ingredients, string name)
        {
            IngredientList = ingredients;
            RecipeName = name;
        }


        public string RecipeName
        {
            get { return recipeName; }
            set { recipeName = value; }
        }

        public List<TestIngredientClass> IngredientList
        {
            get { return ingredientList; }
            set { ingredientList = value; }
        }


        public override string ToString()
        {
            return RecipeName;
        }
    }
}
