using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class Promotion
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public string? PromoName { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Book? Book { get; set; }
}
