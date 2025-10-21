namespace PPOISSecondFirst
{
    public class InstitutionInformation<T>
    {
        public Meneger _meneger { get; set; }

        public string Description { get; set; }

        public T Type { get; set; }

        public double Mark { get; set; }

        InstitutionInformation(Meneger meneger,
            string Description,
            T Type,
            double Mark,
            double countOfMeetting,
            IEnumerable<Food> Menu)
        {

            _meneger = meneger;
            this.Description = Description;
            this.Type = Type;
            this.Mark = Mark;
            this.Menu = Menu;



        }

        public double countOfMeetting { get; set; }

        public IEnumerable<Food> Menu { get; set; }
    }
}
