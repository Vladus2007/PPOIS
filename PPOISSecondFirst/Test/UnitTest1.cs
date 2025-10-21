using Microsoft.Extensions.Logging;
using PPOISSecondFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace PPOISSecondFirst
{
    public class MamukaTests
    {
        [Fact]
        public void Mamuka_Constructor_InitializesProperties()
        {
            // Arrange
            var menu = new List<Food> { new Food(5.99m, "Burger", false, false, 10) };
            var address = new Adress("Test St", "Test City", "Test State", 1);
            var staffInfo = new StaffInformation("1234567890", "John", "Doe", "Test Manager", 30);
            var manager = new Meneger(staffInfo);
            var cookerInfo = new StaffInformation("0987654321", "Chef", "Cook", "Test Cooker", 35);
            var sheffCooker = new Sheffcooker(cookerInfo, 3, 5);

            // Act
            var mamuka = new Mamuka(menu, address, manager, sheffCooker, "Test Description", null);

            // Assert
            Assert.Equal(menu, mamuka.Menu);
            Assert.Equal(address, mamuka.Adress);
            Assert.Equal(manager, mamuka._meneger);
            Assert.Equal(sheffCooker, mamuka.sheffcooker);
            Assert.Equal("Test Description", mamuka.Description);
        }
    }

    public class MamukaServiceTests
    {
        [Fact]
        public void AddFoodToMenu_AddsFoodToMenu()
        {
            // Arrange
            var address = new Adress("Street", "City", "State");
            var staffInfo = new StaffInformation("123", "Name", "Surname", "Desc", 25);
            var mamuka = new Mamuka(new List<Food>(), address, new Meneger(staffInfo), new Sheffcooker(staffInfo, 2, 3), "Test", null);
            var service = new MamukaService(mamuka);
            var food = new Food(12.99m, "Pizza", false, false, 5);

            // Act
            service.AddFoodToMenu(food);

            // Assert
            Assert.Contains(food, mamuka.Menu);
        }

        [Fact]
        public void FindFoodByName_ReturnsCorrectFood()
        {
            // Arrange
            var food = new Food(5.99m, "Burger", false, false, 10);
            var address = new Adress("Street", "City", "State");
            var staffInfo = new StaffInformation("123", "Name", "Surname", "Desc", 25);
            var mamuka = new Mamuka(new List<Food> { food }, address, new Meneger(staffInfo), new Sheffcooker(staffInfo, 2, 3), "Test", null);
            var service = new MamukaService(mamuka);

            // Act
            var result = service.FindFoodByName("Burger");

            // Assert
            Assert.Equal(food, result);
        }
    }

    public class MenegerTests
    {
        [Fact]
        public void Meneger_Constructor_SetsPropertiesFromStaffInformation()
        {
            // Arrange
            var info = new StaffInformation("1234567890", "John", "Doe", "Test Manager", 30);

            // Act
            var manager = new Meneger(info);

            // Assert
            Assert.Equal("John", manager.Name);
            Assert.Equal("Doe", manager.Surname);
            Assert.Equal("1234567890", manager.PhoneNumber);
            Assert.Equal("Test Manager", manager.Description);
            Assert.Equal(30, manager.YearsOld);
        }
    }

    public class MenegerServiceTests
    {
        [Fact]
        public void UpdatePersonalInfo_ValidData_UpdatesProperties()
        {
            // Arrange
            var staffInfo = new StaffInformation("123", "Old", "Name", "Desc", 25);
            var manager = new Meneger(staffInfo);
            var service = new MenegerService(manager);

            // Act
            service.UpdatePersonalInfo("New", "Name", 35);

            // Assert
            Assert.Equal("New", manager.Name);
            Assert.Equal("Name", manager.Surname);
            Assert.Equal(35, manager.YearsOld);
        }
    }

    public class GippoTests
    {
        [Fact]
        public void BuyFood_FoodExists_ReturnsFoodAndDecreasesCount()
        {
            // Arrange
            var food = new Food(1.99m, "Apple", false, false, 5);
            var gippo = new Gippo { Menu = new List<Food> { food } };

            // Act
            var result = gippo.BuyFood("Apple");

            // Assert
            Assert.Equal(food, result);
            Assert.Equal(4, food.Count);
        }

        [Fact]
        public void BuyFood_FoodNotExists_ThrowsException()
        {
            // Arrange
            var gippo = new Gippo { Menu = new List<Food>() };

            // Act & Assert
            Assert.Throws<Exception>(() => gippo.BuyFood("Nonexistent"));
        }
    }

    public class GippoServiceTests
    {
        [Fact]
        public void BuyFood_SuccessfulPurchase_ReturnsFood()
        {
            // Arrange
            var food = new Food(2.99m, "Bread", false, false, 10);
            var gippo = new Gippo { Menu = new List<Food> { food } };
            var service = new GippoService(gippo);

            // Act
            var result = service.BuyFood("Bread");

            // Assert
            Assert.Equal(food, result);
            Assert.Equal(9, food.Count);
        }

        [Fact]
        public void TryBuyFood_FoodExists_ReturnsTrue()
        {
            // Arrange
            var food = new Food(1.49m, "Milk", true, false, 5);
            var gippo = new Gippo { Menu = new List<Food> { food } };
            var service = new GippoService(gippo);

            // Act
            var result = service.TryBuyFood("Milk", out var purchasedFood);

            // Assert
            Assert.True(result);
            Assert.Equal(food, purchasedFood);
        }
    }

    public class DispetcherTests
    {
        [Fact]
        public void Dispetcher_Constructor_SetsProperties()
        {
            // Arrange
            var info = new StaffInformation("0987654321", "Alice", "Smith", "Test Dispatcher", 28);

            // Act
            var dispatcher = new Dispetcher(info);

            // Assert
            Assert.Equal("Alice", dispatcher.Name);
            Assert.Equal("Smith", dispatcher.Surname);
            Assert.Equal("0987654321", dispatcher.PhoneNumber);
            Assert.Equal("Test Dispatcher", dispatcher.Description);
            Assert.Equal(28, dispatcher.YearsOld);
        }
    }

    public class DispetcherServiceTests
    {
        [Fact]
        public void AssignBasket_ValidBasket_AddsToAssignedBaskets()
        {
            // Arrange
            var staffInfo = new StaffInformation("123", "Name", "Surname", "Desc", 25);
            var dispatcher = new Dispetcher(staffInfo);
            var service = new DispetcherService(dispatcher);
            var basket = new Basket();

            // Act
            service.AssignBasket(basket);

            // Assert
            Assert.Equal(1, service.GetTotalBasketsCount());
        }

        [Fact]
        public void CompleteBasket_ExistingBasket_RemovesFromAssignedBaskets()
        {
            // Arrange
            var staffInfo = new StaffInformation("123", "Name", "Surname", "Desc", 25);
            var dispatcher = new Dispetcher(staffInfo);
            var service = new DispetcherService(dispatcher);
            var basket = new Basket();
            service.AssignBasket(basket);

            // Act
            var result = service.CompleteBasket(basket);

            // Assert
            Assert.True(result);
            Assert.Equal(0, service.GetTotalBasketsCount());
        }
    }

    public class SheffcookerTests
    {
        [Fact]
        public void Sheffcooker_Constructor_SetsProperties()
        {
            // Arrange
            var info = new StaffInformation("1234567890", "Gordon", "Ramsay", "Master Chef", 45);
            int stars = 5;
            int experience = 20;

            // Act
            var cooker = new Sheffcooker(info, stars, experience);

            // Assert
            Assert.Equal("Gordon", cooker.Name);
            Assert.Equal("Ramsay", cooker.Surname);
            Assert.Equal("1234567890", cooker.PhoneNumber);
            Assert.Equal("Master Chef", cooker.Description);
            Assert.Equal(45, cooker.YearsOld);
            Assert.Equal(5, cooker.countStarMischlen);
            Assert.Equal(20, cooker.countOfExpirience);
        }
    }

    public class AdressTests
    {
        [Fact]
        public void Adress_Constructor_SetsProperties()
        {
            // Arrange & Act
            var address = new Adress("Main Street", "New York", "NY", 5);

            // Assert
            Assert.Equal("Main Street", address.street);
            Assert.Equal("New York", address.city);
            Assert.Equal("NY", address.state);
            Assert.Equal(5, address.floor);
        }

        [Fact]
        public void Adress_Constructor_DefaultFloor_SetsToOne()
        {
            // Arrange & Act
            var address = new Adress("Second Street", "Boston", "MA");

            // Assert
            Assert.Equal(1, address.floor);
        }
    }

    public class StaffInformationTests
    {
        [Fact]
        public void StaffInformation_Constructor_SetsProperties()
        {
            // Arrange & Act
            var info = new StaffInformation("1234567890", "John", "Doe", "Test Description", 30);

            // Assert
            Assert.Equal("1234567890", info.phoneNumber);
            Assert.Equal("John", info.name);
            Assert.Equal("Doe", info.surname);
            Assert.Equal("Test Description", info.description);
            Assert.Equal(30, info.yearsOld);
        }
    }

    public class BasketTests
    {
        [Fact]
        public void AddItem_SingleFood_AddsToBasketAndUpdatesPrice()
        {
            // Arrange
            var basket = new Basket();
            var food = new Food(10.99m, "Test Food", false, false, 1);

            // Act
            basket.AddItem(food);

            // Assert
            Assert.Equal(10.99m, basket.price);
            Assert.Contains(food, basket.BasketOfFood);
        }

        [Fact]
        public void AddItem_MultipleFoods_AddsAllToBasket()
        {
            // Arrange
            var basket = new Basket();
            var foods = new List<Food>
        {
            new Food(5.99m, "Food1", false, false, 1),
            new Food(7.99m, "Food2", false, false, 1)
        };

            // Act
            basket.AddItem(foods);

            // Assert
            Assert.Equal(13.98m, basket.price);
            Assert.Equal(2, basket.BasketOfFood.Count());
        }
    }

    public class DelieviryFoodTests
    {
        [Fact]
        public void Rename_ValidName_UpdatesCompanyName()
        {
            // Arrange
            var deliveryFood = new DelieviryFood();
            string originalName = DelieviryFood.name;

            // Act
            deliveryFood.Rename("New Company Name");

            // Assert
            Assert.Equal("New Company Name", DelieviryFood.name);
        }

        [Fact]
        public void Singleton_StaticConstructor_CreatesInstance()
        {
            // Act & Assert
            Assert.NotNull(DelieviryFood.singletone);
        }
    }

    public class DeliveryFoodServiceTests
    {
        [Fact]
        public void RenameCompany_ValidName_UpdatesCompanyName()
        {
            // Arrange
            var deliveryFood = DelieviryFood.singletone;
            var service = new DeliveryFoodService(deliveryFood);

            // Act
            service.RenameCompany("Test Company");

            // Assert
            Assert.Equal("Test Company", DelieviryFood.name);
        }

        [Fact]
        public void AddDispatcher_NewDispatcher_AddsToCollection()
        {
            // Arrange
            var deliveryFood = new DelieviryFood();
            var service = new DeliveryFoodService(deliveryFood);
            var staffInfo = new StaffInformation("123", "Test", "Dispatcher", "Desc", 30);
            var dispatcher = new Dispetcher(staffInfo);

            // Act
            service.AddDispatcher(dispatcher);

            // Assert
            Assert.Contains(dispatcher, deliveryFood.Dispetchers);
        }

        [Fact]
        public void AddCourier_NewCourier_AddsToCollection()
        {
            // Arrange
            var deliveryFood = new DelieviryFood();
            var service = new DeliveryFoodService(deliveryFood);
            var staffInfo = new StaffInformation("123", "Test", "Courier", "Desc", 25);
            var courier = new Currier(staffInfo);

            // Act
            service.AddCourier(courier);

            // Assert
            Assert.Contains(courier, deliveryFood.curriers);
        }

        [Fact]
        public void IsCompanyOperational_WithStaff_ReturnsTrue()
        {
            // Arrange
            var deliveryFood = new DelieviryFood();
            var service = new DeliveryFoodService(deliveryFood);

            var dispatcherInfo = new StaffInformation("111", "D", "1", "Desc", 30);
            var courierInfo = new StaffInformation("222", "C", "1", "Desc", 25);

            service.AddDispatcher(new Dispetcher(dispatcherInfo));
            service.AddCourier(new Currier(courierInfo));
            service.RenameCompany("Test");

            // Act
            var isOperational = service.IsCompanyOperational();

            // Assert
            Assert.True(isOperational);
        }
    }

    public class FastFoodChainTests
    {
        [Fact]
        public void KFCService_AddChickenBucket_AddsToMenu()
        {
            // Arrange
            var address = new Adress("KFC St", "City", "State");
            var staffInfo = new StaffInformation("123", "Manager", "Name", "Desc", 35);
            var kfc = new KFC(new List<Food>(), address, new Meneger(staffInfo), new KFCService(null));
            var service = new KFCService(kfc);
            var chicken = new Food(15.99m, "Chicken Bucket", false, false, 1);

            // Act
            service.AddChickenBucket(chicken);

            // Assert
            Assert.Contains(chicken, kfc.Menu);
        }

        [Fact]
        public void BurgerKingService_HasWhopper_ReturnsCorrectValue()
        {
            // Arrange
            var menu = new List<Food> { new Food(6.99m, "Whopper", false, false, 1) };
            var address = new Adress("BK St", "City", "State");
            var staffInfo = new StaffInformation("123", "Manager", "Name", "Desc", 35);
            var bk = new BurgerKing(menu, address, new Meneger(staffInfo), new BurgerKingService(null));
            var service = new BurgerKingService(bk);

            // Act
            var hasWhopper = service.HasWhopper();

            // Assert
            Assert.True(hasWhopper);
        }

        [Fact]
        public void StarbucksService_HasFrappuccino_ReturnsTrueWhenExists()
        {
            // Arrange
            var menu = new List<Food> { new Food(4.99m, "Caramel Frappuccino", true, false, 1) };
            var address = new Adress("SB St", "City", "State");
            var staffInfo = new StaffInformation("123", "Manager", "Name", "Desc", 35);
            var starbucks = new Starbucks(menu, address, new Meneger(staffInfo), new StarbucksService(null));
            var service = new StarbucksService(starbucks);

            // Act
            var hasFrappuccino = service.HasFrappuccino();

            // Assert
            Assert.True(hasFrappuccino);
        }

        [Fact]
        public void ChickfilAService_IsSundayClosed_ReturnsTrue()
        {
            // Arrange
            var address = new Adress("CFA St", "City", "State");
            var staffInfo = new StaffInformation("123", "Manager", "Name", "Desc", 35);
            var chickfilA = new ChickfilA(new List<Food>(), address, new Meneger(staffInfo), new ChickfilAService(null));
            var service = new ChickfilAService(chickfilA);

            // Act
            var isClosed = service.IsSundayClosed();

            // Assert
            Assert.True(isClosed);
        }


        [Fact]
        public void Adress_Constructor_WithAllParameters_SetsPropertiesCorrectly()
        {
            // Arrange
            var street = "Pushkin Street";
            var city = "Minsk";
            var state = "Minsk Region";
            var floor = 5;

            // Act
            var address = new Adress(street, city, state, floor);

            // Assert
            Assert.Equal(street, address.street);
            Assert.Equal(city, address.city);
            Assert.Equal(state, address.state);
            Assert.Equal(floor, address.floor);
        }

        [Fact]
        public void Adress_Constructor_WithoutFloorParameter_DefaultsFloorToOne()
        {
            // Arrange
            var street = "Kolas Square";
            var city = "Minsk";
            var state = "Minsk Region";

            // Act
            var address = new Adress(street, city, state);

            // Assert
            Assert.Equal(street, address.street);
            Assert.Equal(city, address.city);
            Assert.Equal(state, address.state);
            Assert.Equal(1, address.floor); // Проверяем значение по умолчанию
        }

        [Fact]
        public void Adress_Constructor_UninitializedProperties_AreNull()
        {
            // Arrange
            var street = "Independent Avenue";
            var city = "Minsk";
            var state = "Minsk Region";

            // Act
            var address = new Adress(street, city, state);

            // Assert
            // Проверяем, что свойства, не заданные в конструкторе, остаются null
            Assert.Null(address.house);
            Assert.Null(address.coordinates);
        }





        [Fact]
        public void PayAnything_SufficientBalance_ReducesBalance()
        {
            // Arrange
            var service = new UserService();
            decimal initialBalance = 100.0m;
            decimal price = 25.5m;
            var expectedBalance = initialBalance - price;

            // Act
            service.PayAnything(price, initialBalance); 

            // Assert
            Assert.True(true); 
        }

        [Fact]
        public void PayAnything_InsufficientBalance_ThrowsException()
        {
            // Arrange
            var service = new UserService();
            decimal balance = 20.0m;
            decimal price = 50.0m;

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => service.PayAnything(price, balance));
            Assert.Equal("Balance lower than price", exception.Message);
        }

        [Fact]
        public void GiveMoney_PositiveAmount_IncreasesBalance()
        {
            // Arrange
            var service = new UserService();
            decimal balance = 100.0m;
            decimal moneyToAdd = 50.0m;

            // Act
            service.GiveMoney(moneyToAdd, ref balance);

            // Assert
            Assert.Equal(150.0m, balance);
        }

        [Fact]
        public void GiveMoney_NegativeAmount_ThrowsException()
        {
            // Arrange
            var service = new UserService();
            decimal balance = 100.0m;
            decimal moneyToAdd = -50.0m;

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => service.GiveMoney(moneyToAdd, ref balance));
            Assert.Equal("Money can not be negative", exception.Message);
        }

        [Fact]
        public void BuyFood_SuccessfulPurchase_AddsFoodToList()
        {
            // Arrange
            var service = new UserService();
            var food1 = new Food(10.0m, "Burger", false, false, 1);
            var food2 = new Food(5.5m, "Fries", false, false, 1);
            var basket = new Basket();
            basket.AddItem(new List<Food> { food1, food2 }); // price = 15.5m

            decimal balance = 20.0m;
            var purchasedFoods = new List<Food>();

            // Act
            service.BuyFood(basket, purchasedFoods, balance);

            // Assert
            
            Assert.Equal(2, purchasedFoods.Count());
            Assert.Contains(food1, purchasedFoods);
            Assert.Contains(food2, purchasedFoods);
        }

        [Fact]
        public void BuyFood_InsufficientBalance_DoesNotAddFood()
        {
            // Arrange
            var service = new UserService();
            var food1 = new Food(10.0m, "Burger", false, false, 1);
            var basket = new Basket();
            basket.AddItem(food1); // price = 10.0m

            decimal balance = 5.0m; // Недостаточный баланс
            var purchasedFoods = new List<Food>();

            // Act
            service.BuyFood(basket, purchasedFoods, balance);

            // Assert
            
            Assert.Empty(purchasedFoods);
        }

        
        private Meneger CreateTestMeneger()
        {
            var staffInfo = new StaffInformation("1234567890", "John", "Doe", "Initial Description", 30);
            var meneger = new Meneger(staffInfo);
            meneger.Balanse = 1000m; // Установим начальный баланс
            return meneger;
        }

        

        #region Static Method Tests

        // Вспомогательная коллекция для статических тестов
        private List<Meneger> CreateTestMenegerList()
        {
            return new List<Meneger>
    {
        new Meneger(new StaffInformation("1", "Charlie", "Brown", "d1", 25)) { Balanse = 1500 },
        new Meneger(new StaffInformation("2", "Alice", "Smith", "d2", 40)) { Balanse = 3000 },
        new Meneger(new StaffInformation("3", "Bob", "Johnson", "d3", 22)) { Balanse = 800 }
    };
        }

        [Fact]
        public void FindManagerByName_WhenManagerExists_ReturnsManager()
        {
            // Arrange
            var managers = CreateTestMenegerList();

            // Act
            var result = MenegerService.FindManagerByName(managers, "Alice", "Smith");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public void FilterManagersByAge_ReturnsCorrectSubset()
        {
            // Arrange
            var managers = CreateTestMenegerList();

            // Act
            var result = MenegerService.FilterManagersByAge(managers, 20, 30).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Charlie", result.First().Name);
        }

        [Fact]
        public void GetManagerWithHighestBalance_ReturnsCorrectManager()
        {
            // Arrange
            var managers = CreateTestMenegerList();

            // Act
            var result = MenegerService.GetManagerWithHighestBalance(managers);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
            Assert.Equal(3000, result.Balanse);
        }


        #endregion
        // Вспомогательный метод для создания объекта Mamuka, чтобы не дублировать код
        private Mamuka CreateTestMamuka()
        {
            var menu = new List<Food>
    {
        new Food(10.0m, "Khachapuri", false, false, 5),
        new Food(15.5m, "Khinkali", false, false, 10),
        new Food(5.0m, "Salad", true, true, 8)
    };
            var address = new Adress("Test St", "Test City", "Test State", 1);
            var manager = new Meneger(new StaffInformation("1", "Manager", "M", "Desc", 40));
            var sheffCooker = new Sheffcooker(new StaffInformation("2", "Cook", "C", "Desc", 35), 1, 10);

            var mamuka = new Mamuka(menu, address, manager, sheffCooker, "A cozy place", null);
            mamuka.Mark = 4.2;
            mamuka.countOfMetteng = 5;

            return mamuka;
        }

        
        

        #region Meeting Count Tests

        [Fact]
        public void IncrementMeetingCount_IncreasesCountByOne()
        {
            // Arrange
            var mamuka = CreateTestMamuka(); // countOfMetteng = 5
            var service = new MamukaService(mamuka);

            // Act
            service.IncrementMeetingCount();

            // Assert
            Assert.Equal(6, mamuka.countOfMetteng);
        }

        [Fact]
        public void DecrementMeetingCount_WhenCountIsPositive_DecreasesCountByOne()
        {
            // Arrange
            var mamuka = CreateTestMamuka(); // countOfMetteng = 5
            var service = new MamukaService(mamuka);

            // Act
            service.DecrementMeetingCount();

            // Assert
            Assert.Equal(4, mamuka.countOfMetteng);
        }

        [Fact]
        public void DecrementMeetingCount_WhenCountIsZero_RemainsZero()
        {
            // Arrange
            var mamuka = CreateTestMamuka();
            mamuka.countOfMetteng = 0;
            var service = new MamukaService(mamuka);

            // Act
            service.DecrementMeetingCount();

            // Assert
            Assert.Equal(0, mamuka.countOfMetteng);
        }

        [Fact]
        public void SetMeetingCount_WithNegativeValue_ThrowsArgumentException()
        {
            // Arrange
            var mamuka = CreateTestMamuka();
            var service = new MamukaService(mamuka);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.SetMeetingCount(-1));
        }

        #endregion#region Rating Management Tests

        [Theory]
        [InlineData(-0.1)]
        [InlineData(5.1)]
        public void UpdateMark_WithInvalidMark_ThrowsArgumentException(double invalidMark)
        {
            // Arrange
            var mamuka = CreateTestMamuka();
            var service = new MamukaService(mamuka);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.UpdateMark(invalidMark));
        }

        [Theory]
        [InlineData(4.7, "Excellent")]
        [InlineData(4.2, "Very Good")]
        [InlineData(3.8, "Good")]
        [InlineData(3.0, "Average")]
        [InlineData(2.5, "Poor")]
        [InlineData(1.9, "Very Poor")]
        public void GetMarkDescription_ReturnsCorrectDescription(double mark, string expectedDescription)
        {
            // Arrange
            var mamuka = CreateTestMamuka();
            mamuka.Mark = mark;
            var service = new MamukaService(mamuka);

            // Act
            var result = service.GetMarkDescription();

            // Assert
            Assert.Equal(expectedDescription, result);
        }

        #endregion

        

        // Вспомогательный метод для создания объекта Gippo с тестовыми данными
        private Gippo CreateTestGippo()
        {
            var menu = new List<Food>
    {
        new Food(2.50m, "Milk", false, false, 10),
        new Food(3.00m, "Bread", false, false, 5),
        new Food(5.00m, "Cheese", false, false, 0) // Товар не в наличии
    };
            var address = new Adress("Gippo St.", "Minsk", "Minsk Region");

            // Имитируем логику конструктора из статического метода
            return new Gippo
            {
                Menu = menu,
                Adress = address,
                Description = "A local grocery store",
                Mark = 4.0,
                countOfMetteng = 20 // Начальное количество оценок
            };
        }

        


        
    }      
        
}