using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace bai_4.Models
{
    public partial class QLBanHangContext : DbContext
    {
        public QLBanHangContext()
        {
        }

        public QLBanHangContext(DbContextOptions<QLBanHangContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-N8SDIRR\\SQLEXPRESS ;Initial Catalog=QLBanHang; Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<LoaiSanPham>(entity =>
            {
                entity.HasKey(e => e.Maloai)
                    .HasName("PK__LoaiSanP__734B3AEA94A42DC0");

                entity.ToTable("LoaiSanPham");

                entity.Property(e => e.Maloai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("maloai")
                    .IsFixedLength(true);

                entity.Property(e => e.Tenloai).HasColumnName("tenloai");
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.Masp)
                    .HasName("PK__SanPham__7A217672F9196490");

                entity.ToTable("SanPham");

                entity.Property(e => e.Masp)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("masp")
                    .IsFixedLength(true);

                entity.Property(e => e.Dongia)
                    .HasColumnType("money")
                    .HasColumnName("dongia");

                entity.Property(e => e.Maloai)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("maloai")
                    .IsFixedLength(true);

                entity.Property(e => e.Soluong).HasColumnName("soluong");

                entity.Property(e => e.Tensp).HasColumnName("tensp");

                entity.HasOne(d => d.MaloaiNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.Maloai)
                    .HasConstraintName("fk_maloai");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
