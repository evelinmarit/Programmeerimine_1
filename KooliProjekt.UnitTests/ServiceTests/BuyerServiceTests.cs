using KooliProjekt.Data;
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
    public class BuyerServiceTests : ServiceTestBase
    {
        private readonly BuyerService _service;

        public BuyerServiceTests() 
        {
            _service = new BuyerService(DbContext);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            await DbContext.Buyers.AddRangeAsync(
                new Buyer { Name = "Buyer 1", Email = "buyer1@example.com", ShippingAddress = "Test 1" },
                new Buyer { Name = "Buyer 2", Email = "buyer2@example.com", ShippingAddress = "Test 2" },
                new Buyer { Name = "Buyer 3", Email = "buyer3@example.com", ShippingAddress = "Test 3" }
            );
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 2); // Page 1, 2 items per page

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count);  // Kontrollib, kas leheküljel on 2 elementi
            Assert.Equal(3, result.RowCount);       // Kontrollib, kas kõikide elementide koguarv on 3
            Assert.Equal(2, result.PageCount);      // Kontrollib, et lehekülgede arv oleks korrektne
            Assert.Equal(1, result.CurrentPage);    // Kontrollib, et hetkelehekülg on 1
            Assert.Contains(result.Results, r => r.Name == "Buyer 1");
            Assert.Contains(result.Results, r => r.Name == "Buyer 2");
        }

        [Fact]
        public async Task Get_ShouldReturnBuyerWithOrders()
        {
            // Arrange
            var buyer = new Buyer
            {
                Name = "Mati Test",
                Email = "Test@example.com",
                ShippingAddress = "Test 1",
                Orders = new List<Order>
        {
            new Order { OrderDate = DateTime.UtcNow, Status = "Shipped" },
            new Order { OrderDate = DateTime.UtcNow, Status = "Pending" }
        }
            };

            DbContext.Buyers.Add(buyer);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(buyer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(buyer.Name, result.Name);
            Assert.Equal(2, result.Orders.Count);

            // Kontrollime, et kõik tellimused on seotud õige BuyerId-ga
            Assert.All(result.Orders, o => Assert.Equal(buyer.Id, o.BuyerId));
        }


        [Fact]
        public async Task Save_ShouldAddNewBuyer()
        {
            // Arrange
            var buyer = new Buyer { Name = "New Name", Email = "test@example.com", ShippingAddress = "Test 1" };

            // Act
            await _service.Save(buyer);
            var savedBuyer = await DbContext.Buyers.FirstOrDefaultAsync(c => c.Name == "New Name");

            // Assert
            Assert.NotNull(savedBuyer);
            Assert.Equal("New Name", savedBuyer.Name);
        }

        [Fact]
        public async Task Save_ShouldUpdateExistingBuyer()
        {
            // Arrange
            var buyer = new Buyer { Name = "Old Name", Email = "test@example.com", ShippingAddress = "Test 1" };
            await DbContext.Buyers.AddAsync(buyer);
            await DbContext.SaveChangesAsync();

            // Act
            buyer.Name = "Updated Name";
            await _service.Save(buyer);
            var updatedBuyer = await DbContext.Buyers.FirstOrDefaultAsync(c => c.Id == buyer.Id);

            // Assert
            Assert.NotNull(updatedBuyer);
            Assert.Equal("Updated Name", updatedBuyer.Name);
        }

        [Fact]
        public async Task Delete_should_remove_existing_buyer()
        {
            // Arrange
            var buyer = new Buyer { Name = "Test", Email = "test@example.com", ShippingAddress = "Test 1" };
            DbContext.Buyers.Add(buyer);
            DbContext.SaveChanges();

            // Act
            await _service.Delete(buyer.Id);

            // Assert
            var count = DbContext.Buyers.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task Delete_should_return_if_buyer_was_not_found()
        {
            // Arrange
            var id = -100;

            // Act
            await _service.Delete(id);

            // Assert
            var count = DbContext.Buyers.Count();
            Assert.Equal(0, count);
        }

    }
}
