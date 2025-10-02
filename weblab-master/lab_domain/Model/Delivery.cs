using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class Delivery
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? DeliveryAdress { get; set; }

    public string? DeliveryMethod { get; set; }

    public virtual Order? Order { get; set; }
}
