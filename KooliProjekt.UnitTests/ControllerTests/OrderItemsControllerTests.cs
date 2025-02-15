using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OrderItemsControllerTests
    {
        private readonly Mock<IOrderItemService> _orderItemServiceMock;
        private readonly OrderItemsController _controller;

        public OrderItemsControllerTests()
        {
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _controller = new OrderItemsController(_orderItemServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
 
            _orderItemServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null));

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            // Assert.Equal(result.Model, result.ViewName);
        }

        [Fact]
        public async Task CreateOrderItem_should_redirect_after_successful_create()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id, ProductId = 1, OrderId = 1 };

            var mockOrders = new List<Order> { new Order { Id = 1 } };
            var mockProducts = new List<Product> { new Product { Id = 1, Name = "Test Product" } };

            _orderItemServiceMock
                .Setup(x => x.Save(orderItem))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(orderItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CreateOrderItem_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id, ProductId = 1, PriceAtOrderTime = 5, Quantity = 1, OrderId = 1 };

            var mockOrders = new List<Order> { new Order { Id = 1 } };
            var mockProducts = new List<Product> { new Product { Id = 1, Name = "Test Product" } };

            _orderItemServiceMock
                .Setup(x => x.ListOrders())
                .ReturnsAsync(mockOrders);

            _orderItemServiceMock
                .Setup(x => x.ListProducts())
                .ReturnsAsync(mockProducts);

            // Act
            _controller.ModelState.AddModelError("error", "error"); // Lisame vigase ModelState
            var result = await _controller.Create(orderItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create");
            Assert.NotNull(result.Model);
            Assert.Equal(orderItem, result.Model);
        }


        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = 2 };

            // Act
            var result = await _controller.Edit(id, orderItem) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditOrderItem_should_redirect_after_successful_save()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id, ProductId = 1, OrderId = 1 };
            _orderItemServiceMock
                .Setup(x => x.Save(orderItem))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(id, orderItem) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderItemServiceMock.VerifyAll();
        }

        [Fact]
        public async Task EditOrderItem_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id, ProductId = 1, OrderId = 2 };

            var mockOrders = new List<Order> { new Order { Id = 1 } };
            var mockProducts = new List<Product> { new Product { Id = 1, Name = "Test Product" } };

            _orderItemServiceMock
                .Setup(x => x.ListOrders())
                .ReturnsAsync(mockOrders);

            _orderItemServiceMock
                .Setup(x => x.ListProducts())
                .ReturnsAsync(mockProducts);

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Edit(id, orderItem) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(orderItem, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = 2 };

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteOrderItem_should_redirect_after_successful_delete()
        {
            // Arrange
            int id = 1;
            var orderItem = new OrderItem { Id = id, ProductId = 1, OrderId = 1 };
            _orderItemServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderItemServiceMock.VerifyAll();
        }
    }
}
