using System;
using System.ComponentModel.DataAnnotations;

namespace lab_domain.Model
{
    public class Discount
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Виберіть книгу")]
        public int BookId { get; set; }

        public virtual Book? Book { get; set; }

        [Required(ErrorMessage = "Вкажіть процес знижки")]
        [Range(0, 100, ErrorMessage = "Процент повинен бути від 0 до 100")]
        [Display(Name = "Discount (%)")]
        public decimal? DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Вкажіть дату спочатку")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Вкажіть дату завершення")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime? EndDate { get; set; }
    }
}
