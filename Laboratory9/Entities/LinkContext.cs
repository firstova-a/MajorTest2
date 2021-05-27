using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratory9.Entities
{
    public class LinkContext:DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<News> News { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=.\SQLEXPRESS;Database=Mtest2;Trusted_Connection=True;");
        }

        public LinkContext()
        {
            Database.EnsureCreated();
        }
    }
}
