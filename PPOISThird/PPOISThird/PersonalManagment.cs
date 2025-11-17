// Employee.cs
using CoreDomainEntities;
using SecuritySystems;
using Types;

namespace PersonalManagment
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public Department Department { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public WorkSchedule Schedule { get; set; }
        public List<Skill> Skills { get; set; }

        public int GetYearsOfService() => DateTime.Now.Year - HireDate.Year;
        public bool CanHandleArtwork(Artwork artwork) => Skills.Any(s => s.IsRelevantForArtwork(artwork));
    }

    
    public class Curator : Employee
    {
        public string Specialization { get; set; }
        public List<Exhibition> ManagedExhibitions { get; set; }
        public List<ArtMovement> ExpertMovements { get; set; }

        public bool CanCurateExhibition(Exhibition exhibition) =>
            ExpertMovements.Any(em => exhibition.Theme.Name.Contains(em.Name));
    }

   
    public class SecurityGuard : Employee
    {
        public string Shift { get; set; }
        public List<string> CertifiedWeapons { get; set; }
        public SecurityZone AssignedZone { get; set; }
        public DateTime LastTraining { get; set; }

        public bool NeedsRetraining() => DateTime.Now - LastTraining > TimeSpan.FromDays(180);
    }

    
    public class Restorer : Employee
    {
        public string Specialization { get; set; }
        public List<Material> ExpertMaterials { get; set; }
        public List<RestorationRecord> CompletedRestorations { get; set; }
        public decimal SuccessRate { get; set; }

        public bool CanRestore(Artwork artwork) =>
            ExpertMaterials.Any(em => artwork.Materials.Any(m => m.Name == em.Name));
    }

    
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Employee Manager { get; set; }
        public List<Employee> Employees { get; set; }
        public Budget DepartmentBudget { get; set; }
    }

    
    public class WorkSchedule
    {
        public DayOfWeek[] WorkDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsFullTime { get; set; }

        public bool IsWorkingDay(DateTime date) => WorkDays.Contains(date.DayOfWeek);
    }

    
    public class Skill
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public DateTime CertificationDate { get; set; }

        public bool IsRelevantForArtwork(Artwork artwork) =>
            artwork.Materials.Any(m => m.Name.Contains(Name)) ||
            artwork.Movements.Any(m => m.Name.Contains(Name));
    }

    
    public class SecurityZone
    {
        public string Name { get; set; }
        public List<GalleryHall> Halls { get; set; }
        public SecurityLevel RequiredLevel { get; set; }
        public List<SecurityCamera> Cameras { get; set; }
    }

    
    public class RestorationRecord
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Restorer PerformedBy { get; set; }
        public decimal Cost { get; set; }
        public string BeforeCondition { get; set; }
        public string AfterCondition { get; set; }
    }

    
    public class Training
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Employee Trainer { get; set; }
        public List<Employee> Attendees { get; set; }
    }

    
    public class PerformanceReview
    {
        public DateTime ReviewDate { get; set; }
        public Employee Employee { get; set; }
        public Employee Reviewer { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public List<Goal> Goals { get; set; }
    }

    
    public class Goal
    {
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }
}