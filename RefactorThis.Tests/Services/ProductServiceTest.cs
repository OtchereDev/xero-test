using Moq;
using refactor_this.Models;
using refactor_this.Services;

namespace RefactorThis.Tests.Services
{
    [TestFixture]
    public class ProductServicesTests
    {
        private Mock<ProductRepository> mockRepository;
        private ProductServices productServices;

        [SetUp]
        public void SetUp()
        {
            // Create mock of the ProductRepository
            mockRepository = new Mock<ProductRepository>();
            
            // Create instance of ProductServices with the mocked ProductRepository
            productServices = new ProductServices(mockRepository.Object);
        }

        [Test]
        public void GetAllProducts_WhenCalled_ShouldReturnAllProducts()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Price = 100, DeliveryPrice = 10 },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Price = 150, DeliveryPrice = 15 }
            };

            mockRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<Guid>((id) => productList.Find(p => p.Id == id));

            // Act
            var result = productServices.GetAllProducts();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetAllProducts_WithNameFilter_ShouldReturnFilteredProducts()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Apple", Price = 100, DeliveryPrice = 10 },
                new Product { Id = Guid.NewGuid(), Name = "Banana", Price = 150, DeliveryPrice = 15 }
            };

            mockRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<Guid>((id) => productList.Find(p => p.Id == id));

            // Act
            var result = productServices.GetAllProducts("apple");

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Apple"));
        }

        [Test]
        public void CreateProduct_ShouldCallRepositorySave()
        {
            // Arrange
            var newProduct = new Product { Id = Guid.NewGuid(), Name = "New Product", Price = 100, DeliveryPrice = 10 };

            // Act
            productServices.CreateProduct(newProduct);

            // Assert
            mockRepository.Verify(r => r.Save(It.Is<Product>(p => p.Name == newProduct.Name)), Times.Once);
        }

        [Test]
        public void UpdateProduct_WhenProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product { Id = productId, Name = "Existing Product", Price = 100, DeliveryPrice = 10 };
            var updatedProduct = new Product { Name = "Updated Product", Price = 120, DeliveryPrice = 15 };

            mockRepository.Setup(r => r.GetById(productId)).Returns(existingProduct);

            // Act
            var result = productServices.UpdateProduct(updatedProduct, productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(updatedProduct.Name));
            Assert.That(result.Price, Is.EqualTo(updatedProduct.Price));
            Assert.That(result.DeliveryPrice, Is.EqualTo(updatedProduct.DeliveryPrice));

            // Verify Save method was called once to save the updated product
            mockRepository.Verify(r => r.Save(It.Is<Product>(p => p.Name == updatedProduct.Name)), Times.Once);
        }

        [Test]
        public void UpdateProduct_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updatedProduct = new Product { Name = "Updated Product", Price = 120, DeliveryPrice = 15 };

            mockRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act
            var result = productServices.UpdateProduct(updatedProduct, productId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DeleteProduct_WhenProductExists_ShouldDeleteProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product to Delete", Price = 100, DeliveryPrice = 10 };

            mockRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act
            var result = productServices.DeleteProduct(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(productId));

            // Verify Delete method was called once
            mockRepository.Verify(r => r.Delete(It.Is<Product>(p => p.Id == productId)), Times.Once);
        }

        [Test]
        public void DeleteProduct_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            mockRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act
            var result = productServices.DeleteProduct(productId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
