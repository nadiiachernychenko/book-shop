using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public int? Quantity { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
