using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPOISSecondFirst
{
    public class GippoService
    {
        private Gippo _gippo;

        public GippoService(Gippo gippo)
        {
            _gippo = gippo ?? throw new ArgumentNullException(nameof(gippo));
        }

        // Методы для работы с меню и покупками
        public Food BuyFood(string foodName)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ArgumentException("Food name cannot be null or empty", nameof(foodName));

            try
            {
                return _gippo.BuyFood(foodName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to buy food: {ex.Message}", ex);
            }
        }

        public bool TryBuyFood(string foodName, out Food purchasedFood)
        {
            purchasedFood = null;

            if (string.IsNullOrWhiteSpace(foodName))
                return false;

            try
            {
                purchasedFood = _gippo.BuyFood(foodName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddFoodToMenu(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));

            var menuList = _gippo.Menu?.ToList() ?? new List<Food>();
            menuList.Add(food);
            _gippo.Menu = menuList;
        }

        public void RemoveFoodFromMenu(string foodName)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ArgumentException("Food name cannot be null or empty", nameof(foodName));

            var menuList = _gippo.Menu?.ToList() ?? new List<Food>();
            var foodToRemove = menuList.FirstOrDefault(f => f.Name == foodName);

            if (foodToRemove != null)
            {
                menuList.Remove(foodToRemove);
                _gippo.Menu = menuList;
            }
        }

        public void UpdateFoodCount(string foodName, int newCount)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ArgumentException("Food name cannot be null or empty", nameof(foodName));

            if (newCount < 0)
                throw new ArgumentException("Food count cannot be negative", nameof(newCount));

            var food = _gippo.Menu?.FirstOrDefault(f => f.Name == foodName);
            if (food != null)
            {
                food.Count = newCount;
            }
        }

        public Food FindFoodByName(string foodName)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ArgumentException("Food name cannot be null or empty", nameof(foodName));

            return _gippo.Menu?.FirstOrDefault(f =>
                f.Name?.Equals(foodName, StringComparison.OrdinalIgnoreCase) == true);
        }

        public IEnumerable<Food> GetAvailableFood()
        {
            return _gippo.Menu?.Where(f => f.Count > 0) ?? Enumerable.Empty<Food>();
        }

        public IEnumerable<Food> GetOutOfStockFood()
        {
            return _gippo.Menu?.Where(f => f.Count <= 0) ?? Enumerable.Empty<Food>();
        }

        // Методы для работы с рейтингом
        public void AddRating(double userRating)
        {
            if (userRating < 0 || userRating > 5)
                throw new ArgumentException("Rating must be between 0 and 5", nameof(userRating));

            _gippo.GetMark(userRating);
        }

        public string GetRatingDescription()
        {
            return _gippo.Mark switch
            {
                >= 4.5 => "Excellent shop",
                >= 4.0 => "Very good shop",
                >= 3.5 => "Good shop",
                >= 3.0 => "Average shop",
                >= 2.0 => "Poor shop",
                _ => "Very poor shop"
            };
        }

        public bool IsHighlyRated()
        {
            return _gippo.Mark >= 4.0;
        }

        // Методы для работы с менеджером
        public void SetManager(Meneger manager)
        {
            // Используем рефлексию для доступа к приватному полю, так как в классе Gippo оно приватное
            var field = typeof(Gippo).GetField("_meneger",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(_gippo, manager);
        }

        public Meneger GetManager()
        {
            var field = typeof(Gippo).GetField("_meneger",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(_gippo) as Meneger;
        }

        // Методы для работы с адресом
        public void UpdateAddress(Adress newAddress)
        {
            _gippo.Adress = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        }

        public string GetFullAddress()
        {
            return _gippo.Adress?.ToString() ?? "Address not set";
        }

        // Методы для работы с описанием
        public void UpdateDescription(string newDescription)
        {
            _gippo.Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
        }

        // Методы для работы с типом магазина
        public void SetShopType(Shop shopType)
        {
            _gippo.Type = shopType ?? throw new ArgumentNullException(nameof(shopType));
        }

        // Методы для получения информации
        public string GetShopInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Shop: Gippo");
            sb.AppendLine($"Description: {_gippo.Description}");
            sb.AppendLine($"Address: {GetFullAddress()}");
            sb.AppendLine($"Rating: {_gippo.Mark:F1}/5.0 ({GetRatingDescription()})");
            sb.AppendLine($"Total ratings: {_gippo.countOfMetteng}");
            sb.AppendLine($"Menu items: {_gippo.Menu?.Count() ?? 0}");
            sb.AppendLine($"Available items: {GetAvailableFood().Count()}");
            sb.AppendLine($"Out of stock: {GetOutOfStockFood().Count()}");

            var manager = GetManager();
            if (manager != null)
            {
                sb.AppendLine($"Manager: {manager.Name} {manager.Surname}");
            }

            return sb.ToString();
        }

        public string GetMenuInfo()
        {
            var menu = _gippo.Menu?.ToList() ?? new List<Food>();
            if (!menu.Any())
                return "Menu is empty";

            var sb = new StringBuilder();
            sb.AppendLine("Menu:");
            foreach (var food in menu)
            {
                var status = food.Count > 0 ? $"{food.Count} available" : "OUT OF STOCK";
                sb.AppendLine($"  - {food.Name}: {status}");
            }

            return sb.ToString();
        }

        public string GetInventorySummary()
        {
            var availableFood = GetAvailableFood().ToList();
            var outOfStockFood = GetOutOfStockFood().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Inventory Summary:");
            sb.AppendLine($"Total items: {availableFood.Count + outOfStockFood.Count}");
            sb.AppendLine($"Available: {availableFood.Count}");
            sb.AppendLine($"Out of stock: {outOfStockFood.Count}");

            if (availableFood.Any())
            {
                sb.AppendLine($"Total available quantity: {availableFood.Sum(f => f.Count)}");
            }

            return sb.ToString();
        }

        // Методы для проверки состояния
        public bool HasManager()
        {
            return GetManager() != null;
        }

        public bool IsFullyStocked()
        {
            return _gippo.Menu?.All(f => f.Count > 0) == true;
        }

        public bool HasAvailableFood()
        {
            return GetAvailableFood().Any();
        }

        
        public static Gippo CreateGippo(
            IEnumerable<Food> menu,
            Adress address,
            string description,
            Shop shopType)
        {
            return new Gippo
            {
                Menu = menu ?? Enumerable.Empty<Food>(),
                Adress = address,
                Description = description,
                Type = shopType,
                Mark = 0,
                countOfMetteng = 0
            };
        }

        public static Gippo FindShopByAddress(IEnumerable<Gippo> shops, Adress address)
        {
            return shops.FirstOrDefault(s => s.Adress?.Equals(address) == true);
        }

        public static IEnumerable<Gippo> FilterShopsByRating(IEnumerable<Gippo> shops, double minRating)
        {
            return shops.Where(s => s.Mark >= minRating);
        }

        public static Gippo GetBestRatedShop(IEnumerable<Gippo> shops)
        {
            return shops.OrderByDescending(s => s.Mark).FirstOrDefault();
        }
    }
}