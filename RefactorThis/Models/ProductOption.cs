using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace refactor_this.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Product option name is required.")]
        [StringLength(100, ErrorMessage = "Product option name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description name is required.")]
        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
    }
}