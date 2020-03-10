using Microsoft.EntityFrameworkCore;
using AdGate.Models;

namespace AdGate.Data
{
    public class AdGateContext : DbContext
    {
        public AdGateContext(DbContextOptions<AdGateContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<PublisherProfile> PublisherProfiles { get; set; }
        public DbSet<AdvertiserProfile> AdvertiserProfiles { get; set; }
        public DbSet<ContentProfile> ContentProfiles { get; set; }
        public DbSet<AdSpaceListing> AdSpaceListings { get; set; }
        public DbSet<AdSpacePurchase> AdSpacePurchases { get; set; }
        public DbSet<AdvertiserProfileTag> AdvertiserProfileTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ContentProfileTag> ContentProfileTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PublisherProfile-ContentProfile 1:M Relationship Config
            modelBuilder.Entity<ContentProfile>()
                .HasOne(cp => cp.Owner)
                .WithMany(pp => pp.ContentProfiles)
                .HasForeignKey(cp => cp.PublisherProfileId);

            // AdSpaceListings-ContentProfile 1:M Relationship Config
            modelBuilder.Entity<AdSpaceListing>()
                .HasOne(asl => asl.ContentProfile)
                .WithMany(cp => cp.AdSpaceListings)
                .HasForeignKey(asl => asl.ContentProfileId);

            // ContentProfileTag M-N Relationship Config
            modelBuilder.Entity<ContentProfileTag>()
                .HasKey(cpt => new { cpt.ContentProfileId, cpt.TagId });
            modelBuilder.Entity<ContentProfileTag>()
                .HasOne(cpt => cpt.ContentProfile)
                .WithMany(cp => cp.ContentProfileTags)
                .HasForeignKey(cpt => cpt.ContentProfileId);
            modelBuilder.Entity<ContentProfileTag>()
                .HasOne(cpt => cpt.Tag)
                .WithMany(t => t.ContentProfileTags)
                .HasForeignKey(cpt => cpt.TagId);

            // AdvertiserProfileTag M-N Relationship Config
            modelBuilder.Entity<AdvertiserProfileTag>()
                .HasKey(apt => new { apt.AdvertiserProfileId, apt.TagId });
            modelBuilder.Entity<AdvertiserProfileTag>()
                .HasOne(apt => apt.AdvertiserProfile)
                .WithMany(ap => ap.AdvertiserProfileTags)
                .HasForeignKey(apt => apt.AdvertiserProfileId);
            modelBuilder.Entity<AdvertiserProfileTag>()
                .HasOne(apt => apt.Tag)
                .WithMany(t => t.AdvertiserProfileTags)
                .HasForeignKey(apt => apt.TagId);

            // AdSpacePurchase M-N Relationship Config
            modelBuilder.Entity<AdSpacePurchase>()
                .HasKey(asp => new { asp.AdvertiserProfileId, asp.AdSpaceListingId });
            modelBuilder.Entity<AdSpacePurchase>()
                .HasOne(asp => asp.AdvertiserProfile)
                .WithMany(ap => ap.AdSpacePurchases)
                .HasForeignKey(asp => asp.AdvertiserProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AdSpacePurchase>()
                .HasOne(asp => asp.AdSpaceListing)
                .WithMany(asl => asl.AdSpacePurchases)
                .HasForeignKey(asp => asp.AdSpaceListingId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserProfile 1:1 Relationship Config
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            // User index
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
