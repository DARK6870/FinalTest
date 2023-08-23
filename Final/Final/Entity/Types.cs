using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Final.Entity
{
    public class Types
    {
        [Key, Required, Column(TypeName = "tinyint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TypeId { get; set; }

        [Required, StringLength(20), Column(TypeName = "nvarchar(20)")]
        public string TypeName { get; set; }

        [JsonIgnore]
        public ICollection<ProductTypes> ProductTypes { get; set; }
    }
}
