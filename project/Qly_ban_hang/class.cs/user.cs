using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace ManagermentSale.Models
{
    public class User
    {
        public int UserID{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Role { get; set; }
    }
}