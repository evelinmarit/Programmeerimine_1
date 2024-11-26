using KooliProjekt.Controllers;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Drawing;
using static System.Net.WebRequestMethods;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }

            var category1 = new Category();
            category1.Name = "One Colored T-Shirts";
            context.Categories.Add(category1);

            var category2 = new Category();
            category2.Name = "Polo Shirts";
            context.Categories.Add(category2);

            var category3 = new Category();
            category3.Name = "Funny T-Shirts";
            context.Categories.Add(category3);

            var category4 = new Category();
            category4.Name = "Party T-Shirts";
            context.Categories.Add(category4);

            var category5 = new Category();
            category5.Name = "Celebration T-Shirts";
            context.Categories.Add(category5);

            var product1 = new Product();
            product1.Name = "Simple Red T-Shirt";
            product1.Description = "One colored dark red T-Shirt";
            product1.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product1.Category = category1;
            product1.Price = 10;
            product1.Quantity = 15;
            context.Products.Add(product1);

            var product2 = new Product();
            product2.Name = "Dark Blue Polo Shirt";
            product2.Description = "One colored dark Blue Polo Shirt";
            product2.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product2.Category = category2;
            product2.Price = 10;
            product2.Quantity = 5;
            context.Products.Add(product2);

            var product3 = new Product();
            product3.Name = "Red T-Shirt";
            product3.Description = "One colored dark red T-Shirt";
            product3.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product3.Category = category3;
            product3.Price = 10;
            product3.Quantity = 20;
            context.Products.Add(product3);

            var orderItem1 = new OrderItem();         
            orderItem1.ProductId = 1;
            orderItem1.Product = product1;
            orderItem1.PriceAtOrderTime = 7;
            orderItem1.Quantity = 1;
            context.OrderItems.Add(orderItem1);



            context.SaveChanges();
        }
    }
}
