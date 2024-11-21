using System.Web.Http.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using refactor_this.Controllers;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Tests.Controllers
{
    [TestFixture]
    public class ProductOptionsControllerTests
    {
        private IProductOptionsServices _productOptionsService;
        private ProductOptionsController _controller;

        [SetUp]
        public void Setup()
        {
            _productOptionsService = Substitute.For<IProductOptionsServices>();
            _controller = new ProductOptionsController(_productOptionsService);
        }
        
        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task GetOptions_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productOptionsService.GetProductOptionsAsync(productId.ToString()).Throws(new Exception("Test exception"));

            // Act
            var result = await _controller.GetOptions(productId) as ExceptionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Exception.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public async Task GetOption_ShouldReturnOkWithOption_WhenOptionExists()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption { Id = optionId, Name = "Test Option" };
            _productOptionsService.GetProductOptionByIdAsync(optionId).Returns(productOption);

            // Act
            var result = await _controller.GetOption(Guid.NewGuid(), optionId) as OkNegotiatedContentResult<ProductOption>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo(productOption));
        }

        [Test]
        public async Task GetOption_ShouldReturnNotFound_WhenOptionDoesNotExist()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            _productOptionsService.GetProductOptionByIdAsync(optionId).Returns((ProductOption)null);

            // Act
            var result = await _controller.GetOption(Guid.NewGuid(), optionId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateOption_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.CreateOption(Guid.NewGuid(), new ProductOption());

            // Assert
            Assert.That(result, Is.InstanceOf<InvalidModelStateResult>());
        }

        [Test]
        public async Task UpdateOption_ShouldReturnNotFound_WhenOptionDoesNotExist()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption { Id = optionId, Name = "Non-Existent Option" };
            _productOptionsService.UpdateProductOptionAsync(productOption, optionId).Returns((ProductOption)null);

            // Act
            var result = await _controller.UpdateOption(optionId, productOption);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteOption_ShouldReturnNotFound_WhenOptionDoesNotExist()
        {
            // Arrange
            var optionId = Guid.NewGuid();
            _productOptionsService.DeleteProductOptionAsync(optionId).Returns((ProductOption)null);

            // Act
            var result = await _controller.DeleteOption(optionId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}