using E_commerce.Model.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_commerce_project.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        [JsonIgnore]
        public List<Product>? Products { get; set; }
    }
}
