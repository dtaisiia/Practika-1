using System;
using System.Collections.Generic;
using System.Text;

namespace Management_of_Employees
{
    public class EmployeeManager
    {
        private string _filePath = "employees.csv";

        public EmployeeManager()
        {
            if (!File.Exists(_filePath))
            {
                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine("Id,FullName,Position,Department,MonthlySalary,VacationDays");
                }
            }
        }

        public List<Employee> LoadEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string header = reader.ReadLine();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 6)
                    {
                        Employee emp = new Employee(
                            int.Parse(parts[0]),
                            parts[1],
                            parts[2],
                            parts[3],
                            double.Parse(parts[4]),
                            int.Parse(parts[5])
                        );
                        employees.Add(emp);
                    }
                }
            }
            return employees;
        }

        public void SaveEmployees(List<Employee> employees)
        {
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.WriteLine("Id,FullName,Position,Department,MonthlySalary,VacationDays");
                foreach (Employee emp in employees)
                {
                    writer.WriteLine($"{emp.Id},{emp.FullName},{emp.Position},{emp.Department},{emp.MonthlySalary},{emp.VacationDays}");
                }
            }
        }

        // Метод генерації нового ID (класичним циклом)
        public int GetNextId(List<Employee> employees)
        {
            int maxId = 0;
            foreach (Employee emp in employees)
            {
                if (emp.Id > maxId)
                {
                    maxId = emp.Id;
                }
            }
            return maxId + 1;
        }
    }
}
