namespace PPOISSecondFirst
{
    public class Currier : Staff
    {

        public Currier(StaffInformation information)
        {
            PhoneNumber = information.phoneNumber;
            Name = information.name;
            Surname = information.surname;
            Description = information.description;
            YearsOld = information.yearsOld;

        }
        public override decimal Salary { get; }
        public override decimal Balanse { get; set; }
        public override string PhoneNumber { get; set; } = null!;


        public override string Name { get; set; } = null!;

        public override string Surname { get; set; } = null!;


        public override string Description { get; set; } = null!;

        public override int YearsOld { get; set; }

    }
}
