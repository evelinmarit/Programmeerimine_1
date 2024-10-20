using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public decimal PriceAtOrderTime { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
