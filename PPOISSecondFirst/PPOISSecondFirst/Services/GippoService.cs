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
                throw new ShopValidationException("FoodName", "Название еды не может быть пустым");

            ValidateShopConfiguration();

            var food = FindFoodByName(foodName);
            if (food == null)
                throw new FoodNotFoundException(foodName);

            if (food.Count <= 0)
                throw new FoodOutOfStockException(foodName, food.Count);

            try
            {
                return _gippo.BuyFood(foodName);
            }
            catch (Exception ex)
            {
                throw new ShopOperationException("BuyFood", ex.Message);
            }
        }

        public bool TryBuyFood(string foodName, out Food purchasedFood)
        {
            purchasedFood = null;

            if (string.IsNullOrWhiteSpace(foodName))
                return false;

            try
            {
                purchasedFood = BuyFood(foodName);
                return true;
            }
            catch (GippoServiceException)
            {
                return false;
            }
        }

        public void AddFoodToMenu(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));

            if (string.IsNullOrWhiteSpace(food.Name))
                throw new ShopValidationException("FoodName", "Название еды не может быть пустым");

            // Проверка на дубликат
            var existingFood = FindFoodByName(food.Name);
            if (existingFood != null)
                throw new DuplicateFoodException(food.Name);

            if (food.Count < 0)
                throw new InvalidFoodCountException(food.Count);

            var menuList = _gippo.Menu?.ToList() ?? new List<Food>();
            menuList.Add(food);
            _gippo.Menu = menuList;
        }

        public void RemoveFoodFromMenu(string foodName)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ShopValidationException("FoodName", "Название еды не может быть пустым");

            var menuList = _gippo.Menu?.ToList() ?? new List<Food>();
            var foodToRemove = menuList.FirstOrDefault(f => f.Name == foodName);

            if (foodToRemove == null)
                throw new FoodNotFoundException(foodName);

            menuList.Remove(foodToRemove);
            _gippo.Menu = menuList;
        }

        public void UpdateFoodCount(string foodName, int newCount)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ShopValidationException("FoodName", "Название еды не может быть пустым");

            if (newCount < 0)
                throw new InvalidFoodCountException(newCount);

            var food = FindFoodByName(foodName);
            if (food == null)
                throw new FoodNotFoundException(foodName);

            food.Count = newCount;
        }

        public void IncreaseFoodCount(string foodName, int amount)
        {
            if (amount <= 0)
                throw new ShopValidationException("Amount", $"Количество для увеличения должно быть положительным: {amount}");

            var food = FindFoodByName(foodName);
            if (food == null)
                throw new FoodNotFoundException(foodName);

            food.Count += amount;
        }

        public void DecreaseFoodCount(string foodName, int amount)
        {
            if (amount <= 0)
                throw new ShopValidationException("Amount", $"Количество для уменьшения должно быть положительным: {amount}");

            var food = FindFoodByName(foodName);
            if (food == null)
                throw new FoodNotFoundException(foodName);

            if (food.Count < amount)
                throw new FoodOutOfStockException(foodName, food.Count);

            food.Count -= amount;
        }

        public Food FindFoodByName(string foodName)
        {
            if (string.IsNullOrWhiteSpace(foodName))
                throw new ShopValidationException("FoodName", "Название еды не может быть пустым");

            return _gippo.Menu?.FirstOrDefault(f =>
                f.Name?.Equals(foodName, StringComparison.OrdinalIgnoreCase) == true);
        }

        public Food FindFoodByNameOrThrow(string foodName)
        {
            var food = FindFoodByName(foodName);
            if (food == null)
                throw new FoodNotFoundException(foodName);
            return food;
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
                throw new InvalidRatingException(userRating);

            _gippo.GetMark(userRating);
        }

        public void AddRatingWithValidation(double userRating, string comment = null)
        {
            if (userRating < 0 || userRating > 5)
                throw new InvalidRatingException(userRating);

            if (userRating < 1 && string.IsNullOrEmpty(comment))
                throw new ShopValidationException("Comment", "Для рейтинга ниже 1 обязателен комментарий");

            _gippo.GetMark(userRating);
        }

        public string GetRatingDescription()
        {
            return _gippo.Mark switch
            {
                >= 4.5 => "Отличный магазин",
                >= 4.0 => "Очень хороший магазин",
                >= 3.5 => "Хороший магазин",
                >= 3.0 => "Средний магазин",
                >= 2.0 => "Плохой магазин",
                _ => "Очень плохой магазин"
            };
        }

        public bool IsHighlyRated()
        {
            return _gippo.Mark >= 4.0;
        }

        public void ValidateRating()
        {
            if (_gippo.Mark < 0 || _gippo.Mark > 5)
                throw new InvalidRatingException(_gippo.Mark);
        }

        // Методы для работы с менеджером
        public void SetManager(Meneger manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            // Валидация менеджера
            if (string.IsNullOrWhiteSpace(manager.Name) || string.IsNullOrWhiteSpace(manager.Surname))
                throw new ShopValidationException("Manager", "Менеджер должен иметь имя и фамилию");

            // Используем рефлексию для доступа к приватному полю, так как в классе Gippo оно приватное
            var field = typeof(Gippo).GetField("_meneger",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field == null)
                throw new ShopOperationException("SetManager", "Не удалось найти поле менеджера в классе Gippo");

            field.SetValue(_gippo, manager);
        }

        public Meneger GetManager()
        {
            var field = typeof(Gippo).GetField("_meneger",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(_gippo) as Meneger;
        }

        public Meneger GetManagerOrThrow()
        {
            var manager = GetManager();
            if (manager == null)
                throw new ManagerNotAssignedException();
            return manager;
        }

        // Методы для работы с адресом
        public void UpdateAddress(Adress newAddress)
        {
            _gippo.Adress = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        }

        public string GetFullAddress()
        {
            return _gippo.Adress?.ToString() ?? "Адрес не установлен";
        }

        public Adress GetAddressOrThrow()
        {
            if (_gippo.Adress == null)
                throw new AddressNotSetException();
            return _gippo.Adress;
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
            ValidateShopConfiguration();

            var sb = new StringBuilder();
            sb.AppendLine($"Магазин: Gippo");
            sb.AppendLine($"Описание: {_gippo.Description}");
            sb.AppendLine($"Адрес: {GetFullAddress()}");
            sb.AppendLine($"Рейтинг: {_gippo.Mark:F1}/5.0 ({GetRatingDescription()})");
            sb.AppendLine($"Всего оценок: {_gippo.countOfMetteng}");
            sb.AppendLine($"Позиций в меню: {_gippo.Menu?.Count() ?? 0}");
            sb.AppendLine($"Доступно позиций: {GetAvailableFood().Count()}");
            sb.AppendLine($"Нет в наличии: {GetOutOfStockFood().Count()}");

            var manager = GetManager();
            if (manager != null)
            {
                sb.AppendLine($"Менеджер: {manager.Name} {manager.Surname}");
            }

            return sb.ToString();
        }

        public string GetMenuInfo()
        {
            var menu = _gippo.Menu?.ToList() ?? new List<Food>();
            if (!menu.Any())
                throw new EmptyMenuException();

            var sb = new StringBuilder();
            sb.AppendLine("Меню:");
            foreach (var food in menu)
            {
                var status = food.Count > 0 ? $"{food.Count} в наличии" : "НЕТ В НАЛИЧИИ";
                sb.AppendLine($"  - {food.Name}: {status}");
            }

            return sb.ToString();
        }

        public string GetInventorySummary()
        {
            var availableFood = GetAvailableFood().ToList();
            var outOfStockFood = GetOutOfStockFood().ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Сводка по инвентарю:");
            sb.AppendLine($"Всего позиций: {availableFood.Count + outOfStockFood.Count}");
            sb.AppendLine($"В наличии: {availableFood.Count}");
            sb.AppendLine($"Нет в наличии: {outOfStockFood.Count}");

            if (availableFood.Any())
            {
                sb.AppendLine($"Общее количество: {availableFood.Sum(f => f.Count)}");
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

        public void ValidateShopConfiguration()
        {
            if (_gippo.Adress == null)
                throw new AddressNotSetException();

            if (_gippo.Menu == null || !_gippo.Menu.Any())
                throw new EmptyMenuException();

            if (!HasAvailableFood())
                throw new ShopOperationException("Validation", "В магазине нет доступных товаров");
        }

        // Статические методы
        public static Gippo CreateGippo(
            IEnumerable<Food> menu,
            Adress address,
            string description,
            Shop shopType)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            if (string.IsNullOrWhiteSpace(description))
                throw new ShopValidationException("Description", "Описание магазина не может быть пустым");

            if (shopType == null)
                throw new ArgumentNullException(nameof(shopType));

            return new Gippo
            {
                Menu = menu,
                Adress = address,
                Description = description,
                Type = shopType,
                Mark = 0,
                countOfMetteng = 0
            };
        }

        public static Gippo FindShopByAddress(IEnumerable<Gippo> shops, Adress address)
        {
            if (shops == null)
                throw new ArgumentNullException(nameof(shops));

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            return shops.FirstOrDefault(s => s.Adress?.Equals(address) == true);
        }

        public static Gippo FindShopByAddressOrThrow(IEnumerable<Gippo> shops, Adress address)
        {
            var shop = FindShopByAddress(shops, address);
            if (shop == null)
                throw new ShopOperationException("FindShop", $"Магазин по адресу {address} не найден");
            return shop;
        }

        public static IEnumerable<Gippo> FilterShopsByRating(IEnumerable<Gippo> shops, double minRating)
        {
            if (shops == null)
                throw new ArgumentNullException(nameof(shops));

            if (minRating < 0 || minRating > 5)
                throw new InvalidRatingException(minRating);

            return shops.Where(s => s.Mark >= minRating);
        }

        public static Gippo GetBestRatedShop(IEnumerable<Gippo> shops)
        {
            if (shops == null)
                throw new ArgumentNullException(nameof(shops));

            if (!shops.Any())
                throw new EmptyMenuException();

            return shops.OrderByDescending(s => s.Mark).FirstOrDefault();
        }

        public static Gippo GetBestRatedShopOrThrow(IEnumerable<Gippo> shops)
        {
            var shop = GetBestRatedShop(shops);
            if (shop == null)
                throw new ShopOperationException("GetBestRated", "Не удалось найти магазин с наивысшим рейтингом");
            return shop;
        }
    }

    public class GippoServiceException : Exception
{
    public GippoServiceException() { }
    public GippoServiceException(string message) : base(message) { }
    public GippoServiceException(string message, Exception inner) : base(message, inner) { }
}

public class ShopValidationException : GippoServiceException
{
    public string PropertyName { get; }

    public ShopValidationException(string propertyName, string message) 
        : base(message)
    {
        PropertyName = propertyName;
    }
}
public class FoodNotFoundException : GippoServiceException
{
    public string FoodName { get; }

    public FoodNotFoundException(string foodName) 
        : base($"Еда '{foodName}' не найдена в меню")
    {
        FoodName = foodName;
    }
}

public class FoodOutOfStockException : GippoServiceException
{
    public string FoodName { get; }
    public int AvailableCount { get; }

    public FoodOutOfStockException(string foodName, int count) 
        : base($"Еда '{foodName}' закончилась. В наличии: {count}")
    {
        FoodName = foodName;
        AvailableCount = count;
    }
}

public class InvalidRatingException : ShopValidationException
{
    public double InvalidRating { get; }

    public InvalidRatingException(double rating) 
        : base("Rating", $"Некорректный рейтинг: {rating}. Рейтинг должен быть от 0 до 5")
    {
        InvalidRating = rating;
    }
}

public class EmptyMenuException : GippoServiceException
{
    public EmptyMenuException() 
        : base("Меню магазина пустое")
    {
    }
}

public class ShopNotConfiguredException : GippoServiceException
{
    public string MissingComponent { get; }

    public ShopNotConfiguredException(string component) 
        : base($"Магазин не настроен: отсутствует {component}")
    {
        MissingComponent = component;
    }
}

public class InvalidFoodCountException : ShopValidationException
{
    public int InvalidCount { get; }

    public InvalidFoodCountException(int count) 
        : base("Count", $"Некорректное количество еды: {count}. Количество не может быть отрицательным")
    {
        InvalidCount = count;
    }
}

public class ManagerNotAssignedException : GippoServiceException
{
    public ManagerNotAssignedException() 
        : base("Менеджер не назначен магазину")
    {
    }
}

public class AddressNotSetException : GippoServiceException
{
    public AddressNotSetException() 
        : base("Адрес магазина не установлен")
    {
    }
}

public class DuplicateFoodException : GippoServiceException
{
    public string FoodName { get; }

    public DuplicateFoodException(string foodName) 
        : base($"Еда с названием '{foodName}' уже существует в меню")
    {
        FoodName = foodName;
    }
}

public class ShopOperationException : GippoServiceException
{
    public string Operation { get; }

    public ShopOperationException(string operation, string message) 
        : base($"Ошибка операции '{operation}': {message}")
    {
        Operation = operation;
    }
}
}
