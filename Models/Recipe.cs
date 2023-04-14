
namespace OMC.Models
{
    public class Recipe
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public int ProductID { get; set; }
        public int Syrup { get; set; }
        public int Milk { get; set; }
        public int Water { get; set; }
        public Product? Product { get; set; }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
