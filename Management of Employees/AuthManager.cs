using System;
using System.Collections.Generic;
using System.Text;

namespace Management_of_Employees
{
    public class AuthManager
    {
        private string _usersFilePath = "users.csv";

        public AuthManager()
        {
            InitializeFile();
        }

        // Перевіряємо, чи є файл, якщо ні - створюємо і додаємо першого Адміна
        private void InitializeFile()
        {
            if (!File.Exists(_usersFilePath))
            {
                using (StreamWriter writer = new StreamWriter(_usersFilePath))
                {
                    writer.WriteLine("Id,Name,Email,PasswordHash,Role");

                    // Створюємо базового адміна при першому запуску програми
                    string adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                    writer.WriteLine($"1,Головний Адмін,admin@test.com,{adminHash},Admin");
                }
                Console.WriteLine("Файл користувачів створено. Базовий логін: admin@test.com, пароль: admin123");
            }
        }

        // Зчитуємо всіх користувачів без LINQ
        public List<User> LoadUsers()
        {
            List<User> users = new List<User>();

            using (StreamReader reader = new StreamReader(_usersFilePath))
            {
                string header = reader.ReadLine(); // Пропускаємо заголовок
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        User user = new User(
                            int.Parse(parts[0]),
                            parts[1],
                            parts[2],
                            parts[3],
                            parts[4]
                        );
                        users.Add(user);
                    }
                }
            }
            return users;
        }

        // Метод для входу в систему
        public User Login(string email, string password)
        {
            List<User> users = LoadUsers();

            // Шукаємо користувача класичним циклом (без LINQ)
            foreach (User u in users)
            {
                if (u.Email == email)
                {
                    // Перевіряємо, чи збігається введений пароль із хешем у файлі
                    if (BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
                    {
                        return u; // Пароль правильний, повертаємо профайл
                    }
                }
            }
            return null; // Якщо email не знайдено або пароль невірний
        }
    }
}
