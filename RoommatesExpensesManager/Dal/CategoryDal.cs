using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Dal
{
    public class CategoryDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().ToTable("category");
        }
        public DbSet<Category> Categories { get; set; }
    }
}