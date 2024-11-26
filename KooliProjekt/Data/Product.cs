using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string PhotoUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
