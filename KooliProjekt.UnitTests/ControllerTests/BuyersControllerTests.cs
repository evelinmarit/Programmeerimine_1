using KooliProjekt.Controllers;
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
    }
}
