using System.Net.Http.Headers;

namespace Foxic.Entities
{
    public class WishListItem:BaseEntity
    {
        public int DressId { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public Dress? Dress { get; set; }
    }
}
