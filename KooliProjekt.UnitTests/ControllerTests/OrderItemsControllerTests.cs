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
    }
}
