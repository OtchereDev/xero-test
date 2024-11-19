using System;
using System.Net;
using System.Web.Http;
using refactor_this.Models;
using refactor_this.Services;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController: ApiController
    {
        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptionsServices GetOptions(Guid productId)
        {
            return new ProductOptionsServices(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var repo = new ProductOptionRepository();

            var option = repo.GetById(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            var repo = new ProductOptionRepository();
            option.ProductId = productId;
            repo.Save(option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var repo = new ProductOptionRepository();

            var orig = repo.GetById(id);

            if (orig != null)
            {
                orig.Name = option.Name;
                orig.Description = option.Description;
                repo.Save(orig);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var repo = new ProductOptionRepository();

            var opt = repo.GetById(id);
            repo.Delete(opt);
        }
    }
}