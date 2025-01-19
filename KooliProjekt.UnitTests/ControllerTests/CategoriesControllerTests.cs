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
    }
}
