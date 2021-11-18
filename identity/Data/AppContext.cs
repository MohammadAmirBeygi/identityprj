using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Data
{
    public class App_Context :IdentityDbContext
    {
        public App_Context(DbContextOptions<App_Context> option) : base(option)
        {

        }
    }
}
