using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;

namespace MysqlEntityFramework.test1
{
    public partial class test_1Context : DbContext
    {
        public test_1Context()
        {
        }

        public test_1Context(DbContextOptions<test_1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<NewTable> NewTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string sVlaue = ConfigurationManager.AppSettings["test_database"];
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(sVlaue);
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
               // optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=abcd1992qyl;database=test_1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewTable>(entity =>
            {
                entity.ToTable("new_table");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Hight)
                    .HasColumnName("hight")
                    .HasColumnType("decimal(4,1)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight")
                    .HasColumnType("decimal(4,2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
