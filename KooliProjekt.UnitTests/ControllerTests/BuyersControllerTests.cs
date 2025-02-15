using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class BuyersControllerTests
    {
        private readonly Mock<IBuyerService> _buyerServiceMock;
        private readonly BuyersController _controller;

        public BuyersControllerTests()
        {
            _buyerServiceMock = new Mock<IBuyerService>();
            _controller = new BuyersController(_buyerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;

            _buyerServiceMock
                .Setup(x => x.List(page, It.IsAny<int>()));

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            Assert.Equal(result.Model, result.ViewName);
        }

        [Fact]
        public async Task CreatePost_should_redirect_after_successful_create()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = id, Name = "Test", ShippingAddress = "Veski 2 Tallinn", Email = "Juku@gmail.com", PhoneNumber = 1234556, RegisteredDate = DateTime.Now, LoyaltyPoints = 0, Discount = 0 };
            _buyerServiceMock
                .Setup(x => x.Save(buyer))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(buyer) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _buyerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CreatePost_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = id, Name = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(buyer) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
            Assert.Equal(buyer, result.Model);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = 2 };

            // Act
            var result = await _controller.Edit(id, buyer) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task EditPost_should_redirect_after_successful_save()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = id, Name = "Test" };
            _buyerServiceMock
                .Setup(x => x.Save(buyer))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(id, buyer) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _buyerServiceMock.VerifyAll();
        }
        [Fact]
        public async Task EditPost_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = id, Name = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Edit(id, buyer) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(buyer, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var buyer = new Buyer { Id = 2 };

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
            var buyer = new Buyer { Id = id, Name = "Test" };
            _buyerServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _buyerServiceMock.VerifyAll();
        }
    }
}
