using System.Web.Http.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using refactor_this.Controllers;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Tests.Controllers
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private IProductServices _productServices;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _productServices = Substitute.For<IProductServices>();
            _controller = new ProductsController(_productServices);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

                [Test]
        public async Task GetAllAsync_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _productServices.GetAllProductsAsync(null).Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllAsync() as ExceptionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exception.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public async Task GetProductAsync_ShouldReturnOkWithProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            _productServices.GetProductAsync(productId).Returns(product);

            // Act
            var result = await _controller.GetProductAsync(productId) as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo(product));
        }

        [Test]
        public async Task GetProductAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productServices.GetProductAsync(productId).Returns((Product)null);

            // Act
            var result = await _controller.GetProductAsync(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateAsync_ShouldReturnBadRequest_WhenProductIsNull()
        {
            // Act
            var result = await _controller.CreateAsync(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestErrorMessageResult>());
        }

        
        
       

        [Test]
        public async Task Update_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Non-Existent Product" };
            _productServices.UpdateProductAsync(product, productId).Returns((Product)null);

            // Act
            var result = await _controller.Update(productId, product);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productServices.DeleteProductAsync(productId).Returns((Product)null);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}