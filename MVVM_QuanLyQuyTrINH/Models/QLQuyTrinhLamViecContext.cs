using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class QLQuyTrinhLamViecContext : DbContext
    {
        public QLQuyTrinhLamViecContext()
        {
        }

        public QLQuyTrinhLamViecContext(DbContextOptions<QLQuyTrinhLamViecContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<BaoCaoDuAn> BaoCaoDuAns { get; set; } = null!;
        public virtual DbSet<CongViec> CongViecs { get; set; } = null!;
        public virtual DbSet<DuAn> DuAns { get; set; } = null!;
        public virtual DbSet<LichSuCongViec> LichSuCongViecs { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhanCong> PhanCongs { get; set; } = null!;
        public virtual DbSet<QuanLy> QuanLies { get; set; } = null!;
        public virtual DbSet<QuyTrinh> QuyTrinhs { get; set; } = null!;
        public virtual DbSet<ThongBao> ThongBaos { get; set; } = null!;
        public virtual DbSet<ThongKeCv> ThongKeCvs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VaiTro> VaiTros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=QLQuyTrinhLamViec;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.MaAd)
                    .HasName("PK__Admin__27247E46B476C379");

                entity.ToTable("Admin");

                entity.Property(e => e.MaAd)
                    .ValueGeneratedNever()
                    .HasColumnName("MaAD");

                entity.Property(e => e.QuyenAdmin).HasMaxLength(50);

                entity.HasOne(d => d.MaAdNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.MaAd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Admin__MaAD__4316F928");
            });

            modelBuilder.Entity<BaoCaoDuAn>(entity =>
            {
                entity.HasKey(e => e.MaBaoCao)
                    .HasName("PK__BaoCaoDu__25A9188CD76B80B7");

                entity.ToTable("BaoCaoDuAn");

                entity.Property(e => e.CvdangLam)
                    .HasColumnName("CVDangLam")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CvhoanThanh)
                    .HasColumnName("CVHoanThanh")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CvtreHan)
                    .HasColumnName("CVTreHan")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NgayBaoCao)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenDuAn).HasMaxLength(100);

                entity.Property(e => e.TongCv)
                    .HasColumnName("TongCV")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TyLeHoanThanh).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaDuAnNavigation)
                    .WithMany(p => p.BaoCaoDuAns)
                    .HasForeignKey(d => d.MaDuAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BaoCaoDuA__MaDuA__6A30C649");
            });

            modelBuilder.Entity<CongViec>(entity =>
            {
                entity.HasKey(e => e.MaCv)
                    .HasName("PK__CongViec__27258E76DAEA4FA8");

                entity.ToTable("CongViec");

                entity.Property(e => e.MaCv).HasColumnName("MaCV");

                entity.Property(e => e.DoUuTien).HasMaxLength(20);

                entity.Property(e => e.HanHoanThanh).HasColumnType("date");

                entity.Property(e => e.MaNvphuTrach).HasColumnName("MaNVPhuTrach");

                entity.Property(e => e.MoTa).HasMaxLength(300);

                entity.Property(e => e.NgayGiao).HasColumnType("date");

                entity.Property(e => e.TenCv)
                    .HasMaxLength(100)
                    .HasColumnName("TenCV");

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.MaDuAnNavigation)
                    .WithMany(p => p.CongViecs)
                    .HasForeignKey(d => d.MaDuAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CongViec__MaDuAn__48CFD27E");

                entity.HasOne(d => d.MaNvphuTrachNavigation)
                    .WithMany(p => p.CongViecs)
                    .HasForeignKey(d => d.MaNvphuTrach)
                    .HasConstraintName("FK__CongViec__MaNVPh__49C3F6B7");
            });

            modelBuilder.Entity<DuAn>(entity =>
            {
                entity.HasKey(e => e.MaDuAn)
                    .HasName("PK__DuAn__EFD751E4A5A0AE8C");

                entity.ToTable("DuAn");

                entity.Property(e => e.MoTa).HasMaxLength(300);

                entity.Property(e => e.NgayBatDau).HasColumnType("date");

                entity.Property(e => e.NgayKetThuc).HasColumnType("date");

                entity.Property(e => e.TenDuAn).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.MaTruongNhomNavigation)
                    .WithMany(p => p.DuAns)
                    .HasForeignKey(d => d.MaTruongNhom)
                    .HasConstraintName("FK__DuAn__MaTruongNh__45F365D3");
            });

            modelBuilder.Entity<LichSuCongViec>(entity =>
            {
                entity.HasKey(e => e.MaLichSu)
                    .HasName("PK__LichSuCo__C443222A730A6210");

                entity.ToTable("LichSuCongViec");

                entity.Property(e => e.MaCv).HasColumnName("MaCV");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.NgayCapNhat)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoiDung).HasMaxLength(300);

                entity.Property(e => e.TrangThaiMoi).HasMaxLength(50);

                entity.HasOne(d => d.MaCvNavigation)
                    .WithMany(p => p.LichSuCongViecs)
                    .HasForeignKey(d => d.MaCv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichSuCong__MaCV__4D94879B");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.LichSuCongViecs)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichSuCong__MaNV__4E88ABD4");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv)
                    .HasName("PK__NhanVien__2725D70AF2658244");

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaNv)
                    .ValueGeneratedNever()
                    .HasColumnName("MaNV");

                entity.Property(e => e.MaPb)
                    .HasMaxLength(10)
                    .HasColumnName("MaPB");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithOne(p => p.NhanVien)
                    .HasForeignKey<NhanVien>(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__NhanVien__MaNV__3D5E1FD2");
            });

            modelBuilder.Entity<PhanCong>(entity =>
            {
                entity.HasKey(e => e.MaPhanCong)
                    .HasName("PK__PhanCong__C279D91681454EB9");

                entity.ToTable("PhanCong");

                entity.Property(e => e.MaCv).HasColumnName("MaCV");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.VaiTroTrongCv)
                    .HasMaxLength(100)
                    .HasColumnName("VaiTroTrongCV");

                entity.HasOne(d => d.MaCvNavigation)
                    .WithMany(p => p.PhanCongs)
                    .HasForeignKey(d => d.MaCv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhanCong__MaCV__5165187F");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.PhanCongs)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhanCong__MaNV__52593CB8");
            });

            modelBuilder.Entity<QuanLy>(entity =>
            {
                entity.HasKey(e => e.MaQl)
                    .HasName("PK__QuanLy__2725F8520B7826EE");

                entity.ToTable("QuanLy");

                entity.Property(e => e.MaQl)
                    .ValueGeneratedNever()
                    .HasColumnName("MaQL");

                entity.Property(e => e.CapQuanLy).HasMaxLength(50);

                entity.HasOne(d => d.MaQlNavigation)
                    .WithOne(p => p.QuanLy)
                    .HasForeignKey<QuanLy>(d => d.MaQl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuanLy__MaQL__403A8C7D");
            });

            modelBuilder.Entity<QuyTrinh>(entity =>
            {
                entity.HasKey(e => e.MaBuoc)
                    .HasName("PK__QuyTrinh__E47102993448F867");

                entity.ToTable("QuyTrinh");

                entity.Property(e => e.Mota).HasMaxLength(300);

                entity.Property(e => e.TenBuoc).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(d => d.MaDuAnNavigation)
                    .WithMany(p => p.QuyTrinhs)
                    .HasForeignKey(d => d.MaDuAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuyTrinh__MaDuAn__5535A963");
            });

            modelBuilder.Entity<ThongBao>(entity =>
            {
                entity.HasKey(e => e.MaTb)
                    .HasName("PK__ThongBao__2725006F644C4C3E");

                entity.ToTable("ThongBao");

                entity.Property(e => e.MaTb).HasColumnName("MaTB");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.NgayGui)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoiDung).HasMaxLength(300);

                entity.Property(e => e.TieuDe).HasMaxLength(100);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Chưa đọc')");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.ThongBaos)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThongBao__MaNV__59FA5E80");
            });

            modelBuilder.Entity<ThongKeCv>(entity =>
            {
                entity.HasKey(e => e.MaThongKe)
                    .HasName("PK__ThongKeC__60E521F48F85E2CE");

                entity.ToTable("ThongKeCV");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.NgayThongKe)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoCvdanglam)
                    .HasColumnName("SoCVDanglam")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SoCvhoanThanh)
                    .HasColumnName("SoCVHoanThanh")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SoCvtrehan)
                    .HasColumnName("SoCVTrehan")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaDuAnNavigation)
                    .WithMany(p => p.ThongKeCvs)
                    .HasForeignKey(d => d.MaDuAn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThongKeCV__MaDuA__619B8048");

                entity.HasOne(d => d.MaNvNavigation)
                    .WithMany(p => p.ThongKeCvs)
                    .HasForeignKey(d => d.MaNv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ThongKeCV__MaNV__60A75C0F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.TenDangNhap, "UQ__User__55F68FC0D1DDCDC5")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.MaVaiTro).HasDefaultValueSql("((1))");

                entity.Property(e => e.MatKhau).HasMaxLength(100);

                entity.Property(e => e.TenDangNhap).HasMaxLength(50);

                entity.HasOne(d => d.MaVaiTroNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.MaVaiTro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__MaVaiTro__3A81B327");
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.HasKey(e => e.MaVaiTro)
                    .HasName("PK__VaiTro__C24C41CF53A43AED");

                entity.ToTable("VaiTro");

                entity.Property(e => e.MoTa).HasMaxLength(200);

                entity.Property(e => e.TenVaiTro).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
