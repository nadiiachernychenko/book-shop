using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class Publisher
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
