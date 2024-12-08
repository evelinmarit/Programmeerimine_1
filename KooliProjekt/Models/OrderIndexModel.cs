using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OrderIndexModel
    {
        public OrderSearch Search { get; set; }
        public PagedResult<Order> Data { get; set; }
    }
}
