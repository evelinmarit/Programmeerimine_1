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
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _controller = new OrdersController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            var page = 1;
            _orderServiceMock
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
        public async Task CreateOrder_should_redirect_after_successful_create()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, Status = "Test" };
            _orderServiceMock
                .Setup(x => x.Save(order))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CreateOrder_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, Status = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(order) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
            Assert.Equal(order, result.Model);
        }


        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = 2 };

            // Act
            var result = await _controller.Edit(id, order) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task EditOrder_should_redirect_after_successful_save()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, Status = "Test" };
            _orderServiceMock
                .Setup(x => x.Save(order))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(id, order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderServiceMock.VerifyAll();
        }

        [Fact]
        public async Task EditOrder_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, Status = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Edit(id, order) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(order, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = 2 };

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeletePost_should_redirect_after_successful_delete()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, Status = "Test" };
            _orderServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _orderServiceMock.VerifyAll();
        }
    }
}
