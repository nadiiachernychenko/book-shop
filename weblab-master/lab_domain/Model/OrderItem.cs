﻿namespace lab_domain.Model
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }   

        public virtual Order Order { get; set; } = null!;

        public virtual Book Book { get; set; } = null!;
    }
}
