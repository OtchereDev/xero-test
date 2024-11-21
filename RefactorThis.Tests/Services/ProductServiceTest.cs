using NSubstitute;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Tests
{
    [TestFixture]
    public class ProductServicesTests
    {
        private IProductRepository _productRepository;
        private ProductServices _productServices;

        [SetUp]
        public void Setup()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _productServices = new ProductServices(_productRepository);
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnProducts_WhenProductsExist()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" }
            };

            _productRepository.GetAllAsync(null).Returns(expectedProducts);

            // Act
            var products = await _productServices.GetAllProductsAsync();

            // Assert
            Assert.That(products.Count, Is.EqualTo(expectedProducts.Count));
            Assert.That(products[0].Name, Is.EqualTo(expectedProducts[0].Name));
        }

        [Test]
        public async Task CreateProductAsync_ShouldSaveAndReturnProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "New Product",
                Description = "Description",
                Price = 10.99m,
                DeliveryPrice = 2.99m
            };

            // Act
            var createdProduct = await _productServices.CreateProductAsync(product);

            // Assert
            Assert.That(createdProduct.Name, Is.EqualTo(product.Name));
            await _productRepository.Received(1).SaveAsync(product);
        }

        [Test]
        public async Task UpdateProductAsync_ShouldUpdateAndReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var originalProduct = new Product
            {
                Id = productId,
                Name = "Original Product",
                Description = "Original Description",
                Price = 5.99m,
                DeliveryPrice = 1.99m
            };

            var updatedProduct = new Product
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 6.99m,
                DeliveryPrice = 2.99m
            };

            _productRepository.GetByIdAsync(productId).Returns(originalProduct);

            // Act
            var result = await _productServices.UpdateProductAsync(updatedProduct, productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedProduct.Name));
            Assert.That(result.Description, Is.EqualTo(updatedProduct.Description));
            Assert.That(result.Price, Is.EqualTo(updatedProduct.Price));
            Assert.That(result.DeliveryPrice, Is.EqualTo(updatedProduct.DeliveryPrice));
            await _productRepository.Received(1).UpdateAsync(originalProduct);
        }

        [Test]
        public async Task UpdateProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updatedProduct = new Product { Id = productId, Name = "Non-Existent Product" };

            _productRepository.GetByIdAsync(productId).Returns((Product)null);

            // Act
            var result = await _productServices.UpdateProductAsync(updatedProduct, productId);

            // Assert
            Assert.That(result, Is.Null);
            await _productRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>());
        }

        [Test]
        public async Task DeleteProductAsync_ShouldDeleteAndReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product to Delete" };

            _productRepository.GetByIdAsync(productId).Returns(product);

            // Act
            var deletedProduct = await _productServices.DeleteProductAsync(productId);

            // Assert
            Assert.That(deletedProduct, Is.Not.Null);
            Assert.That(deletedProduct.Name, Is.EqualTo(product.Name));
            await _productRepository.Received(1).DeleteAsync(product);
        }

        [Test]
        public async Task DeleteProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _productRepository.GetByIdAsync(productId).Returns((Product)null);

            // Act
            var result = await _productServices.DeleteProductAsync(productId);

            // Assert
            Assert.That(result, Is.Null);
            await _productRepository.DidNotReceive().DeleteAsync(Arg.Any<Product>());
        }
    }
}