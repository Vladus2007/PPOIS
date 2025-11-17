





namespace CoreDomainEntities
{
    public abstract class ArtEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public List<ArtMovement> Movements { get; set; } = new List<ArtMovement>();

        public abstract string GetDisplayName();
        public virtual bool IsContemporary() => CreationDate.Year > 1900;
    }
}


namespace FinancicalSystem
{
    public abstract class FinancialEntity : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public abstract bool IsValid();
        public virtual string GetCurrencySymbol() => Currency switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            _ => Currency
        };
    }
}