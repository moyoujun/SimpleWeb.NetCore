using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<ZhuanlanDO> ZhuanlanDOs { get; set; }
        public virtual DbSet<ZhuanlanTagDO> ZhuanlanTagDOs { get; set; }
    }
}
