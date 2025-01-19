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
    }
}
