using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class Login_Information
    {
        public int id { get; set; } = 0;
        public int id_user { get; set; } = 0;
        public string accout_name { get; set; } = string.Empty;
        public string accout_password { get; set; } = string.Empty;

        public int verified { get; set; } = 0;
        public string verified_code { get; set; } = string.Empty;

        public Login_Information() { }
        public Login_Information(int id, int id_user, string name, string password, int verified, string verified_code)
        {
            this.id = id;
            this.id_user = id_user;
            this.accout_name = name;
            this.accout_password = password;
            this.verified = verified;
            this.verified_code = verified_code;
        }
    }
}
