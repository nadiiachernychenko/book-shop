using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
