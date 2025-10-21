using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PPOISSecondFirst
    {
        public class MamukaService
        {
            private Mamuka _mamuka;

            public MamukaService(Mamuka mamuka)
            {
                _mamuka = mamuka ?? throw new ArgumentNullException(nameof(mamuka));
            }

            // Методы для работы с меню
            public void AddFoodToMenu(Food food)
            {
                if (food == null)
                    throw new ArgumentNullException(nameof(food));

                var menuList = _mamuka.Menu.ToList();
                menuList.Add(food);
                _mamuka.Menu = menuList;
            }

            public void RemoveFoodFromMenu(Food food)
            {
                if (food == null)
                    throw new ArgumentNullException(nameof(food));

                var menuList = _mamuka.Menu.ToList();
                menuList.Remove(food);
                _mamuka.Menu = menuList;
            }

            public Food FindFoodByName(string foodName)
            {
                if (string.IsNullOrWhiteSpace(foodName))
                    throw new ArgumentException("Food name cannot be null or empty", nameof(foodName));

                return _mamuka.Menu.FirstOrDefault(f =>
                    f.Name?.Equals(foodName, StringComparison.OrdinalIgnoreCase) == true);
            }

            public IEnumerable<Food> GetFoodByPriceRange(decimal minPrice, decimal maxPrice)
            {
                if (minPrice < 0 || maxPrice < 0)
                    throw new ArgumentException("Prices cannot be negative");
                if (minPrice > maxPrice)
                    throw new ArgumentException("Min price cannot be greater than max price");

                return _mamuka.Menu.Where(f => f.Price >= minPrice && f.Price <= maxPrice);
            }

            // Методы для работы с персоналом
            public void ChangeManager(Meneger newManager)
            {
                _mamuka._meneger = newManager ?? throw new ArgumentNullException(nameof(newManager));
            }

            public void ChangeSheffCooker(Sheffcooker newSheffCooker)
            {
                _mamuka.sheffcooker = newSheffCooker ?? throw new ArgumentNullException(nameof(newSheffCooker));
            }

            // Методы для работы с рейтингом
            public void UpdateMark(double newMark)
            {
                if (newMark < 0 || newMark > 5)
                    throw new ArgumentException("Mark must be between 0 and 5");

                _mamuka.Mark = newMark;
            }

            public string GetMarkDescription()
            {
                return _mamuka.Mark switch
                {
                    >= 4.5 => "Excellent",
                    >= 4.0 => "Very Good",
                    >= 3.5 => "Good",
                    >= 3.0 => "Average",
                    >= 2.0 => "Poor",
                    _ => "Very Poor"
                };
            }

            // Методы для работы с адресом
            public void UpdateAddress(Adress newAddress)
            {
                _mamuka.Adress = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
            }

            public string GetFullAddress()
            {
                return _mamuka.Adress?.ToString() ?? "Address not set";
            }

            // Методы для работы с описанием
            public void UpdateDescription(string newDescription)
            {
                _mamuka.Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
            }

            // Методы для работы с количеством встреч
            public void IncrementMeetingCount()
            {
                _mamuka.countOfMetteng++;
            }

            public void DecrementMeetingCount()
            {
                if (_mamuka.countOfMetteng > 0)
                    _mamuka.countOfMetteng--;
            }

            public void SetMeetingCount(int count)
            {
                if (count < 0)
                    throw new ArgumentException("Meeting count cannot be negative");

                _mamuka.countOfMetteng = count;
            }

            // Методы для получения информации
            public string GetRestaurantInfo()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Restaurant: Mamuka");
                sb.AppendLine($"Description: {_mamuka.Description}");
                sb.AppendLine($"Address: {GetFullAddress()}");
                sb.AppendLine($"Rating: {_mamuka.Mark:F1} ({GetMarkDescription()})");
                sb.AppendLine($"Manager: {_mamuka._meneger?.Name ?? "Not set"}");
                sb.AppendLine($"Head Cook: {_mamuka.sheffcooker?.Name ?? "Not set"}");
                sb.AppendLine($"Menu Items: {_mamuka.Menu.Count()}");
                sb.AppendLine($"Meetings Count: {_mamuka.countOfMetteng}");

                return sb.ToString();
            }

            public string GetMenuSummary()
            {
                var menuItems = _mamuka.Menu.ToList();
                if (!menuItems.Any())
                    return "Menu is empty";

                var sb = new StringBuilder();
                sb.AppendLine("Menu Summary:");
                sb.AppendLine($"Total items: {menuItems.Count}");
                sb.AppendLine($"Average price: {menuItems.Average(f => f.Price):C2}");
                sb.AppendLine($"Most expensive: {menuItems.Max(f => f.Price):C2}");
                sb.AppendLine($"Least expensive: {menuItems.Min(f => f.Price):C2}");

                return sb.ToString();
            }

            
            public bool IsFullyStaffed()
            {
                return _mamuka._meneger != null && _mamuka.sheffcooker != null;
            }

            public bool HasMenuItems()
            {
                return _mamuka.Menu.Any();
            }

            public bool IsHighlyRated()
            {
                return _mamuka.Mark >= 4.0;
            }

          

        }
    }

