using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPOISSecondFirst
{
    public class MenegerService
    {
        private Meneger _meneger;

        public MenegerService(Meneger meneger)
        {
            _meneger = meneger ?? throw new ArgumentNullException(nameof(meneger));
        }

        // Основные методы работы с менеджером
        public void UpdatePersonalInfo(string name, string surname, int yearsOld)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname cannot be null or empty", nameof(surname));
            
            if (yearsOld < 18 || yearsOld > 100)
                throw new ArgumentException("Age must be between 18 and 100", nameof(yearsOld));

            _meneger.Name = name;
            _meneger.Surname = surname;
            _meneger.YearsOld = yearsOld;
        }

        public void UpdateDescription(string description)
        {
            _meneger.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));

            if (!IsValidPhoneNumber(phoneNumber))
                throw new ArgumentException("Invalid phone number format", nameof(phoneNumber));

            _meneger.PhoneNumber = phoneNumber;
        }

        // Методы для работы с балансом
        public void AddToBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            _meneger.Balanse += amount;
        }

        public bool WithdrawFromBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            if (_meneger.Balanse >= amount)
            {
                _meneger.Balanse -= amount;
                return true;
            }
            
            return false;
        }

        public void TransferTo(Meneger targetMeneger, decimal amount)
        {
            if (targetMeneger == null)
                throw new ArgumentNullException(nameof(targetMeneger));
            
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive", nameof(amount));

            if (WithdrawFromBalance(amount))
            {
                var targetService = new MenegerService(targetMeneger);
                targetService.AddToBalance(amount);
            }
            else
            {
                throw new InvalidOperationException("Insufficient balance for transfer");
            }
        }

        // Методы для получения информации
        public string GetFullInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Manager: {_meneger.Name} {_meneger.Surname}");
            sb.AppendLine($"Age: {_meneger.YearsOld}");
            sb.AppendLine($"Phone: {_meneger.PhoneNumber}");
            sb.AppendLine($"Balance: {_meneger.Balanse:C}");
            sb.AppendLine($"Description: {_meneger.Description}");
            
            return sb.ToString();
        }

        public string GetShortInfo()
        {
            return $"{_meneger.Name} {_meneger.Surname} ({_meneger.YearsOld} years) - {_meneger.PhoneNumber}";
        }

        public bool IsEligibleForPromotion()
        {
            return _meneger.YearsOld >= 25 && _meneger.YearsOld <= 60;
        }

        public int YearsUntilRetirement(int retirementAge = 65)
        {
            var yearsLeft = retirementAge - _meneger.YearsOld;
            return Math.Max(0, yearsLeft);
        }

        // Валидационные методы
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Простая валидация номера телефона
            return phoneNumber.Length >= 10 && phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')');
        }

        public bool ValidateManagerData()
        {
            return !string.IsNullOrWhiteSpace(_meneger.Name) &&
                   !string.IsNullOrWhiteSpace(_meneger.Surname) &&
                   !string.IsNullOrWhiteSpace(_meneger.PhoneNumber) &&
                   _meneger.YearsOld >= 18 &&
                   _meneger.YearsOld <= 100;
        }

        // Методы для работы с коллекцией менеджеров
        public static Meneger FindManagerByName(IEnumerable<Meneger> managers, string name, string surname)
        {
            return managers.FirstOrDefault(m => 
                m.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                m.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Meneger> FilterManagersByAge(IEnumerable<Meneger> managers, int minAge, int maxAge)
        {
            return managers.Where(m => m.YearsOld >= minAge && m.YearsOld <= maxAge);
        }

        public static Meneger GetManagerWithHighestBalance(IEnumerable<Meneger> managers)
        {
            return managers.OrderByDescending(m => m.Balanse).FirstOrDefault();
        }

        public static IEnumerable<Meneger> SortManagersByName(IEnumerable<Meneger> managers, bool ascending = true)
        {
            return ascending ? 
                managers.OrderBy(m => m.Surname).ThenBy(m => m.Name) :
                managers.OrderByDescending(m => m.Surname).ThenByDescending(m => m.Name);
        }

        // Методы для создания и клонирования
        public static Meneger CreateManager(StaffInformation info)
        {
            return new Meneger(info);
        }

        

       
        public static decimal CalculateTotalBalance(IEnumerable<Meneger> managers)
        {
            return managers.Sum(m => m.Balanse);
        }

        public static double CalculateAverageAge(IEnumerable<Meneger> managers)
        {
            return managers.Average(m => m.YearsOld);
        }
    }
}