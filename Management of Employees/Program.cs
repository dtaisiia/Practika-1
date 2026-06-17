using System;

namespace Management_of_Employees
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthManager auth = new AuthManager();
            EmployeeManager empManager = new EmployeeManager();

            Console.WriteLine("=== СИСТЕМА УПРАВЛІННЯ ПЕРСОНАЛОМ ===");
            Console.Write("Введіть Email: ");
            string email = Console.ReadLine();

            Console.Write("Введіть пароль: ");
            string password = Console.ReadLine();

            User currentUser = auth.Login(email, password);

            if (currentUser != null)
            {
                Console.Clear();
                Console.WriteLine($"Успішний вхід! Вітаємо, {currentUser.Name}.");
                ShowMainMenu(currentUser, empManager);
            }
            else
            {
                Console.WriteLine("Помилка: Невірний email або пароль.");
                Console.ReadLine();
            }
        }

        static void ShowMainMenu(User user, EmployeeManager empManager)
        {
            List<Employee> employees = empManager.LoadEmployees();
            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("\n=== ГОЛОВНЕ МЕНЮ ===");
                Console.WriteLine("1. Переглянути список працівників");
                Console.WriteLine("2. Пошук працівника за ID");
                Console.WriteLine("3. Фільтрація списку за відділом");
                Console.WriteLine("4. Сортування списку за алфавітом (Бульбашкою)");

                // Пункти меню тільки для адміністратора
                if (user.Role == "Admin")
                {
                    Console.WriteLine("5. Додати працівника");
                    Console.WriteLine("6. Редагувати працівника");
                    Console.WriteLine("7. Видалити працівника");
                }

                Console.WriteLine("0. Вихід з програми");
                Console.Write("Оберіть дію: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ViewEmployees(employees);
                        break;
                    case "2":
                        SearchEmployeeById(employees);
                        break;
                    case "3":
                        FilterByDepartment(employees);
                        break;
                    case "4":
                        SortEmployeesByName(employees);
                        break;
                    case "5":
                        AddEmployee(user, employees, empManager);
                        break;
                    case "6":
                        EditEmployee(user, employees, empManager);
                        break;
                    case "7":
                        DeleteEmployee(user, employees, empManager);
                        break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Завершення роботи. Дані збережено!");
                        break;
                    default:
                        Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                        break;
                }
            }
        }

        // ==========================================
        // ОКРЕМІ МЕТОДИ ДЛЯ КОЖНОЇ ДІЇ З МЕНЮ
        // ==========================================

        static void ViewEmployees(List<Employee> employees)
        {
            Console.WriteLine("--- СПИСОК ПРАЦІВНИКІВ ---");
            if (employees.Count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            foreach (Employee emp in employees)
            {
                Console.WriteLine($"[{emp.Id}] {emp.FullName} | Відділ: {emp.Department} | Посада: {emp.Position} | Оклад: {emp.MonthlySalary} грн");
            }
        }

        static void AddEmployee(User user, List<Employee> employees, EmployeeManager empManager)
        {
            if (user.Role != "Admin")
            {
                Console.WriteLine("У вас немає прав для цієї дії.");
                return;
            }

            Console.Write("Введіть ПІБ: ");
            string name = Console.ReadLine();
            Console.Write("Введіть посаду: ");
            string position = Console.ReadLine();
            Console.Write("Введіть відділ: ");
            string department = Console.ReadLine();
            Console.Write("Введіть оклад (за місяць): ");
            double salary = double.Parse(Console.ReadLine());
            Console.Write("Дні відпустки: ");
            int vacation = int.Parse(Console.ReadLine());

            int newId = empManager.GetNextId(employees);
            Employee newEmp = new Employee(newId, name, position, department, salary, vacation);

            employees.Add(newEmp);
            empManager.SaveEmployees(employees);
            Console.WriteLine("Працівника успішно додано!");
        }

        static void DeleteEmployee(User user, List<Employee> employees, EmployeeManager empManager)
        {
            if (user.Role != "Admin")
            {
                Console.WriteLine("У вас немає прав для цієї дії.");
                return;
            }

            Console.Write("Введіть ID працівника для видалення: ");
            int idToRemove = int.Parse(Console.ReadLine());

            Employee empToRemove = null;
            foreach (Employee emp in employees)
            {
                if (emp.Id == idToRemove)
                {
                    empToRemove = emp;
                    break;
                }
            }

            if (empToRemove != null)
            {
                employees.Remove(empToRemove);
                empManager.SaveEmployees(employees);
                Console.WriteLine("Працівника успішно видалено!");
            }
            else
            {
                Console.WriteLine("Працівника з таким ID не знайдено.");
            }
        }

        static void EditEmployee(User user, List<Employee> employees, EmployeeManager empManager)
        {
            if (user.Role != "Admin")
            {
                Console.WriteLine("У вас немає прав для цієї дії.");
                return;
            }

            Console.Write("Введіть ID працівника для редагування: ");
            int idToEdit = int.Parse(Console.ReadLine());

            Employee empToEdit = null;
            foreach (Employee emp in employees)
            {
                if (emp.Id == idToEdit)
                {
                    empToEdit = emp;
                    break;
                }
            }

            if (empToEdit != null)
            {
                Console.Write($"Нова посада (поточна: {empToEdit.Position}): ");
                empToEdit.Position = Console.ReadLine();

                Console.Write($"Новий оклад (поточний: {empToEdit.MonthlySalary}): ");
                empToEdit.MonthlySalary = double.Parse(Console.ReadLine());

                empManager.SaveEmployees(employees);
                Console.WriteLine("Дані працівника успішно оновлено!");
            }
            else
            {
                Console.WriteLine("Працівника з таким ID не знайдено.");
            }
        }

        static void SearchEmployeeById(List<Employee> employees)
        {
            Console.Write("Введіть ID для пошуку: ");
            int searchId = int.Parse(Console.ReadLine());

            bool found = false;
            foreach (Employee emp in employees)
            {
                if (emp.Id == searchId)
                {
                    Console.WriteLine($"Знайдено: {emp.FullName} | Відділ: {emp.Department} | Посада: {emp.Position}");
                    found = true;
                    break;
                }
            }

            if (!found) Console.WriteLine("Працівника не знайдено.");
        }

        static void FilterByDepartment(List<Employee> employees)
        {
            Console.Write("Введіть назву відділу: ");
            string searchDept = Console.ReadLine();

            int count = 0;
            Console.WriteLine($"\n--- Працівники відділу '{searchDept}' ---");
            foreach (Employee emp in employees)
            {
                // ToLower() дозволяє ігнорувати регістр при пошуку
                if (emp.Department.ToLower() == searchDept.ToLower())
                {
                    Console.WriteLine($"[{emp.Id}] {emp.FullName} - {emp.Position}");
                    count++;
                }
            }

            if (count == 0) Console.WriteLine("У цьому відділі немає працівників.");
        }

        static void SortEmployeesByName(List<Employee> employees)
        {
            // Класичне бульбашкове сортування (Bubble Sort)
            int n = employees.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Порівнюємо імена за алфавітом (String.Compare)
                    if (string.Compare(employees[j].FullName, employees[j + 1].FullName) > 0)
                    {
                        // Міняємо місцями
                        Employee temp = employees[j];
                        employees[j] = employees[j + 1];
                        employees[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Список успішно відсортовано за алфавітом!");
            ViewEmployees(employees); // Одразу показуємо результат
        }
    }
}