using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JwtGenerator
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string ValidTo { get; set; }
    }
}
