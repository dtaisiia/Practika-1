using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BCrypt.Net;

namespace Management_of_Employees
{
    /// <summary>
    /// Клас для управління процесами автентифікації та авторизації користувачів.
    /// </summary>
    public class AuthManager
    {
        private string _usersFilePath = "users.csv";

        /// <summary>
        /// Ініціалізує новий екземпляр менеджера авторизації та перевіряє наявність файлу користувачів.
        /// </summary>
        public AuthManager()
        {
            InitializeFile();
        }

        /// <summary>
        /// Перевіряє існування файлу користувачів та створює його з базовим адміністратором за відсутності.
        /// </summary>
        private void InitializeFile()
        {
            if (!File.Exists(_usersFilePath))
            {
                using (StreamWriter writer = new StreamWriter(_usersFilePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Id,Name,Email,PasswordHash,Role");
                    string adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                    writer.WriteLine($"1,Головний Адмін,admin@test.com,{adminHash},Admin");
                }
            }
        }

        /// <summary>
        /// Завантажує список усіх зареєстрованих користувачів із файлу CSV.
        /// </summary>
        /// <returns>Колекція об'єктів користувачів.</returns>
        public List<User> LoadUsers()
        {
            List<User> users = new List<User>();

            using (StreamReader reader = new StreamReader(_usersFilePath, Encoding.UTF8))
            {
                string header = reader.ReadLine();
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

        /// <summary>
        /// Виконує перевірку облікових даних користувача для входу в систему.
        /// </summary>
        /// <param name="email">Електронна пошта користувача.</param>
        /// <param name="password">Введений пароль у відкритому вигляді.</param>
        /// <returns>Об'єкт користувача у разі успіху, або null при помилці.</returns>
        public User Login(string email, string password)
        {
            List<User> users = LoadUsers();

            foreach (User u in users)
            {
                if (u.Email == email)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, u.PasswordHash))
                    {
                        return u;
                    }
                }
            }
            return null;
        }
    }
}