using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Category
    {
        //internal object Item;
        //public ICollection<Product> Items { get; set; } = new List<Product>();
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
