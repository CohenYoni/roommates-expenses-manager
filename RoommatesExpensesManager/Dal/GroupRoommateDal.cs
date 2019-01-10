using RoommatesExpensesManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RoommatesExpensesManager.Dal
{
    public class GroupRoommateDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupRoommate>().ToTable("groupRoommates");
        }
        public DbSet<GroupRoommate> GroupsRoommates { get; set; }
    }
}