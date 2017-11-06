using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.Opskrifter
{
    public enum IngredientType
    {
        Fisk,
        Oksekød,
        Grøntsag,
        Frugt,
        Mejeriprodukter,
        Mel,
        Kolonial
    }

    public class TestIngredientClass
    {
        private int price;
        private string name;
        private IngredientType type;

        public TestIngredientClass(IngredientType type, string name, int price)
        {
            Type = type;
            Name = name;
            Price = price;
        }

        public int Price { get => price; set => price = value; }
        public string Name { get => name; set => name = value; }
        public IngredientType Type { get => type; set => type = value; }
    }
}
