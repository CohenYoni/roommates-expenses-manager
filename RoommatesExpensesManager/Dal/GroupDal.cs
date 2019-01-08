using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Dal
{
    public class GroupDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Group>().ToTable("group");
        }
        public DbSet<Group> Groups { get; set; }

    }
}