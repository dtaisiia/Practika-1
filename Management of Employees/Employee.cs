using System;

namespace Management_of_Employees
{
    /// <summary>
    /// Клас, що представляє сутність співробітника підприємства.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор співробітника.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отримує або задає повне ім'я (ПІБ) співробітника.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Отримує або задає посаду співробітника.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Отримує або задає назву структурного підрозділу (відділу).
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Отримує або задає місячний оклад співробітника.
        /// </summary>
        public double MonthlySalary { get; set; }

        /// <summary>
        /// Отримує річну заробітну плату співробітника, яка обчислюється автоматично.
        /// </summary>
        public double YearlySalary => MonthlySalary * 12;

        /// <summary>
        /// Отримує або задає кількість доступних днів відпустки.
        /// </summary>
        public int VacationDays { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу Employee із заданими параметрами.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор.</param>
        /// <param name="fullName">Повне ім'я.</param>
        /// <param name="position">Посада.</param>
        /// <param name="department">Відділ.</param>
        /// <param name="monthlySalary">Місячний оклад.</param>
        /// <param name="vacationDays">Дні відпустки.</param>
        public Employee(int id, string fullName, string position, string department, double monthlySalary, int vacationDays)
        {
            Id = id;
            FullName = fullName;
            Position = position;
            Department = department;
            MonthlySalary = monthlySalary;
            VacationDays = vacationDays;
        }
    }
}