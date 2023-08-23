using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final.Entity
{
    public class AppUser : IdentityUser
    {
        [NotMapped]
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
