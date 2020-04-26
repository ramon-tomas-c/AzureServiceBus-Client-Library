using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SB.WebAPI.Infrastructure.Options
{
    public class JwtBearer
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
