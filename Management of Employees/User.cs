using System;

namespace Management_of_Employees
{
    /// <summary>
    /// Клас, що представляє обліковий запис користувача інформаційної системи.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Отримує або задає унікальний ідентифікатор користувача.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Отримує або задає ім'я користувача для відображення в інтерфейсі.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Отримує або задає електронну пошту, яка виступає логіном для входу.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Отримує або задає захищений хеш пароля користувача.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Отримує або задає роль користувача, що визначає рівень прав доступу.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Ініціалізує новий екземпляр класу User із заданими параметрами.
        /// </summary>
        /// <param name="id">Унікальний ідентифікатор.</param>
        /// <param name="name">Ім'я користувача.</param>
        /// <param name="email">Електронна пошта.</param>
        /// <param name="passwordHash">Хеш пароля.</param>
        /// <param name="role">Роль доступу.</param>
        public User(int id, string name, string email, string passwordHash, string role)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        /// <summary>
        /// Перетворює дані користувача у рядок для збереження у форматі CSV.
        /// </summary>
        /// <returns>Рядок із розділювачами-комами.</returns>
        public string ToCsvFormat()
        {
            return $"{Id},{Name},{Email},{PasswordHash},{Role}";
        }
    }
}