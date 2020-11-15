using NotesAPI.Models.Account;
using NotesAPI.Models.Notes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace NotesAPI.Models
{
    public class DataContext:DbContext
    {
        public DataContext() : base("Name=NotesDB") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithRequired(n => n.User)
                .HasForeignKey(n => n.UserId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}