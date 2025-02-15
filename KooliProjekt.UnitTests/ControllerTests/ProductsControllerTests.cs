using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Data.Migrations;
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
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            var page = 1;
            var data = new List<Product>
            {
                new Product { Id = 1, Name = "Test 1" },
                new Product { Id = 2, Name = "Test 2" }
            };
            var PagedResult = new PagedResult<Product>
            {
                Results = data,
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                RowCount = 2,
            };

            _productServiceMock
                .Setup(x => x.List(page, It.IsAny<int>(), null));

            // Act

            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index" ||
                        string.IsNullOrEmpty(result.ViewName));
            //Assert.Equal(result.Model, result.ViewName);
        }

        [Fact]
        public async Task CreatePost_should_redirect_after_successful_create()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test" };
            _productServiceMock
                .Setup(x => x.Save(product))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CreateProduct_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test", Description = "Great", PhotoUrl = "http://www.foto", Price = 7, CategoryId = 1, AtStock = false };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
            Assert.Equal(product, result.Model);
        }

        //[Fact]
        //public async Task CreateProduct_should_stay_on_view_when_model_is_not_valid()
        //{
        //    Arrange
        //    int id = 1;
        //    var product = new Product { Id = id, Name = "Test", Description = "Great", PhotoUrl = "http://www.foto", Price = 7, CategoryId = 1, AtStock = false };

        //    Simuleerime kategooriate olemasolu(kui see on vajalik)
        //    var categories = new List<Category> { new Category { Id = 1, Name = "Test Category" } };
        //    _productService = Mock.Of<IProductService>(service =>
        //        service.ListCategories() == Task.FromResult(categories) &&
        //        service.Save(It.IsAny<Product>()) == Task.CompletedTask
        //    );
        //    _controller = new ProductsController(_productService);

        //    Act
        //    _controller.ModelState.AddModelError("error", "error");
        //    var result = await _controller.Create(product) as ViewResult;

        //    Assert
        //    Assert.NotNull(result);
        //    Assert.True(
        //        string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Create"
        //    );

        //    Veendu, et mudel on sama
        //    var model = result.Model as Product;
        //    Assert.NotNull(model);
        //    Assert.Equal(product.Id, model.Id);
        //    Assert.Equal(product.Name, model.Name);
        //    Assert.Equal(product.Description, model.Description);
        //    Assert.Equal(product.PhotoUrl, model.PhotoUrl);
        //    Assert.Equal(product.Price, model.Price);
        //    Assert.Equal(product.CategoryId, model.CategoryId);
        //    Assert.Equal(product.AtStock, model.AtStock);
        //}

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = 2 };

            // Act
            var result = await _controller.Edit(id, product) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditProduct_should_redirect_after_successful_save()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test" };
            _productServiceMock
                .Setup(x => x.Save(product))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(id, product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productServiceMock.VerifyAll();
        }

        [Fact]
        public async Task EditProduct_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Edit(id, product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(product, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = 2 };

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
            var product = new Product { Id = id, Name = "Test" };
            _productServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productServiceMock.VerifyAll();
        }
    }
}
