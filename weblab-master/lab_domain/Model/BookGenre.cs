using System;
using System.Collections.Generic;

namespace lab_domain.Model;

public partial class BookGenre
{
    public int Id { get; set; }

    public string? GenreName { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
