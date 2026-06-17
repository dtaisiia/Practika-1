using System;
using System.Collections.Generic;
using System.Text;

namespace Management_of_Employees
{
    /// <summary>
    /// Головний клас програми, що забезпечує роботу текстового меню та координацію модулів.
    /// </summary>
    class Program
    {
        static string CurrentRole = "";
        static string CurrentUser = "";
        static AuthManager auth = new AuthManager();
        static EmployeeManager empManager = new EmployeeManager();

        /// <summary>
        /// Головна точка входу в консольний додаток.
        /// </summary>
        /// <param name="args">Аргументи командного рядка.</param>
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.Unicode;

            while (true)
            {
                if (CurrentRole == "") AuthMenu();
                else MainMenu();
            }
        }

        /// <summary>
        /// Відображає інтерфейс авторизації користувача.
        /// </summary>
        static void AuthMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== СИСТЕМА УПРАВЛІННЯ ПЕРСОНАЛОМ ===");
            Console.ResetColor();
            Console.WriteLine("1. Увійти в систему \n0. Вихід");

            int choice = GetInt("Оберіть пункт (0-1):", 0, 1);
            if (choice == 0) Environment.Exit(0);

            string email = GetStr("Email/Логін:");
            string pass = GetStr("Пароль:");

            User user = auth.Login(email, pass);

            if (user != null)
            {
                CurrentRole = user.Role;
                CurrentUser = user.Name;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n[СИСТЕМА]: Вітаємо, {CurrentUser}! Ви успішно увійшли як {CurrentRole}.");
                Console.ResetColor();
                Console.WriteLine("Натисніть Enter, щоб продовжити...");
                Console.ReadLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[ПОМИЛКА]: Користувача не знайдено або пароль невірний.");
                Console.ResetColor();
                Console.WriteLine("Натисніть Enter, щоб повернутися...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Відображає головне меню після успішної авторизації.
        /// </summary>
        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine($"Користувач: {CurrentUser} [{CurrentRole}]");
            Console.WriteLine("1. ПРАЦІВНИКИ \n2. Вихід з акаунта");

            switch (GetInt("Оберіть пункт (1-2):", 1, 2))
            {
                case 1: EntityMenu("Працівники"); break;
                case 2:
                    CurrentRole = "";
                    CurrentUser = "";
                    break;
            }
        }

        /// <summary>
        /// Відображає меню дій над обраною сутністю системи.
        /// </summary>
        /// <param name="type">Назва сутності для відображення в заголовку.</param>
        static void EntityMenu(string type)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"--- УПРАВЛІННЯ: {type.ToUpper()} ---");
                Console.ResetColor();

                Console.WriteLine("1. Список \n2. Пошук (за ID) \n3. Сортування (за алфавітом) \n4. Статистика (Зарплати) \n5. Фільтрація (за відділом)");

                int maxVal = 5;
                if (CurrentRole == "Admin")
                {
                    Console.WriteLine("6. Додати \n7. Видалити \n8. Редагувати");
                    maxVal = 8;
                }
                Console.WriteLine("0. Назад");

                int ch = GetInt($"Оберіть пункт (0-{maxVal}):", 0, maxVal);
                if (ch == 0) break;

                if (type == "Працівники") HandleEmployeeActions(ch);

                Console.WriteLine("\nНатисніть Enter...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Перенаправляє запит користувача на відповідний метод обробки даних працівників.
        /// </summary>
        /// <param name="ch">Номер обраного пункту меню.</param>
        static void HandleEmployeeActions(int ch)
        {
            List<Employee> employees = empManager.LoadEmployees();
            switch (ch)
            {
                case 1: ShowEmployees(employees); break;
                case 2: SearchEntity(employees); break;
                case 3: SortEmployees(employees); break;
                case 4: ShowStatistics(employees); break;
                case 5: FilterEntity(employees); break;
                case 6: if (CurrentRole == "Admin") AddEntity(employees); break;
                case 7: if (CurrentRole == "Admin") DeleteEntity(employees); break;
                case 8: if (CurrentRole == "Admin") UpdateEntity(employees); break;
            }
        }

        /// <summary>
        /// Виводить у консоль форматовану таблицю зі списком співробітників.
        /// </summary>
        /// <param name="list">Список співробітників для відображення.</param>
        static void ShowEmployees(List<Employee> list)
        {
            Console.WriteLine("\n{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", "ID", "ПІБ", "Посада", "Відділ", "Оклад (грн)");
            Console.WriteLine(new string('-', 75));
            foreach (var emp in list)
            {
                Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10:F2}", emp.Id, emp.FullName, emp.Position, emp.Department, emp.MonthlySalary);
            }
        }

        /// <summary>
        /// Виконує пошук конкретного співробітника за його унікальним числовим ідентифікатором.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void SearchEntity(List<Employee> list)
        {
            int id = GetInt("Введіть ID для пошуку:", 1);
            bool found = false;

            foreach (var emp in list)
            {
                if (emp.Id == id)
                {
                    Console.WriteLine($"\nЗнайдено: {emp.FullName} | Відділ: {emp.Department} | Оклад: {emp.MonthlySalary} грн | ЗП за рік: {emp.YearlySalary} грн");
                    found = true;
                    break;
                }
            }

            if (!found) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Працівника не знайдено."); Console.ResetColor(); }
        }

