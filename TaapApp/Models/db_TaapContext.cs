using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaapApp.Models
{
    public partial class db_TaapContext : DbContext
    {
        public db_TaapContext(DbContextOptions<db_TaapContext> options) : base(options) { }
        
        public virtual DbSet<MasterModels> MasterModels { get; set; }
        public virtual DbSet<MasterShops> MasterShops { get; set; }
        public virtual DbSet<MasterTypes> MasterTypes { get; set; }
        public virtual DbSet<MasterTypeItem> MasterTypeItem { get; set; }
        public virtual DbSet<PartReceive> PartReceive { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<ReceiveReference> ReceiveReference { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }

        // Store procedure
        public virtual DbSet<ReportPartReceive> SPReportPartReceive { get; set; }
        public virtual DbSet<ReportPartMovement> SPReportPartMovement { get; set; }
        public virtual DbSet<ReportStockAvailable> SPReportStorcAvailable { get; set; }
        public virtual DbSet<ReportStockMaterial> SPReportStockMaterial { get; set; }
        public virtual DbSet<ReportCarMovement> SPReportCarMovement { get; set; }
        public virtual DbSet<ReportCpl> SPReportCpl { get; set; }
        public virtual DbSet<ReportFpl> SPReportFpl { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterModels>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasColumnName("create_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasColumnName("model_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<MasterShops>(entity =>
            {
                entity.HasKey(e => e.ShopId);

                entity.Property(e => e.ShopId).HasColumnName("shop_id");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasColumnName("create_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShopName)
                    .IsRequired()
                    .HasColumnName("shop_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<MasterTypes>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasColumnName("create_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("type_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<MasterTypeItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasColumnName("create_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ShopId).HasColumnName("shop_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<PartReceive>(entity =>
            {
                entity.HasKey(e => e.ReceiveId);

                entity.Property(e => e.ReceiveId).HasColumnName("ReceiveID");

                entity.Property(e => e.CommissionFrom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CommissionTo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Consignment)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomEntryNo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DateToProduction).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PackingMonth)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PartDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PartNo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PartType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qpv).HasColumnName("QPV");

                entity.Property(e => e.ReceiveNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BuyOffRecNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Shop)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Um)
                    .IsRequired()
                    .HasColumnName("UM")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Form)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            modelBuilder.Entity<ReceiveReference>(entity =>
            {
                entity.HasKey(e => e.Vdoid);

                entity.Property(e => e.Vdoid).HasColumnName("VDOID");

                entity.Property(e => e.ChassisNo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CommissionNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.EngineNo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.LotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModelType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaintTrim)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateToProduction)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TransferNoCkd)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransferNoLoc)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.SetNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vdono)
                    .HasColumnName("VDONo")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transfer>(entity =>
            {

                entity.HasKey(e => e.TfId);

                entity.Property(e => e.TfId);

                //entity.Property(e => e.ReceiveNo)
                    //.IsRequired()
                    //.HasMaxLength(50)
                    //.IsUnicode(false);

                entity.Property(e => e.DateToProduction).HasColumnType("datetime");

                entity.Property(e => e.PackingMonth).HasMaxLength(50);

                entity.Property(e => e.Consignment).HasMaxLength(50);

                entity.Property(e => e.PartType).HasMaxLength(25);

                entity.Property(e => e.TfNo).HasMaxLength(50);
            });
        }
    }
}
