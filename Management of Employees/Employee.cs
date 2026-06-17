using System;

namespace Management_of_Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public double MonthlySalary { get; set; }

        // Зарплата за год вычисляется автоматически
        public double YearlySalary => MonthlySalary * 12;

        public int VacationDays { get; set; }

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