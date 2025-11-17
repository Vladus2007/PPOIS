using Types;
using Exceptions;
using FinancicalSystem;
using SecuritySystems;
using VisitorManagment;
using PersonalManagment;
namespace FinancicalSystem
{
    public class FinancialAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string BankName { get; set; }
        public List<Transaction> Transactions { get; set; }
        public AccountType Type { get; set; }

        public void ProcessTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0) throw new InvalidTransactionAmountException();
            Balance += transaction.Amount;
            Transactions.Add(transaction);
        }

        public bool CanWithdraw(decimal amount) => Balance >= amount;
    }

    
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public FinancialAccount Account { get; set; }
        public string ReferenceNumber { get; set; }

        public bool IsValid() => Amount != 0 && Date <= DateTime.Now;
        public bool IsIncome() => Amount > 0;
    }

    
    public class PaymentMethod
    {
        public string Type { get; set; } 
        public string Details { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool IsValid() => IsActive && DateTime.Now <= ExpiryDate;
    }

    // Invoice.cs
    public class Invoice
    {
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Client { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public InvoiceStatus Status { get; set; }

        public bool IsOverdue() => DateTime.Now > DueDate && Status != InvoiceStatus.Paid;
        public decimal CalculateTotal() => Items.Sum(i => i.Price * i.Quantity);
    }

    // InvoiceItem.cs
    public class InvoiceItem
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }

        public decimal GetTotal() => Price * Quantity * (1 + TaxRate);
    }

    
    public class BudgetAllocation
    {
        public string Category { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public decimal GetRemaining() => AllocatedAmount - SpentAmount;
        public bool IsOverBudget() => SpentAmount > AllocatedAmount;
    }

    
    public class TaxRecord
    {
        public int Year { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal TaxPaid { get; set; }
        public DateTime FilingDate { get; set; }

        public decimal CalculateNetProfit() => Income - Expenses;
    }

    
    public class Donation
    {
        public string DonorName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Purpose { get; set; }
        public bool IsTaxDeductible { get; set; }
        public string RecognitionLevel { get; set; }
    }
    public abstract class FinancialDocument : FinancialEntity
    {
        public string DocumentNumber { get; set; }
        public FinancialStatus Status { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public decimal CalculateTotal() => Items.Sum(item => item.TotalPrice);
        public abstract void Validate();
    }


    public class Money : IValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative");

            Amount = amount;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add different currencies");
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public override bool Equals(object obj) =>
            obj is Money other && Amount == other.Amount && Currency == other.Currency;

        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    }


    public class Invoice : FinancialDocument
    {
        public Customer Customer { get; set; }
        public DateTime DueDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }

        public override bool IsValid() =>
            !string.IsNullOrEmpty(DocumentNumber) &&
            Amount > 0 &&
            DueDate > TransactionDate;

        public override void Validate()
        {
            if (!IsValid())
                throw new InvalidInvoiceException("Invoice is not valid");
        }

        public void MarkAsPaid()
        {
            IsPaid = true;
            Status = FinancialStatus.Completed;
            MarkAsModified();
        }
    }
}