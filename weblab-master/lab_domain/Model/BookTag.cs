using System.Text.Json.Serialization;

namespace lab_domain.Model
{
    public partial class BookTag
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Tag { get; set; } = null!;

        [JsonIgnore]                // не сериліазуємо у відповіді
        public virtual Book? Book { get; set; }   //  nullable
    }
}
