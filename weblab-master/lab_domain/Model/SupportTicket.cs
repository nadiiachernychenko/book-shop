namespace lab_domain.Model
{
    public class SupportTicket
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? TicketSubject { get; set; }

        public string? TicketStatus { get; set; }

        public string? Message { get; set; }  

        public DateTime? CreatedDate { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
