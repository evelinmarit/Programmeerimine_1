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
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_view_and_data()
        {
            // Arrange
            //var categoryServiceMock = new Mock<ICategoryService>();
            //var controller = new CategoriesController(categoryServiceMock.Object);
            var page = 1;

            // var data = new List<CategoryService>
            // {
            //     new CategoryService { Id = 1, Name = "Test 1" },

            // };
            // var pagedResult = new PagedResult<CategoryService> { Results = data };
            _categoryServiceMock
                .Setup(x => x.List(page, It.IsAny<int>()));
                // .ReturnsAsync(PagedResult);
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
            var category = new Category { Id = id, Name = "Test" };
            _categoryServiceMock
                .Setup(x => x.Save(category))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Create(category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _categoryServiceMock.VerifyAll();
        }

        [Fact]
        public async Task CreatePost_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var category = new Category { Id = id, Name = "Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Create(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Create"
            );
            Assert.Equal(category, result.Model);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var category = new Category { Id = 2 };

            // Act
            var result = await _controller.Edit(id, category) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditPost_should_redirect_after_successful_save()
        {
            // Arrange
            int id = 1;
            var category = new Category { Id = id, Name = "Test" };
            _categoryServiceMock
                .Setup(x => x.Save(category))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.Edit(id, category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _categoryServiceMock.VerifyAll();
        }

        [Fact]
        public async Task EditPost_should_stay_on_view_when_model_is_not_valid()
        {
            // Arrange
            int id = 1;
            var category = new Category { Id = id, Name="Test" };

            // Act
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Edit(id, category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(
                string.IsNullOrEmpty(result.ViewName) ||
                result.ViewName == "Edit"
            );
            Assert.Equal(category, result.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int id = 1;
            var category = new Category { Id = 2 };

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
            var category = new Category { Id = id, Name = "Test" };
            _categoryServiceMock
                .Setup(x => x.Delete(id))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _categoryServiceMock.VerifyAll();
        }
    }
}
