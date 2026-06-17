using System;
using System.Collections.Generic;
using System.Text;

namespace Management_of_Employees
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin" або "User"

        public User(int id, string name, string email, string passwordHash, string role)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        // Метод для зручного запису у файл
        public string ToCsvFormat()
        {
            return $"{Id},{Name},{Email},{PasswordHash},{Role}";
        }
    }
}
