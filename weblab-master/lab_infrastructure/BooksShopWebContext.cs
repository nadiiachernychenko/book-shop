using System;
using System.Collections.Generic;
using lab_domain.Model;
using Microsoft.EntityFrameworkCore;

namespace lab_infrastructure;

public partial class BooksShopWebContext : DbContext
{
    public BooksShopWebContext() { }

    public BooksShopWebContext(DbContextOptions<BooksShopWebContext> options)
        : base(options) { }

    public virtual DbSet<Publisher> Publishers { get; set; }
    public virtual DbSet<BookTag> BookTags { get; set; }
    public virtual DbSet<BookGenre> BookGenres { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<Delivery> Deliveries { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<Favorite> Favorites { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Promotion> Promotions { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<SupportTicket> SupportTickets { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
        => optionsBuilder.UseSqlServer(
            "Server=DESKTOP-TK7G7R2\\SQLEXPRESS; Database=BookStore_WEB; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Publisher
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("Publisher");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.Country).HasColumnName("country").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        });

        // BookTag
        modelBuilder.Entity<BookTag>(entity =>
        {
            entity.ToTable("Book_Tags");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Tag).HasColumnName("tag").IsUnicode(false).HasMaxLength(255);

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.BookTags)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Book_Tags_Books");
        });

        // BookGenre
        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.ToTable("Book_Genres");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GenreName).HasColumnName("genre_name").IsUnicode(false).HasMaxLength(255);
        });

        // Book
        // Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsUnicode(false);
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date"); // datetime/date в БД
            entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasMaxLength(255).IsUnicode(false);

            entity.HasOne(d => d.Publisher)
                  .WithMany(p => p.Books)
                  .HasForeignKey(d => d.PublisherId)
                  .HasConstraintName("FK_Books_Publisher");

            entity.HasOne(d => d.Genre)
                  .WithMany(g => g.Books)
                  .HasForeignKey(d => d.GenreId)
                  .HasConstraintName("FK_Books_Book_Genres");
        });

        // BookTag
        modelBuilder.Entity<BookTag>(entity =>
        {
            entity.ToTable("Book_Tags");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Tag).HasColumnName("tag").HasMaxLength(255).IsUnicode(false);

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.BookTags)          // навігация к Book.BookTags
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Book_Tags_Books");
        });


        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.Carts)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Cart_Books");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Carts)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Cart_User");
        });

        // Delivery
        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Delivery");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.DeliveryAdress).HasColumnName("delivery_adress").HasColumnType("text");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeliveryMethod).HasColumnName("delivery_method").IsUnicode(false).HasMaxLength(50);

            entity.HasOne(d => d.Order)
                  .WithMany(p => p.Deliveries)
                  .HasForeignKey(d => d.OrderId)
                  .HasConstraintName("FK_Delivery_Order");
        });

        // Discount
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.ToTable("Discount");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.Property(e => e.DiscountPercentage)
                  .HasColumnName("discount_percentage")
                  .HasColumnType("decimal(5, 2)");

            entity.Property(e => e.StartDate)
                  .HasColumnName("start_date")
                  .HasColumnType("datetime");

            entity.Property(e => e.EndDate)
                  .HasColumnName("end_date")
                  .HasColumnType("datetime");

            entity.HasOne(e => e.Book)
                  .WithMany(b => b.Discounts)
                  .HasForeignKey(e => e.BookId)
                  .HasConstraintName("FK_Discount_Books");
        });



        // Favorite
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.ToTable("Favorite");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.Favorites)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Favorite_Books");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Favorites)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Favorite_User");
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order"); 
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.OrderDate).HasColumnName("order_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasColumnName("status").IsUnicode(false).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnName("total_price").HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Order_User");
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem"); 
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10, 2)").HasDefaultValue(0);

            entity.HasOne(d => d.Order)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(d => d.OrderId)
                  .HasConstraintName("FK_OrderItem_Order");

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.OrderItems)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_OrderItem_Books");
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method").IsUnicode(false).HasMaxLength(50);
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order)
                  .WithMany(p => p.Payments)
                  .HasForeignKey(d => d.OrderId)
                  .HasConstraintName("FK_Payment_Order");
        });

        // Promotion
        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.PromoName).HasColumnName("promo_name").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.Promotions)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Promotion_Books");
        });

        // Review
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewText).HasColumnName("review_text").HasColumnType("text");
            entity.Property(e => e.ReviewDate).HasColumnName("review_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Book)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(d => d.BookId)
                  .HasConstraintName("FK_Review_Books");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Review_User");
        });

        // SupportTicket
        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.ToTable("SupportTicket"); 
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TicketSubject).HasColumnName("ticket_subject").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.TicketStatus).HasColumnName("ticket_status").IsUnicode(false).HasMaxLength(50);
            entity.Property(e => e.Message).HasColumnName("message").HasColumnType("nvarchar(max)");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ResolvedDate).HasColumnName("resolved_date").HasColumnType("datetime");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.SupportTickets)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_SupportTicket_User");
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.Email).HasColumnName("email").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsUnicode(false).HasMaxLength(255);
            entity.Property(e => e.UserType).HasColumnName("user_type").IsUnicode(false).HasMaxLength(50).HasDefaultValue("customer");
            entity.Property(e => e.RegistrationDate).HasColumnName("registration_date").HasColumnType("datetime").HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
