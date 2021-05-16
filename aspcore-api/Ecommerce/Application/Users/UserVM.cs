using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public class UserVM
    {
        public string _id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

        public string ValidTo { get; set; }
    }
}
