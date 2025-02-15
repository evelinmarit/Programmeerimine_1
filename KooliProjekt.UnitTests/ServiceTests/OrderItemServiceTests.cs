using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OrderItemServiceTests : ServiceTestBase
    {
        private readonly OrderItemService _service;

        public OrderItemServiceTests()
        {
            _service = new OrderItemService(DbContext);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 1, Name = "Test Product 1", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order);

            for (int i = 1; i <= 20; i++)
            {
                DbContext.OrderItems.Add(new OrderItem
                {
                    Id = i,
                    Product = product,
                    ProductId = product.Id,
                    PriceAtOrderTime = 10,
                    Quantity = 1,
                    Order = order,
                    OrderId = order.Id
                });
            }
                //DbContext.OrderItems.AddRange(
                //new OrderItem { Id = 1, Product = product, ProductId = product.Id, PriceAtOrderTime = 10, Quantity = 1, Order = order, OrderId = order.Id },
                //new OrderItem { Id = 2, Product = product, ProductId = product.Id, PriceAtOrderTime = 15, Quantity = 2, Order = order, OrderId = order.Id },
                //new OrderItem { Id = 3, Product = product, ProductId = product.Id, PriceAtOrderTime = 12, Quantity = 1, Order = order, OrderId = order.Id }
            //);
            await DbContext.SaveChangesAsync();

            var service = new OrderItemService(DbContext);

            // Act
            var result = await service.List(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(20, result.RowCount);       // Kontrollib, kas kõikide elementide koguarv on 3
            Assert.Equal(2, result.PageCount);      // Kontrollib, et lehekülgede arv oleks korrektne
            Assert.Equal(1, result.CurrentPage);    // Kontrollib, et hetkelehekülg on 1
            Assert.Equal(10, result.Results.Count);
        }

        [Fact]
        public async Task Get_ShouldReturnOrderItem_WhenOrderItemExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 1, Name = "Test Product 1", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order);
            DbContext.OrderItems.Add(new OrderItem { Id = 1, Product = product, ProductId = product.Id, PriceAtOrderTime = 10, Quantity = 1, Order = order, OrderId = order.Id });
            await DbContext.SaveChangesAsync();

            var service = new OrderItemService(DbContext);

            // Act
            var result = await service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.Quantity);
            Assert.Equal(10, result.PriceAtOrderTime);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenOrderItemDoesNotExist()
        {
            // Arrange
            var service = new OrderItemService(DbContext);

            // Act
            var result = await service.Get(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ListProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            DbContext.Products.AddRange(
                new Product { Id = 1, Name = "Test Product 1", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 },
                new Product {
                    Id = 2,
                    Name = "Test Product 2",
                    Description = "Test2",
                    PhotoUrl = "http://example.com/phone.jpg",
                    AtStock = true,
                    Category = category,
                    Price = 15 }
            );
            await DbContext.SaveChangesAsync();

            var service = new OrderItemService(DbContext);

            // Act
            var result = await service.ListProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Save_ShouldAddNewOrderItem_WhenIdIsZero()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 0, Name = "New Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, CategoryId = category.Id, Price = 25 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };                                                                                                                                        
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();                                                                                                                                             

            var service = new OrderItemService(DbContext);
            var orderItem = new OrderItem { Id = 0, Product = product, ProductId = product.Id, PriceAtOrderTime = 10, Quantity = 1, Order = order, OrderId = order.Id };

            // Act
            await service.Save(orderItem);

            // Assert
            var savedOrderItem = DbContext.OrderItems.FirstOrDefault(oi => oi.ProductId == product.Id);
            Assert.NotNull(savedOrderItem);
            Assert.Equal(10, savedOrderItem.PriceAtOrderTime);
            Assert.Equal(1, savedOrderItem.Quantity);
        }

        [Fact]
        public async Task Save_ShouldUpdateExistingProduct_WhenIdIsNotZero()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 0, Name = "New Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, CategoryId = category.Id, Price = 25 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order);

            var existingOrderItem = new OrderItem { Id = 1, Product = product, ProductId = product.Id, PriceAtOrderTime = 10, Quantity = 1, Order = order, OrderId = order.Id };
            DbContext.OrderItems.Add(existingOrderItem);
            await DbContext.SaveChangesAsync();

            var service = new OrderItemService(DbContext);
            existingOrderItem.Quantity = 1;
            existingOrderItem.PriceAtOrderTime = 10;

            // Act
            await service.Save(existingOrderItem);

            // Assert
            var updatedOrderItem = DbContext.OrderItems.FirstOrDefault(p => p.Id == 1);
            Assert.NotNull(updatedOrderItem);
            Assert.Equal(1, updatedOrderItem.Quantity);
            Assert.Equal(10, updatedOrderItem.PriceAtOrderTime);
        }

        [Fact]
        public async Task ListOrderItems_ShouldFilterByOrderId_WhenOrderSearchIsValidInteger()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 0, Name = "New Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, CategoryId = category.Id, Price = 25 };
            DbContext.Products.Add(product);
            var order1 = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order1);
            var order2 = new Order { Id = 2, Status = "Delivered" };
            DbContext.Orders.Add(order2);
            DbContext.OrderItems.Add(new OrderItem { Id = 1, Product = product, Order = order1, Quantity = 1, PriceAtOrderTime = 100.00M });
            DbContext.OrderItems.Add(new OrderItem { Id = 2, Product = product, Order = order2, Quantity = 2, PriceAtOrderTime = 50.00M });
            await DbContext.SaveChangesAsync();

            var search = new OrderItemSearch { OrderSearch = "1" };  // OrderId väärtus on "1"

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);  // Ainult üks tulemus peaks olema
            Assert.Equal(1, result.First().OrderId);
            Assert.Equal(1, result.First().OrderId);
        }

        [Fact]
        public async Task ListOrderItems_ShouldReturnAllOrderItems_WhenOrderSearchIsNullOrWhitespace()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 0, Name = "New Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, CategoryId = category.Id, Price = 25 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order);
            DbContext.OrderItems.Add(new OrderItem { Id = 1, Product = product, Order = order, Quantity = 1, PriceAtOrderTime = 100.00M });
            DbContext.OrderItems.Add(new OrderItem { Id = 2, Product = product, Order = order, Quantity = 2, PriceAtOrderTime = 50.00M });
            await DbContext.SaveChangesAsync();

            var search = new OrderItemSearch { OrderSearch = "   " };  // Tühjad või tühikud

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.ToList().Count);
        }

        [Fact]
        public async Task ListOrders_ShouldReturnAllOrders()
        {
            // Arrange
            var order1 = new Order { Id = 1, OrderDate = DateTime.UtcNow, Status = "Pending" };
            var order2 = new Order { Id = 2, OrderDate = DateTime.UtcNow, Status = "Delivered" };

            DbContext.Orders.Add(order1);
            DbContext.Orders.Add(order2);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.ListOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => o.Status == "Pending");
            Assert.Contains(result, o => o.Status == "Delivered");
        }

        [Fact]
        public async Task Delete_should_remove_existing_orderItem()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            var product = new Product { Name = "Test", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            var orderItem = new OrderItem { Id = 1, Product = product, PriceAtOrderTime = 10, Quantity = 1 };
            DbContext.OrderItems.Add(orderItem);
            DbContext.SaveChanges();

            // Act
            await _service.Delete(orderItem.Id);

            // Assert
            var count = DbContext.OrderItems.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task Delete_should_return_if_orderItem_was_not_found()
        {
            // Arrange
            var id = -100;

            // Act
            await _service.Delete(id);

            // Assert
            var count = DbContext.OrderItems.Count();
            Assert.Equal(0, count);
        }
    }
}
