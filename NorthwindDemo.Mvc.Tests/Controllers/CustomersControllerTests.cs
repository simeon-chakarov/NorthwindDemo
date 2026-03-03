using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.Contracts.Customers;
using Northwind.Contracts.Orders;
using NorthwindDemo.Mvc.Controllers;
using NorthwindDemo.Mvc.Services;
using NorthwindDemo.Mvc.ViewModels;

namespace NorthwindDemo.Mvc.Tests.Controllers
{
    public class CustomersControllerTests
    {
        [Fact]
        public async Task Index_WhenNoSearch_ReturnsViewWithCustomers()
        {
            // Arrange
            var customers = new List<CustomerListItemDto>
            {
                new("ALFKI", "Alfreds", 3),
                new("ANATR", "Ana", 1),
            };

            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);
            api.Setup(a => a.GetCustomersAsync(null, It.IsAny<CancellationToken>()))
               .ReturnsAsync(customers);

            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Index(null, CancellationToken.None);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CustomerIndexViewModel>(view.Model);

            Assert.Null(model.Search);
            Assert.Equal(2, model.Customers.Count);

            api.VerifyAll();
        }

        [Fact]
        public async Task Index_WhenSearchProvided_PassesSearchToApi_AndReturnsView()
        {
            // Arrange
            var customers = new List<CustomerListItemDto>
            {
                new("ALFKI", "Alfreds", 3)
            };

            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);
            api.Setup(a => a.GetCustomersAsync("alf", It.IsAny<CancellationToken>())).ReturnsAsync(customers);

            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Index("alf", CancellationToken.None);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CustomerIndexViewModel>(view.Model);

            Assert.Equal("alf", model.Search);
            Assert.Single(model.Customers);

            api.VerifyAll();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Details_WhenIdIsEmpty_ReturnsBadRequest(string id)
        {
            // Arrange
            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);
            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Details(id, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestResult>(result);

            api.Verify(
                a => a.GetCustomerByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);

            api.Verify(
                a => a.GetCustomerOrdersAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Details_WhenCustomerNotFound_ReturnsNotFound_AndDoesNotRequestOrders()
        {
            // Arrange
            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);

            api.Setup(a => a.GetCustomerByIdAsync("NOPE", It.IsAny<CancellationToken>()))
               .ReturnsAsync((CustomerDetailsDto?)null);

            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Details("NOPE", CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            api.Verify(a => a.GetCustomerByIdAsync("NOPE", It.IsAny<CancellationToken>()), Times.Once);
            api.Verify(a => a.GetCustomerOrdersAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

            api.VerifyAll();
        }

        [Fact]
        public async Task Details_WhenCustomerExists_ReturnsViewWithDetailsAndOrders()
        {
            // Arrange
            var customer = CreateValidCustomer(id: "ALFKI");

            var ordersResponse = new CustomerOrdersResponseDto(
                CustomerId: "ALFKI",
                Orders:
                [
                    new(10248, DateTime.Parse("1996-07-04"), 100m, 3, null),
                    new(10249, DateTime.Parse("1996-07-05"), 200m, 1, "Possible fulfillment issue...")
                ]
            );

            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);

            api.Setup(a => a.GetCustomerByIdAsync("ALFKI", It.IsAny<CancellationToken>()))
               .ReturnsAsync(customer);

            api.Setup(a => a.GetCustomerOrdersAsync("ALFKI", It.IsAny<CancellationToken>()))
               .ReturnsAsync(ordersResponse);

            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Details("ALFKI", CancellationToken.None);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CustomerDetailsViewModel>(view.Model);

            Assert.Equal("ALFKI", model.Customer.Id);
            Assert.Equal(2, model.Orders.Count);

            api.VerifyAll();
        }

        [Fact]
        public async Task Details_WhenOrdersNotFoundStill_ReturnsViewWithEmptyOrders()
        {
            // Arrange
            var customer = CreateValidCustomer(id: "ALFKI");

            var api = new Mock<ICustomerApiClient>(MockBehavior.Strict);

            api.Setup(a => a.GetCustomerByIdAsync("ALFKI", It.IsAny<CancellationToken>()))
               .ReturnsAsync(customer);

            api.Setup(a => a.GetCustomerOrdersAsync("ALFKI", It.IsAny<CancellationToken>()))
               .ReturnsAsync((CustomerOrdersResponseDto?)null);

            var controller = new CustomersController(api.Object);

            // Act
            var result = await controller.Details("ALFKI", CancellationToken.None);

            // Assert
            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CustomerDetailsViewModel>(view.Model);

            Assert.Equal("ALFKI", model.Customer.Id);
            Assert.Empty(model.Orders);

            api.VerifyAll();
        }

        private static CustomerDetailsDto CreateValidCustomer(
            string id = "ALFKI",
            string companyName = "Alfreds Futterkiste")
        {
            return new CustomerDetailsDto(
                Id: id,
                CompanyName: companyName,
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
        }
    }
}
