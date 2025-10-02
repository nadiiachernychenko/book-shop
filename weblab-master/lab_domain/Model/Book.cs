using System;
using System.Collections.Generic;

namespace lab_domain.Model
{
    public partial class Book
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? PublisherId { get; set; }
        public virtual Publisher? Publisher { get; set; }

        public int? GenreId { get; set; }
        public virtual BookGenre? Genre { get; set; }

        public DateTime? ReleaseDate { get; set; }   // .
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Description { get; set; }
        public double? Rating { get; set; }
        public string? ImageUrl { get; set; }

        // для навігації тегів
        public virtual ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();

        // інша колекцція
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
