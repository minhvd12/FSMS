using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace FSMS.Entity.Models
{
    public partial class FruitSeasonManagementSystemV10Context : DbContext
    {
        public FruitSeasonManagementSystemV10Context()
        {
        }

        public FruitSeasonManagementSystemV10Context(DbContextOptions<FruitSeasonManagementSystemV10Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoryFruit> CategoryFruits { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CropVariety> CropVarieties { get; set; } = null!;
        public virtual DbSet<CropVarietyGrowthTask> CropVarietyGrowthTasks { get; set; } = null!;
        public virtual DbSet<CropVarietyStage> CropVarietyStages { get; set; } = null!;
        public virtual DbSet<Fruit> Fruits { get; set; } = null!;
        public virtual DbSet<FruitDiscount> FruitDiscounts { get; set; } = null!;
        public virtual DbSet<FruitHistory> FruitHistories { get; set; } = null!;
        public virtual DbSet<FruitImage> FruitImages { get; set; } = null!;
        public virtual DbSet<Garden> Gardens { get; set; } = null!;
        public virtual DbSet<GardenTask> GardenTasks { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Plant> Plants { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<ReviewFruit> ReviewFruits { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Season> Seasons { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Weather> Weathers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    var builder = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    IConfigurationRoot configuration = builder.Build();
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDB"));
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryFruit>(entity =>
            {
                entity.ToTable("CategoryFruit");

                entity.Property(e => e.CategoryFruitId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryFruit_id");

                entity.Property(e => e.CategoryFruitName)
                    .HasMaxLength(255)
                    .HasColumnName("categoryFruit_name");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId)
                    .ValueGeneratedNever()
                    .HasColumnName("comment_id");

                entity.Property(e => e.CommentContent).HasColumnName("comment_content");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<CropVariety>(entity =>
            {
                entity.ToTable("CropVariety");

                entity.Property(e => e.CropVarietyId)
                    .ValueGeneratedNever()
                    .HasColumnName("cropVariety_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.CropVarietyName)
                    .HasMaxLength(50)
                    .HasColumnName("cropVariety_name");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");
            });

            modelBuilder.Entity<CropVarietyGrowthTask>(entity =>
            {
                entity.HasKey(e => e.GrowthTaskId)
                    .HasName("PK__CropVari__D297F3D9E15090C5");

                entity.ToTable("CropVarietyGrowthTask");

                entity.Property(e => e.GrowthTaskId)
                    .ValueGeneratedNever()
                    .HasColumnName("growthTask_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.CropVarietyStageId).HasColumnName("cropVarietyStage_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.TaskName)
                    .HasMaxLength(50)
                    .HasColumnName("task_name");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.CropVarietyStage)
                    .WithMany(p => p.CropVarietyGrowthTasks)
                    .HasForeignKey(d => d.CropVarietyStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CropVarietyGrowthTask_CropVarietyStage");
            });

            modelBuilder.Entity<CropVarietyStage>(entity =>
            {
                entity.ToTable("CropVarietyStage");

                entity.Property(e => e.CropVarietyStageId)
                    .ValueGeneratedNever()
                    .HasColumnName("cropVarietyStage_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.CropVarietyId).HasColumnName("cropVariety_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.StageName)
                    .HasMaxLength(50)
                    .HasColumnName("stage_name");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.CropVariety)
                    .WithMany(p => p.CropVarietyStages)
                    .HasForeignKey(d => d.CropVarietyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CropVarietyStage_CropVariety");
            });

            modelBuilder.Entity<Fruit>(entity =>
            {
                entity.ToTable("Fruit");

                entity.Property(e => e.FruitId)
                    .ValueGeneratedNever()
                    .HasColumnName("fruit_id");

                entity.Property(e => e.CategoryFruitId).HasColumnName("categoryFruit_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.FruitDescription).HasColumnName("fruit_description");

                entity.Property(e => e.FruitName)
                    .HasMaxLength(255)
                    .HasColumnName("fruit_name");

                entity.Property(e => e.OrderType)
                    .HasMaxLength(20)
                    .HasColumnName("order_type");

                entity.Property(e => e.OriginCity)
                    .HasMaxLength(255)
                    .HasColumnName("origin_city");

                entity.Property(e => e.PlantId).HasColumnName("plant_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("price");

                entity.Property(e => e.QuantityAvailable).HasColumnName("quantity_available");

                entity.Property(e => e.QuantityInTransit).HasColumnName("quantity_in_transit");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.CategoryFruit)
                    .WithMany(p => p.Fruits)
                    .HasForeignKey(d => d.CategoryFruitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fruit_CategoryFruit");

                entity.HasOne(d => d.Plant)
                    .WithMany(p => p.Fruits)
                    .HasForeignKey(d => d.PlantId)
                    .HasConstraintName("FK_Fruit_Plant");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Fruits)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fruit_User");
            });

            modelBuilder.Entity<FruitDiscount>(entity =>
            {
                entity.ToTable("FruitDiscount");

                entity.Property(e => e.FruitDiscountId)
                    .ValueGeneratedNever()
                    .HasColumnName("fruit_discount_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.DepositAmount)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("deposit_amount");

                entity.Property(e => e.DiscountExpiryDate)
                    .HasColumnType("date")
                    .HasColumnName("discount_expiry_date");

                entity.Property(e => e.DiscountName)
                    .HasMaxLength(255)
                    .HasColumnName("discount_name");

                entity.Property(e => e.DiscountPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("discount_percentage");

                entity.Property(e => e.DiscountThreshold).HasColumnName("discount_threshold");

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.FruitDiscounts)
                    .HasForeignKey(d => d.FruitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDiscount_Product");
            });

            modelBuilder.Entity<FruitHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__FruitHis__096AA2E92948DDDE");

                entity.ToTable("FruitHistory");

                entity.Property(e => e.HistoryId).HasColumnName("history_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.FruitName)
                    .HasMaxLength(255)
                    .HasColumnName("fruit_name");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FruitHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FruitHistory_User");
            });

            modelBuilder.Entity<FruitImage>(entity =>
            {
                entity.ToTable("FruitImage");

                entity.Property(e => e.FruitImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("fruitImage_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("image_url");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.FruitImages)
                    .HasForeignKey(d => d.FruitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FruitImage_Fruit");
            });

            modelBuilder.Entity<Garden>(entity =>
            {
                entity.ToTable("Garden");

                entity.Property(e => e.GardenId)
                    .ValueGeneratedNever()
                    .HasColumnName("garden_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.GardenName)
                    .HasMaxLength(50)
                    .HasColumnName("garden_name");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.QuantityPlanted).HasColumnName("quantity_planted");

                entity.Property(e => e.Region)
                    .HasMaxLength(255)
                    .HasColumnName("region");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Gardens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Garden_User");
            });

            modelBuilder.Entity<GardenTask>(entity =>
            {
                entity.ToTable("GardenTask");

                entity.Property(e => e.GardenTaskId)
                    .ValueGeneratedNever()
                    .HasColumnName("gardenTask_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.GardenId).HasColumnName("garden_id");

                entity.Property(e => e.GardenTaskDate)
                    .HasColumnType("date")
                    .HasColumnName("gardenTask_date");

                entity.Property(e => e.GardenTaskName)
                    .HasMaxLength(50)
                    .HasColumnName("gardenTask_name");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.PlantId).HasColumnName("plant_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Garden)
                    .WithMany(p => p.GardenTasks)
                    .HasForeignKey(d => d.GardenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GardenTask_Garden");

                entity.HasOne(d => d.Plant)
                    .WithMany(p => p.GardenTasks)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GardenTask_Plant");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("notification_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .HasColumnName("message");

                entity.Property(e => e.NotificationType)
                    .HasMaxLength(50)
                    .HasColumnName("notification_type");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_User");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("order_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(255)
                    .HasColumnName("delivery_address");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.ParentOrderId).HasColumnName("parent_order_id");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(255)
                    .HasColumnName("payment_method");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_User");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.FruitId })
                    .HasName("PK_OrderDetail_Fruit_Order");

                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.FruitDiscountId).HasColumnName("fruit_discount_id");

                entity.Property(e => e.OderDetailType)
                    .HasMaxLength(50)
                    .HasColumnName("oder_detail_type");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("unit_price");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.FruitDiscount)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.FruitDiscountId)
                    .HasConstraintName("FK_OrderDetail_FruitDiscount");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.FruitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Fruit");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId)
                    .ValueGeneratedNever()
                    .HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("payment_method");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .HasColumnName("payment_type");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Payment_Order");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_User");
            });

            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("Plant");

                entity.Property(e => e.PlantId)
                    .ValueGeneratedNever()
                    .HasColumnName("plant_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.CropVarietyId).HasColumnName("cropVariety_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EstimatedHarvestQuantity).HasColumnName("estimated_harvest_quantity");

                entity.Property(e => e.GardenId).HasColumnName("garden_id");

                entity.Property(e => e.HarvestingDate)
                    .HasColumnType("date")
                    .HasColumnName("harvesting_date");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.PlantName)
                    .HasMaxLength(50)
                    .HasColumnName("plant_name");

                entity.Property(e => e.PlantingDate)
                    .HasColumnType("date")
                    .HasColumnName("planting_date");

                entity.Property(e => e.QuantityPlanted).HasColumnName("quantity_planted");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.CropVariety)
                    .WithMany(p => p.Plants)
                    .HasForeignKey(d => d.CropVarietyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plant_CropVariety");

                entity.HasOne(d => d.Garden)
                    .WithMany(p => p.Plants)
                    .HasForeignKey(d => d.GardenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plant_Garden");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostId)
                    .ValueGeneratedNever()
                    .HasColumnName("post_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.PostContent).HasColumnName("post_content");

                entity.Property(e => e.PostImage)
                    .HasMaxLength(255)
                    .HasColumnName("post_image");

                entity.Property(e => e.PostTitle).HasColumnName("post_title");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<ReviewFruit>(entity =>
            {
                entity.HasKey(e => e.ReviewId)
                    .HasName("PK__ReviewFr__60883D900E5C539B");

                entity.ToTable("ReviewFruit");

                entity.Property(e => e.ReviewId)
                    .ValueGeneratedNever()
                    .HasColumnName("review_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Rating)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("rating");

                entity.Property(e => e.ReviewComment).HasColumnName("review_comment");

                entity.Property(e => e.ReviewImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("review_image_url");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.ReviewFruits)
                    .HasForeignKey(d => d.FruitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Fruit");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReviewFruits)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("role_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .HasColumnName("role_name");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("Season");

                entity.Property(e => e.SeasonId)
                    .ValueGeneratedNever()
                    .HasColumnName("season_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.GardenId).HasColumnName("garden_id");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.SeasonName)
                    .HasMaxLength(50)
                    .HasColumnName("season_name");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Garden)
                    .WithMany(p => p.Seasons)
                    .HasForeignKey(d => d.GardenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Season_Garden");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.ImageMomoUrl)
                    .HasMaxLength(255)
                    .HasColumnName("image_momo_url");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");

                entity.Property(e => e.ProfileImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("profile_image_url");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("Weather");

                entity.Property(e => e.WeatherId).HasColumnName("weather_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WeatherName)
                    .HasMaxLength(255)
                    .HasColumnName("weather_name");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Weathers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weather_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
