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
    }
}
