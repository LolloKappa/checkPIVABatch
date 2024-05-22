using Microsoft.EntityFrameworkCore;

namespace checkPIVABatch.Models
{
    internal class CheckIVABatchDBContext : DbContext
    {
        public DbSet<TaxInterrogationHistory> taxInterrogationsHistories { get; set; }

        public CheckIVABatchDBContext(DbContextOptions<CheckIVABatchDBContext> options)
        : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxInterrogationHistory>(entity =>
            {
                entity.ToTable("taxInterrogationHistory");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CountryCode)
                    .HasColumnName("countryCode")
                    .IsRequired();
                entity.Property(e => e.VatNumber)
                    .HasColumnName("vatNumber")
                    .IsRequired();
                entity.Property(e => e.RequestDate)
                    .HasColumnName("requestDate")
                    .IsRequired();
                entity.Property(e => e.Valid)
                    .HasColumnName("valid")
                    .IsRequired();
                entity.Property(e => e.RequestIdentifier)
                    .HasColumnName("requestIdentifier")
                    .IsRequired();
                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired();
                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .IsRequired();

                //unique constraint to the countryCode and vatNumber column column 
                entity.HasIndex(e => new { e.CountryCode , e.VatNumber}).IsUnique();
            });
        }
    }
}
