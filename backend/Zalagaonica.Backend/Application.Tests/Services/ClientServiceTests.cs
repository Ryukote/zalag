using Domain.Entities;
using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Services
{
    public class ClientServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientService _service;

        public ClientServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new ClientService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Ivan Horvat",
                    City = "Zagreb",
                    Address = "Ilica 1",
                    IdCardNumber = "12345678901",
                    Type = "individual",
                    Status = "active"
                },
                new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Marko Marić",
                    City = "Split",
                    Address = "Obala 2",
                    IdCardNumber = "98765432109",
                    Type = "individual",
                    Status = "active"
                }
            };

            _context.Clients.AddRange(clients);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Name == "Ivan Horvat");
            result.Should().Contain(c => c.Name == "Marko Marić");
        }

        [Fact]
        public async Task GetAllAsync_WithNoClients_ShouldReturnEmptyList()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnClient()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "Ivan Horvat",
                City = "Zagreb",
                Address = "Ilica 1",
                IdCardNumber = "12345678901",
                Email = "ivan@example.com",
                Type = "individual",
                Status = "active"
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetByIdAsync(clientId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(clientId);
            result.Name.Should().Be("Ivan Horvat");
            result.Email.Should().Be("ivan@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _service.GetByIdAsync(nonExistentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddClientToDatabase()
        {
            // Arrange
            var client = new Client
            {
                Name = "Petra Petrić",
                City = "Rijeka",
                Address = "Korzo 10",
                IdCardNumber = "11122233344",
                Email = "petra@example.com",
                Type = "individual",
                Status = "active"
            };

            // Act
            var result = await _service.CreateAsync(client);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be("Petra Petrić");

            var savedClient = await _context.Clients.FindAsync(result.Id);
            savedClient.Should().NotBeNull();
            savedClient!.Name.Should().Be("Petra Petrić");
            savedClient.City.Should().Be("Rijeka");
        }

        [Fact]
        public async Task CreateAsync_ShouldGenerateNewGuid()
        {
            // Arrange
            var client1 = new Client
            {
                Name = "Client 1",
                City = "Zagreb",
                Address = "Address 1",
                IdCardNumber = "11111111111",
                Type = "individual",
                Status = "active"
            };

            var client2 = new Client
            {
                Name = "Client 2",
                City = "Split",
                Address = "Address 2",
                IdCardNumber = "22222222222",
                Type = "individual",
                Status = "active"
            };

            // Act
            var result1 = await _service.CreateAsync(client1);
            var result2 = await _service.CreateAsync(client2);

            // Assert
            result1.Id.Should().NotBe(result2.Id);
            result1.Id.Should().NotBeEmpty();
            result2.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WithExistingClient_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "Original Name",
                City = "Zagreb",
                Address = "Original Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            _context.Entry(client).State = EntityState.Detached;

            var updatedClient = new Client
            {
                Id = clientId,
                Name = "Updated Name",
                City = "Split",
                Address = "Updated Address",
                IdCardNumber = "12345678901",
                Email = "updated@example.com",
                Type = "individual",
                Status = "active"
            };

            // Act
            var result = await _service.UpdateAsync(updatedClient);

            // Assert
            result.Should().BeTrue();

            var savedClient = await _context.Clients.FindAsync(clientId);
            savedClient.Should().NotBeNull();
            savedClient!.Name.Should().Be("Updated Name");
            savedClient.City.Should().Be("Split");
            savedClient.Address.Should().Be("Updated Address");
            savedClient.Email.Should().Be("updated@example.com");
        }

        [Fact]
        public async Task UpdateAsync_WithNonExistingClient_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentClient = new Client
            {
                Id = Guid.NewGuid(),
                Name = "Non-existent Client",
                City = "Zagreb",
                Address = "Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            // Act
            var result = await _service.UpdateAsync(nonExistentClient);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_WithExistingClient_ShouldRemoveAndReturnTrue()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "Client To Delete",
                City = "Zagreb",
                Address = "Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active"
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteAsync(clientId);

            // Assert
            result.Should().BeTrue();

            var deletedClient = await _context.Clients.FindAsync(clientId);
            deletedClient.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingClient_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _service.DeleteAsync(nonExistentId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateAsync_WithLegalEntityType_ShouldSaveCorrectly()
        {
            // Arrange
            var client = new Client
            {
                Name = "Test Company d.o.o.",
                City = "Zagreb",
                Address = "Business Street 123",
                IdCardNumber = "12345678901", // OIB for legal entity
                Type = "legal",
                Status = "active",
                Iban = "HR1234567890123456789"
            };

            // Act
            var result = await _service.CreateAsync(client);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be("legal");
            result.Iban.Should().Be("HR1234567890123456789");

            var savedClient = await _context.Clients.FindAsync(result.Id);
            savedClient.Should().NotBeNull();
            savedClient!.Type.Should().Be("legal");
        }

        [Fact]
        public async Task UpdateAsync_ShouldMaintainCreatedAtTimestamp()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var originalCreatedAt = DateTime.UtcNow.AddDays(-10);
            var client = new Client
            {
                Id = clientId,
                Name = "Original Name",
                City = "Zagreb",
                Address = "Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active",
                CreatedAt = originalCreatedAt
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            _context.Entry(client).State = EntityState.Detached;

            var updatedClient = new Client
            {
                Id = clientId,
                Name = "Updated Name",
                City = "Zagreb",
                Address = "Address",
                IdCardNumber = "12345678901",
                Type = "individual",
                Status = "active",
                CreatedAt = originalCreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            await _service.UpdateAsync(updatedClient);

            // Assert
            var savedClient = await _context.Clients.FindAsync(clientId);
            savedClient!.CreatedAt.Should().BeCloseTo(originalCreatedAt, TimeSpan.FromSeconds(1));
        }
    }
}
