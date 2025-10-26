using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPOISSecondFirst
{
   public class StaffService<T> where T : Staff
{
    public void WorkSucces(T staff)
    {
        try
        {
            // Проверка входных параметров
            ValidateStaff(staff);
            
            // Основная логика
            staff.Balanse += staff.Salary;
        }
        catch (StaffServiceException)
        {
            throw; // Перебрасываем наши кастомные исключения
        }
        catch (Exception ex)
        {
            // Оборачиваем системные исключения в наши
            throw new StaffServiceException("Ошибка при выполнении операции", ex);
        }
    }

    private void ValidateStaff(T staff)
    {
        if (staff == null)
            throw new NullStaffException();
            
        if (staff.Salary <= 0)
            throw new InvalidSalaryException(staff.Salary);
            
        // Предположим, что у Staff есть свойство IsActive
        if (!staff.IsActive)
            throw new InactiveStaffException(staff);
    }

    // Дополнительный метод с более безопасной обработкой
    public bool TryWorkSucces(T staff)
    {
        try
        {
            WorkSucces(staff);
            return true;
        }
        catch (StaffServiceException)
        {
            return false;
        }
    }
}
    public class StaffServiceException : Exception
    {
    public StaffServiceException() { }
    public StaffServiceException(string message) : base(message) { }
    public StaffServiceException(string message, Exception inner) : base(message, inner) { }
    }
    // Исключение для null сотрудника
public class NullStaffException : StaffServiceException
{
    public NullStaffException() : base("Сотрудник не может быть null") { }
}

// Исключение для некорректной зарплаты
public class InvalidSalaryException : StaffServiceException
{
    public decimal InvalidSalary { get; }
    
    public InvalidSalaryException(decimal salary) 
        : base($"Некорректная зарплата: {salary}. Зарплата должна быть положительной.")
    {
        InvalidSalary = salary;
    }
}

// Исключение для операции с уволенным сотрудником
public class InactiveStaffException : StaffServiceException
{
    public Staff Staff { get; }
    
    public InactiveStaffException(Staff staff) 
        : base($"Операция невозможна: сотрудник {staff.Name} не активен")
    {
        Staff = staff;
    }
}
}
