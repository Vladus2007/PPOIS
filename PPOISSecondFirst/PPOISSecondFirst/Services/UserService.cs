using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PPOISSecondFirst
public class UserService : IUserService
{
    public void BuyFood(Basket basket, ICollection<Food> listOfFoods, ref decimal balance)
    {
        try
        {
            // Валидация входных параметров
            ValidateBasket(basket);
            ValidateFoodList(listOfFoods);
            
            PayAnything(basket.Price, ref balance);

            // Добавляем продукты в список (используем Add вместо Append)
            foreach (Food food in basket.BasketOfFood)
            {
                listOfFoods.Add(food);
            }
            
            basket.ClearBasket(); // Очищаем корзину после покупки
        }
        catch (UserServiceException ex)
        {
            Console.WriteLine($"Ошибка покупки: {ex.Message}");
            throw; // Перебрасываем исключение для обработки на уровне выше
        }
    }

    public void PayAnything(decimal price, ref decimal balance)
    {
        if (price < 0)
            throw new NegativeAmountException(price);
            
        if (balance < price)
            throw new InsufficientBalanceException(balance, price);
            
        balance -= price;
    }

    public void GiveMoney(decimal money, ref decimal balance)
    {
        if (money < 0)
            throw new NegativeAmountException(money);
            
        balance += money;
    }

    // Методы валидации
    private void ValidateBasket(Basket basket)
    {
        if (basket == null)
            throw new InvalidBasketException();
            
        if (basket.BasketOfFood == null || !basket.BasketOfFood.Any())
            throw new EmptyBasketException();
    }

    private void ValidateFoodList(ICollection<Food> listOfFoods)
    {
        if (listOfFoods == null)
            throw new ArgumentNullException(nameof(listOfFoods), "Список продуктов не может быть null");
    }

    // Безопасный метод покупки
    public bool TryBuyFood(Basket basket, ICollection<Food> listOfFoods, ref decimal balance)
    {
        try
        {
            BuyFood(basket, listOfFoods, ref balance);
            return true;
        }
        catch (UserServiceException)
        {
            return false;
        }
    }
}
public class UserServiceException : Exception
{
    public UserServiceException() { }
    public UserServiceException(string message) : base(message) { }
    public UserServiceException(string message, Exception inner) : base(message, inner) { }
}

public class InsufficientBalanceException : UserServiceException
{
    public decimal CurrentBalance { get; }
    public decimal RequiredAmount { get; }
    
    public InsufficientBalanceException(decimal balance, decimal price) 
        : base($"Недостаточно средств. Баланс: {balance}, требуется: {price}")
    {
        CurrentBalance = balance;
        RequiredAmount = price;
    }
}


public class NegativeAmountException : UserServiceException
{
    public decimal InvalidAmount { get; }
    
    public NegativeAmountException(decimal amount) 
        : base($"Сумма не может быть отрицательной: {amount}")
    {
        InvalidAmount = amount;
    }
}


public class EmptyBasketException : UserServiceException
{
    public EmptyBasketException() 
        : base("Корзина пуста. Невозможно совершить покупку.")
    {
    }
}


public class InvalidBasketException : UserServiceException
{
    public InvalidBasketException() 
        : base("Корзина недействительна (null)")
    {
    }
}
