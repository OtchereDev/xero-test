using NSubstitute;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Tests
{
    [TestFixture]
    public class ProductOptionsServicesTests
    {
        private IProductOptionRepository _productOptionRepository;
        private ProductOptionsServices _productOptionsServices;

        [SetUp]
        public void Setup()
        {
            // Mock the repository
            _productOptionRepository = Substitute.For<IProductOptionRepository>();
            // Create an instance of the service with the mocked repository
            _productOptionsServices = new ProductOptionsServices(_productOptionRepository);
        }

        [Test]
        public async Task GetProductOptionsAsync_ShouldReturnOptions_WhenOptionsExist()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var expectedOptions = new List<ProductOption>
            {
                new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.Parse(productId), Name = "Option 1" },
                new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.Parse(productId), Name = "Option 2" }
            };

            _productOptionRepository.GetAllAsync(productId).Returns(expectedOptions);

            // Act
            var options = await _productOptionsServices.GetProductOptionsAsync(productId);

            // Assert
            Assert.That(options.Count, Is.EqualTo(expectedOptions.Count));
            Assert.That(options[0].Name, Is.EqualTo(expectedOptions[0].Name));
        }

        [Test]
        public async Task CreateProductOptionAsync_ShouldSaveAndReturnOption()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productOption = new ProductOption
            {
                Id = Guid.NewGuid(),
                Name = "New Option",
                Description = "Description"
            };

            // Act
            var createdOption = await _productOptionsServices.CreateProductOptionAsync(productOption, productId);

            // Assert
            Assert.That(createdOption.ProductId, Is.EqualTo(productId));
            Assert.That(createdOption.Name, Is.EqualTo(productOption.Name));
            await _productOptionRepository.Received(1).SaveAsync(productOption);
        }

        [Test]
        public async Task UpdateProductOptionAsync_ShouldUpdateAndReturnOption_WhenOptionExists()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();
            var originalOption = new ProductOption
            {
                Id = productOptionId,
                Name = "Original Option",
                Description = "Original Description"
            };

            var updatedOption = new ProductOption
            {
                Id = productOptionId,
                Name = "Updated Option",
                Description = "Updated Description"
            };

            _productOptionRepository.GetByIdAsync(productOptionId).Returns(originalOption);

            // Act
            var result = await _productOptionsServices.UpdateProductOptionAsync(updatedOption, productOptionId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedOption.Name));
            Assert.That(result.Description, Is.EqualTo(updatedOption.Description));
            await _productOptionRepository.Received(1).UpdateAsync(originalOption);
        }

        [Test]
        public async Task UpdateProductOptionAsync_ShouldReturnNull_WhenOptionDoesNotExist()
        {
            // Arrange
            var productOptionId = Guid.NewGuid();
            var updatedOption = new ProductOption { Id = productOptionId, Name = "Non-Existent Option" };

            _productOptionRepository.GetByIdAsync(productOptionId).Returns((ProductOption)null);

            // Act
            var result = await _productOptionsServices.UpdateProductOptionAsync(updatedOption, productOptionId);

            // Assert
            Assert.That(result, Is.Null);
            await _productOptionRepository.DidNotReceive().UpdateAsync(Arg.Any<ProductOption>());
        }

        [Test]
        public async Task DeleteProductOptionAsync_ShouldDeleteAndReturnOption_WhenOptionExists()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption { Id = optionId, Name = "Option to Delete" };

            _productOptionRepository.GetByIdAsync(optionId).Returns(productOption);

            // Act
            var deletedOption = await _productOptionsServices.DeleteProductOptionAsync(optionId);

            // Assert
            Assert.That(deletedOption, Is.Not.Null);
            Assert.That(deletedOption.Name, Is.EqualTo(productOption.Name));
            await _productOptionRepository.Received(1).DeleteAsync(productOption);
        }

        [Test]
        public async Task DeleteProductOptionAsync_ShouldReturnNull_WhenOptionDoesNotExist()
        {
            // Arrange
            var optionId = Guid.NewGuid();

            _productOptionRepository.GetByIdAsync(optionId).Returns((ProductOption)null);

            // Act
            var result = await _productOptionsServices.DeleteProductOptionAsync(optionId);

            // Assert
            Assert.That(result, Is.Null);
            await _productOptionRepository.DidNotReceive().DeleteAsync(Arg.Any<ProductOption>());
        }

        [Test]
        public async Task GetProductOptionByIdAsync_ShouldReturnOption_WhenOptionExists()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var expectedOption = new ProductOption
            {
                Id = optionId,
                Name = "Existing Option",
                Description = "Description"
            };

            _productOptionRepository.GetByIdAsync(optionId).Returns(expectedOption);

            // Act
            var option = await _productOptionsServices.GetProductOptionByIdAsync(optionId);

            // Assert
            Assert.That(option, Is.Not.Null);
            Assert.That(option.Name, Is.EqualTo(expectedOption.Name));
        }

        [Test]
        public async Task GetProductOptionByIdAsync_ShouldReturnNull_WhenOptionDoesNotExist()
        {
            // Arrange
            var optionId = Guid.NewGuid();

            _productOptionRepository.GetByIdAsync(optionId).Returns((ProductOption)null);

            // Act
            var result = await _productOptionsServices.GetProductOptionByIdAsync(optionId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}