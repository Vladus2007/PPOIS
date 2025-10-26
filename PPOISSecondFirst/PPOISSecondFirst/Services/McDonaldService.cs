using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
    public class McDonaldService
    {
        public Food BuyFood(string name, IEnumerable<Food> Menu)
        {
            // Валидация входных параметров
            ValidateMenu(Menu);
            ValidateFoodName(name);
            
            var zakaz = Menu.FirstOrDefault(n => n.Name == name);
            
            if (zakaz == null) 
                throw new FoodNotFoundException(name);
                
            if (zakaz.Count <= 0)
                throw new FoodOutOfStockException(name, zakaz.Count);
            
            zakaz.Count--;
            return zakaz;
        }

        public void GetMark(double markOfUser, ref double Mark, ref int countOfMetting)
        {
            // Валидация оценки
            if (markOfUser < 1 || markOfUser > 5)
                throw new InvalidMarkException(markOfUser);
                
            Mark = (markOfUser + (countOfMetting * Mark)) / ++countOfMetting;
        }

        
        public void DisplayFullMenu(IEnumerable<Food> menu)
        {
            if (menu == null || !menu.Any())
            {
                Console.WriteLine("Меню пустое");
                return;
            }

            Console.WriteLine("=== МЕНЮ MCDONALD'S ===");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10}", "Название", "Цена", "Количество", "Доступно");
            Console.WriteLine(new string('-', 60));
            
            foreach (var food in menu)
            {
                string available = food.Count > 0 ? "✓ В наличии" : "✗ Нет в наличии";
                Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10}", 
                    food.Name, 
                    $"{food.Price:C}", 
                    food.Count, 
                    available);
            }
            Console.WriteLine();
        }

      
        public void DisplayAvailableFood(IEnumerable<Food> menu)
        {
            var availableFood = menu.Where(f => f.Count > 0);
            
            if (!availableFood.Any())
            {
                Console.WriteLine("К сожалению, все позиции временно недоступны");
                return;
            }

            Console.WriteLine("=== ДОСТУПНЫЕ ДЛЯ ЗАКАЗА ===");
            foreach (var food in availableFood)
            {
                Console.WriteLine($"🍔 {food.Name} - {food.Price:C} (осталось: {food.Count})");
            }
            Console.WriteLine();
        }

       
        public void DisplayPopularItems(IEnumerable<Food> menu, int topN = 3)
        {
            var popularItems = menu
                .Where(f => f.Count > 0)
                .OrderByDescending(f => f.Popularity) // Предполагаем, что у Food есть свойство Popularity
                .Take(topN);

            Console.WriteLine($"=== ТОП-{topN} ПОПУЛЯРНЫХ ПОЗИЦИЙ ===");
            int position = 1;
            foreach (var food in popularItems)
            {
                string emoji = position == 1 ? "🥇" : position == 2 ? "🥈" : "🥉";
                Console.WriteLine($"{emoji} {position}. {food.Name} - {food.Price:C}");
                position++;
            }
            Console.WriteLine();
        }

       
        public void DisplayWelcomeMessage()
        {
            Console.WriteLine(" Добро пожаловать в McDonald's! ");
            Console.WriteLine(" Самые вкусные бургеры в городе!");
            Console.WriteLine(" Круглосуточно 24/7");
            Console.WriteLine(" Сегодняшние акции:");
            Console.WriteLine("   • Комбо-обед со скидкой 20%");
            Console.WriteLine("   • Бесплатная картошка при заказе от 500 руб");
            Console.WriteLine("   • Кофе в подарок с утренним меню");
            Console.WriteLine();
        }

        /// <summary>
        /// Выводит информацию о рейтинге заведения
        /// </summary>
        public void DisplayRestaurantRating(double mark, int countOfReviews)
        {
            Console.WriteLine("=== РЕЙТИНГ ЗАВЕДЕНИЯ ===");
            string stars = GetStars(mark);
            Console.WriteLine($"⭐ Рейтинг: {mark:F1}/5 {stars}");
            Console.WriteLine($"📊 Количество отзывов: {countOfReviews}");
            Console.WriteLine($"💬 {GetRatingComment(mark)}");
            Console.WriteLine();
        }

       

        private void ValidateMenu(IEnumerable<Food> menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu), "Меню не может быть null");
                
            if (!menu.Any())
                throw new EmptyMenuException();
        }

        private void ValidateFoodName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название еды не может быть пустым", nameof(name));
        }

        private string GetStars(double rating)
        {
            int fullStars = (int)rating;
            return new string('★', fullStars) + new string('☆', 5 - fullStars);
        }

        private string GetRatingComment(double rating)
        {
            return rating switch
            {
                >= 4.5 => "Отличное качество! ",
                >= 4.0 => "Очень хорошо! ",
                >= 3.0 => "Неплохо ",
                >= 2.0 => "Есть над чем работать ",
                _ => "Требуются улучшения "
            };
        }

        
        public bool TryBuyFood(string name, IEnumerable<Food> menu, out Food purchasedFood)
        {
            purchasedFood = null;
            try
            {
                purchasedFood = BuyFood(name, menu);
                return true;
            }
            catch (McDonaldServiceException)
            {
                return false;
            }
        }
    }
    public class McDonaldServiceException : Exception
{
    public McDonaldServiceException() { }
    public McDonaldServiceException(string message) : base(message) { }
    public McDonaldServiceException(string message, Exception inner) : base(message, inner) { }
}

public class FoodNotFoundException : McDonaldServiceException
{
    public string FoodName { get; }
    
    public FoodNotFoundException(string foodName) 
        : base($"Еда '{foodName}' не найдена в меню или отсутствует в магазине")
    {
        FoodName = foodName;
    }
}

public class FoodOutOfStockException : McDonaldServiceException
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

public class InvalidMarkException : McDonaldServiceException
{
    public double InvalidMark { get; }
    
    public InvalidMarkException(double mark) 
        : base($"Некорректная оценка: {mark}. Оценка должна быть от 1 до 5")
    {
        InvalidMark = mark;
    }
}

public class EmptyMenuException : McDonaldServiceException
{
    public EmptyMenuException() 
        : base("Меню пустое. Невозможно совершить покупку")
    {
    }
}
}
