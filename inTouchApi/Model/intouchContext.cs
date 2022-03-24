using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace inTouchApi.Model
{
    public partial class intouchContext : DbContext
    {
        public intouchContext()
        {
        }

        public intouchContext(DbContextOptions<intouchContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Guestacce> Guestacces { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Notificationdetail> Notificationdetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Urbanization> Urbanizations { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost;Database=intouch;Uid=root;Pwd=root");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>(entity =>
            {
                entity.ToTable("guests");

                entity.HasIndex(e => e.HouseId, "houseID");

                entity.HasIndex(e => e.UrbanizationId, "urbanizationID");

                entity.HasIndex(e => e.UserId, "userID");

                entity.Property(e => e.GuestId).HasColumnName("guestID");

                entity.Property(e => e.DateCreation)
                    .HasColumnType("date")
                    .HasColumnName("dateCreation");

                entity.Property(e => e.Delivery)
                    .HasColumnType("bit(1)")
                    .HasColumnName("delivery");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.HouseId).HasColumnName("houseID");

                entity.Property(e => e.Identification)
                    .HasMaxLength(100)
                    .HasColumnName("identification");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(14)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Plate)
                    .HasMaxLength(50)
                    .HasColumnName("plate");

                entity.Property(e => e.Restaurant)
                    .HasMaxLength(100)
                    .HasColumnName("restaurant");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.House)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.HouseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guests_ibfk_3");

                entity.HasOne(d => d.Urbanization)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.UrbanizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guests_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guests_ibfk_1");
            });

            modelBuilder.Entity<Guestacce>(entity =>
            {
                entity.HasKey(e => e.GuestAccessId)
                    .HasName("PRIMARY");

                entity.ToTable("guestacces");

                entity.HasIndex(e => e.GuestId, "guestID");

                entity.HasIndex(e => e.UrbanizationId, "urbanizationID");

                entity.HasIndex(e => e.UserId, "userID");

                entity.Property(e => e.GuestAccessId).HasColumnName("guestAccessID");

                entity.Property(e => e.accessFrom).HasColumnName("accessFrom");

                entity.Property(e => e.accessTo).HasColumnName("accessTo");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(100)
                    .HasColumnName("accessCode");

                entity.Property(e => e.AccessImage)
                    .HasMaxLength(100)
                    .HasColumnName("accessImage");

                entity.Property(e => e.GuestId).HasColumnName("guestID");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Guestacces)
                    .HasForeignKey(d => d.GuestId)
                    .HasConstraintName("guestacces_ibfk_1");

                entity.HasOne(d => d.Urbanization)
                    .WithMany(p => p.Guestacces)
                    .HasForeignKey(d => d.UrbanizationId)
                    .HasConstraintName("guestacces_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Guestacces)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("guestacces_ibfk_2");
            });

            modelBuilder.Entity<House>(entity =>
            {
                entity.ToTable("houses");

                entity.HasIndex(e => e.UrbanizationId, "urbanizationID");

                entity.HasIndex(e => e.UserId, "userID");

                entity.Property(e => e.HouseId).HasColumnName("houseID");

                entity.Property(e => e.FullAddress)
                    .HasMaxLength(500)
                    .HasColumnName("fullAddress");

                entity.Property(e => e.Mz)
                    .HasMaxLength(100)
                    .HasColumnName("mz");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500)
                    .HasColumnName("notes");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(14)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Villa)
                    .HasMaxLength(100)
                    .HasColumnName("villa");

                entity.HasOne(d => d.Urbanization)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.UrbanizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("houses_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("houses_ibfk_2");
            });

            modelBuilder.Entity<Notificationdetail>(entity =>
            {
                entity.ToTable("notificationdetail");

                entity.HasIndex(e => e.UrbanizationId, "urbanizationID");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FechaEstablecida)
                    .HasColumnType("date")
                    .HasColumnName("fechaEstablecida");

                entity.Property(e => e.Notas)
                    .HasMaxLength(255)
                    .HasColumnName("notas");

                entity.Property(e => e.tiempo).HasColumnName("tiempo");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .HasColumnName("titulo");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.HasOne(d => d.Urbanization)
                    .WithMany(p => p.Notificationdetails)
                    .HasForeignKey(d => d.UrbanizationId)
                    .HasConstraintName("notificationdetail_ibfk_1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(100)
                    .HasColumnName("roleDescription");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(30)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<Urbanization>(entity =>
            {
                entity.ToTable("urbanizations");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.Property(e => e.Active)
                    .HasColumnType("bit(1)")
                    .HasColumnName("active");

                entity.Property(e => e.activeFrom).HasColumnName("activeFrom");

                entity.Property(e => e.activeTo).HasColumnName("activeTo");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(100)
                    .HasColumnName("contactEmail");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100)
                    .HasColumnName("contactName");

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(100)
                    .HasColumnName("contactNumber");

                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .HasColumnName("country");

                entity.Property(e => e.Image)
                    .HasMaxLength(250)
                    .HasColumnName("image");

                entity.Property(e => e.Ruc)
                    .HasMaxLength(50)
                    .HasColumnName("ruc");

                entity.Property(e => e.Urbanization1)
                    .HasMaxLength(200)
                    .HasColumnName("urbanization");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.RoleId, "roleID");

                entity.HasIndex(e => e.UrbanizationId, "urbanizationID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Active)
                    .HasColumnType("bit(1)")
                    .HasColumnName("active");

                entity.Property(e => e.activeFrom).HasColumnName("activeFrom");

                entity.Property(e => e.activeTo).HasColumnName("activeTo");

                entity.Property(e => e.lastLogin).HasColumnName("lastLogin");

                entity.Property(e => e.Deleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("deleted");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.EmailConfirmation)
                    .HasColumnType("bit(1)")
                    .HasColumnName("emailConfirmation");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("firstName");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.Property(e => e.UrbanizationId).HasColumnName("urbanizationID");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("userName");

                entity.Property(e => e.UserParentId).HasColumnName("userParentID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_ibfk_2");

                entity.HasOne(d => d.Urbanization)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UrbanizationId)
                    .HasConstraintName("users_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
