namespace PPOISSecondFirst
{
    public class StaffInformation
    {
        public string phoneNumber { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string description { get; set; }
        public int yearsOld { get; set; }
        

        public StaffInformation(string phoneNumber, string name, string surname, string description, int yearsOld)
        {
            this.phoneNumber = phoneNumber;
            this.name = name;
            this.surname = surname;
            this.description = description;
            this.yearsOld = yearsOld;
            
        }
    }
}
