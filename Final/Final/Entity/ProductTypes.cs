using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Final.Entity
{
    public class ProductTypes
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [NotMapped]
        public Product Product { get; set; }

        [Required, ForeignKey(nameof(Types))]
        public int TypeId { get; set; }
        [NotMapped]
        public Types Types { get; set; }
    }
}
