using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JwtGenerator
{
    public interface IJwtGenerator
    {
        Task<TokenModel> CreateToken(User user);
    }
}
