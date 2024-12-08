using KooliProjekt.Data.Migrations;
using KooliProjekt.Search;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KooliProjekt.Models
{
    public class ProductIndexModel
    {
        public ProductSearch Search { get; set; }
        public PagedResult<Product> Data { get; set; }
    }
}
