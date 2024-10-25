using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Buyer
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public List<Order>? Orders { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int LoyaltyPoints { get; set; }
        public int Discount { get; set; }
    } 
}
