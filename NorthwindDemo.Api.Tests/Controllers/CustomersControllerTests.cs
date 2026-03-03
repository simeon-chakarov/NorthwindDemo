using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Api.Controllers;
using NorthwindDemo.Api.Services;

namespace NorthwindDemo.Api.Tests.Controllers
{
    public class CustomersControllerTests
    {
        [Fact]
        public async Task GetCustomers_ReturnsOkWithList()
        {
            // Arrange
            var customers = new List<CustomerListItemDto>
            {
                new("ALFKI", "Alfreds Futterkiste", 5),
                new("ANATR", "Ana Trujillo", 2)
            };

            var service = new Mock<ICustomerService>();
            service
                .Setup(s => s.GetCustomersAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customers);

            var controller = new CustomersController(service.Object);

            // Act
            var result = await controller.GetCustomers(null, CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IReadOnlyList<CustomerListItemDto>>(ok.Value);

            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task GetCustomerById_WhenServiceReturnsCustomer_ReturnsOkWithDto()
        {
            // Arrange
            var dto = new CustomerDetailsDto(
                Id: "ALFKI",
                CompanyName: "Alfreds Futterkiste",
                ContactName: null,
                ContactTitle: null,
                Address: null,
                City: null,
                Region: null,
                PostalCode: null,
                Country: null,
                Phone: null,
                Fax: null
            );

            var service = new Mock<ICustomerService>(MockBehavior.Strict);
            service
                .Setup(s => s.GetCustomerByIdAsync("ALFKI", It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            var controller = new CustomersController(service.Object);

            // Act
            var result = await controller.GetCustomerById("ALFKI", CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<CustomerDetailsDto>(ok.Value);
            Assert.Equal("ALFKI", model.Id);
            Assert.Equal("Alfreds Futterkiste", model.CompanyName);
        }

        [Fact]
        public async Task GetCustomerById_WhenIdIsWhitespace_ReturnsBadRequest()
        {
            var service = new Mock<ICustomerService>(MockBehavior.Loose);
            var controller = new CustomersController(service.Object);

            var result = await controller.GetCustomerById("   ", CancellationToken.None);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomerById_WhenServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var service = new Mock<ICustomerService>(MockBehavior.Strict);
            service
                .Setup(s => s.GetCustomerByIdAsync("NOPE", It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerDetailsDto?)null);

            var controller = new CustomersController(service.Object);

            // Act
            var result = await controller.GetCustomerById("NOPE", CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomerOrders_WhenServiceReturnsData_ReturnsOk()
        {
            var response = new CustomerOrdersResponseDto(
                "ALFKI",
                [
                    new(10248, DateTime.Now, 100m, 3, string.Empty)
                ]
            );

            var service = new Mock<ICustomerService>();
            service
                .Setup(s => s.GetCustomerOrdersAsync("ALFKI", It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var controller = new CustomersController(service.Object);

            var result = await controller.GetCustomerOrders("ALFKI", CancellationToken.None);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<CustomerOrdersResponseDto>(ok.Value);

            Assert.Equal("ALFKI", model.CustomerId);
        }

        [Fact]
        public async Task GetCustomerOrders_WhenIdIsWhitespace_ReturnsBadRequest()
        {
            // Arrange
            var service = new Mock<ICustomerService>(MockBehavior.Loose);
            var controller = new CustomersController(service.Object);

            // Act
            var result = await controller.GetCustomerOrders("   ", CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetCustomerOrders_WhenServiceReturnsNull_ReturnsNotFound()
        {
            var service = new Mock<ICustomerService>();
            service
                .Setup(s => s.GetCustomerOrdersAsync("NOPE", It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerOrdersResponseDto?)null);

            var controller = new CustomersController(service.Object);

            var result = await controller.GetCustomerOrders("NOPE", CancellationToken.None);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
