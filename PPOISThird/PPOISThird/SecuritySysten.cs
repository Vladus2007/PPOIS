
using CoreDomainEntities;
using PersonalManagment;
using Types;

namespace SecuritySystems
{ 
    public class SecurityCamera
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public CameraType Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastMaintenance { get; set; }
        public SecurityZone Zone { get; set; }

        public bool NeedsMaintenance() => DateTime.Now - LastMaintenance > TimeSpan.FromDays(90);
    }

    
    public class AlarmSystem
    {
        public bool IsArmed { get; set; }
        public AlarmType Type { get; set; }
        public List<string> TriggerZones { get; set; }
        public DateTime LastTest { get; set; }

        public void TriggerAlarm(string zone) => IsArmed = true;
        public bool NeedsTesting() => DateTime.Now - LastTest > TimeSpan.FromDays(30);
    }

   
    public class AccessControlSystem
    {
        public List<AccessCard> Cards { get; set; }
        public List<AccessPoint> AccessPoints { get; set; }
        public Dictionary<string, List<string>> Permissions { get; set; }

        public bool HasAccess(AccessCard card, string area) =>
            Permissions[card.Level].Contains(area);
    }

    
    public class AccessCard
    {
        public string CardNumber { get; set; }
        public string Level { get; set; }
        public Employee Holder { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }

        public bool IsValid() => IsActive && DateTime.Now <= ExpiryDate;
    }

    
    public class AccessPoint
    {
        public string Location { get; set; }
        public AccessLevel RequiredLevel { get; set; }
        public bool IsOperational { get; set; }
        public DateTime LastMaintenance { get; set; }
    }
    public class SecuritySystem : BaseEntity
    {
        public SecurityLevel SecurityLevel { get; set; }
        public List<SecurityCamera> Cameras { get; set; } = new List<SecurityCamera>();
        public List<SecurityGuard> Guards { get; set; } = new List<SecurityGuard>();
        public AlarmSystem Alarm { get; set; }
        public AccessControlSystem AccessControl { get; set; }

        public bool CanSecureArtwork(Artwork artwork) =>
            artwork.EstimatedValue <= GetMaxSecuredValue();

        private decimal GetMaxSecuredValue() => SecurityLevel switch
        {
            SecurityLevel.Low => 10000,
            SecurityLevel.Medium => 100000,
            SecurityLevel.High => decimal.MaxValue,
            _ => 0
        };

        public override void MarkAsModified()
        {
            base.MarkAsModified();
            
        }
    }
}