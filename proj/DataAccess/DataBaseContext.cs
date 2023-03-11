using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ColorCard> ColorCards { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .ToTable("Cards", "dbo");

			modelBuilder.Entity<Category>()
				.ToTable("Categories", "dbo");

			modelBuilder.Entity<Card>()
                .HasMany(x => x.Operations)
                .WithOne(x => x.Card)
                .HasForeignKey(x => x.CardId);

            modelBuilder.Entity<User>()
                .ToTable("Users", "dbo");

            modelBuilder.Entity<User>()
                .HasMany(x => x.Cards)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Operation>()
                .ToTable("Operations", "dbo");

            modelBuilder.Entity<Operation>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.CardId);


			modelBuilder.Entity<Category>()
				.HasMany(x => x.Operations)
				.WithOne(x => x.Category)
				.HasForeignKey(x => x.CategoryId);


			modelBuilder.Entity<ColorCard>()
				.HasMany(x => x.Cards)
				.WithOne(x => x.ColorCard)
				.HasForeignKey(x => x.ColorId);
		}
    }
}
