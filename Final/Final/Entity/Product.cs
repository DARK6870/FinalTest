using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Final.Entity
{
    public class Product
    {
        [Key, Required]
        public int ProductId { get; set; }

        [Required, StringLength(30), Column(TypeName = "nvarchar(30)")]
        public string ProductName { get; set; }

        [Required, StringLength(100), Column(TypeName = "nvarchar(100)")]
        public string ImagePath { get; set; }

        [Required, Column(TypeName = "Decimal(6, 2)")]
        public Decimal price { get; set; }
        [JsonIgnore]
        public ICollection<ProductTypes> ProductTypes { get; set;}
        [JsonIgnore]
        public ICollection<WishList> WishLists { get; set; }

    }
}
