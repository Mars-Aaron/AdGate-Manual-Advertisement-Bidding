﻿// <auto-generated />
using System;
using AdGate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdGate.Migrations
{
    [DbContext(typeof(AdGateContext))]
    partial class AdGateContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdGate.Models.AdSpaceListing", b =>
                {
                    b.Property<int>("AdSpaceListingId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdBanner");

                    b.Property<int>("ContentProfileId");

                    b.Property<decimal>("CostPerClick");

                    b.Property<decimal>("CostPerThousand");

                    b.Property<decimal>("CostPerView");

                    b.Property<DateTime>("ListingExpiry");

                    b.Property<int>("Status");

                    b.HasKey("AdSpaceListingId");

                    b.HasIndex("ContentProfileId");

                    b.ToTable("AdSpaceListings");
                });

            modelBuilder.Entity("AdGate.Models.AdSpacePurchase", b =>
                {
                    b.Property<int>("AdvertiserProfileId");

                    b.Property<int>("AdSpaceListingId");

                    b.Property<int>("PaymentModel");

                    b.HasKey("AdvertiserProfileId", "AdSpaceListingId");

                    b.HasAlternateKey("AdSpaceListingId", "AdvertiserProfileId");

                    b.ToTable("AdSpacePurchases");
                });

            modelBuilder.Entity("AdGate.Models.AdvertiserProfileTag", b =>
                {
                    b.Property<int>("AdvertiserProfileId");

                    b.Property<int>("TagId");

                    b.HasKey("AdvertiserProfileId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("AdvertiserProfileTags");
                });

            modelBuilder.Entity("AdGate.Models.ContentProfile", b =>
                {
                    b.Property<int>("ContentProfileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContentMedium");

                    b.Property<string>("ContentName")
                        .IsRequired();

                    b.Property<bool>("IsVerified");

                    b.Property<int>("PublisherProfileId");

                    b.Property<int>("VisitsPerMonth");

                    b.HasKey("ContentProfileId");

                    b.HasIndex("PublisherProfileId");

                    b.ToTable("ContentProfiles");
                });

            modelBuilder.Entity("AdGate.Models.ContentProfileTag", b =>
                {
                    b.Property<int>("ContentProfileId");

                    b.Property<int>("TagId");

                    b.HasKey("ContentProfileId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ContentProfileTags");
                });

            modelBuilder.Entity("AdGate.Models.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("FamilyName")
                        .IsRequired();

                    b.Property<string>("GivenName")
                        .IsRequired();

                    b.Property<string>("ProfilePicture");

                    b.Property<int>("UserId");

                    b.HasKey("ProfileId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profiles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Profile");
                });

            modelBuilder.Entity("AdGate.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeDescription");

                    b.Property<string>("TypeName")
                        .IsRequired();

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("AdGate.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("UserType");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AdGate.Models.AdvertiserProfile", b =>
                {
                    b.HasBaseType("AdGate.Models.Profile");

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.ToTable("AdvertiserProfile");

                    b.HasDiscriminator().HasValue("AdvertiserProfile");
                });

            modelBuilder.Entity("AdGate.Models.PublisherProfile", b =>
                {
                    b.HasBaseType("AdGate.Models.Profile");


                    b.ToTable("PublisherProfile");

                    b.HasDiscriminator().HasValue("PublisherProfile");
                });

            modelBuilder.Entity("AdGate.Models.AdSpaceListing", b =>
                {
                    b.HasOne("AdGate.Models.ContentProfile", "ContentProfile")
                        .WithMany("AdSpaceListings")
                        .HasForeignKey("ContentProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdGate.Models.AdSpacePurchase", b =>
                {
                    b.HasOne("AdGate.Models.AdSpaceListing", "AdSpaceListing")
                        .WithMany("AdSpacePurchases")
                        .HasForeignKey("AdSpaceListingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AdGate.Models.AdvertiserProfile", "AdvertiserProfile")
                        .WithMany("AdSpacePurchases")
                        .HasForeignKey("AdvertiserProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AdGate.Models.AdvertiserProfileTag", b =>
                {
                    b.HasOne("AdGate.Models.AdvertiserProfile", "AdvertiserProfile")
                        .WithMany("AdvertiserProfileTags")
                        .HasForeignKey("AdvertiserProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AdGate.Models.Tag", "Tag")
                        .WithMany("AdvertiserProfileTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdGate.Models.ContentProfile", b =>
                {
                    b.HasOne("AdGate.Models.PublisherProfile", "Owner")
                        .WithMany("ContentProfiles")
                        .HasForeignKey("PublisherProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdGate.Models.ContentProfileTag", b =>
                {
                    b.HasOne("AdGate.Models.ContentProfile", "ContentProfile")
                        .WithMany("ContentProfileTags")
                        .HasForeignKey("ContentProfileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AdGate.Models.Tag", "Tag")
                        .WithMany("ContentProfileTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdGate.Models.Profile", b =>
                {
                    b.HasOne("AdGate.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("AdGate.Models.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
