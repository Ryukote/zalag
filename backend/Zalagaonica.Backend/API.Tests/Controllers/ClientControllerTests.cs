using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Zalagaonica.Backend.Controllers;

namespace API.Tests.Controllers
{
    public class ClientControllerTests
    {
        private readonly Mock<ClientService> _mockClientService;
        private readonly ClientController _controller;

        public ClientControllerTests()
        {
            _mockClientService = new Mock<ClientService>(Mock.Of<Infrastructure.ApplicationDbContext>());
            _controller = new ClientController(_mockClientService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Client 1",
                    City = "Zagreb",
                    Address = "Address 1",
                    IdCardNumber = "12345678901",
                    Type = "individual",
                    Status = "active"
                },
                new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Client 2",
                    City = "Split",
                    Address = "Address 2",
                    IdCardNumber = "98765432109",
                    Type = "individual",
                    Status = "active"
                }
            };

            _mockClientService
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(clients);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnedClients = okResult.Value as List<Client>;
            returnedClients.Should().HaveCount(2);
            returnedClients.Should().Contain(c => c.Name == "Test Client 1");
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOkWithClient()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "Test Client",
                City = "Zagreb",
                Address = "Test Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            _mockClientService
                .Setup(s => s.GetByIdAsync(clientId))
                .ReturnsAsync(client);

            // Act
            var result = await _controller.GetById(clientId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            var returnedClient = okResult.Value as Client;
            returnedClient.Should().NotBeNull();
            returnedClient!.Id.Should().Be(clientId);
            returnedClient.Name.Should().Be("Test Client");
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            _mockClientService
                .Setup(s => s.GetByIdAsync(invalidId))
                .ReturnsAsync((Client?)null);

            // Act
            var result = await _controller.GetById(invalidId);

            // Assert
            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_WithValidClient_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newClient = new Client
            {
                Name = "New Client",
                City = "Rijeka",
                Address = "New Address",
                IdCardNumber = "11122233344",
                Type = "individual",
                Status = "active"
            };

            var createdClient = new Client
            {
                Id = Guid.NewGuid(),
                Name = newClient.Name,
                City = newClient.City,
                Address = newClient.Address,
                IdCardNumber = newClient.IdCardNumber,
                Type = newClient.Type,
                Status = newClient.Status
            };

            _mockClientService
                .Setup(s => s.CreateAsync(It.IsAny<Client>()))
                .ReturnsAsync(createdClient);

            // Act
            var result = await _controller.Create(newClient);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            var returnedClient = createdResult.Value as Client;
            returnedClient.Should().NotBeNull();
            returnedClient!.Name.Should().Be("New Client");
        }

        [Fact]
        public async Task Update_WithExistingClient_ShouldReturnNoContent()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var updatedClient = new Client
            {
                Id = clientId,
                Name = "Updated Client",
                City = "Zagreb",
                Address = "Updated Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            _mockClientService
                .Setup(s => s.UpdateAsync(It.IsAny<Client>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(clientId, updatedClient);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Update_WithNonExistingClient_ShouldReturnNotFound()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var updatedClient = new Client
            {
                Id = clientId,
                Name = "Updated Client",
                City = "Zagreb",
                Address = "Updated Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            _mockClientService
                .Setup(s => s.UpdateAsync(It.IsAny<Client>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(clientId, updatedClient);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Delete_WithExistingClient_ShouldReturnNoContent()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            _mockClientService
                .Setup(s => s.DeleteAsync(clientId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(clientId);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_WithNonExistingClient_ShouldReturnNotFound()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            _mockClientService
                .Setup(s => s.DeleteAsync(clientId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(clientId);

            // Assert
            var notFoundResult = result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }
    }
}
