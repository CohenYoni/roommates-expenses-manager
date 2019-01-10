using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Dal
{
    public class ExpenseDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Expense>().ToTable("expense");
        }
        public DbSet<Expense> Expenses { get; set; }
    }
}