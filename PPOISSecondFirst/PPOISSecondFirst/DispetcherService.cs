using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPOISSecondFirst
{
    public class DispetcherService
    {
        private Dispetcher _dispetcher;
        private List<Basket> _assignedBaskets;
        private readonly ILogger _logger;

        public Dispetcher Dispatcher { get; }
        public ILogger Logger { get; }

        public DispetcherService(Dispetcher dispetcher, ILogger logger=null)
        {
            _dispetcher = dispetcher ?? throw new ArgumentNullException(nameof(dispetcher),
                "Dispatcher cannot be null. Please provide a valid dispatcher instance.");

            _logger = logger;
            _assignedBaskets = new List<Basket>();

            LogInfo($"Dispatcher service created for {dispetcher.Name} {dispetcher.Surname}");
        }




        // Основные методы работы с диспетчером (остаются без изменений)
        public void UpdatePersonalInfo(string name, string surname, int yearsOld)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null, empty or contain only whitespace characters.", nameof(name));

                if (string.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Surname cannot be null, empty or contain only whitespace characters.", nameof(surname));

                if (yearsOld < 18)
                    throw new ArgumentOutOfRangeException(nameof(yearsOld), yearsOld,
                        "Dispatcher must be at least 18 years old. Provided age is too young.");

                if (yearsOld > 100)
                    throw new ArgumentOutOfRangeException(nameof(yearsOld), yearsOld,
                        "Dispatcher age cannot exceed 100 years. Provided age is too high.");

                string oldName = _dispetcher.Name;
                string oldSurname = _dispetcher.Surname;
                int oldAge = _dispetcher.YearsOld;

                _dispetcher.Name = name.Trim();
                _dispetcher.Surname = surname.Trim();
                _dispetcher.YearsOld = yearsOld;

                LogInfo($"Personal info updated for dispatcher. Old: {oldName} {oldSurname} ({oldAge}y), New: {name} {surname} ({yearsOld}y)");
            }
            catch (Exception ex)
            {
                LogError($"Failed to update personal info for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to update personal information.", ex);
            }
        }

        public void UpdateDescription(string description)
        {
            try
            {
                if (description == null)
                    throw new ArgumentNullException(nameof(description),
                        "Description cannot be null. Use empty string if no description is needed.");

                if (description.Length > 500)
                    throw new ArgumentException($"Description length cannot exceed 500 characters. Current length: {description.Length}",
                        nameof(description));

                string oldDescription = _dispetcher.Description;
                _dispetcher.Description = description;

                LogInfo($"Description updated for dispatcher {_dispetcher.Name}. Old: '{oldDescription?.Substring(0, Math.Min(50, oldDescription?.Length ?? 0))}...', New: '{description.Substring(0, Math.Min(50, description.Length))}...'");
            }
            catch (Exception ex)
            {
                LogError($"Failed to update description for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to update description.", ex);
            }
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new ArgumentException("Phone number cannot be null, empty or contain only whitespace characters.", nameof(phoneNumber));

                if (!IsValidPhoneNumber(phoneNumber))
                    throw new ArgumentException($"Invalid phone number format: '{phoneNumber}'. Phone number must contain only digits, spaces, hyphens, parentheses and plus sign.", nameof(phoneNumber));

                if (phoneNumber.Length < 10)
                    throw new ArgumentException($"Phone number is too short: {phoneNumber.Length} characters. Minimum required: 10.", nameof(phoneNumber));

                if (phoneNumber.Length > 20)
                    throw new ArgumentException($"Phone number is too long: {phoneNumber.Length} characters. Maximum allowed: 20.", nameof(phoneNumber));

                string oldPhoneNumber = _dispetcher.PhoneNumber;
                _dispetcher.PhoneNumber = phoneNumber;

                LogInfo($"Phone number updated for dispatcher {_dispetcher.Name}. Old: {oldPhoneNumber}, New: {phoneNumber}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to update phone number for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to update phone number.", ex);
            }
        }

        // Методы для работы с балансом (остаются без изменений)
        public void AddToBalance(decimal amount)
        {
            try
            {
                if (amount <= 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), amount,
                        "Amount to add must be positive. Negative or zero values are not allowed.");

                if (amount > 1000000)
                    throw new ArgumentOutOfRangeException(nameof(amount), amount,
                        "Amount to add is too large. Maximum allowed: 1,000,000.");

                if (_dispetcher.Balanse + amount < _dispetcher.Balanse)
                    throw new OverflowException($"Balance overflow detected. Current balance: {_dispetcher.Balanse}, amount to add: {amount}");

                decimal oldBalance = _dispetcher.Balanse;
                _dispetcher.Balanse += amount;

                LogInfo($"Balance updated for dispatcher {_dispetcher.Name}. Added: {amount:C}, Old balance: {oldBalance:C}, New balance: {_dispetcher.Balanse:C}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to add {amount:C} to balance for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to add amount to balance.", ex);
            }
        }

        public bool WithdrawFromBalance(decimal amount)
        {
            try
            {
                if (amount <= 0)
                    throw new ArgumentOutOfRangeException(nameof(amount), amount,
                        "Withdrawal amount must be positive. Negative or zero values are not allowed.");

                if (amount > 1000000)
                    throw new ArgumentOutOfRangeException(nameof(amount), amount,
                        "Withdrawal amount is too large. Maximum allowed: 1,000,000.");

                if (_dispetcher.Balanse < amount)
                {
                    LogWarning($"Insufficient balance for withdrawal. Dispatcher: {_dispetcher.Name}, Requested: {amount:C}, Available: {_dispetcher.Balanse:C}");
                    return false;
                }

                decimal oldBalance = _dispetcher.Balanse;
                _dispetcher.Balanse -= amount;

                LogInfo($"Balance withdrawal for dispatcher {_dispetcher.Name}. Withdrawn: {amount:C}, Old balance: {oldBalance:C}, New balance: {_dispetcher.Balanse:C}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to withdraw {amount:C} from balance for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to withdraw from balance.", ex);
            }
        }

        // Методы для работы с корзинами (адаптированы под Basket)
        public void AssignBasket(Basket basket)
        {
            try
            {
                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (basket.price < 0)
                    throw new ArgumentException($"Invalid basket price: {basket.price}. Price cannot be negative.", nameof(basket));

                if (_assignedBaskets.Contains(basket))
                    throw new InvalidOperationException($"Basket is already assigned to dispatcher {_dispetcher.Name}.");

                if (!IsAvailableForNewBaskets())
                    throw new DispatcherOverloadedException(_dispetcher.Name, _assignedBaskets.Count, GetActiveBaskets().Count());

                _assignedBaskets.Add(basket);

                LogInfo($"Basket assigned to dispatcher {_dispetcher.Name}. Price: {basket.price:C}, Total items: {basket.BasketOfFood?.Count() ?? 0}, Total assigned baskets: {_assignedBaskets.Count}");
            }
            catch (DispatcherOverloadedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogError($"Failed to assign basket to dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to assign basket.", ex);
            }
        }

        public bool CompleteBasket(Basket basket)
        {
            try
            {
                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (!_assignedBaskets.Contains(basket))
                {
                    LogWarning($"Basket not found in assigned baskets for dispatcher {_dispetcher.Name}");
                    return false;
                }

                // В реальной системе здесь была бы логика отметки корзины как выполненной
                _assignedBaskets.Remove(basket);

                // Начисляем бонус за выполненную корзину
                decimal bonus = CalculateBasketBonus(basket);
                AddToBalance(bonus);

                LogInfo($"Basket completed by dispatcher {_dispetcher.Name}. Price: {basket.price:C}, Bonus: {bonus:C}, Remaining baskets: {_assignedBaskets.Count}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to complete basket for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to complete basket.", ex);
            }
        }

        public bool CancelBasket(Basket basket)
        {
            try
            {
                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (!_assignedBaskets.Contains(basket))
                {
                    LogWarning($"Basket not found in assigned baskets for dispatcher {_dispetcher.Name}");
                    return false;
                }

                _assignedBaskets.Remove(basket);

                LogInfo($"Basket cancelled by dispatcher {_dispetcher.Name}. Price: {basket.price:C}, Remaining baskets: {_assignedBaskets.Count}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to cancel basket for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to cancel basket.", ex);
            }
        }

        public void ReassignBasket(Basket basket, Dispetcher newDispatcher)
        {
            try
            {
                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (newDispatcher == null)
                    throw new ArgumentNullException(nameof(newDispatcher), "New dispatcher cannot be null.");

                if (newDispatcher == _dispetcher)
                    throw new ArgumentException("Cannot reassign basket to the same dispatcher.", nameof(newDispatcher));

                if (!_assignedBaskets.Contains(basket))
                    throw new InvalidOperationException($"Basket not found in assigned baskets for dispatcher {_dispetcher.Name}.");

                var newDispatcherService = new DispetcherService(newDispatcher, _logger);
                if (!newDispatcherService.IsAvailableForNewBaskets())
                    throw new DispatcherOverloadedException(newDispatcher.Name,
                        newDispatcherService.GetTotalBasketsCount(), newDispatcherService.GetActiveBaskets().Count());

                newDispatcherService.AssignBasket(basket);
                _assignedBaskets.Remove(basket);

                LogInfo($"Basket reassigned from dispatcher {_dispetcher.Name} to {newDispatcher.Name}. Price: {basket.price:C}");
            }
            catch (DispatcherOverloadedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogError($"Failed to reassign basket from dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to reassign basket.", ex);
            }
        }

        // Методы для получения информации о корзинах
        public IEnumerable<Basket> GetActiveBaskets()
        {
            try
            {
                return _assignedBaskets.ToList();
            }
            catch (Exception ex)
            {
                LogError($"Failed to get active baskets for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to retrieve active baskets.", ex);
            }
        }

        public int GetTotalBasketsCount()
        {
            try
            {
                return _assignedBaskets.Count;
            }
            catch (Exception ex)
            {
                LogError($"Failed to get total baskets count for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to retrieve baskets count.", ex);
            }
        }

        public decimal GetTotalBasketsValue()
        {
            try
            {
                return _assignedBaskets.Sum(b => b.price);
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate total baskets value for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to calculate total baskets value.", ex);
            }
        }

        public int GetTotalFoodItemsCount()
        {
            try
            {
                return _assignedBaskets.Sum(b => b.BasketOfFood?.Count() ?? 0);
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate total food items count for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to calculate total food items count.", ex);
            }
        }

        // Методы для расчета бонусов и производительности
        public decimal CalculateBasketBonus(Basket basket)
        {
            try
            {
                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (basket.price < 0)
                    throw new ArgumentException("Basket price cannot be negative.", nameof(basket));

                // Бонус: 5% от стоимости корзины, но не менее 10 и не более 500
                decimal bonus = basket.price * 0.05m;
                bonus = Math.Max(10, Math.Min(500, bonus));

                LogInfo($"Bonus calculated for basket. Price: {basket.price:C}, Bonus: {bonus:C}");
                return bonus;
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate bonus for basket", ex);
                throw new DispatcherServiceException("Failed to calculate basket bonus.", ex);
            }
        }

        public decimal CalculateTotalPotentialBonus()
        {
            try
            {
                return _assignedBaskets.Sum(basket => CalculateBasketBonus(basket));
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate total potential bonus for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to calculate total potential bonus.", ex);
            }
        }

        public double GetEfficiencyRating()
        {
            try
            {
                if (_assignedBaskets.Count == 0)
                    return 0;

                decimal avgBasketValue = GetTotalBasketsValue() / _assignedBaskets.Count;
                int totalItems = GetTotalFoodItemsCount();

                if (totalItems == 0)
                    return 0;

                // Эффективность рассчитывается на основе средней стоимости корзины и количества товаров
                double efficiency = (double)(avgBasketValue * totalItems) / 1000;

                if (double.IsNaN(efficiency) || double.IsInfinity(efficiency))
                    return 0;

                return Math.Min(100, efficiency); // Ограничиваем 100%
            }
            catch (Exception ex)
            {
                LogError($"Failed to calculate efficiency rating for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to calculate efficiency rating.", ex);
            }
        }

        // Методы для получения информации о диспетчере
        public string GetFullInfo()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Dispatcher: {_dispetcher.Name} {_dispetcher.Surname}");
                sb.AppendLine($"Age: {_dispetcher.YearsOld}");
                sb.AppendLine($"Phone: {_dispetcher.PhoneNumber}");
                sb.AppendLine($"Balance: {_dispetcher.Balanse:C}");
                sb.AppendLine($"Description: {_dispetcher.Description}");
                sb.AppendLine($"Assigned Baskets: {GetTotalBasketsCount()}");
                sb.AppendLine($"Total Basket Value: {GetTotalBasketsValue():C}");
                sb.AppendLine($"Total Food Items: {GetTotalFoodItemsCount()}");
                sb.AppendLine($"Efficiency Rating: {GetEfficiencyRating():F1}%");
                sb.AppendLine($"Potential Bonus: {CalculateTotalPotentialBonus():C}");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError($"Failed to generate full info for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to generate dispatcher information.", ex);
            }
        }

        public string GetPerformanceReport()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Performance Report for {_dispetcher.Name} {_dispetcher.Surname}");
                sb.AppendLine($"=========================================");
                sb.AppendLine($"Total baskets handled: {GetTotalBasketsCount()}");
                sb.AppendLine($"Total basket value: {GetTotalBasketsValue():C}");
                sb.AppendLine($"Total food items: {GetTotalFoodItemsCount()}");
                sb.AppendLine($"Efficiency rating: {GetEfficiencyRating():F1}%");
                sb.AppendLine($"Current balance: {_dispetcher.Balanse:C}");
                sb.AppendLine($"Potential bonus: {CalculateTotalPotentialBonus():C}");

                var activeBaskets = GetActiveBaskets();
                if (activeBaskets.Any())
                {
                    sb.AppendLine($"Active baskets: {activeBaskets.Count()}");
                    foreach (var basket in activeBaskets)
                    {
                        sb.AppendLine($"  - Basket value: {basket.price:C}, Items: {basket.BasketOfFood?.Count() ?? 0}");
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError($"Failed to generate performance report for dispatcher {_dispetcher.Name}", ex);
                throw new DispatcherServiceException("Failed to generate performance report.", ex);
            }
        }

        // Валидационные методы
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return false;

                string cleanNumber = new string(phoneNumber.Where(c => char.IsDigit(c)).ToArray());

                return cleanNumber.Length >= 10 &&
                       cleanNumber.Length <= 15 &&
                       phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')') &&
                       !phoneNumber.Contains("  ") &&
                       phoneNumber.Trim() == phoneNumber;
            }
            catch (Exception ex)
            {
                LogError($"Phone number validation failed for input: '{phoneNumber}'", ex);
                return false;
            }
        }

        public bool ValidateDispatcherData()
        {
            try
            {
                bool isValid = !string.IsNullOrWhiteSpace(_dispetcher.Name) &&
                             !string.IsNullOrWhiteSpace(_dispetcher.Surname) &&
                             !string.IsNullOrWhiteSpace(_dispetcher.PhoneNumber) &&
                             _dispetcher.YearsOld >= 18 &&
                             _dispetcher.YearsOld <= 100 &&
                             IsValidPhoneNumber(_dispetcher.PhoneNumber) &&
                             _dispetcher.Name.Length <= 50 &&
                             _dispetcher.Surname.Length <= 50 &&
                             _dispetcher.Description?.Length <= 500;

                if (!isValid)
                {
                    LogWarning($"Dispatcher data validation failed for {_dispetcher.Name} {_dispetcher.Surname}");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                LogError($"Dispatcher data validation process failed for {_dispetcher.Name}", ex);
                return false;
            }
        }

        public bool IsAvailableForNewBaskets(int maxBasketsPerDispatcher = 15)
        {
            try
            {
                if (maxBasketsPerDispatcher <= 0)
                    throw new ArgumentOutOfRangeException(nameof(maxBasketsPerDispatcher), maxBasketsPerDispatcher,
                        "Maximum baskets per dispatcher must be positive.");

                int activeBasketsCount = GetActiveBaskets().Count();
                bool isAvailable = activeBasketsCount < maxBasketsPerDispatcher;

                if (!isAvailable)
                {
                    LogWarning($"Dispatcher {_dispetcher.Name} is overloaded. Active baskets: {activeBasketsCount}, Max allowed: {maxBasketsPerDispatcher}");
                }

                return isAvailable;
            }
            catch (Exception ex)
            {
                LogError($"Failed to check availability for dispatcher {_dispetcher.Name}", ex);
                return false;
            }
        }

        
        public static Dispetcher FindDispatcherByName(IEnumerable<Dispetcher> dispatchers, string name, string surname)
        {
            try
            {
                if (dispatchers == null)
                    throw new ArgumentNullException(nameof(dispatchers), "Dispatchers collection cannot be null.");

                return dispatchers.FirstOrDefault(d =>
                    d.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    d.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                // Логирование здесь, так как это статический метод
                System.Diagnostics.Debug.WriteLine($"Error finding dispatcher by name: {ex.Message}");
                throw new DispatcherServiceException("Failed to find dispatcher by name.", ex);
            }
        }

        public static Dispetcher FindMostEfficientDispatcher(IEnumerable<Dispetcher> dispatchers)
        {
            try
            {
                if (dispatchers == null)
                    throw new ArgumentNullException(nameof(dispatchers), "Dispatchers collection cannot be null.");

                if (!dispatchers.Any())
                    return null;

                return dispatchers.OrderByDescending(d =>
                {
                    var service = new DispetcherService(d);
                    return service.GetEfficiencyRating();
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error finding most efficient dispatcher: {ex.Message}");
                throw new DispatcherServiceException("Failed to find most efficient dispatcher.", ex);
            }
        }

        // Вспомогательные методы для логирования
        private void LogInfo(string message)
        {
            _logger?.LogInformation($"[DispatcherService] {message}");
        }

        private void LogWarning(string message)
        {
            _logger?.LogWarning($"[DispatcherService] {message}");
        }

        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[DispatcherService] {message}");
        }
    }

   
    public class DispatcherServiceException : Exception
    {
        public DispatcherServiceException() : base() { }
        public DispatcherServiceException(string message) : base(message) { }
        public DispatcherServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DispatcherOverloadedException : InvalidOperationException
    {
        public string DispatcherName { get; }
        public int TotalBaskets { get; }
        public int ActiveBaskets { get; }

        public DispatcherOverloadedException(string dispatcherName, int totalBaskets, int activeBaskets)
            : base($"Dispatcher '{dispatcherName}' is overloaded. Total baskets: {totalBaskets}, Active baskets: {activeBaskets}")
        {
            DispatcherName = dispatcherName;
            TotalBaskets = totalBaskets;
            ActiveBaskets = activeBaskets;
        }
    }
}