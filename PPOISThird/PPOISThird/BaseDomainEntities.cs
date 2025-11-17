
namespace CoreDomainEntities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual void MarkAsModified()
        {
            ModifiedAt = DateTime.Now;
        }
    }

    public abstract class AuditableEntity : BaseEntity
    {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }





    public interface IValueObject
    {
        bool Equals(object obj);
        int GetHashCode();
    }



    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }




    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
        void AddDomainEvent(IDomainEvent eventItem);
    }
}