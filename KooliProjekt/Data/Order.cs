using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime OrderDate { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [Required]
        public Buyer Buyer { get; set; }
        public int BuyerId { get; set; }
        [Required]
        public IList<OrderItem> OrderItems { get; set; }
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public decimal TotalAmount
        {
            get
            {
                return OrderItems.Sum(item => item.PriceAtOrderTime * item.Quantity); // Kogusumma
            }
        }

        //public void MarkAsPaid()
        //{
        //    Status = "Paid";
        //}

        //public void MarkAsDelivered()
        //{
        //    if (Status == "Paid")
        //    {
        //        Status = "Delivered";
        //    }
        //}
    }
}
