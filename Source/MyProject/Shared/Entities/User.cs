using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class User
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Number_phone { get; set; } = 0;
        public int Role { get; set; } = 0;
        public string Role_name { get; set; } = string.Empty;

        public string accout_name { get; set; } = string.Empty;
        public string accout_password { set; get; } = string.Empty;
        public int verified { get; set; } = 0;
        public string verified_code { get ; set; } = string.Empty;

        public bool logined { get; set; } = false;

        public string access_token { get; set; } = string.Empty;
        public string time_access { get; set; } = string.Empty;
        public DateTime birth_day { get; set; } = DateTime.Now;
        public User(int id, string name, string email, string address, int number_phone, int role, string role_name)
        {
            this.id = id;
            this.Name = name;
            this.Email = email;
            this.Address = address;
            this.Number_phone = number_phone;
            this.Role = role;
            this.Role_name = role_name;
        }

        public User(int id, string name, string email, string address, int number_phone, int role, string role_name, string accout_name, string accout_password, int verified, string verified_code) : this(id, name, email, address, number_phone, role, role_name)
        {
            this.id = id;
            this.Name = name;
            this.Email = email;
            this.Address = address;
            this.Number_phone = number_phone;
            this.Role = role;
            this.Role_name = role_name;
            this.accout_name = accout_name;
            this.accout_password = accout_password;
            this.verified = verified;
            this.verified_code = verified_code;
        }

        public User() { }
    }
}
