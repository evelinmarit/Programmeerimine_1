using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class OrderServiceTests : ServiceTestBase
    {
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(DbContext);
        }

        [Fact]
        public async Task List_ShouldReturnPagedOrders_WhenOrdersExist()
        {
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "Test Test", Email = "buyer@example.com", ShippingAddress = "Test 3" };
            DbContext.Buyers.Add(buyer);

            var order1 = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Paid", Buyer = buyer };
            var order2 = new Order { Id = 2, OrderDate = DateTime.Now, Status = "Delivered", Buyer = buyer };

            DbContext.Orders.AddRange(order1, order2);
            await DbContext.SaveChangesAsync();

            var search = new OrderSearch { Status = "Paid" };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results); // Kontrollib, et ainult üks tellimus vastab otsingule.
            Assert.Equal("Paid", result.Results.First().Status);
        }

        [Fact]
        public async Task Get_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "Test Test", Email = "buyer@example.com", ShippingAddress = "Test 3" };
            DbContext.Buyers.Add(buyer);
            var order = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Paid", Buyer = buyer };
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Paid", result.Status);
        }

        [Fact]
        public async Task Save_ShouldAddNewOrder_WhenIdIsZero()
        {
            // Arrange
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "Test Test", Email = "buyer@example.com", ShippingAddress = "Test 3" };
            DbContext.Buyers.Add(buyer);
            var order = new Order { Id = 0, OrderDate = DateTime.Now, Status = "New", Buyer = buyer };

            // Act
            await _service.Save(order);

            // Assert
            var savedOrder = await DbContext.Orders.FirstOrDefaultAsync(o => o.Status == "New");
            Assert.NotNull(savedOrder);
            Assert.Equal("New", savedOrder.Status);
        }

        [Fact]
        public async Task Save_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "Test Test", Email = "buyer@example.com", ShippingAddress = "Test 3" };
            DbContext.Buyers.Add(buyer);
            var order = new Order { Id = 1, OrderDate = DateTime.Now, Status = "New", Buyer = buyer };
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();

            order.Status = "Updated";

            // Act
            await _service.Save(order);

            // Assert
            var updatedOrder = await DbContext.Orders.FirstOrDefaultAsync(o => o.Id == 1);
            Assert.NotNull(updatedOrder);
            Assert.Equal("Updated", updatedOrder.Status);
        }

        [Fact]
        public async Task List_ShouldFilterOrdersById_WhenKeywordIsNumeric()
        {
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "John Smith", ShippingAddress = "123 Main St", Email = "john.smith@example.com" };
            DbContext.Buyers.Add(buyer);
            DbContext.Orders.Add(new Order { Id = 100, OrderDate = DateTime.UtcNow, Status = "Pending", Buyer = buyer });
            DbContext.Orders.Add(new Order { Id = 200, OrderDate = DateTime.UtcNow, Status = "Shipped", Buyer = buyer });
            await DbContext.SaveChangesAsync();

            var searchCriteria = new OrderSearch { Keyword = "100" };

            // Act
            var result = await _service.List(1, 10, searchCriteria);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Results);
            Assert.Equal(100, result.Results.First().Id);
        }

        [Fact]
        public async Task ListBuyers_ShouldReturnAllBuyers_WhenBuyersExist()
        {
            // Arrange
            DbContext.Buyers.Add(new Buyer { Id = 1, Name = "John Doe", ShippingAddress = "123 Main St", Email = "john@example.com" });
            DbContext.Buyers.Add(new Buyer { Id = 2, Name = "Jane Doe", ShippingAddress = "456 Elm St", Email = "jane@example.com" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.ListBuyers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, b => b.Name == "John Doe");
            Assert.Contains(result, b => b.Name == "Jane Doe");
        }

        [Fact]
        public async Task ListOrderItems_ShouldReturnAllOrderItems_WhenOrderItemsExist()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product = new Product { Id = 1, Name = "Test Product 1", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            DbContext.Products.Add(product);
            var order = new Order { Id = 1, Status = "Delivered" };
            DbContext.Orders.Add(order);
            DbContext.OrderItems.Add(new OrderItem { Id = 1, Product = product, Quantity = 1, PriceAtOrderTime = 100.00M, Order = order });
            DbContext.OrderItems.Add(new OrderItem { Id = 2, Product = product, Quantity = 2, PriceAtOrderTime = 50.00M, Order = order });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.ListOrderItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, i => i.Quantity == 1 );
            Assert.Contains(result, i => i.Quantity == 2 );
        }

        [Fact]
        public async Task Delete_ShouldRemoveOrder_WhenOrderExists()
        {
            // Arrange
            var buyer = new Buyer { Id = 1, Name = "Test Test", Email = "buyer@example.com", ShippingAddress = "Test 3" };
            DbContext.Buyers.Add(buyer);
            var order = new Order { Id = 1, OrderDate = DateTime.Now, Status = "New", Buyer = buyer };
            DbContext.Orders.Add(order);
            await DbContext.SaveChangesAsync();

            // Act
            await _service.Delete(1);

            // Assert
            var deletedOrder = await DbContext.Orders.FirstOrDefaultAsync(o => o.Id == 1);
            Assert.Null(deletedOrder);
        }
    }
}
