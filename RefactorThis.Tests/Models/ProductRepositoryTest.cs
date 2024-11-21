using System.Data.SqlClient;
using Moq;
using refactor_this.Models;
using refactor_this.Services;

namespace RefactorThis.Tests.Models
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        // private Mock<SqlConnection> mockConnection;
        // private Mock<SqlCommand> mockCommand;
        // private ProductRepository repository;

        // [SetUp]
        // public void SetUp()
        // {
        //     // Mock the SqlConnection and SqlCommand
        //     mockConnection = new Mock<SqlConnection>();
        //     mockCommand = new Mock<SqlCommand>();
        //
        //     // Mock the behavior of the SqlCommand (e.g., ExecuteNonQuery and ExecuteReader)
        //     mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); // Simulate successful SQL command execution
        //     
        //     // Use the mock SqlConnection and SqlCommand in your repository method
        //     repository = new ProductRepository();
        // }
        //
        // [Test]
        // public void Save_WhenProductIsNew_ShouldExecuteInsert()
        // {
        //     // Arrange
        //     var product = new Product
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = "New Product",
        //         Description = "Product Description",
        //         Price = 100.00m,
        //         DeliveryPrice = 15.00m,
        //     };
        //
        //     // Act
        //     repository.Save(product);
        //
        //     // Assert that ExecuteNonQuery() was called once to execute the insert command
        //     mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        // }
        //
        // [Test]
        // public void Save_WhenProductIsNotNew_ShouldExecuteUpdate()
        // {
        //     // Arrange
        //     var product = new Product
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = "Updated Product",
        //         Description = "Updated Description",
        //         Price = 120.00m,
        //         DeliveryPrice = 20.00m,
        //     };
        //
        //     // Act
        //     repository.Save(product);
        //
        //     // Assert that ExecuteNonQuery() was called once to execute the update command
        //     mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        // }
        //
        // [Test]
        // public void Delete_ShouldExecuteDeleteAndDeleteProductOptions()
        // {
        //     // Arrange
        //     var product = new Product
        //     {
        //         Id = Guid.NewGuid(),
        //         Name = "Product to Delete",
        //         Description = "Description",
        //         Price = 50.00m,
        //         DeliveryPrice = 10.00m
        //     };
        //
        //     // Mock the ProductOptionRepository and ProductOptionsServices
        //     var mockProductOptionRepo = new Mock<ProductOptionRepository>();
        //     var mockProductOptionsService = new Mock<ProductOptionsServices>(mockProductOptionRepo.Object);
        //
        //     // Act
        //     repository.Delete(product);
        //
        //     // Assert that Delete() on the ProductOptionRepository was called
        //     mockProductOptionRepo.Verify(repo => repo.Delete(It.IsAny<ProductOption>()), Times.AtLeastOnce);
        //
        //     // Assert that ExecuteNonQuery() was called once to execute the delete command
        //     mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
        // }
        //
        // [Test]
        // public void GetById_WhenProductExists_ShouldReturnProduct()
        // {
        //     // Arrange
        //     var productId = Guid.NewGuid();
        //     var product = new Product
        //     {
        //         Id = productId,
        //         Name = "Fetched Product",
        //         Description = "Description of fetched product",
        //         Price = 100.00m,
        //         DeliveryPrice = 10.00m
        //     };
        //
        //     // Mock the SqlCommand behavior to return the desired reader
        //     mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(MockReader(product));
        //
        //     // Act
        //     var result = repository.GetById(productId);
        //
        //     // Assert that the returned product is not null
        //     Assert.That(result, Is.Not.Null);
        //     Assert.Equals(productId, result.Id);
        //     Assert.Equals("Fetched Product", result.Name);
        // }
        //
        // [Test]
        // public void GetById_WhenProductDoesNotExist_ShouldReturnNull()
        // {
        //     // Arrange
        //     var productId = Guid.NewGuid();
        //
        //     // Mock ExecuteReader to simulate no data found
        //     mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(MockReader(null));
        //
        //     // Act
        //     var result = repository.GetById(productId);
        //
        //     // Assert that the result is null since the product doesn't exist
        //     Assert.That(result, Is.Null);
        // }
        //
        // // Helper method to mock the SqlDataReader
        // private SqlDataReader MockReader(Product product)
        // {
        //     var mockReader = new Mock<SqlDataReader>();
        //     if (product != null)
        //     {
        //         mockReader.SetupSequence(rdr => rdr.Read())
        //                   .Returns(true)
        //                   .Returns(false); // First return true to simulate data read, then false for no more data
        //         mockReader.Setup(rdr => rdr["Id"]).Returns(product.Id.ToString());
        //         mockReader.Setup(rdr => rdr["Name"]).Returns(product.Name);
        //         mockReader.Setup(rdr => rdr["Description"]).Returns(product.Description);
        //         mockReader.Setup(rdr => rdr["Price"]).Returns(product.Price.ToString());
        //         mockReader.Setup(rdr => rdr["DeliveryPrice"]).Returns(product.DeliveryPrice.ToString());
        //     }
        //     else
        //     {
        //         mockReader.Setup(rdr => rdr.Read()).Returns(false); // Simulate no data found
        //     }
        //     return mockReader.Object;
        // }
    }
}
