using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.Entities
{
    public class Recipe
    {
        private List<Ingredient> ingredients;
        private string name;
        private int persons;
        public readonly int id;

        public Recipe(string name, List<Ingredient> ingredients, int persons)
        {
            Name = name;
            Ingredients = ingredients;
            Persons = persons;
        }

        public Recipe(string name, List<Ingredient> ingredients, int persons, int id)
        {
            Name = name;
            Ingredients = ingredients;
            Persons = persons;
            this.id = id;
        }

        public int Persons
        {
            get { return persons; }
            set {
                if (value < 1)
                    throw new ArgumentException("A recipe cannot feed less than one person.");
                persons = value;
            }
        }

        public string Name
        {
            get { return name; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A recipe cannot be unnamed.");
                if (value.Contains("'"))
                    throw new ArgumentException("A recipe name cannot contain the ' character.");
                name = value;
            }
        }

        public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }

        public List<IngredientType> GetIngredientTypes()
        {
            List<IngredientType> types = new List<IngredientType>();
            foreach (Ingredient i in Ingredients)
            {
                types.Add(i.Type);
            }
            return types;
        }

        public decimal GetPrice()
        {
            decimal price = 0;
            foreach (Ingredient i in Ingredients)
            {
                price += i.Price;
            }
            return price;
        }

        public decimal GetPricePerPerson()
        {
            return GetPrice() / persons;
        }

        public override string ToString()
        {
            return $"{Name} can feed {persons} persons, and costs {GetPrice()} kr. to make.";
        }
    }
}
