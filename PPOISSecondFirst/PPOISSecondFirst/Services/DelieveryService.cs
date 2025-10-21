using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PPOISSecondFirst
{
    public class DeliveryFoodService
    {
        private DelieviryFood _deliveryFood;
        private readonly ILogger _logger;

        public DeliveryFoodService(DelieviryFood deliveryFood, ILogger logger=null)
        {
            _deliveryFood = deliveryFood ?? throw new ArgumentNullException(nameof(deliveryFood),
                "DeliveryFood instance cannot be null. Please provide a valid instance.");

            _logger = logger;

            LogInfo($"DeliveryFoodService created for company: {DelieviryFood.name}");
        }


        public void RenameCompany(string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newName))
                    throw new ArgumentException("Company name cannot be null, empty or contain only whitespace characters.", nameof(newName));

                if (newName.Length > 100)
                    throw new ArgumentException($"Company name is too long: {newName.Length} characters. Maximum allowed: 100.", nameof(newName));

                if (newName.Any(char.IsDigit))
                    throw new ArgumentException("Company name cannot contain digits.", nameof(newName));

                string oldName = DelieviryFood.name;
                _deliveryFood.Rename(newName);

                LogInfo($"Company renamed successfully. Old name: '{oldName}', New name: '{DelieviryFood.name}'");
            }
            catch (Exception ex)
            {
                LogError($"Failed to rename company to '{newName}'", ex);
                throw new DeliveryFoodServiceException("Failed to rename company.", ex);
            }
        }

        public string GetCompanyInfo()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Company: {DelieviryFood.name}");
                sb.AppendLine($"Dispatchers Count: {_deliveryFood.Dispetchers?.Count() ?? 0}");
                sb.AppendLine($"Couriers Count: {_deliveryFood.curriers?.Count() ?? 0}");
                sb.AppendLine($"Total Staff: {GetTotalStaffCount()}");
                sb.AppendLine($"Singleton Instance: {(DelieviryFood.singletone != null ? "Created" : "Not Created")}");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError("Failed to get company information", ex);
                throw new DeliveryFoodServiceException("Failed to retrieve company information.", ex);
            }
        }

        // Методы для работы с диспетчерами
        public void AddDispatcher(Dispetcher dispatcher)
        {
            try
            {
                if (dispatcher == null)
                    throw new ArgumentNullException(nameof(dispatcher), "Dispatcher cannot be null.");

                var dispatchersList = _deliveryFood.Dispetchers?.ToList() ?? new List<Dispetcher>();

                if (dispatchersList.Any(d => d.Name == dispatcher.Name && d.Surname == dispatcher.Surname))
                    throw new InvalidOperationException($"Dispatcher {dispatcher.Name} {dispatcher.Surname} already exists in the company.");

                dispatchersList.Add(dispatcher);
                _deliveryFood.Dispetchers = dispatchersList;

                LogInfo($"Dispatcher {dispatcher.Name} {dispatcher.Surname} added to company. Total dispatchers: {dispatchersList.Count}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to add dispatcher {dispatcher?.Name} {dispatcher?.Surname}", ex);
                throw new DeliveryFoodServiceException("Failed to add dispatcher.", ex);
            }
        }

        public bool RemoveDispatcher(string name, string surname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Dispatcher name cannot be null or empty.", nameof(name));

                if (string.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Dispatcher surname cannot be null or empty.", nameof(surname));

                var dispatchersList = _deliveryFood.Dispetchers?.ToList();
                if (dispatchersList == null || !dispatchersList.Any())
                {
                    LogWarning("No dispatchers available to remove.");
                    return false;
                }

                var dispatcherToRemove = dispatchersList.FirstOrDefault(d =>
                    d.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    d.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

                if (dispatcherToRemove == null)
                {
                    LogWarning($"Dispatcher {name} {surname} not found in company.");
                    return false;
                }

                dispatchersList.Remove(dispatcherToRemove);
                _deliveryFood.Dispetchers = dispatchersList;

                LogInfo($"Dispatcher {name} {surname} removed from company. Remaining dispatchers: {dispatchersList.Count}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to remove dispatcher {name} {surname}", ex);
                throw new DeliveryFoodServiceException("Failed to remove dispatcher.", ex);
            }
        }

        public Dispetcher FindDispatcher(string name, string surname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Dispatcher name cannot be null or empty.", nameof(name));

                if (string.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Dispatcher surname cannot be null or empty.", nameof(surname));

                return _deliveryFood.Dispetchers?.FirstOrDefault(d =>
                    d.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    d.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                LogError($"Failed to find dispatcher {name} {surname}", ex);
                throw new DeliveryFoodServiceException("Failed to find dispatcher.", ex);
            }
        }

        // Методы для работы с курьерами
        public void AddCourier(Currier courier)
        {
            try
            {
                if (courier == null)
                    throw new ArgumentNullException(nameof(courier), "Courier cannot be null.");

                var couriersList = _deliveryFood.curriers?.ToList() ?? new List<Currier>();

                if (couriersList.Any(c => c.Name == courier.Name && c.Surname == courier.Surname))
                    throw new InvalidOperationException($"Courier {courier.Name} {courier.Surname} already exists in the company.");

                couriersList.Add(courier);
                _deliveryFood.curriers = couriersList;

                LogInfo($"Courier {courier.Name} {courier.Surname} added to company. Total couriers: {couriersList.Count}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to add courier {courier?.Name} {courier?.Surname}", ex);
                throw new DeliveryFoodServiceException("Failed to add courier.", ex);
            }
        }

        public bool RemoveCourier(string name, string surname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Courier name cannot be null or empty.", nameof(name));

                if (string.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Courier surname cannot be null or empty.", nameof(surname));

                var couriersList = _deliveryFood.curriers?.ToList();
                if (couriersList == null || !couriersList.Any())
                {
                    LogWarning("No couriers available to remove.");
                    return false;
                }

                var courierToRemove = couriersList.FirstOrDefault(c =>
                    c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    c.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));

                if (courierToRemove == null)
                {
                    LogWarning($"Courier {name} {surname} not found in company.");
                    return false;
                }

                couriersList.Remove(courierToRemove);
                _deliveryFood.curriers = couriersList;

                LogInfo($"Courier {name} {surname} removed from company. Remaining couriers: {couriersList.Count}");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Failed to remove courier {name} {surname}", ex);
                throw new DeliveryFoodServiceException("Failed to remove courier.", ex);
            }
        }

        public Currier FindCourier(string name, string surname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Courier name cannot be null or empty.", nameof(name));

                if (string.IsNullOrWhiteSpace(surname))
                    throw new ArgumentException("Courier surname cannot be null or empty.", nameof(surname));

                return _deliveryFood.curriers?.FirstOrDefault(c =>
                    c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                    c.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                LogError($"Failed to find courier {name} {surname}", ex);
                throw new DeliveryFoodServiceException("Failed to find courier.", ex);
            }
        }

        // Методы для работы с покупками
        public void ProcessFoodPurchase(User user, Basket basket)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "User cannot be null.");

                if (basket == null)
                    throw new ArgumentNullException(nameof(basket), "Basket cannot be null.");

                if (basket.price <= 0)
                    throw new ArgumentException("Basket price must be positive.", nameof(basket));

                if (!basket.BasketOfFood.Any())
                    throw new InvalidOperationException("Cannot process empty basket.");

                // Проверяем доступность диспетчеров и курьеров
                if (!_deliveryFood.Dispetchers.Any())
                    throw new InvalidOperationException("No dispatchers available to process the order.");

                if (!_deliveryFood.curriers.Any())
                    throw new InvalidOperationException("No couriers available for delivery.");

                _deliveryFood.BuyFoodForUser(ref user, basket);

                LogInfo($"Food purchase processed successfully for user {user.Name}. Basket price: {basket.price:C}, Items count: {basket.BasketOfFood.Count()}");
            }
            catch (Exception ex)
            {
                LogError($"Failed to process food purchase for user {user?.Name}", ex);
                throw new DeliveryFoodServiceException("Failed to process food purchase.", ex);
            }
        }

        public bool TryProcessFoodPurchase(User user, Basket basket)
        {
            try
            {
                ProcessFoodPurchase(user, basket);
                return true;
            }
            catch (Exception ex)
            {
                LogWarning($"Food purchase processing failed for user {user?.Name}: {ex.Message}");
                return false;
            }
        }

        // Методы для получения статистики
        public int GetTotalStaffCount()
        {
            try
            {
                int dispatchersCount = _deliveryFood.Dispetchers?.Count() ?? 0;
                int couriersCount = _deliveryFood.curriers?.Count() ?? 0;
                return dispatchersCount + couriersCount;
            }
            catch (Exception ex)
            {
                LogError("Failed to calculate total staff count", ex);
                throw new DeliveryFoodServiceException("Failed to calculate staff count.", ex);
            }
        }

        public string GetStaffStatistics()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Staff Statistics:");
                sb.AppendLine($"Total Staff: {GetTotalStaffCount()}");
                sb.AppendLine($"Dispatchers: {_deliveryFood.Dispetchers?.Count() ?? 0}");
                sb.AppendLine($"Couriers: {_deliveryFood.curriers?.Count() ?? 0}");

                if (_deliveryFood.Dispetchers.Any())
                {
                    sb.AppendLine("\nDispatchers List:");
                    foreach (var dispatcher in _deliveryFood.Dispetchers)
                    {
                        sb.AppendLine($"  - {dispatcher.Name} {dispatcher.Surname} ({dispatcher.YearsOld} years)");
                    }
                }

                if (_deliveryFood.curriers.Any())
                {
                    sb.AppendLine("\nCouriers List:");
                    foreach (var courier in _deliveryFood.curriers)
                    {
                        sb.AppendLine($"  - {courier.Name} {courier.Surname} ({courier.YearsOld} years)");
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogError("Failed to generate staff statistics", ex);
                throw new DeliveryFoodServiceException("Failed to generate staff statistics.", ex);
            }
        }

        public bool IsCompanyOperational()
        {
            try
            {
                bool hasDispatchers = _deliveryFood.Dispetchers?.Any() == true;
                bool hasCouriers = _deliveryFood.curriers?.Any() == true;
                bool hasName = !string.IsNullOrWhiteSpace(DelieviryFood.name);
                bool singletonExists = DelieviryFood.singletone != null;

                bool isOperational = hasDispatchers && hasCouriers && hasName && singletonExists;

                if (!isOperational)
                {
                    LogWarning($"Company operational check failed. Dispatchers: {hasDispatchers}, Couriers: {hasCouriers}, Name: {hasName}, Singleton: {singletonExists}");
                }

                return isOperational;
            }
            catch (Exception ex)
            {
                LogError("Failed to check company operational status", ex);
                return false;
            }
        }

        // Методы для поиска и фильтрации
        public IEnumerable<Dispetcher> FindAvailableDispatchers()
        {
            try
            {
                var availableDispatchers = new List<Dispetcher>();

                foreach (var dispatcher in _deliveryFood.Dispetchers ?? Enumerable.Empty<Dispetcher>())
                {
                    var dispatcherService = new DispetcherService(dispatcher, _logger);
                    if (dispatcherService.IsAvailableForNewBaskets())
                    {
                        availableDispatchers.Add(dispatcher);
                    }
                }

                LogInfo($"Found {availableDispatchers.Count} available dispatchers out of {_deliveryFood.Dispetchers?.Count() ?? 0}");
                return availableDispatchers;
            }
            catch (Exception ex)
            {
                LogError("Failed to find available dispatchers", ex);
                throw new DeliveryFoodServiceException("Failed to find available dispatchers.", ex);
            }
        }

        public IEnumerable<Dispetcher> FindDispatchersByAge(int minAge, int maxAge)
        {
            try
            {
                if (minAge < 18)
                    throw new ArgumentOutOfRangeException(nameof(minAge), "Minimum age cannot be less than 18.");

                if (maxAge > 100)
                    throw new ArgumentOutOfRangeException(nameof(maxAge), "Maximum age cannot exceed 100.");

                if (minAge > maxAge)
                    throw new ArgumentException("Minimum age cannot be greater than maximum age.");

                var filteredDispatchers = _deliveryFood.Dispetchers?
                    .Where(d => d.YearsOld >= minAge && d.YearsOld <= maxAge)
                    .ToList() ?? new List<Dispetcher>();

                LogInfo($"Found {filteredDispatchers.Count} dispatchers aged between {minAge} and {maxAge}");
                return filteredDispatchers;
            }
            catch (Exception ex)
            {
                LogError($"Failed to find dispatchers by age range {minAge}-{maxAge}", ex);
                throw new DeliveryFoodServiceException("Failed to find dispatchers by age.", ex);
            }
        }

        // Методы для управления singleton
        public static DelieviryFood GetInstance()
        {
            try
            {
                if (DelieviryFood.singletone == null)
                {
                    throw new InvalidOperationException("Singleton instance of DelieviryFood has not been initialized.");
                }

                return DelieviryFood.singletone;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting DeliveryFood instance: {ex.Message}");
                throw new DeliveryFoodServiceException("Failed to get DeliveryFood instance.", ex);
            }
        }

        public static void EnsureInstanceInitialized()
        {
            try
            {
                if (DelieviryFood.singletone == null)
                {
                    throw new InvalidOperationException("Singleton instance is null. Static constructor may not have been called.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ensuring instance initialization: {ex.Message}");
                throw new DeliveryFoodServiceException("DeliveryFood instance is not properly initialized.", ex);
            }
        }

        // Вспомогательные методы для логирования
        private void LogInfo(string message)
        {
            _logger?.LogInformation($"[DeliveryFoodService] {message}");
        }

        private void LogWarning(string message)
        {
            _logger?.LogWarning($"[DeliveryFoodService] {message}");
        }

        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[DeliveryFoodService] {message}");
        }
    }

    // Специализированные исключения
    public class DeliveryFoodServiceException : Exception
    {
        public DeliveryFoodServiceException() : base() { }
        public DeliveryFoodServiceException(string message) : base(message) { }
        public DeliveryFoodServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}