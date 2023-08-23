using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Final.Entity
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey(nameof(AppUser))]
        public string AppUserId { get; set; }
        [JsonIgnore]
        public virtual AppUser AppUser { get; set; }
        [Required]
        public int ProductId { get; set; }
        [NotMapped]
        public virtual Product Product { get; set; }
    }
}
