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
                throw new InvalidManagerDataException("Name", "Имя не может быть пустым или null");
            
            if (string.IsNullOrWhiteSpace(surname))
                throw new InvalidManagerDataException("Surname", "Фамилия не может быть пустой или null");
            
            if (yearsOld < 18 || yearsOld > 100)
                throw new InvalidAgeException(yearsOld);

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
                throw new InvalidManagerDataException("PhoneNumber", "Номер телефона не может быть пустым");

            if (!IsValidPhoneNumber(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber);

            _meneger.PhoneNumber = phoneNumber;
        }

        // Методы для работы с балансом
        public void AddToBalance(decimal amount)
        {
            if (amount <= 0)
                throw new NegativeAmountException(amount);

            _meneger.Balanse += amount;
        }

        public bool WithdrawFromBalance(decimal amount)
        {
            if (amount <= 0)
                throw new NegativeAmountException(amount);

            if (_meneger.Balanse >= amount)
            {
                _meneger.Balanse -= amount;
                return true;
            }
            
            return false;
        }

        public void WithdrawFromBalanceWithException(decimal amount)
        {
            if (amount <= 0)
                throw new NegativeAmountException(amount);

            if (_meneger.Balanse < amount)
                throw new InsufficientBalanceException(_meneger.Balanse, amount);

            _meneger.Balanse -= amount;
        }

        public void TransferTo(Meneger targetMeneger, decimal amount)
        {
            if (targetMeneger == null)
                throw new ArgumentNullException(nameof(targetMeneger));
            
            if (ReferenceEquals(_meneger, targetMeneger))
                throw new TransferToSelfException();
            
            if (amount <= 0)
                throw new NegativeAmountException(amount);

            if (_meneger.Balanse < amount)
                throw new InsufficientBalanceException(_meneger.Balanse, amount);

            // Выполняем перевод
            WithdrawFromBalanceWithException(amount);
            var targetService = new MenegerService(targetMeneger);
            targetService.AddToBalance(amount);
        }

        // Методы для получения информации
        public string GetFullInfo()
        {
            if (!ValidateManagerData())
                throw new InvalidManagerDataException("General", "Данные менеджера невалидны");

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
            if (string.IsNullOrWhiteSpace(_meneger.Name) || string.IsNullOrWhiteSpace(_meneger.Surname))
                throw new InvalidManagerDataException("Name/Surname", "Имя и фамилия должны быть заполнены");

            return $"{_meneger.Name} {_meneger.Surname} ({_meneger.YearsOld} years) - {_meneger.PhoneNumber}";
        }

        public bool IsEligibleForPromotion()
        {
            return _meneger.YearsOld >= 25 && _meneger.YearsOld <= 60;
        }

        public int YearsUntilRetirement(int retirementAge = 65)
        {
            if (retirementAge <= _meneger.YearsOld)
                throw new InvalidAgeException(retirementAge);

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

        public void ValidateManagerDataWithException()
        {
            if (string.IsNullOrWhiteSpace(_meneger.Name))
                throw new InvalidManagerDataException("Name", "Имя обязательно для заполнения");
            
            if (string.IsNullOrWhiteSpace(_meneger.Surname))
                throw new InvalidManagerDataException("Surname", "Фамилия обязательна для заполнения");
            
            if (string.IsNullOrWhiteSpace(_meneger.PhoneNumber))
                throw new InvalidManagerDataException("PhoneNumber", "Номер телефона обязателен для заполнения");
            
            if (_meneger.YearsOld < 18 || _meneger.YearsOld > 100)
                throw new InvalidAgeException(_meneger.YearsOld);
        }

        // Методы для работы с коллекцией менеджеров
        public static Meneger FindManagerByName(IEnumerable<Meneger> managers, string name, string surname)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));
            
            if (!managers.Any())
                throw new EmptyManagersCollectionException();

            var manager = managers.FirstOrDefault(m => 
                m.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                m.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

            if (manager == null)
                throw new ManagerNotFoundException(name, surname);

            return manager;
        }

        public static IEnumerable<Meneger> FilterManagersByAge(IEnumerable<Meneger> managers, int minAge, int maxAge)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));

            if (minAge < 18 || maxAge > 100 || minAge > maxAge)
                throw new InvalidAgeException(minAge);

            return managers.Where(m => m.YearsOld >= minAge && m.YearsOld <= maxAge);
        }

        public static Meneger GetManagerWithHighestBalance(IEnumerable<Meneger> managers)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));
            
            if (!managers.Any())
                throw new EmptyManagersCollectionException();

            return managers.OrderByDescending(m => m.Balanse).FirstOrDefault();
        }

        public static IEnumerable<Meneger> SortManagersByName(IEnumerable<Meneger> managers, bool ascending = true)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));

            return ascending ? 
                managers.OrderBy(m => m.Surname).ThenBy(m => m.Name) :
                managers.OrderByDescending(m => m.Surname).ThenByDescending(m => m.Name);
        }

        // Методы для создания и клонирования
        public static Meneger CreateManager(StaffInformation info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            return new Meneger(info);
        }

        public static decimal CalculateTotalBalance(IEnumerable<Meneger> managers)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));

            return managers.Sum(m => m.Balanse);
        }

        public static double CalculateAverageAge(IEnumerable<Meneger> managers)
        {
            if (managers == null)
                throw new ArgumentNullException(nameof(managers));
            
            if (!managers.Any())
                throw new EmptyManagersCollectionException();

            return managers.Average(m => m.YearsOld);
        }

        // Безопасные версии методов
        public bool TryUpdatePersonalInfo(string name, string surname, int yearsOld)
        {
            try
            {
                UpdatePersonalInfo(name, surname, yearsOld);
                return true;
            }
            catch (ManagerServiceException)
            {
                return false;
            }
        }

        public bool TryTransferTo(Meneger targetMeneger, decimal amount)
        {
            try
            {
                TransferTo(targetMeneger, amount);
                return true;
            }
            catch (ManagerServiceException)
            {
                return false;
            }
        }
    }
    public class InvalidManagerDataException : ManagerValidationException
{
    public InvalidManagerDataException(string propertyName, string message) 
        : base(propertyName, message)
    {
    }
}