        /// <summary>
        /// Сортує список співробітників за алфавітом за допомогою алгоритму бульбашки.
        /// </summary>
        /// <param name="list">Список співробітників для сортування.</param>
        static void SortEmployees(List<Employee> list)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (string.Compare(list[j].FullName, list[j + 1].FullName) > 0)
                    {
                        Employee temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("\nСписок успішно відсортовано за алфавітом!");
            ShowEmployees(list);
            empManager.SaveEmployees(list);
        }

        /// <summary>
        /// Обчислює та виводить статистичні показники заробітних плат по підприємству.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void ShowStatistics(List<Employee> list)
        {
            if (list.Count == 0) return;
            double sum = 0, max = 0, min = double.MaxValue;

            foreach (var emp in list)
            {
                sum += emp.MonthlySalary;
                if (emp.MonthlySalary > max) max = emp.MonthlySalary;
                if (emp.MonthlySalary < min) min = emp.MonthlySalary;
            }

            Console.WriteLine($"\nЗагальна кількість працівників: {list.Count}");
            Console.WriteLine($"Середній оклад: {sum / list.Count:F2} грн");
            Console.WriteLine($"Максимальний оклад: {max:F2} грн");
            Console.WriteLine($"Мінімальний оклад: {min:F2} грн");
        }

        /// <summary>
        /// Фільтрує списки співробітників за вказаною назвою структурного підрозділу.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void FilterEntity(List<Employee> list)
        {
            string targetDept = GetStr("Введіть назву відділу для фільтрації:").ToLower();
            List<Employee> result = new List<Employee>();

            foreach (var emp in list)
            {
                if (emp.Department.ToLower() == targetDept)
                {
                    result.Add(emp);
                }
            }

            if (result.Count > 0)
            {
                Console.WriteLine($"\n--- Працівники відділу '{targetDept.ToUpper()}' ---");
                ShowEmployees(result);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("У цьому відділі немає працівників.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Реалізує процедуру додавання нового співробітника до бази даних.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void AddEntity(List<Employee> list)
        {
            string name = GetStr("ПІБ:");
            string pos = GetStr("Посада:");
            string dept = GetStr("Відділ:");
            double sal = GetDouble("Оклад (грн):", 0);
            int vac = GetInt("Дні відпустки:", 0, 100);

            list.Add(new Employee(empManager.GetNextId(list), name, pos, dept, sal, vac));
            empManager.SaveEmployees(list);
            Console.WriteLine("Працівника успішно додано!");
        }

        /// <summary>
        /// Реалізує процедуру модифікації даних існуючого співробітника за його ідентифікатором.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void UpdateEntity(List<Employee> list)
        {
            int id = GetInt("ID працівника для редагування:", 1);
            bool found = false;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == id)
                {
                    found = true;
                    string newPos = GetStr($"Нова посада (поточна: {list[i].Position}):");
                    if (newPos != "") list[i].Position = newPos;

                    list[i].MonthlySalary = GetDouble($"Новий оклад (поточний: {list[i].MonthlySalary}):", 0);
                    break;
                }
            }

            if (found)
            {
                empManager.SaveEmployees(list);
                Console.WriteLine("Дані оновлено!");
            }
            else Console.WriteLine("ID не знайдено.");
        }

        /// <summary>
        /// Реалізує видалення запису співробітника за вказаним ідентифікатором.
        /// </summary>
        /// <param name="list">Поточний список співробітників.</param>
        static void DeleteEntity(List<Employee> list)
        {
            ShowEmployees(list);
            Console.WriteLine();

            int id = GetInt("ID для видалення:", 1);
            bool found = false;
            List<Employee> upd = new List<Employee>();

            foreach (var emp in list)
            {
                if (emp.Id != id) upd.Add(emp);
                else found = true;
            }

            if (found)
            {
                empManager.SaveEmployees(upd);
                Console.WriteLine("Працівника видалено!");
            }
            else Console.WriteLine("ID не знайдено.");
        }

        /// <summary>
        /// Зчитує текстовий рядок, введений користувачем у консолі.
        /// </summary>
        /// <param name="p">Супровідний текст підказки для введення.</param>
        /// <returns>Введений користувачем рядок.</returns>
        static string GetStr(string p)
        {
            Console.Write(p + " ");
            return Console.ReadLine() ?? "";
        }

        /// <summary>
        /// Запитує ціле число у користувача та здійснює перевірку на коректність і діапазон значень.
        /// </summary>
        /// <param name="p">Супровідний текст підказки.</param>
        /// <param name="min">Мінімально можливе значення.</param>
        /// <param name="max">Максимально можливе значення.</param>
        /// <returns>Валідоване ціле число.</returns>
        static int GetInt(string p = "Дія:", int min = int.MinValue, int max = int.MaxValue)
        {
            int r;
            while (true)
            {
                Console.Write(p + " ");
                if (int.TryParse(Console.ReadLine(), out r) && r >= min && r <= max) return r;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка! Введіть число від {min} до {max}.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Запитує дробове число у користувача з перевіркою формату знаку розділювача та мінімальної межі.
        /// </summary>
        /// <param name="p">Супровідний текст підказки.</param>
        /// <param name="min">Мінімально можливе значення числа.</param>
        /// <returns>Валідоване число типу double.</returns>
        static double GetDouble(string p, double min = 0)
        {
            double r;
            while (true)
            {
                Console.Write(p + " ");
                string input = Console.ReadLine()?.Replace('.', ',') ?? "";
                if (double.TryParse(input, out r) && r >= min) return r;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка! Введіть коректне число (мін: {min}).");
                Console.ResetColor();
            }
        }
    }
}