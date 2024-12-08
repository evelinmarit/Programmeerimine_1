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

            var category6 = new Category();
            category6.Name = "Limited Edition T-Shirts";
            context.Categories.Add(category6);

            var category7 = new Category();
            category7.Name = "T-Shirts at the Customer`s Special Request";
            context.Categories.Add(category7);

            var category8 = new Category();
            category8.Name = "Special T-Shirts Made by an Artist";
            context.Categories.Add(category8);

            var product1 = new Product();
            product1.Name = "Simple Dark Red T-Shirt";
            product1.Description = "One colored dark red T-Shirt";
            product1.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product1.Category = category1;
            product1.Price = 10;
            product1.Quantity = 105;
            product1.AtStock = false;
            context.Products.Add(product1);

            var product2 = new Product();
            product2.Name = "Dark Blue Polo Shirt";
            product2.Description = "One colored dark Blue Polo Shirt";
            product2.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product2.Category = category2;
            product2.Price = 10;
            product2.Quantity = 13;
            product2.AtStock = true;
            context.Products.Add(product2);

            var product3 = new Product();
            product3.Name = "Light Red T-Shirt";
            product3.Description = "One colored light red T-Shirt";
            product3.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product3.Category = category1;
            product3.Price = 10;
            product3.Quantity = 20;
            product3.AtStock = true;
            context.Products.Add(product3);

            var product4 = new Product();
            product4.Name = "Dark Blue T-Shirt";
            product4.Description = "One colored dark blue T-Shirt";
            product4.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product4.Category = category1;
            product4.Price = 9;
            product4.Quantity = 23;
            product4.AtStock = true;
            context.Products.Add(product4);

            var product5 = new Product();
            product5.Name = "Funny T-Shirt for Daddies";
            product5.Description = "Funny T-Shirt for Daddies with Daddies`Day picture";
            product5.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product5.Category = category5;
            product5.Price = 15;
            product5.Quantity = 56;
            product5.AtStock = false;
            context.Products.Add(product5);

            var product6 = new Product();
            product6.Name = "Funny T-Shirt for Daddies";
            product6.Description = "We can make T-shirt according to the customer's special request. Customer can send a picture, photo, tell preferred color of T-shirt.";
            product6.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product6.Category = category7;
            product6.Price = 25;
            product6.Quantity = 15;
            product6.AtStock = true;
            context.Products.Add(product6);

            var product7 = new Product();
            product7.Name = "T-Shirt with Landscape Picture";
            product7.Description = "Light Green T-Shirt with Landscape Picture made by RR";
            product7.PhotoUrl = "https://www.tamectrade.ee/userfiles/image/products/bigThumb/e675464b-828c-4988-8517-e90176c8758c.png";
            product7.Category = category8;
            product7.Price = 30;
            product7.Quantity = 4;
            product7.AtStock = true;
            context.Products.Add(product7);

            var buyer1 = new Buyer();
            buyer1.Name = "Mattis Matikas";
            buyer1.ShippingAddress = "Paadi 4-5, Tallinn";
            buyer1.Email = "mattismatikas@gmail.com";
            buyer1.PhoneNumber = 55666666;
            buyer1.RegisteredDate = DateTime.Now;
            buyer1.LoyaltyPoints = 0;
            buyer1.Discount = 0;
            context.Buyers.Add(buyer1);

            var buyer2 = new Buyer();
            buyer2.Name = "Juta Jutukas";
            buyer2.ShippingAddress = "Aia 4, Põlva";
            buyer2.Email = "jutajutukas@gmail.com";
            buyer2.PhoneNumber = 55555555;
            buyer2.RegisteredDate = DateTime.Now;
            buyer2.LoyaltyPoints = 5;
            buyer2.Discount = 5;
            context.Buyers.Add(buyer2);

            var buyer3 = new Buyer();
            buyer3.Name = "Miku Mikuke";
            buyer3.ShippingAddress = "Tamme 34-8, Tartu";
            buyer3.Email = "mikumikuke@gmail.com";
            buyer3.PhoneNumber = 4444444;
            buyer3.RegisteredDate = DateTime.Now;
            buyer3.LoyaltyPoints = 0;
            buyer3.Discount = 0;
            context.Buyers.Add(buyer3);

            var buyer4 = new Buyer();
            buyer4.Name = "Karl Karlsson";
            buyer4.ShippingAddress = "Linnu tee 45, Tallinn";
            buyer4.Email = "karlkarlsson@gmail.com";
            buyer4.PhoneNumber = 55777777;
            buyer4.RegisteredDate = DateTime.Now;
            buyer4.LoyaltyPoints = 0;
            buyer4.Discount = 0;
            context.Buyers.Add(buyer4);

            var buyer5 = new Buyer();
            buyer5.Name = "Anni Annukas";
            buyer5.ShippingAddress = "Anni tee 6, Tallinn";
            buyer5.Email = "anniannukas@mail.ee";
            buyer5.PhoneNumber = 777777777;
            buyer5.RegisteredDate = DateTime.Now;
            buyer5.LoyaltyPoints = 0;
            buyer5.Discount = 0;
            context.Buyers.Add(buyer5);

            var order1 = new Order();
            //order1.Id = 1;
            order1.OrderDate = DateTime.Now;
            order1.Status = "Delivered";
            order1.Buyer = buyer1;
            order1.OrderItems = new List<OrderItem>();
            context.Orders.Add(order1);

            var order2 = new Order();
            order2.OrderDate = DateTime.Now;
            order2.Status = "Delivered";
            order2.Buyer = buyer2;
            order2.OrderItems = new List<OrderItem>();
            context.Orders.Add(order2);

            var order3 = new Order();
            order3.OrderDate = DateTime.Now;
            order3.Status = "Delivered";
            order3.Buyer = buyer3;
            order3.OrderItems = new List<OrderItem>();
            context.Orders.Add(order3);

            var orderItem1 = new OrderItem();
            orderItem1.Product = product1;
            orderItem1.PriceAtOrderTime = 7;
            orderItem1.Quantity = 1;
            orderItem1.Order = order1;
            context.OrderItems.Add(orderItem1);

            var orderItem2 = new OrderItem();
            orderItem2.Product = product2;
            orderItem2.PriceAtOrderTime = 9;
            orderItem2.Quantity = 2;
            orderItem2.Order = order1;
            context.OrderItems.Add(orderItem2);

            var orderItem3 = new OrderItem();
            orderItem3.Product = product2;
            orderItem3.PriceAtOrderTime = 12;
            orderItem3.Quantity = 3;
            orderItem3.Order = order2;
            context.OrderItems.Add(orderItem3);

            var orderItem4 = new OrderItem();
            orderItem4.Product = product3;
            orderItem4.PriceAtOrderTime = 8;
            orderItem4.Quantity = 2;
            orderItem4.Order = order2;
            context.OrderItems.Add(orderItem4);

            var orderItem5 = new OrderItem();
            orderItem5.Product = product5;
            orderItem5.PriceAtOrderTime = 15;
            orderItem5.Quantity = 1;
            orderItem5.Order = order3;
            context.OrderItems.Add(orderItem5);

            context.SaveChanges();
        }
    }
}
