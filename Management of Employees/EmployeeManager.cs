using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Management_of_Employees
{
    /// <summary>
    /// Клас для управління даними працівників. 
    /// Відповідає за зчитування, збереження та генерацію унікальних ідентифікаторів.
    /// </summary>
    public class EmployeeManager
    {
        private string _filePath = "employees.csv";

        /// <summary>
        /// Ініціалізує новий екземпляр менеджера. 
        /// Перевіряє наявність файлу бази даних і створює його із заголовками у форматі UTF-8.
        /// </summary>
        public EmployeeManager()
        {
            if (!File.Exists(_filePath))
            {
                using (StreamWriter writer = new StreamWriter(_filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Id,FullName,Position,Department,MonthlySalary,VacationDays");
                }
            }
        }

        /// <summary>
        /// Зчитує список працівників із файлу CSV.
        /// </summary>
        /// <returns>Повертає колекцію об'єктів працівників.</returns>
        public List<Employee> LoadEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (StreamReader reader = new StreamReader(_filePath, Encoding.UTF8))
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

        /// <summary>
        /// Зберігає актуальний список працівників у файл CSV, повністю перезаписуючи попередні дані.
        /// </summary>
        /// <param name="employees">Список працівників для збереження.</param>
        public void SaveEmployees(List<Employee> employees)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Id,FullName,Position,Department,MonthlySalary,VacationDays");
                foreach (Employee emp in employees)
                {
                    writer.WriteLine($"{emp.Id},{emp.FullName},{emp.Position},{emp.Department},{emp.MonthlySalary},{emp.VacationDays}");
                }
            }
        }

        /// <summary>
        /// Генерує наступний доступний унікальний ідентифікатор для нового запису.
        /// </summary>
        /// <param name="employees">Поточний список працівників.</param>
        /// <returns>Новий числовий ідентифікатор.</returns>
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