public class InsufficientBalanceException : ManagerServiceException
{
    public decimal CurrentBalance { get; }
    public decimal RequiredAmount { get; }

    public InsufficientBalanceException(decimal currentBalance, decimal requiredAmount) 
        : base($"Недостаточно средств. Текущий баланс: {currentBalance:C}, требуется: {requiredAmount:C}")
    {
        CurrentBalance = currentBalance;
        RequiredAmount = requiredAmount;
    }
}

public class InvalidAgeException : ManagerValidationException
{
    public int InvalidAge { get; }

    public InvalidAgeException(int age) 
        : base("YearsOld", $"Некорректный возраст: {age}. Возраст должен быть от 18 до 100 лет")
    {
        InvalidAge = age;
    }
}

public class InvalidPhoneNumberException : ManagerValidationException
{
    public string InvalidPhone { get; }

    public InvalidPhoneNumberException(string phoneNumber) 
        : base("PhoneNumber", $"Некорректный номер телефона: {phoneNumber}")
    {
        InvalidPhone = phoneNumber;
    }
}

public class NegativeAmountException : ManagerServiceException
{
    public decimal InvalidAmount { get; }

    public NegativeAmountException(decimal amount) 
        : base($"Сумма не может быть отрицательной или нулевой: {amount}")
    {
        InvalidAmount = amount;
    }
}

public class EmptyManagersCollectionException : ManagerServiceException
{
    public EmptyManagersCollectionException() 
        : base("Коллекция менеджеров пуста")
    {
    }
}

public class TransferToSelfException : ManagerServiceException
{
    public TransferToSelfException() 
        : base("Невозможно выполнить перевод самому себе")
    {
    }
}
}
