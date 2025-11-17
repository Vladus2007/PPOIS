
using CoreDomainEntities.PersonalManagment;
using Exceptions;
using FinancicalSystem;
using PersonalManagment;
using SecuritySystems;
using Types;
using VisitorManagment;

namespace CoreDomainEntities
{
    
    public class Exhibition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ExhibitionTheme Theme { get; set; }
        public List<Artwork> Artworks { get; set; }
        public GalleryHall Hall { get; set; }
        public List<Visitor> Visitors { get; set; }
        public Curator Curator { get; set; }
        public Budget Budget { get; set; }

        public bool IsActive() => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
        public int GetArtworkCount() => Artworks?.Count ?? 0;
        public bool CanAddArtwork(Artwork artwork) => Artworks.Count < Hall.Capacity;
    }

    
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime FoundedDate { get; set; }
        public List<GalleryHall> Halls { get; set; }
        public List<Employee> Employees { get; set; }
        public FinancialAccount BankAccount { get; set; }
        public Director Director { get; set; }
        public List<Exhibition> Exhibitions { get; set; }

        public decimal CalculateTotalArtValue() => Halls.SelectMany(h => h.Artworks).Sum(a => a.EstimatedValue);
        public int GetTotalEmployees() => Employees.Count;
    }

    
    public class GalleryHall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public HallType Type { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
        public List<Artwork> Artworks { get; set; }
        public SecuritySystem Security { get; set; }
        public LightingSystem Lighting { get; set; }

        public bool CanAddArtwork(CoreDomainEntities.Artwork artwork) => Artworks.Count < Capacity && Security.CanSecureArtwork(artwork);
        public decimal CalculateMaintenanceCost() => Capacity * 10m;
    }

   
    public class Dimensions
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public string Unit { get; set; }

        public decimal CalculateArea() => Width * Height;
        public decimal CalculateVolume() => Width * Height * Depth;
    }

    
    public class Biography
    {
        public string EarlyLife { get; set; }
        public string Career { get; set; }
        public string StyleDescription { get; set; }
        public List<string> Awards { get; set; }
        public List<string> Education { get; set; }
    }

    
    public class ArtMovement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Artwork> Artworks { get; set; }
    }

    
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal DurabilityRating { get; set; }
    }

    
    public class RestorationHistory
    {
        public List<RestorationRecord> Records { get; set; }
        public DateTime LastRestorationDate { get; set; }
        public string CurrentCondition { get; set; }

        public bool NeedsInspection() => DateTime.Now - LastRestorationDate > TimeSpan.FromDays(365);
    }

    
    public class InsurancePolicy
    {
        public string PolicyNumber { get; set; }
        public string Provider { get; set; }
        public decimal CoverageAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Premium { get; set; }

        public bool IsActive() => DateTime.Now <= ExpiryDate;
    }

    
    public class ContactInfo
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public string Website { get; set; }
    }

    
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }

    
    public class ExhibitionTheme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ColorScheme ColorScheme { get; set; }
    }

    
    public class Budget
    {
        public decimal TotalAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public Dictionary<string, decimal> Categories { get; set; }

        public decimal GetRemainingBudget() => TotalAmount - SpentAmount;
    }

    
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public List<Decision> Decisions { get; set; }
    }

    
    public class LightingSystem
    {
        public string Type { get; set; }
        public int Intensity { get; set; }
        public string ColorTemperature { get; set; }
        public bool IsUVFiltered { get; set; }

        public bool IsSafeForArtwork(Artwork artwork) => IsUVFiltered && Intensity <= 500;
    }

    
    public class ColorScheme
    {
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string AccentColor { get; set; }
    }

   
    public class Decision
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Impact { get; set; }
    }
    
   
        public class Artwork : ArtEntity, IAggregateRoot
        {
            
            public decimal EstimatedValue { get; set; }
            public ArtworkCondition Condition { get; set; }
            public Dimensions Dimensions { get; set; }
            public Artist Artist { get; set; }
            public Exhibition CurrentExhibition { get; set; }
            public List<Material> Materials { get; set; } = new List<Material>();
            public RestorationHistory RestorationHistory { get; set; }
            public InsurancePolicy Insurance { get; set; }

            // Domain Events
            private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
            public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

            public void AddDomainEvent(IDomainEvent eventItem) => _domainEvents.Add(eventItem);
            public void ClearDomainEvents() => _domainEvents.Clear();

            // Методы
            public override string GetDisplayName() => $"{Title} by {Artist?.FullName}";
            public bool CanBeSold() => Condition != ArtworkCondition.Damaged;
            public decimal CalculateInsuranceCost() => EstimatedValue * 0.01m;
            public bool RequiresRestoration() => Condition == ArtworkCondition.Poor;

            
            public void MarkForRestoration()
            {
                if (RequiresRestoration())
                {
                    AddDomainEvent(new ArtworkRestorationRequested(this));
                }
            }
        }
    



    public class Artist : Person
    {
        public DateTime? DeathDate { get; set; }
        public string Nationality { get; set; }
        public Biography Biography { get; set; }
        public List<Artwork> Artworks { get; set; } = new List<Artwork>();
        public ArtistStyle PrimaryStyle { get; set; }
        public List<ArtMovement> Movements { get; set; } = new List<ArtMovement>();
        public ContactInfo ContactInfo { get; set; }

        public override int GetAge() => DeathDate?.Year ?? DateTime.Now.Year - BirthDate.Year;
        public bool IsContemporary() => BirthDate.Year > 1900;
        public int GetArtworkCount() => Artworks?.Count ?? 0;
    } 
}