using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CVGS_Main.Areas.Identity.Data;

public class CvgsDbContext : IdentityDbContext<CvgsUser>
{
    public virtual DbSet<CvgsGame> CvgsGame { get; set; }
    public virtual DbSet<CvgsEventRegistration> CvgsEventRegistration { get; set; }
    public virtual DbSet<CvgsEvent> CvgsEvent { get; set; }
    public virtual DbSet<CvgsGenre> CvgsGenre { get; set; }
    public virtual DbSet<CvgsPlatform> CvgsPlatform { get; set; }
    public virtual DbSet<CvgsPaymentMethod> CvgsPaymentMethod { get; set; }
    public virtual DbSet<CvgsWishlistItems> CvgsWishlistItems { get; set; }
    public virtual DbSet<CvgsReview> CvgsReviews { get; set; }
    public virtual DbSet<CvgsLineItem> CvgsLineItem { get; set; }
    public virtual DbSet<CvgsCart> CvgsCart { get; set; }
    public virtual DbSet<CvgsFriendList> CvgsFriendList { get; set; }
    public virtual DbSet<CvgsFriend> CvgsFriends { get; set; }

    public CvgsDbContext(DbContextOptions<CvgsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
      

        builder.ApplyConfiguration(new CvgsUserEntityConfiguration());


        builder.Entity<IdentityRole>().HasData(new IdentityRole 
        { 
            Id = "919d93a8-3810-4ee4-8b51-a6b0a1605508", 
            Name = "Administrator", 
            NormalizedName = "ADMINISTRATOR" 
        });


        // Publishers 


        builder.Entity<CvgsPublisher>().HasData(new CvgsPublisher
        { 
            PublisherId = 1,
            Name = "Activision"
        });

        builder.Entity<CvgsPublisher>().HasData(new CvgsPublisher
        {
            PublisherId = 2,
            Name = "Bandai Namco Entertainment"
        });

        builder.Entity<CvgsPublisher>().HasData(new CvgsPublisher
        {
            PublisherId = 3,
            Name = "Rockstar Games"
        });

        builder.Entity<CvgsPublisher>().HasData(new CvgsPublisher
        {
            PublisherId = 4,
            Name = "Square Enix"
        });


        // Developers

        builder.Entity<CvgsDeveloper>().HasData(new CvgsDeveloper
        {
             DeveloperId = 1,
            Name = "Infinity Ward"
        });

        builder.Entity<CvgsDeveloper>().HasData(new CvgsDeveloper
        {
            DeveloperId = 2,
            Name = "Bandai Namco Studios"
        });

        builder.Entity<CvgsDeveloper>().HasData(new CvgsDeveloper
        {
            DeveloperId = 3,
            Name = "SmileRPG"
        });

        builder.Entity<CvgsDeveloper>().HasData(new CvgsDeveloper
        {
            DeveloperId = 4,
            Name = "Iron Gate Studios"
        });


        // Genres
        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 1,
            Type = "Action"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 2,
            Type = "Adventure"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 3,
            Type = "Puzzle"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 4,
            Type = "RPG"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 5,
            Type = "MMO"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 6,
            Type = "Strategy"
        });

        builder.Entity<CvgsGenre>().HasData(new CvgsGenre
        {
            GenreId = 7,
            Type = "FPS"
        });

        builder.Entity<CvgsPlatform>().HasData(new CvgsPlatform
        {
            PlatformId = 1,
            Name = "Playstation",
            
        });

        builder.Entity<CvgsPlatform>().HasData(new CvgsPlatform
        {
            PlatformId = 2,
            Name = "Windows",
        });

        builder.Entity<CvgsPlatform>().HasData(new CvgsPlatform
        {
            PlatformId = 3,
            Name = "Xbox",
        });
    }
}

public class CvgsUserEntityConfiguration : IEntityTypeConfiguration<CvgsUser>
{
    public void Configure(EntityTypeBuilder<CvgsUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(25).IsRequired(true);
        builder.Property(u => u.LastName).HasMaxLength(25).IsRequired(true);
        builder.Property(u => u.Gender).HasMaxLength(25).IsRequired(true);
        builder.Property(u => u.DateOfBirth).IsRequired(true);
        builder.Property(u => u.MailingAddress).HasMaxLength(50).IsRequired(false);
        builder.Property(u => u.ShippingAddress).HasMaxLength(50).IsRequired(false);
        builder.Property(u => u.ReceivePromotion).IsRequired(true);
    }
}
