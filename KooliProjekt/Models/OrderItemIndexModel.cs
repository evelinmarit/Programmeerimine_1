using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OrderItemIndexModel
    {
        public OrderItemSearch Search { get; set; }
        public PagedResult<OrderItem> Data { get; set; }
    }
}
