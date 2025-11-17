
using CoreDomainEntities;
using System.Net.Sockets;
using Types;
namespace VisitorManagment
{
    public class Visitor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public VisitorType Type { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Membership Membership { get; set; }
        public List<Exhibition> VisitedExhibitions { get; set; }

        public bool HasValidTicket(Exhibition exhibition) =>
            Tickets.Any(t => t.Exhibition == exhibition && t.IsValid());
    }

    
    public class Ticket
    {
        public string Code { get; set; }
        public Exhibition Exhibition { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public decimal Price { get; set; }
        public TicketType Type { get; set; }
        public Visitor Visitor { get; set; }

        public bool IsValid() => DateTime.Now <= ValidUntil;
        public bool IsGroupTicket() => Type == TicketType.Group;
    }

    
    public class Membership
    {
        public string Number { get; set; }
        public MembershipType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<Benefit> Benefits { get; set; }
        public decimal AnnualFee { get; set; }

        public bool IsActive() => DateTime.Now <= ExpiryDate;
        public int GetRemainingDays() => (ExpiryDate - DateTime.Now).Days;
    }

    
    public class Benefit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}