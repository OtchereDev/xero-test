using NSubstitute;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using refactor_this.Models;

[TestFixture]
public class ProductOptionRepositoryTests
{
    private IDatabase _mockDatabase;
    private IDbConnectionWrapper _mockConnection;
    private IDbCommand _mockCommand;
    private IDataReader _mockReader;
    private ProductOptionRepository _repository;
    
    [SetUp]
    public void SetUp()
    {
        _mockDatabase = Substitute.For<IDatabase>();
        _mockConnection = Substitute.For<IDbConnectionWrapper>();
        _mockCommand = Substitute.For<IDbCommand>();
        _mockReader = Substitute.For<IDataReader>();

        _mockDatabase.GetConnection().Returns(_mockConnection);

        // Mock connection behavior
        _mockConnection.Open().Returns(Task.CompletedTask);
        _mockConnection.State.Returns(ConnectionState.Open); // Use the added State property
        _mockConnection.CreateCommand().Returns(_mockCommand);

        _mockCommand.ExecuteReader().Returns(_mockReader);

        _repository = new ProductOptionRepository(_mockDatabase);
    }

    [TearDown]
    public void TearDown()
    {
        _mockCommand?.Dispose();
        _mockReader?.Dispose();

        // This will ensure that mocks are reset or cleaned up
        _mockDatabase = null;
        _mockConnection = null;
        _mockCommand = null;
        _mockReader = null;
    }

    [Test]
    public async Task GetByIdAsync_ReturnsProductOption_WhenIdExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var name = "Test Product Option";
        var description = "Test Description";

        // Explicitly specify the return type for Read()
        _mockReader.Read().Returns(true, false); // First ReadAsync returns true, second ReadAsync returns false

        // Set up the data being returned from the reader
        _mockReader["Id"].Returns(id.ToString());
        _mockReader["ProductId"].Returns(productId.ToString());
        _mockReader["Name"].Returns(name);
        _mockReader["Description"].Returns(description);

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(id));
        Assert.That(result.ProductId, Is.EqualTo(productId));
        Assert.That(result.Name, Is.EqualTo(name));
        Assert.That(result.Description, Is.EqualTo(description));
    }
    
    // [Test]
    // public async Task SaveAsync_ShouldInsertProductOption_WhenProductOptionIsNew()
    // {
    //     // Arrange
    //     var mockConnection = Substitute.For<IDbConnectionWrapper>();
    //     var mockCommand = Substitute.For<IDbCommand>();
    //     _mockDatabase.GetConnection().Returns(mockConnection);
    //
    //     // Ensure the connection state is "Open"
    //     mockConnection.State.Returns(ConnectionState.Open);
    //
    //     // Simulate opening the connection
    //     mockConnection.Open().Returns(Task.CompletedTask);
    //
    //     var productOption = new ProductOption
    //     {
    //         Id = Guid.NewGuid(),
    //         ProductId = Guid.NewGuid(),
    //         Name = "Test Name",
    //         Description = "Test Description"
    //     };
    //
    //     // Mock CreateCommand to return the mock command
    //     mockConnection.CreateCommand().Returns(mockCommand);
    //
    //     // Act
    //     await _repository.SaveAsync(productOption);
    //
    //     // Assert
    //     mockCommand.Received().CommandText = "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";
    //     mockCommand.Received().Parameters.Add(Arg.Is<SqlParameter>(p => p.ParameterName == "@Id" && (Guid)p.Value == productOption.Id));
    //     mockCommand.Received().ExecuteNonQuery();
    // }
    
    // [Test]
    // public async Task GetAllAsync_ShouldReturnAllProductOptions_WhenCalled()
    // {
    //     // Arrange
    //     var mockConnection = Substitute.For<IDbConnectionWrapper>();
    //     var mockCommand = Substitute.For<IDbCommand>();
    //     var mockReader = Substitute.For<IDataReader>();
    //     _mockDatabase.GetConnection().Returns(mockConnection);
    //
    //     mockConnection.CreateCommand().Returns(mockCommand);
    //     mockCommand.ExecuteReader().Returns(mockReader);
    //
    //     var productId = Guid.NewGuid().ToString();
    //
    //     mockReader.Read().Returns(true, false); // Simulate one row of data
    //     mockReader["id"].Returns(Guid.NewGuid().ToString());
    //
    //     // Act
    //     var result = await _repository.GetAllAsync(productId);
    //
    //     // Assert
    //     Assert.That(result, Is.Not.Null);
    //     Assert.That(result.Count, Is.EqualTo(1));
    //     mockCommand.Received().CommandText.Contains("SELECT id FROM productoption");
    //     mockCommand.Received().Parameters.Add(Arg.Is<SqlParameter>(p => p.ParameterName == "@ProductId" && (string)p.Value == productId));
    // }
    
    // [Test]
    // public async Task SaveAsync_ShouldInsertProductOption_WhenProductOptionIsNew()
    // {
    //     // Arrange
    //     var mockConnection = Substitute.For<IDbConnectionWrapper>();
    //     var mockCommand = Substitute.For<IDbCommand>();
    //     _mockDatabase.GetConnection().Returns(mockConnection);
    //
    //     // Simulate an open connection
    //     mockConnection.Open().Returns(Task.CompletedTask);
    //     mockConnection.State.Returns(ConnectionState.Open); // Ensure connection state is Open
    //
    //     // Mock CreateCommand to return a valid IDbCommand
    //     mockConnection.CreateCommand().Returns(mockCommand);
    //
    //     var productOption = new ProductOption
    //     {
    //         Id = Guid.NewGuid(),
    //         ProductId = Guid.NewGuid(),
    //         Name = "Test Name",
    //         Description = "Test Description",
    //         // IsNew = true (Uncomment if necessary)
    //     };
    //
    //     // Act
    //     await _repository.SaveAsync(productOption);
    //
    //     // Assert
    //     mockCommand.Received().CommandText = "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";
    //     mockCommand.Received().Parameters.Add(Arg.Is<SqlParameter>(p => p.ParameterName == "@Id" && (Guid)p.Value == productOption.Id));
    //     mockCommand.Received().ExecuteNonQuery();
    // }
    
    [Test]
    public async Task SaveAsync_ShouldInsertProductOption_WhenProductOptionIsNew()
    {
        // Arrange
        var mockConnection = Substitute.For<IDbConnectionWrapper>();
        var mockCommand = Substitute.For<IDbCommand>();
        _mockDatabase.GetConnection().Returns(mockConnection);

        // Mock connection behavior
        mockConnection.Open().Returns(Task.CompletedTask); // Simulate opening the connection
        mockConnection.State.Returns(ConnectionState.Open); // Simulate the connection state as open
        mockConnection.CreateCommand().Returns(mockCommand);

        // Mock command behavior
        // mockCommand.ExecuteNonQuery().Returns(Task.FromResult(1));

        var productOption = new ProductOption
        {
            Id = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Name = "Test Name",
            Description = "Test Description",
        };

        // Act
        await _repository.SaveAsync(productOption);

        // Assert
        mockCommand.Received().CommandText = "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";
        mockCommand.Received().Parameters.Add(Arg.Is<SqlParameter>(p => p.ParameterName == "@Id" && (Guid)p.Value == productOption.Id));
        mockCommand.Received().ExecuteNonQuery();
    }
}