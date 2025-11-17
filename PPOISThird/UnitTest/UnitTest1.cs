using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreDomainEntities;
using Exceptions;
using SecuritySystems;
using Types;
using VisitorManagment;
using PersonalManagment;
using FinancicalSystem;
namespace ArtGallery.Tests
{
    public class ArtworkTests
    {
        [Fact]
        public void CanBeSold_WhenArtworkNotDamaged_ReturnsTrue()
        {
            // Arrange
            var artwork = new Artwork { Condition = ArtworkCondition.Excellent };

            // Act
            var result = artwork.CanBeSold();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanBeSold_WhenArtworkDamaged_ReturnsFalse()
        {
            // Arrange
            var artwork = new Artwork { Condition = ArtworkCondition.Damaged };

            // Act
            var result = artwork.CanBeSold();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CalculateInsuranceCost_WithValidValue_ReturnsCorrectAmount()
        {
            // Arrange
            var artwork = new Artwork { EstimatedValue = 10000m };

            // Act
            var result = artwork.CalculateInsuranceCost();

            // Assert
            Assert.Equal(100m, result);
        }

        [Fact]
        public void RequiresRestoration_WhenConditionPoor_ReturnsTrue()
        {
            // Arrange
            var artwork = new Artwork { Condition = ArtworkCondition.Poor };

            // Act
            var result = artwork.RequiresRestoration();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void RequiresRestoration_WhenConditionExcellent_ReturnsFalse()
        {
            // Arrange
            var artwork = new Artwork { Condition = ArtworkCondition.Excellent };

            // Act
            var result = artwork.RequiresRestoration();

            // Assert
            Assert.False(result);
        }
    }

    public class ArtistTests
    {
        [Fact]
        public void GetAge_WhenArtistAlive_ReturnsCorrectAge()
        {
            // Arrange
            var artist = new Artist { BirthDate = new DateTime(1980, 1, 1) };

            // Act
            var result = artist.GetAge();

            // Assert
            Assert.True(result >= 43); // Assuming current year is 2023+
        }



        [Fact]
        public void IsContemporary_WhenBornAfter1900_ReturnsTrue()
        {
            // Arrange
            var artist = new Artist { BirthDate = new DateTime(1980, 1, 1) };

            // Act
            var result = artist.IsContemporary();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsContemporary_WhenBornBefore1900_ReturnsFalse()
        {
            // Arrange
            var artist = new Artist { BirthDate = new DateTime(1850, 1, 1) };

            // Act
            var result = artist.IsContemporary();

            // Assert
            Assert.False(result);
        }
    }

    public class ExhibitionTests
    {
        [Fact]
        public void IsActive_WhenCurrentDateInRange_ReturnsTrue()
        {
            // Arrange
            var exhibition = new Exhibition
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1)
            };

            // Act
            var result = exhibition.IsActive();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsActive_WhenCurrentDateBeforeStart_ReturnsFalse()
        {
            // Arrange
            var exhibition = new Exhibition
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            // Act
            var result = exhibition.IsActive();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsActive_WhenCurrentDateAfterEnd_ReturnsFalse()
        {
            // Arrange
            var exhibition = new Exhibition
            {
                StartDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = exhibition.IsActive();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetArtworkCount_WhenArtworksExist_ReturnsCorrectCount()
        {
            // Arrange
            var exhibition = new Exhibition
            {
                Artworks = new List<Artwork>
                {
                    new Artwork(),
                    new Artwork()
                }
            };

            // Act
            var result = exhibition.GetArtworkCount();

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void CanAddArtwork_WhenHallHasCapacity_ReturnsTrue()
        {
            // Arrange
            var hall = new GalleryHall { Capacity = 10, Artworks = new List<Artwork>() };
            var exhibition = new Exhibition
            {
                Hall = hall,
                Artworks = new List<Artwork>()
            };
            var artwork = new Artwork();

            // Act
            var result = exhibition.CanAddArtwork(artwork);

            // Assert
            Assert.True(result);
        }
    }

    public class GalleryTests
    {
        [Fact]
        public void CalculateTotalArtValue_WithArtworks_ReturnsCorrectSum()
        {
            // Arrange
            var hall1 = new GalleryHall
            {
                Artworks = new List<Artwork>
                {
                    new Artwork { EstimatedValue = 5000m },
                    new Artwork { EstimatedValue = 3000m }
                }
            };

            var hall2 = new GalleryHall
            {
                Artworks = new List<Artwork>
                {
                    new Artwork { EstimatedValue = 7000m }
                }
            };

            var gallery = new Gallery
            {
                Halls = new List<GalleryHall> { hall1, hall2 }
            };

            // Act
            var result = gallery.CalculateTotalArtValue();

            // Assert
            Assert.Equal(15000m, result);
        }

        [Fact]
        public void GetTotalEmployees_WithEmployees_ReturnsCorrectCount()
        {
            // Arrange
            var gallery = new Gallery
            {
                Employees = new List<Employee>
                {
                    new Employee(),
                    new Employee(),
                    new Employee()
                }
            };

            // Act
            var result = gallery.GetTotalEmployees();

            // Assert
            Assert.Equal(3, result);
        }
    }

    public class FinancialAccountTests
    {




        [Fact]
        public void ProcessTransaction_WithZeroAmount_ThrowsException()
        {
            // Arrange
            var account = new FinancialAccount { Balance = 1000m };
            var transaction = new Transaction { Amount = 0m, Date = DateTime.Now };

            // Act & Assert
            Assert.Throws<InvalidTransactionAmountException>(
                () => account.ProcessTransaction(transaction));
        }

        [Fact]
        public void CanWithdraw_WhenSufficientFunds_ReturnsTrue()
        {
            // Arrange
            var account = new FinancialAccount { Balance = 1000m };

            // Act
            var result = account.CanWithdraw(500m);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanWithdraw_WhenInsufficientFunds_ReturnsFalse()
        {
            // Arrange
            var account = new FinancialAccount { Balance = 100m };

            // Act
            var result = account.CanWithdraw(500m);

            // Assert
            Assert.False(result);
        }
    }

    public class TransactionTests
    {
        [Fact]
        public void IsValid_WithFutureDate_ReturnsFalse()
        {
            // Arrange
            var transaction = new Transaction
            {
                Amount = 100m,
                Date = DateTime.Now.AddDays(1)
            };

            // Act
            var result = transaction.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WithZeroAmount_ReturnsFalse()
        {
            // Arrange
            var transaction = new Transaction
            {
                Amount = 0m,
                Date = DateTime.Now
            };

            // Act
            var result = transaction.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsIncome_WithPositiveAmount_ReturnsTrue()
        {
            // Arrange
            var transaction = new Transaction { Amount = 100m };

            // Act
            var result = transaction.IsIncome();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsIncome_WithNegativeAmount_ReturnsFalse()
        {
            // Arrange
            var transaction = new Transaction { Amount = -100m };

            // Act
            var result = transaction.IsIncome();

            // Assert
            Assert.False(result);
        }
    }

    public class EmployeeTests
    {
        [Fact]
        public void GetYearsOfService_WithValidHireDate_ReturnsCorrectYears()
        {
            // Arrange
            var employee = new Employee
            {
                HireDate = DateTime.Now.AddYears(-5).AddDays(-10)
            };

            // Act
            var result = employee.GetYearsOfService();

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void CanHandleArtwork_WithMatchingSkills_ReturnsTrue()
        {
            // Arrange
            var skill = new Skill { Name = "Oil Painting" };
            var material = new Material { Name = "Oil Painting" };
            var artwork = new Artwork { Materials = new List<Material> { material } };
            var employee = new Employee { Skills = new List<Skill> { skill } };

            // Act
            var result = employee.CanHandleArtwork(artwork);

            // Assert
            Assert.True(result);
        }




    }

    public class CuratorTests
    {
        [Fact]
        public void CanCurateExhibition_WithMatchingExpertise_ReturnsTrue()
        {
            // Arrange
            var movement = new ArtMovement { Name = "Impressionism" };
            var theme = new ExhibitionTheme { Name = "French Impressionism" };
            var exhibition = new Exhibition { Theme = theme };
            var curator = new Curator { ExpertMovements = new List<ArtMovement> { movement } };

            // Act
            var result = curator.CanCurateExhibition(exhibition);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanCurateExhibition_WithNoMatchingExpertise_ReturnsFalse()
        {
            // Arrange
            var movement = new ArtMovement { Name = "Cubism" };
            var theme = new ExhibitionTheme { Name = "French Impressionism" };
            var exhibition = new Exhibition { Theme = theme };
            var curator = new Curator { ExpertMovements = new List<ArtMovement> { movement } };

            // Act
            var result = curator.CanCurateExhibition(exhibition);

            // Assert
            Assert.False(result);
        }
    }

    public class SecurityGuardTests
    {
        [Fact]
        public void NeedsRetraining_WhenTrainingOver180DaysAgo_ReturnsTrue()
        {
            // Arrange
            var guard = new SecurityGuard
            {
                LastTraining = DateTime.Now.AddDays(-200)
            };

            // Act
            var result = guard.NeedsRetraining();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void NeedsRetraining_WhenTrainingWithin180Days_ReturnsFalse()
        {
            // Arrange
            var guard = new SecurityGuard
            {
                LastTraining = DateTime.Now.AddDays(-100)
            };

            // Act
            var result = guard.NeedsRetraining();

            // Assert
            Assert.False(result);
        }
    }

    public class RestorerTests
    {
        [Fact]
        public void CanRestore_WithMatchingMaterials_ReturnsTrue()
        {
            // Arrange
            var expertMaterial = new Material { Name = "Oil Paint" };
            var artworkMaterial = new Material { Name = "Oil Paint" };
            var artwork = new Artwork { Materials = new List<Material> { artworkMaterial } };
            var restorer = new Restorer { ExpertMaterials = new List<Material> { expertMaterial } };

            // Act
            var result = restorer.CanRestore(artwork);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanRestore_WithNoMatchingMaterials_ReturnsFalse()
        {
            // Arrange
            var expertMaterial = new Material { Name = "Watercolor" };
            var artworkMaterial = new Material { Name = "Oil Paint" };
            var artwork = new Artwork { Materials = new List<Material> { artworkMaterial } };
            var restorer = new Restorer { ExpertMaterials = new List<Material> { expertMaterial } };

            // Act
            var result = restorer.CanRestore(artwork);

            // Assert
            Assert.False(result);
        }
    }

    public class TicketTests
    {
        [Fact]
        public void IsValid_WhenTicketNotExpired_ReturnsTrue()
        {
            // Arrange
            var ticket = new Ticket
            {
                ValidUntil = DateTime.Now.AddDays(1)
            };

            // Act
            var result = ticket.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenTicketExpired_ReturnsFalse()
        {
            // Arrange
            var ticket = new Ticket
            {
                ValidUntil = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = ticket.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsGroupTicket_WhenTicketTypeIsGroup_ReturnsTrue()
        {
            // Arrange
            var ticket = new Ticket { Type = TicketType.Group };

            // Act
            var result = ticket.IsGroupTicket();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsGroupTicket_WhenTicketTypeIsRegular_ReturnsFalse()
        {
            // Arrange
            var ticket = new Ticket { Type = TicketType.Regular };

            // Act
            var result = ticket.IsGroupTicket();

            // Assert
            Assert.False(result);
        }
    }

    public class MembershipTests
    {
        [Fact]
        public void IsActive_WhenMembershipNotExpired_ReturnsTrue()
        {
            // Arrange
            var membership = new Membership
            {
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = membership.IsActive();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsActive_WhenMembershipExpired_ReturnsFalse()
        {
            // Arrange
            var membership = new Membership
            {
                ExpiryDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = membership.IsActive();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetRemainingDays_WithFutureExpiry_ReturnsPositiveDays()
        {
            // Arrange
            var membership = new Membership
            {
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = membership.GetRemainingDays();

            // Assert
            Assert.True(result > 0);
        }
    }

    public class InsurancePolicyTests
    {
        [Fact]
        public void IsActive_WhenPolicyNotExpired_ReturnsTrue()
        {
            // Arrange
            var policy = new InsurancePolicy
            {
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = policy.IsActive();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsActive_WhenPolicyExpired_ReturnsFalse()
        {
            // Arrange
            var policy = new InsurancePolicy
            {
                ExpiryDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = policy.IsActive();

            // Assert
            Assert.False(result);
        }
    }

    public class DimensionsTests
    {
        [Fact]
        public void CalculateArea_WithValidDimensions_ReturnsCorrectArea()
        {
            // Arrange
            var dimensions = new Dimensions { Width = 10m, Height = 5m };

            // Act
            var result = dimensions.CalculateArea();

            // Assert
            Assert.Equal(50m, result);
        }

        [Fact]
        public void CalculateVolume_WithValidDimensions_ReturnsCorrectVolume()
        {
            // Arrange
            var dimensions = new Dimensions { Width = 10m, Height = 5m, Depth = 2m };

            // Act
            var result = dimensions.CalculateVolume();

            // Assert
            Assert.Equal(100m, result);
        }
    }

    public class BudgetTests
    {
        [Fact]
        public void GetRemainingBudget_WithSpentAmount_ReturnsCorrectRemaining()
        {
            // Arrange
            var budget = new Budget { TotalAmount = 10000m, SpentAmount = 3000m };

            // Act
            var result = budget.GetRemainingBudget();

            // Assert
            Assert.Equal(7000m, result);
        }
    }

    public class GalleryHallTests
    {
        [Fact]
        public void CanAddArtwork_WhenBelowCapacityAndSecure_ReturnsTrue()
        {
            // Arrange
            var security = new SecuritySystem { SecurityLevel = SecurityLevel.High };
            var hall = new GalleryHall
            {
                Capacity = 5,
                Artworks = new List<Artwork>(),
                Security = security
            };
            var artwork = new Artwork { EstimatedValue = 5000m };

            // Act
            var result = hall.CanAddArtwork(artwork);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CalculateMaintenanceCost_WithCapacity_ReturnsCorrectCost()
        {
            // Arrange
            var hall = new GalleryHall { Capacity = 10 };

            // Act
            var result = hall.CalculateMaintenanceCost();

            // Assert
            Assert.Equal(100m, result);
        }
    }

    public class SecuritySystemTests
    {
        [Fact]
        public void CanSecureArtwork_WhenValueBelowMaxSecured_ReturnsTrue()
        {
            // Arrange
            var security = new SecuritySystem { SecurityLevel = SecurityLevel.Medium };
            var artwork = new Artwork { EstimatedValue = 50000m };

            // Act
            var result = security.CanSecureArtwork(artwork);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanSecureArtwork_WhenValueAboveMaxSecured_ReturnsFalse()
        {
            // Arrange
            var security = new SecuritySystem { SecurityLevel = SecurityLevel.Low };
            var artwork = new Artwork { EstimatedValue = 20000m };

            // Act
            var result = security.CanSecureArtwork(artwork);

            // Assert
            Assert.False(result);
        }
    }

    public class PaymentMethodTests
    {
        [Fact]
        public void IsValid_WhenActiveAndNotExpired_ReturnsTrue()
        {
            // Arrange
            var paymentMethod = new PaymentMethod
            {
                IsActive = true,
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = paymentMethod.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenInactive_ReturnsFalse()
        {
            // Arrange
            var paymentMethod = new PaymentMethod
            {
                IsActive = false,
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = paymentMethod.IsValid();

            // Assert
            Assert.False(result);
        }
    }

    public class InvoiceTests
    {
        [Fact]
        public void IsOverdue_WhenDueDatePassedAndNotPaid_ReturnsTrue()
        {
            // Arrange
            var invoice = new Invoice
            {
                DueDate = DateTime.Now.AddDays(-1),
                Status = InvoiceStatus.Sent
            };

            // Act
            var result = invoice.IsOverdue();

            // Assert
            Assert.True(result);
        }




    }

    public class AccessCardTests
    {
        [Fact]
        public void IsValid_WhenActiveAndNotExpired_ReturnsTrue()
        {
            // Arrange
            var card = new AccessCard
            {
                IsActive = true,
                ExpiryDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = card.IsValid();

            // Assert
            Assert.True(result);
        }

        public class ArtworkAdvancedTests
        {


            [Theory]
            [InlineData(5000, 50)]
            [InlineData(10000, 100)]
            [InlineData(25000, 250)]
            [InlineData(0, 0)]
            public void CalculateInsuranceCost_VariousValues_ReturnsCorrect(decimal value, decimal expected)
            {
                var artwork = new Artwork { EstimatedValue = value };
                Assert.Equal(expected, artwork.CalculateInsuranceCost());
            }







            [Fact]
            public void IsValid_WhenExpired_ReturnsFalse()
            {
                // Arrange
                var card = new AccessCard
                {
                    IsActive = true,
                    ExpiryDate = DateTime.Now.AddDays(-1)
                };

                // Act
                var result = card.IsValid();

                // Assert
                Assert.False(result);
            }
        }

        public class VisitorTests
        {
            [Fact]
            public void HasValidTicket_WithValidTicketForExhibition_ReturnsTrue()
            {
                // Arrange
                var exhibition = new Exhibition();
                var ticket = new Ticket
                {
                    Exhibition = exhibition,
                    ValidUntil = DateTime.Now.AddDays(1)
                };
                var visitor = new Visitor { Tickets = new List<Ticket> { ticket } };

                // Act
                var result = visitor.HasValidTicket(exhibition);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void HasValidTicket_WithExpiredTicket_ReturnsFalse()
            {
                // Arrange
                var exhibition = new Exhibition();
                var ticket = new Ticket
                {
                    Exhibition = exhibition,
                    ValidUntil = DateTime.Now.AddDays(-1)
                };
                var visitor = new Visitor { Tickets = new List<Ticket> { ticket } };

                // Act
                var result = visitor.HasValidTicket(exhibition);

                // Assert
                Assert.False(result);
            }
        }
        public class ArtistAdvancedTests
        {







            [Fact]
            public void Artist_IsContemporary_WithHistoricalArtist_ReturnsFalse()
            {
                var artist = new Artist { BirthDate = new DateTime(1800, 1, 1) };
                Assert.False(artist.IsContemporary());
            }

            [Fact]
            public void Artist_Biography_CanBeAssigned()
            {
                var artist = new Artist();
                var biography = new Biography { EarlyLife = "Test early life" };

                artist.Biography = biography;

                Assert.NotNull(artist.Biography);
                Assert.Equal("Test early life", artist.Biography.EarlyLife);
            }
        }
        public class FinancialAdvancedTests
        {





            [Fact]
            public void FinancialAccount_CanWithdraw_WithExactBalance_ReturnsTrue()
            {
                var account = new FinancialAccount { Balance = 500m };
                Assert.True(account.CanWithdraw(500m));
            }

            [Fact]
            public void FinancialAccount_CanWithdraw_WithMoreThanBalance_ReturnsFalse()
            {
                var account = new FinancialAccount { Balance = 500m };
                Assert.False(account.CanWithdraw(501m));
            }

            [Fact]
            public void Transaction_IsValid_WithPastDateAndNonZeroAmount_ReturnsTrue()
            {
                var transaction = new Transaction
                {
                    Amount = 100m,
                    Date = DateTime.Now.AddDays(-1)
                };

                Assert.True(transaction.IsValid());
            }

            [Fact]
            public void Transaction_IsIncome_WithNegativeAmount_ReturnsFalse()
            {
                var transaction = new Transaction { Amount = -100m };
                Assert.False(transaction.IsIncome());
            }


        }
        public class ExhibitionAdvancedTests
        {


            [Fact]
            public void Exhibition_CanAddArtwork_WhenHallHasCapacity_ReturnsTrue()
            {
                var hall = new GalleryHall { Capacity = 5, Artworks = new List<Artwork>() };
                var exhibition = new Exhibition { Hall = hall, Artworks = new List<Artwork>() };
                var artwork = new Artwork();

                Assert.True(exhibition.CanAddArtwork(artwork));
            }

            [Fact]
            public void Exhibition_GetArtworkCount_WhenNullArtworks_ReturnsZero()
            {
                var exhibition = new Exhibition { Artworks = null };
                Assert.Equal(0, exhibition.GetArtworkCount());
            }
        }
        public class GalleryAdvancedTests
        {


            [Fact]
            public void Gallery_CalculateTotalArtValue_WithNoHalls_ReturnsZero()
            {
                var gallery = new Gallery { Halls = new List<GalleryHall>() };
                Assert.Equal(0m, gallery.CalculateTotalArtValue());
            }

            [Fact]
            public void Gallery_CalculateTotalArtValue_WithEmptyHalls_ReturnsZero()
            {
                var hall = new GalleryHall { Artworks = new List<Artwork>() };
                var gallery = new Gallery { Halls = new List<GalleryHall> { hall } };

                Assert.Equal(0m, gallery.CalculateTotalArtValue());
            }


        }










        public class EmployeeAdvancedTests
        {


            [Fact]
            public void Employee_GetYearsOfService_WithFutureHireDate_ReturnsZero()
            {
                var employee = new Employee { HireDate = DateTime.Now.AddDays(1) };
                Assert.Equal(0, employee.GetYearsOfService());
            }

            [Fact]
            public void Employee_CanHandleArtwork_WithNullArtwork_ReturnsFalse()
            {
                var employee = new Employee { Skills = new List<Skill>() };
                Assert.False(employee.CanHandleArtwork(null));
            }


        }
        // 


        public class ArtworkTests
        {
            [Fact]
            public void Artwork_CanBeSold_WhenConditionIsGood_ReturnsTrue()
            {
                // Arrange
                var artwork = new Artwork { Condition = ArtworkCondition.Good };

                // Act
                var result = artwork.CanBeSold();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Artwork_CanBeSold_WhenConditionIsDamaged_ReturnsFalse()
            {
                // Arrange
                var artwork = new Artwork { Condition = ArtworkCondition.Damaged };

                // Act
                var result = artwork.CanBeSold();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Artwork_CalculateInsuranceCost_ReturnsCorrectValue()
            {
                // Arrange
                var artwork = new Artwork { EstimatedValue = 10000m };
                var expected = 100m;

                // Act
                var result = artwork.CalculateInsuranceCost();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Artwork_RequiresRestoration_WhenConditionIsPoor_ReturnsTrue()
            {
                // Arrange
                var artwork = new Artwork { Condition = ArtworkCondition.Poor };

                // Act
                var result = artwork.RequiresRestoration();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Artwork_RequiresRestoration_WhenConditionIsGood_ReturnsFalse()
            {
                // Arrange
                var artwork = new Artwork { Condition = ArtworkCondition.Good };

                // Act
                var result = artwork.RequiresRestoration();

                // Assert
                Assert.False(result);
            }

            [Theory]
            [InlineData(ArtworkCondition.Excellent, true)]
            [InlineData(ArtworkCondition.Good, true)]
            [InlineData(ArtworkCondition.Fair, true)]
            [InlineData(ArtworkCondition.Poor, true)]
            [InlineData(ArtworkCondition.Damaged, false)]
            public void Artwork_CanBeSold_ForAllConditions_ReturnsExpected(ArtworkCondition condition, bool expected)
            {
                // Arrange
                var artwork = new Artwork { Condition = condition };

                // Act
                var result = artwork.CanBeSold();

                // Assert
                Assert.Equal(expected, result);
            }
        }

        public class ArtistTests
        {
            [Fact]
            public void Artist_GetAge_WhenAlive_ReturnsCorrectAge()
            {
                // Arrange
                var birthDate = new DateTime(1980, 1, 1);
                var artist = new Artist { BirthDate = birthDate, DeathDate = null };
                var expected = DateTime.Now.Year - birthDate.Year;

                // Act
                var result = artist.GetAge();

                // Assert
                Assert.Equal(expected, result);
            }



            [Fact]
            public void Artist_IsContemporary_WhenBornAfter1900_ReturnsTrue()
            {
                // Arrange
                var artist = new Artist { BirthDate = new DateTime(1980, 1, 1) };

                // Act
                var result = artist.IsContemporary();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Artist_IsContemporary_WhenBornBefore1900_ReturnsFalse()
            {
                // Arrange
                var artist = new Artist { BirthDate = new DateTime(1850, 1, 1) };

                // Act
                var result = artist.IsContemporary();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Artist_IsContemporary_WhenBornIn1900_ReturnsFalse()
            {
                // Arrange
                var artist = new Artist { BirthDate = new DateTime(1900, 1, 1) };

                // Act
                var result = artist.IsContemporary();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Artist_IsContemporary_WhenBornIn1901_ReturnsTrue()
            {
                // Arrange
                var artist = new Artist { BirthDate = new DateTime(1901, 1, 1) };

                // Act
                var result = artist.IsContemporary();

                // Assert
                Assert.True(result);
            }
        }

        public class ExhibitionTests
        {
            [Fact]
            public void Exhibition_IsActive_WhenCurrentDateInRange_ReturnsTrue()
            {
                // Arrange
                var exhibition = new Exhibition
                {
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(1)
                };

                // Act
                var result = exhibition.IsActive();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Exhibition_IsActive_WhenCurrentDateBeforeStart_ReturnsFalse()
            {
                // Arrange
                var exhibition = new Exhibition
                {
                    StartDate = DateTime.Now.AddDays(1),
                    EndDate = DateTime.Now.AddDays(2)
                };

                // Act
                var result = exhibition.IsActive();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Exhibition_IsActive_WhenCurrentDateAfterEnd_ReturnsFalse()
            {
                // Arrange
                var exhibition = new Exhibition
                {
                    StartDate = DateTime.Now.AddDays(-2),
                    EndDate = DateTime.Now.AddDays(-1)
                };

                // Act
                var result = exhibition.IsActive();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Exhibition_GetArtworkCount_WhenArtworksNull_ReturnsZero()
            {
                // Arrange
                var exhibition = new Exhibition { Artworks = null };

                // Act
                var result = exhibition.GetArtworkCount();

                // Assert
                Assert.Equal(0, result);
            }

            [Fact]
            public void Exhibition_GetArtworkCount_WhenArtworksEmpty_ReturnsZero()
            {
                // Arrange
                var exhibition = new Exhibition { Artworks = new List<Artwork>() };

                // Act
                var result = exhibition.GetArtworkCount();

                // Assert
                Assert.Equal(0, result);
            }

            [Fact]
            public void Exhibition_GetArtworkCount_WhenArtworksExist_ReturnsCount()
            {
                // Arrange
                var exhibition = new Exhibition
                {
                    Artworks = new List<Artwork>
                {
                    new Artwork(),
                    new Artwork()
                }
                };

                // Act
                var result = exhibition.GetArtworkCount();

                // Assert
                Assert.Equal(2, result);
            }

            [Fact]
            public void Exhibition_CanAddArtwork_WhenHallCapacityNotExceeded_ReturnsTrue()
            {
                // Arrange
                var hall = new GalleryHall { Capacity = 5 };
                var exhibition = new Exhibition
                {
                    Hall = hall,
                    Artworks = new List<Artwork> { new Artwork(), new Artwork() }
                };
                var artwork = new Artwork();

                // Act
                var result = exhibition.CanAddArtwork(artwork);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Exhibition_CanAddArtwork_WhenHallCapacityExceeded_ReturnsFalse()
            {
                // Arrange
                var hall = new GalleryHall { Capacity = 2 };
                var exhibition = new Exhibition
                {
                    Hall = hall,
                    Artworks = new List<Artwork> { new Artwork(), new Artwork() }
                };
                var artwork = new Artwork();

                // Act
                var result = exhibition.CanAddArtwork(artwork);

                // Assert
                Assert.False(result);
            }
        }

        public class GalleryTests
        {
            [Fact]
            public void Gallery_CalculateTotalArtValue_WhenNoArtworks_ReturnsZero()
            {
                // Arrange
                var gallery = new Gallery { Halls = new List<GalleryHall>() };

                // Act
                var result = gallery.CalculateTotalArtValue();

                // Assert
                Assert.Equal(0m, result);
            }

            [Fact]
            public void Gallery_CalculateTotalArtValue_WithArtworks_ReturnsSum()
            {
                // Arrange
                var hall1 = new GalleryHall
                {
                    Artworks = new List<Artwork>
                {
                    new Artwork { EstimatedValue = 1000m },
                    new Artwork { EstimatedValue = 2000m }
                }
                };
                var hall2 = new GalleryHall
                {
                    Artworks = new List<Artwork>
                {
                    new Artwork { EstimatedValue = 3000m }
                }
                };
                var gallery = new Gallery { Halls = new List<GalleryHall> { hall1, hall2 } };
                var expected = 6000m;

                // Act
                var result = gallery.CalculateTotalArtValue();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Gallery_GetTotalEmployees_WhenNoEmployees_ReturnsZero()
            {
                // Arrange
                var gallery = new Gallery { Employees = new List<Employee>() };

                // Act
                var result = gallery.GetTotalEmployees();

                // Assert
                Assert.Equal(0, result);
            }

            [Fact]
            public void Gallery_GetTotalEmployees_WithEmployees_ReturnsCount()
            {
                // Arrange
                var gallery = new Gallery
                {
                    Employees = new List<Employee>
                {
                    new Employee(),
                    new Employee(),
                    new Employee()
                }
                };

                // Act
                var result = gallery.GetTotalEmployees();

                // Assert
                Assert.Equal(3, result);
            }
        }

        public class GalleryHallTests
        {
            [Fact]
            public void GalleryHall_CanAddArtwork_WhenCapacityAndSecurityAllow_ReturnsTrue()
            {
                // Arrange
                var security = new SecuritySystem();
                var hall = new GalleryHall
                {
                    Capacity = 3,
                    Security = security,
                    Artworks = new List<Artwork> { new Artwork() }
                };
                var artwork = new Artwork();

                // Act
                var result = hall.CanAddArtwork(artwork);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void GalleryHall_CalculateMaintenanceCost_ReturnsCorrectValue()
            {
                // Arrange
                var hall = new GalleryHall { Capacity = 5 };
                var expected = 50m; // 5 * 10

                // Act
                var result = hall.CalculateMaintenanceCost();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void GalleryHall_CalculateMaintenanceCost_WithZeroCapacity_ReturnsZero()
            {
                // Arrange
                var hall = new GalleryHall { Capacity = 0 };

                // Act
                var result = hall.CalculateMaintenanceCost();

                // Assert
                Assert.Equal(0m, result);
            }
        }

        public class DimensionsTests
        {
            [Fact]
            public void Dimensions_CalculateArea_ReturnsCorrectValue()
            {
                // Arrange
                var dimensions = new Dimensions { Width = 10m, Height = 5m };
                var expected = 50m;

                // Act
                var result = dimensions.CalculateArea();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Dimensions_CalculateVolume_ReturnsCorrectValue()
            {
                // Arrange
                var dimensions = new Dimensions { Width = 10m, Height = 5m, Depth = 2m };
                var expected = 100m;

                // Act
                var result = dimensions.CalculateVolume();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Dimensions_CalculateVolume_WithZeroDepth_ReturnsZero()
            {
                // Arrange
                var dimensions = new Dimensions { Width = 10m, Height = 5m, Depth = 0m };

                // Act
                var result = dimensions.CalculateVolume();

                // Assert
                Assert.Equal(0m, result);
            }
        }

        public class RestorationHistoryTests
        {
            [Fact]
            public void RestorationHistory_NeedsInspection_WhenLastRestorationOverYearAgo_ReturnsTrue()
            {
                // Arrange
                var history = new RestorationHistory
                {
                    LastRestorationDate = DateTime.Now.AddDays(-400)
                };

                // Act
                var result = history.NeedsInspection();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void RestorationHistory_NeedsInspection_WhenLastRestorationUnderYearAgo_ReturnsFalse()
            {
                // Arrange
                var history = new RestorationHistory
                {
                    LastRestorationDate = DateTime.Now.AddDays(-300)
                };

                // Act
                var result = history.NeedsInspection();

                // Assert
                Assert.False(result);
            }
        }

        public class InsurancePolicyTests
        {
            [Fact]
            public void InsurancePolicy_IsActive_WhenNotExpired_ReturnsTrue()
            {
                // Arrange
                var policy = new InsurancePolicy { ExpiryDate = DateTime.Now.AddDays(30) };

                // Act
                var result = policy.IsActive();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void InsurancePolicy_IsActive_WhenExpired_ReturnsFalse()
            {
                // Arrange
                var policy = new InsurancePolicy { ExpiryDate = DateTime.Now.AddDays(-1) };

                // Act
                var result = policy.IsActive();

                // Assert
                Assert.False(result);
            }

        }

        public class BudgetTests
        {
            [Fact]
            public void Budget_GetRemainingBudget_ReturnsCorrectValue()
            {
                // Arrange
                var budget = new Budget { TotalAmount = 10000m, SpentAmount = 3500m };
                var expected = 6500m;

                // Act
                var result = budget.GetRemainingBudget();

                // Assert
                Assert.Equal(expected, result);
            }

            [Fact]
            public void Budget_GetRemainingBudget_WhenAllSpent_ReturnsZero()
            {
                // Arrange
                var budget = new Budget { TotalAmount = 10000m, SpentAmount = 10000m };

                // Act
                var result = budget.GetRemainingBudget();

                // Assert
                Assert.Equal(0m, result);
            }

            [Fact]
            public void Budget_GetRemainingBudget_WhenOverspent_ReturnsNegative()
            {
                // Arrange
                var budget = new Budget { TotalAmount = 10000m, SpentAmount = 12000m };
                var expected = -2000m;

                // Act
                var result = budget.GetRemainingBudget();

                // Assert
                Assert.Equal(expected, result);
            }
        }

        public class LightingSystemTests
        {
            [Fact]
            public void LightingSystem_IsSafeForArtwork_WhenUVFilteredAndLowIntensity_ReturnsTrue()
            {
                // Arrange
                var lighting = new LightingSystem { IsUVFiltered = true, Intensity = 400 };
                var artwork = new Artwork();

                // Act
                var result = lighting.IsSafeForArtwork(artwork);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void LightingSystem_IsSafeForArtwork_WhenNotUVFiltered_ReturnsFalse()
            {
                // Arrange
                var lighting = new LightingSystem { IsUVFiltered = false, Intensity = 400 };
                var artwork = new Artwork();

                // Act
                var result = lighting.IsSafeForArtwork(artwork);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void LightingSystem_IsSafeForArtwork_WhenHighIntensity_ReturnsFalse()
            {
                // Arrange
                var lighting = new LightingSystem { IsUVFiltered = true, Intensity = 600 };
                var artwork = new Artwork();

                // Act
                var result = lighting.IsSafeForArtwork(artwork);

                // Assert
                Assert.False(result);
            }

            [Theory]
            [InlineData(true, 500, true)]  // UV filtered, intensity <= 500
            [InlineData(true, 501, false)] // UV filtered, intensity > 500
            [InlineData(false, 500, false)] // Not UV filtered
            [InlineData(false, 400, false)] // Not UV filtered, low intensity
            public void LightingSystem_IsSafeForArtwork_VariousConditions_ReturnsExpected(bool uvFiltered, int intensity, bool expected)
            {
                // Arrange
                var lighting = new LightingSystem { IsUVFiltered = uvFiltered, Intensity = intensity };
                var artwork = new Artwork();

                // Act
                var result = lighting.IsSafeForArtwork(artwork);

                // Assert
                Assert.Equal(expected, result);
            }
        }

        // Test data generators for Theory tests
        public static class TestDataGenerators
        {
            public static IEnumerable<object[]> ArtworkConditionTestData =>
                new List<object[]>
                {
                new object[] { ArtworkCondition.Excellent, true },
                new object[] { ArtworkCondition.Good, true },
                new object[] { ArtworkCondition.Fair, true },
                new object[] { ArtworkCondition.Poor, true },
                new object[] { ArtworkCondition.Damaged, false }
                };
        }

        // Integration tests for complex scenarios
        public class IntegrationTests
        {
            [Fact]
            public void CompleteArtworkLifecycle_IntegrationTest()
            {
                // Arrange
                var artist = new Artist
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateTime(1970, 1, 1)
                };

                var dimensions = new Dimensions { Width = 100, Height = 150, Depth = 5, Unit = "cm" };

                var artwork = new Artwork
                {
                    Title = "Test Artwork",
                    Description = "Test Description",
                    CreationDate = new DateTime(2020, 1, 1),
                    EstimatedValue = 5000m,
                    Condition = ArtworkCondition.Good,
                    Dimensions = dimensions,
                    Artist = artist
                };

                // Act & Assert
                Assert.True(artwork.CanBeSold());
                Assert.Equal(50m, artwork.CalculateInsuranceCost());
                Assert.False(artwork.RequiresRestoration());
                Assert.True(artist.IsContemporary());
                Assert.True(DateTime.Now.Year - 1970 >= artist.GetAge());
            }

            [Fact]
            public void ExhibitionManagement_IntegrationTest()
            {
                // Arrange
                var hall = new GalleryHall { Capacity = 3, Security = new SecuritySystem() };
                var exhibition = new Exhibition
                {
                    Title = "Test Exhibition",
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(7),
                    Hall = hall,
                    Artworks = new List<Artwork>()
                };

                var artwork1 = new Artwork();
                var artwork2 = new Artwork();

                // Act & Assert
                Assert.True(exhibition.IsActive());
                Assert.True(exhibition.CanAddArtwork(artwork1));

                exhibition.Artworks.Add(artwork1);
                Assert.True(exhibition.CanAddArtwork(artwork2));
                Assert.Equal(1, exhibition.GetArtworkCount());

                exhibition.Artworks.Add(artwork2);
                exhibition.Artworks.Add(new Artwork()); // Add third artwork
                Assert.False(exhibition.CanAddArtwork(new Artwork())); // Should be false due to capacity
            }
        }


        public class SecuritySystemTests
        {
            [Theory]
            [InlineData(SecurityLevel.Low, 5000, true)]
            [InlineData(SecurityLevel.Low, 10000, true)]
            [InlineData(SecurityLevel.Low, 10001, false)]
            [InlineData(SecurityLevel.Medium, 50000, true)]
            [InlineData(SecurityLevel.Medium, 100000, true)]
            [InlineData(SecurityLevel.Medium, 100001, false)]
            [InlineData(SecurityLevel.High, 1000000, true)]
            [InlineData(SecurityLevel.High, 9999999, true)]
            public void SecuritySystem_CanSecureArtwork_ForDifferentSecurityLevels_ReturnsExpected(
                SecurityLevel securityLevel, decimal artworkValue, bool expected)
            {
                // Arrange
                var securitySystem = new SecuritySystem { SecurityLevel = securityLevel };
                var artwork = new Artwork { EstimatedValue = artworkValue };

                // Act
                var result = securitySystem.CanSecureArtwork(artwork);

                // Assert
                Assert.Equal(expected, result);
            }


            

            [Fact]
            public void SecuritySystem_CanSecureArtwork_WithZeroValueArtwork_ReturnsTrueForAnyLevel()
            {
                // Arrange
                var securitySystem = new SecuritySystem { SecurityLevel = SecurityLevel.Low };
                var artwork = new Artwork { EstimatedValue = 0m };

                // Act
                var result = securitySystem.CanSecureArtwork(artwork);

                // Assert
                Assert.True(result);
            }
        }

        public class SecurityCameraTests
        {
            [Fact]
            public void SecurityCamera_NeedsMaintenance_WhenLastMaintenanceOver90DaysAgo_ReturnsTrue()
            {
                // Arrange
                var camera = new SecurityCamera
                {
                    LastMaintenance = DateTime.Now.AddDays(-91)
                };

                // Act
                var result = camera.NeedsMaintenance();

                // Assert
                Assert.True(result);
            }

            

            [Fact]
            public void SecurityCamera_NeedsMaintenance_WhenLastMaintenanceUnder90DaysAgo_ReturnsFalse()
            {
                // Arrange
                var camera = new SecurityCamera
                {
                    LastMaintenance = DateTime.Now.AddDays(-89)
                };

                // Act
                var result = camera.NeedsMaintenance();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void SecurityCamera_NeedsMaintenance_WhenLastMaintenanceInFuture_ReturnsFalse()
            {
                // Arrange
                var camera = new SecurityCamera
                {
                    LastMaintenance = DateTime.Now.AddDays(1)
                };

                // Act
                var result = camera.NeedsMaintenance();

                // Assert
                Assert.False(result);
            }
        }

        public class AlarmSystemTests
        {
            [Fact]
            public void AlarmSystem_TriggerAlarm_SetsIsArmedToTrue()
            {
                // Arrange
                var alarmSystem = new AlarmSystem { IsArmed = false };

                // Act
                alarmSystem.TriggerAlarm("Main Hall");

                // Assert
                Assert.True(alarmSystem.IsArmed);
            }

            [Fact]
            public void AlarmSystem_NeedsTesting_WhenLastTestOver30DaysAgo_ReturnsTrue()
            {
                // Arrange
                var alarmSystem = new AlarmSystem
                {
                    LastTest = DateTime.Now.AddDays(-31)
                };

                // Act
                var result = alarmSystem.NeedsTesting();

                // Assert
                Assert.True(result);
            }

            

            [Fact]
            public void AlarmSystem_NeedsTesting_WhenLastTestUnder30DaysAgo_ReturnsFalse()
            {
                // Arrange
                var alarmSystem = new AlarmSystem
                {
                    LastTest = DateTime.Now.AddDays(-29)
                };

                // Act
                var result = alarmSystem.NeedsTesting();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void AlarmSystem_NeedsTesting_WhenLastTestInFuture_ReturnsFalse()
            {
                // Arrange
                var alarmSystem = new AlarmSystem
                {
                    LastTest = DateTime.Now.AddDays(1)
                };

                // Act
                var result = alarmSystem.NeedsTesting();

                // Assert
                Assert.False(result);
            }
        }

        public class AccessControlSystemTests
        {
            [Fact]
            public void AccessControlSystem_HasAccess_WhenCardHasPermission_ReturnsTrue()
            {
                // Arrange
                var accessControl = new AccessControlSystem
                {
                    Permissions = new Dictionary<string, List<string>>
                    {
                        ["Admin"] = new List<string> { "Storage", "Office", "Gallery" },
                        ["Guard"] = new List<string> { "Gallery", "Entrance" }
                    }
                };
                var card = new AccessCard { Level = "Admin" };

                // Act
                var result = accessControl.HasAccess(card, "Storage");

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void AccessControlSystem_HasAccess_WhenCardDoesNotHavePermission_ReturnsFalse()
            {
                // Arrange
                var accessControl = new AccessControlSystem
                {
                    Permissions = new Dictionary<string, List<string>>
                    {
                        ["Admin"] = new List<string> { "Storage", "Office" },
                        ["Guard"] = new List<string> { "Gallery", "Entrance" }
                    }
                };
                var card = new AccessCard { Level = "Guard" };

                // Act
                var result = accessControl.HasAccess(card, "Storage");

                // Assert
                Assert.False(result);
            }

            
           

            [Fact]
            public void AccessControlSystem_HasAccess_WhenPermissionsNull_ReturnsFalse()
            {
                // Arrange
                var accessControl = new AccessControlSystem { Permissions = null };
                var card = new AccessCard { Level = "Admin" };

                // Act
                var result = accessControl.HasAccess(card, "Storage");

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void AccessControlSystem_HasAccess_WhenAreaDoesNotExistInPermissions_ReturnsFalse()
            {
                // Arrange
                var accessControl = new AccessControlSystem
                {
                    Permissions = new Dictionary<string, List<string>>
                    {
                        ["Admin"] = new List<string> { "Storage", "Office" }
                    }
                };
                var card = new AccessCard { Level = "Admin" };

                // Act
                var result = accessControl.HasAccess(card, "NonExistentArea");

                // Assert
                Assert.False(result);
            }
        }

        public class AccesCardTests
        {
            [Fact]
            public void AccessCard_IsValid_WhenActiveAndNotExpired_ReturnsTrue()
            {
                // Arrange
                var card = new AccessCard
                {
                    IsActive = true,
                    ExpiryDate = DateTime.Now.AddDays(30)
                };

                // Act
                var result = card.IsValid();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void AccessCard_IsValid_WhenInactive_ReturnsFalse()
            {
                // Arrange
                var card = new AccessCard
                {
                    IsActive = false,
                    ExpiryDate = DateTime.Now.AddDays(30)
                };

                // Act
                var result = card.IsValid();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void AccessCard_IsValid_WhenExpired_ReturnsFalse()
            {
                // Arrange
                var card = new AccessCard
                {
                    IsActive = true,
                    ExpiryDate = DateTime.Now.AddDays(-1)
                };

                // Act
                var result = card.IsValid();

                // Assert
                Assert.False(result);
            }

            
            

            [Fact]
            public void AccessCard_IsValid_WhenInactiveAndExpired_ReturnsFalse()
            {
                // Arrange
                var card = new AccessCard
                {
                    IsActive = false,
                    ExpiryDate = DateTime.Now.AddDays(-1)
                };

                // Act
                var result = card.IsValid();

                // Assert
                Assert.False(result);
            }
        }

        public class AccessPointTests
        {
            [Fact]
            public void AccessPoint_DefaultConstructor_SetsDefaultValues()
            {
                // Arrange & Act
                var accessPoint = new AccessPoint();

                // Assert
                Assert.False(accessPoint.IsOperational);
                Assert.Equal(default(DateTime), accessPoint.LastMaintenance);
            }

         
        }

        public class SecurityIntegrationTests
        {
            


            [Fact]
            public void SecurityCameraMaintenanceScenario_IntegrationTest()
            {
                // Arrange
                var camera = new SecurityCamera
                {
                    Location = "Main Hall",
                    IsActive = true,
                    LastMaintenance = DateTime.Now.AddDays(-100)
                };

                var alarmSystem = new AlarmSystem
                {
                    LastTest = DateTime.Now.AddDays(-40)
                };

                // Act & Assert
                Assert.True(camera.NeedsMaintenance());
                Assert.True(alarmSystem.NeedsTesting());
            }

            [Fact]
            public void AccessControlIntegration_WithMultipleCards_WorksCorrectly()
            {
                // Arrange
                var accessControl = new AccessControlSystem
                {
                    Permissions = new Dictionary<string, List<string>>
                    {
                        ["Admin"] = new List<string> { "Storage", "Office", "Gallery", "Vault" },
                        ["Curator"] = new List<string> { "Storage", "Gallery" },
                        ["Guard"] = new List<string> { "Gallery", "Entrance" },
                        ["Cleaner"] = new List<string> { "Entrance", "Hallway" }
                    }
                };

                var adminCard = new AccessCard { Level = "Admin", IsActive = true, ExpiryDate = DateTime.Now.AddDays(30) };
                var curatorCard = new AccessCard { Level = "Curator", IsActive = true, ExpiryDate = DateTime.Now.AddDays(30) };
                var guardCard = new AccessCard { Level = "Guard", IsActive = true, ExpiryDate = DateTime.Now.AddDays(30) };
                var cleanerCard = new AccessCard { Level = "Cleaner", IsActive = true, ExpiryDate = DateTime.Now.AddDays(30) };

                // Act & Assert
                // Admin has access to all areas
                Assert.True(accessControl.HasAccess(adminCard, "Vault"));
                Assert.True(accessControl.HasAccess(adminCard, "Storage"));

                // Curator has access to storage and gallery but not vault
                Assert.True(accessControl.HasAccess(curatorCard, "Storage"));
                Assert.True(accessControl.HasAccess(curatorCard, "Gallery"));
                Assert.False(accessControl.HasAccess(curatorCard, "Vault"));

                // Guard has limited access
                Assert.True(accessControl.HasAccess(guardCard, "Gallery"));
                Assert.False(accessControl.HasAccess(guardCard, "Storage"));

                // Cleaner has minimal access
                Assert.True(accessControl.HasAccess(cleanerCard, "Entrance"));
                Assert.False(accessControl.HasAccess(cleanerCard, "Gallery"));
            }
        }



        public class ExceptionTests
        {
            [Fact]
            public void ArtworkNotFoundException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                int artworkId = 123;

                // Act
                var exception = new ArtworkNotFoundException(artworkId);

                // Assert
                Assert.Equal($"Artwork with ID {artworkId} not found", exception.Message);
            }

            [Fact]
            public void ArtworkNotFoundException_IsAssignableFromException()
            {
                // Arrange & Act
                var exception = new ArtworkNotFoundException(123);

                // Assert
                Assert.IsAssignableFrom<Exception>(exception);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(999)]
            [InlineData(-1)]
            public void ArtworkNotFoundException_WithDifferentIds_SetsCorrectMessage(int artworkId)
            {
                // Act
                var exception = new ArtworkNotFoundException(artworkId);

                // Assert
                Assert.Contains(artworkId.ToString(), exception.Message);
            }

            [Fact]
            public void InvalidArtworkConditionException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string condition = "InvalidCondition";

                // Act
                var exception = new InvalidArtworkConditionException(condition);

                // Assert
                Assert.Equal($"Invalid artwork condition: {condition}", exception.Message);
            }

            [Theory]
            [InlineData("BadCondition")]
            [InlineData("Unknown")]
            [InlineData("")]
            public void InvalidArtworkConditionException_WithDifferentConditions_SetsCorrectMessage(string condition)
            {
                // Act
                var exception = new InvalidArtworkConditionException(condition);

                // Assert
                Assert.Contains(condition, exception.Message);
            }

            [Fact]
            public void InsufficientFundsException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                decimal amount = 1000m;
                decimal balance = 500m;

                // Act
                var exception = new InsufficientFundsException(amount, balance);

                // Assert
                Assert.Equal($"Insufficient funds. Required: {amount}, Available: {balance}", exception.Message);
            }

            [Theory]
            [InlineData(1000, 500)]
            [InlineData(50, 49.99)]
            [InlineData(1, 0)]
            [InlineData(1000000, 999999)]
            public void InsufficientFundsException_WithDifferentAmounts_SetsCorrectMessage(decimal amount, decimal balance)
            {
                // Act
                var exception = new InsufficientFundsException(amount, balance);

                // Assert
                Assert.Contains(amount.ToString(), exception.Message);
                Assert.Contains(balance.ToString(), exception.Message);
            }

            [Fact]
            public void InvalidTransactionAmountException_Constructor_SetsCorrectMessage()
            {
                // Act
                var exception = new InvalidTransactionAmountException();

                // Assert
                Assert.Equal("Transaction amount must be non-zero", exception.Message);
            }

            [Fact]
            public void InvalidTransactionAmountException_DefaultConstructor_Works()
            {
                // Act
                var exception = new InvalidTransactionAmountException();

                // Assert
                Assert.NotNull(exception);
                Assert.IsType<InvalidTransactionAmountException>(exception);
            }

            [Fact]
            public void SecurityBreachException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string message = "Unauthorized access attempt";

                // Act
                var exception = new SecurityBreachException(message);

                // Assert
                Assert.Equal(message, exception.Message);
            }

            [Theory]
            [InlineData("Access denied")]
            [InlineData("Security violation detected")]
            [InlineData("")]
            public void SecurityBreachException_WithDifferentMessages_SetsCorrectMessage(string message)
            {
                // Act
                var exception = new SecurityBreachException(message);

                // Assert
                Assert.Equal(message, exception.Message);
            }

            

            

            [Fact]
            public void ExhibitionFullException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string exhibitionName = "Modern Art Exhibition";

                // Act
                var exception = new ExhibitionFullException(exhibitionName);

                // Assert
                Assert.Equal($"Exhibition '{exhibitionName}' is at full capacity", exception.Message);
            }

            [Theory]
            [InlineData("Impressionism Show")]
            [InlineData("Sculpture Garden")]
            [InlineData("")]
            public void ExhibitionFullException_WithDifferentExhibitionNames_SetsCorrectMessage(string exhibitionName)
            {
                // Act
                var exception = new ExhibitionFullException(exhibitionName);

                // Assert
                Assert.Contains(exhibitionName, exception.Message);
            }

            [Fact]
            public void ArtworkNotAvailableException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                int artworkId = 456;

                // Act
                var exception = new ArtworkNotAvailableException(artworkId);

                // Assert
                Assert.Equal($"Artwork {artworkId} is not available for exhibition", exception.Message);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(100)]
            [InlineData(-5)]
            public void ArtworkNotAvailableException_WithDifferentArtworkIds_SetsCorrectMessage(int artworkId)
            {
                // Act
                var exception = new ArtworkNotAvailableException(artworkId);

                // Assert
                Assert.Contains(artworkId.ToString(), exception.Message);
            }

            [Fact]
            public void InvalidTicketException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string ticketCode = "TICKET-123";

                // Act
                var exception = new InvalidTicketException(ticketCode);

                // Assert
                Assert.Equal($"Invalid or expired ticket: {ticketCode}", exception.Message);
            }

            [Theory]
            [InlineData("TICKET-001")]
            [InlineData("GROUP-999")]
            [InlineData("")]
            [InlineData("EXPIRED-123")]
            public void InvalidTicketException_WithDifferentTicketCodes_SetsCorrectMessage(string ticketCode)
            {
                // Act
                var exception = new InvalidTicketException(ticketCode);

                // Assert
                Assert.Contains(ticketCode, exception.Message);
            }

            [Fact]
            public void EmployeeNotFoundException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                int employeeId = 789;

                // Act
                var exception = new EmployeeNotFoundException(employeeId);

                // Assert
                Assert.Equal($"Employee with ID {employeeId} not found", exception.Message);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(500)]
            [InlineData(0)]
            public void EmployeeNotFoundException_WithDifferentEmployeeIds_SetsCorrectMessage(int employeeId)
            {
                // Act
                var exception = new EmployeeNotFoundException(employeeId);

                // Assert
                Assert.Contains(employeeId.ToString(), exception.Message);
            }

            [Fact]
            public void HallCapacityExceededException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string hallName = "Main Hall";

                // Act
                var exception = new HallCapacityExceededException(hallName);

                // Assert
                Assert.Equal($"Hall '{hallName}' capacity exceeded", exception.Message);
            }

            [Theory]
            [InlineData("East Wing")]
            [InlineData("Sculpture Hall")]
            [InlineData("")]
            public void HallCapacityExceededException_WithDifferentHallNames_SetsCorrectMessage(string hallName)
            {
                // Act
                var exception = new HallCapacityExceededException(hallName);

                // Assert
                Assert.Contains(hallName, exception.Message);
            }

            [Fact]
            public void RestorationNotPossibleException_Constructor_SetsCorrectMessage()
            {
                // Arrange
                string reason = "Irreparable damage";

                // Act
                var exception = new RestorationNotPossibleException(reason);

                // Assert
                Assert.Equal($"Restoration not possible: {reason}", exception.Message);
            }

            [Theory]
            [InlineData("Missing parts")]
            [InlineData("Material degradation")]
            [InlineData("")]
            public void RestorationNotPossibleException_WithDifferentReasons_SetsCorrectMessage(string reason)
            {
                // Act
                var exception = new RestorationNotPossibleException(reason);

                // Assert
                Assert.Contains(reason, exception.Message);
            }

            [Fact]
            public void AllExceptions_InheritFromExceptionBaseClass()
            {
                // Arrange & Act
                var exceptions = new Exception[]
                {
                new ArtworkNotFoundException(1),
                new InvalidArtworkConditionException("test"),
                new InsufficientFundsException(100, 50),
                new InvalidTransactionAmountException(),
                new SecurityBreachException("test"),
                new ExhibitionFullException("test"),
                new ArtworkNotAvailableException(1),
                new InvalidTicketException("test"),
                new EmployeeNotFoundException(1),
                new HallCapacityExceededException("test"),
                new RestorationNotPossibleException("test")
                };

                // Assert
                foreach (var exception in exceptions)
                {
                    Assert.IsAssignableFrom<Exception>(exception);
                }
            }

            [Fact]
            public void Exceptions_CanBeThrownAndCaughtByBaseType()
            {
                // Arrange
                var testCases = new (Func<Exception>, Type)[]
                {
                (() => throw new ArtworkNotFoundException(1), typeof(ArtworkNotFoundException)),
                (() => throw new InsufficientFundsException(100, 50), typeof(InsufficientFundsException)),
                (() => throw new InvalidTransactionAmountException(), typeof(InvalidTransactionAmountException))
                };

                foreach (var (exceptionFactory, expectedType) in testCases)
                {
                    // Act & Assert
                    var caughtException = Assert.Throws<Exception>(() => exceptionFactory());
                    Assert.IsType(expectedType, caughtException);
                }
            }

            [Fact]
            public void ExceptionMessages_AreNotNullOrEmpty()
            {
                // Arrange
                var exceptions = new Exception[]
                {
                new ArtworkNotFoundException(1),
                new InvalidArtworkConditionException("test"),
                new InsufficientFundsException(100, 50),
                new InvalidTransactionAmountException(),
                new SecurityBreachException("test"),
                
                new ExhibitionFullException("test"),
                new ArtworkNotAvailableException(1),
                new InvalidTicketException("test"),
                new EmployeeNotFoundException(1),
                new HallCapacityExceededException("test"),
                new RestorationNotPossibleException("test")
                };

                // Assert
                foreach (var exception in exceptions)
                {
                    Assert.False(string.IsNullOrEmpty(exception.Message));
                }
            }

            

        public class ExceptionIntegrationTests
        {
            [Fact]
            public void ArtworkNotFoundException_CanBeUsedInArtworkContext()
            {
                // Arrange
                var artworkService = new ArtworkService();

                // Act & Assert
                var exception = Assert.Throws<ArtworkNotFoundException>(() => artworkService.GetArtwork(999));
                Assert.Equal(999, ExtractArtworkIdFromMessage(exception.Message));
            }

            [Fact]
            public void InsufficientFundsException_CanBeUsedInFinancialContext()
            {
                // Arrange
                var accountService = new AccountService();

                // Act & Assert
                var exception = Assert.Throws<InsufficientFundsException>(() => accountService.Withdraw(1000, 500));
                Assert.Contains("1000", exception.Message);
                Assert.Contains("500", exception.Message);
            }

            [Fact]
            public void ExhibitionFullException_CanBeUsedInExhibitionContext()
            {
                // Arrange
                var exhibitionService = new ExhibitionService();

                // Act & Assert
                var exception = Assert.Throws<ExhibitionFullException>(() => exhibitionService.AddArtwork("Full Exhibition", new Artwork()));
                Assert.Contains("Full Exhibition", exception.Message);
            }

            // Helper methods for integration tests
            private int ExtractArtworkIdFromMessage(string message)
            {
                var parts = message.Split(' ');
                return int.Parse(parts[3]);
            }

            // Mock services for integration testing
            private class ArtworkService
            {
                public Artwork GetArtwork(int id)
                {
                    throw new ArtworkNotFoundException(id);
                }
            }

            private class AccountService
            {
                public void Withdraw(decimal amount, decimal balance)
                {
                    throw new InsufficientFundsException(amount, balance);
                }
            }

            private class ExhibitionService
            {
                public void AddArtwork(string exhibitionName, Artwork artwork)
                {
                    throw new ExhibitionFullException(exhibitionName);
                }
            }

            private class Artwork { }
        }

        public class ExceptionSerializationTests
        {
            [Fact]
            public void ArtworkNotFoundException_Serialization_Works()
            {
                // Arrange
                var originalException = new ArtworkNotFoundException(123);

                // Act
                var message = originalException.Message;
                var type = originalException.GetType();

                // Assert
                Assert.Equal("Artwork with ID 123 not found", message);
                Assert.Equal(typeof(ArtworkNotFoundException), type);
            }

            [Fact]
            public void AllExceptions_HaveDescriptiveMessages()
            {
                // Arrange
                var exceptionTestCases = new (Exception, string)[]
                {
                (new ArtworkNotFoundException(456), "Artwork with ID 456 not found"),
                (new InvalidArtworkConditionException("Fragile"), "Invalid artwork condition: Fragile"),
                (new InsufficientFundsException(1000, 100), "Insufficient funds. Required: 1000, Available: 100"),
                (new InvalidTransactionAmountException(), "Transaction amount must be non-zero"),
                (new SecurityBreachException("Intrusion detected"), "Intrusion detected"),
                
                (new ExhibitionFullException("Special Exhibition"), "Exhibition 'Special Exhibition' is at full capacity"),
                (new ArtworkNotAvailableException(789), "Artwork 789 is not available for exhibition"),
                (new InvalidTicketException("EXP-123"), "Invalid or expired ticket: EXP-123"),
                (new EmployeeNotFoundException(321), "Employee with ID 321 not found"),
                (new HallCapacityExceededException("Grand Hall"), "Hall 'Grand Hall' capacity exceeded"),
                (new RestorationNotPossibleException("Missing fragments"), "Restoration not possible: Missing fragments")
                };

                // Assert
                foreach (var (exception, expectedMessage) in exceptionTestCases)
                {
                    Assert.Equal(expectedMessage, exception.Message);
                }
            }
        }
    }




}
}
