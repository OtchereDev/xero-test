using Moq;
using refactor_this.Models;
using refactor_this.Services;

namespace RefactorThis.Tests.Services
{
    [TestFixture]
    public class ProductOptionsServicesTests
    {
    //     private Mock<ProductOptionRepository> mockRepository;
    //     private ProductOptionsServices productOptionsServices;
    //
    //     [SetUp]
    //     public void SetUp()
    //     {
    //         // Create mock of the ProductOptionRepository
    //         mockRepository = new Mock<ProductOptionRepository>();
    //
    //         // Create instance of ProductOptionsServices with the mocked ProductOptionRepository
    //         productOptionsServices = new ProductOptionsServices(mockRepository.Object);
    //     }
    //
    //     [Test]
    //     public void GetProductOptions_WhenCalled_ShouldReturnAllProductOptions()
    //     {
    //         // Arrange
    //         var productOptionList = new List<ProductOption>
    //         {
    //             new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 1", Description = "Option 1 Description" },
    //             new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 2", Description = "Option 2 Description" }
    //         };
    //
    //         mockRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<Guid>((id) => productOptionList.Find(o => o.Id == id));
    //
    //         // Act
    //         var result = productOptionsServices.GetProductOptions();
    //
    //         // Assert
    //         Assert.That(result.Count, Is.EqualTo(2));
    //     }
    //
    //     [Test]
    //     public void GetProductOptions_WithProductId_ShouldReturnFilteredProductOptions()
    //     {
    //         // Arrange
    //         var productOptionList = new List<ProductOption>
    //         {
    //             new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 1", Description = "Option 1 Description" },
    //             new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 2", Description = "Option 2 Description" }
    //         };
    //
    //         mockRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns<Guid>((id) => productOptionList.Find(o => o.Id == id));
    //
    //         // Act
    //         var result = productOptionsServices.GetProductOptions("someProductId");
    //
    //         // Assert
    //         Assert.That(result.Count, Is.EqualTo(2));
    //     }
    //
    //     [Test]
    //     public void CreateProductOption_ShouldCallRepositorySave()
    //     {
    //         // Arrange
    //         var productId = Guid.NewGuid();
    //         var productOption = new ProductOption { Id = Guid.NewGuid(), Name = "New Option", Description = "New Option Description" };
    //
    //         // Act
    //         productOptionsServices.CreateProductOption(productOption, productId);
    //
    //         // Assert
    //         mockRepository.Verify(r => r.Save(It.Is<ProductOption>(po => po.Name == productOption.Name && po.ProductId == productId)), Times.Once);
    //     }
    //
    //     [Test]
    //     public void UpdateProductOption_WhenProductOptionExists_ShouldUpdateProductOption()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //         var existingOption = new ProductOption { Id = productOptionId, Name = "Existing Option", Description = "Existing Option Description" };
    //         var updatedOption = new ProductOption { Name = "Updated Option", Description = "Updated Option Description" };
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns(existingOption);
    //
    //         // Act
    //         var result = productOptionsServices.UpdateProductOption(updatedOption, productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Not.Null);
    //         Assert.That(result.Name, Is.EqualTo(updatedOption.Name));
    //         Assert.That(result.Description, Is.EqualTo(updatedOption.Description));
    //
    //         // Verify Save method was called once to save the updated product option
    //         mockRepository.Verify(r => r.Save(It.Is<ProductOption>(po => po.Name == updatedOption.Name)), Times.Once);
    //     }
    //
    //     [Test]
    //     public void UpdateProductOption_WhenProductOptionDoesNotExist_ShouldReturnNull()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //         var updatedOption = new ProductOption { Name = "Updated Option", Description = "Updated Option Description" };
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns((ProductOption)null);
    //
    //         // Act
    //         var result = productOptionsServices.UpdateProductOption(updatedOption, productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Null);
    //     }
    //
    //     [Test]
    //     public void DeleteProductOption_WhenProductOptionExists_ShouldDeleteProductOption()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //         var productOption = new ProductOption { Id = productOptionId, Name = "Option to Delete", Description = "Option Description" };
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns(productOption);
    //
    //         // Act
    //         var result = productOptionsServices.DeleteProductOption(productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Not.Null);
    //         Assert.That(result.Id, Is.EqualTo(productOptionId));
    //
    //         // Verify Delete method was called once
    //         mockRepository.Verify(r => r.Delete(It.Is<ProductOption>(po => po.Id == productOptionId)), Times.Once);
    //     }
    //
    //     [Test]
    //     public void DeleteProductOption_WhenProductOptionDoesNotExist_ShouldReturnNull()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns((ProductOption)null);
    //
    //         // Act
    //         var result = productOptionsServices.DeleteProductOption(productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Null);
    //     }
    //
    //     [Test]
    //     public void GetProductOptionById_WhenProductOptionExists_ShouldReturnProductOption()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //         var productOption = new ProductOption { Id = productOptionId, Name = "Existing Option", Description = "Existing Option Description" };
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns(productOption);
    //
    //         // Act
    //         var result = productOptionsServices.GetProductOptionById(productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Not.Null);
    //         Assert.That(result.Id, Is.EqualTo(productOptionId));
    //     }
    //
    //     [Test]
    //     public void GetProductOptionById_WhenProductOptionDoesNotExist_ShouldReturnNull()
    //     {
    //         // Arrange
    //         var productOptionId = Guid.NewGuid();
    //
    //         mockRepository.Setup(r => r.GetById(productOptionId)).Returns((ProductOption)null);
    //
    //         // Act
    //         var result = productOptionsServices.GetProductOptionById(productOptionId);
    //
    //         // Assert
    //         Assert.That(result, Is.Null);
    //     }
    }
}
