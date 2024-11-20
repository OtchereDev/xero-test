using System.Data.SqlClient;
using Moq;
using refactor_this.Models;

namespace RefactorThis.Tests.Models
{
    [TestFixture]
    public class ProductOptionRepositoryTests
    {
        private Mock<SqlConnection> mockConnection;
        private Mock<SqlCommand> mockCommand;
        private ProductOptionRepository repository;

        [SetUp]
        public void SetUp()
        {
            mockConnection = new Mock<SqlConnection>();
            mockCommand = new Mock<SqlCommand>();
            
            // Setup a mock connection and command
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); // Simulate successful execution of SQL command
            
            // You could use this mock connection in your repository
            repository = new ProductOptionRepository();
        }
        
        [Test]
        public void Save_WhenProductOptionIsNew_ShouldExecuteInsert()
        {
            // Arrange
            var productOption = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Name = "Test Option",
                Description = "Test Description"
            };
            
            // Act
            repository.Save(productOption);

            // Assert that the appropriate command is executed
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once); // Ensure that ExecuteNonQuery() was called
        }

        [Test]
        public void Save_WhenProductOptionIsNotNew_ShouldExecuteUpdate()
        {
            // Arrange
            var productOption = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Name = "Updated Option",
                Description = "Updated Description",
            };

            // Act
            repository.Save(productOption);

            // Assert that the command to update the database was called
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }

        [Test]
        public void GetById_WhenProductOptionExists_ShouldReturnProductOption()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();
            var productOption = new ProductOption
            {
                Id = productOptionId,
                ProductId = Guid.NewGuid(),
                Name = "Test Option",
                Description = "Test Description"
            };

            // Mock behavior of ExecuteReader
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(MockReader(productOption));

            // Act
            var result = repository.GetById(productOptionId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Equals(productOption.Id, result.Id);
        }

        [Test]
        public void GetById_WhenProductOptionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();

            // Mock ExecuteReader to simulate no data found
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(MockReader(null));

            // Act
            var result = repository.GetById(productOptionId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Delete_ShouldExecuteDelete()
        {
            // Arrange
            var productOption = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Name = "Test Option",
                Description = "Test Description"
            };

            // Act
            repository.Delete(productOption);

            // Assert that ExecuteNonQuery is called to execute delete
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        }

        private SqlDataReader MockReader(ProductOption productOption)
        {
            var mockReader = new Mock<SqlDataReader>();
            if (productOption != null)
            {
                mockReader.SetupSequence(rdr => rdr.Read())
                    .Returns(true)
                    .Returns(false);
                mockReader.Setup(rdr => rdr["Id"]).Returns(productOption.Id.ToString());
                mockReader.Setup(rdr => rdr["ProductId"]).Returns(productOption.ProductId.ToString());
                mockReader.Setup(rdr => rdr["Name"]).Returns(productOption.Name);
                mockReader.Setup(rdr => rdr["Description"]).Returns(productOption.Description);
            }
            else
            {
                mockReader.Setup(rdr => rdr.Read()).Returns(false);
            }
            return mockReader.Object;
        }
    }
}
