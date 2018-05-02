using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.Entities
{
    public enum IngredientType
    {
        Dairy,
        Flour,
        Meat,
        Vegetable,
        Fish,
        RootVegetable,
        Rice,
        Pasta,
        Conserves,
        Mushroom
    }
    public class Ingredient
    {
        public readonly int id;
        private IngredientType type;
        private string name;
        private decimal price;

        public Ingredient(decimal price, string name, IngredientType type)
        {
            Type = type;
            Price = price;
            Name = name;
        }

        public Ingredient(decimal price, string name, IngredientType type, int id)
        {
            Type = type;
            Price = price;
            Name = name;
            this.id = id;
        }

        public decimal Price
        {
            get { return price; }
            set {
                if (value <= 0)
                    throw new ArgumentException("Price needs to be a possitive number.");
                price = value;
            }
        }

        public string Name
        {
            get { return name; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                name = value;
            }
        }

        public IngredientType Type
        {
            get { return type; }
            set {
                if (!Enum.IsDefined(typeof(IngredientType), value))
                    throw new ArgumentException("Not a valid ingredient type");
                type = value;
            }
        }

        public override string ToString()
        {
            return $"{Name}(s) is a {Type} and costs {Price} kr. per kilo.";
        }

    }
}
