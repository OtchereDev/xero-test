using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace refactor_this.Models
{
  public class Product
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Delivery price must be a positive value.")]
        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
    }
}