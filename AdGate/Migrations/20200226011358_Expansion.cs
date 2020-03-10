using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdGate.Migrations
{
    public partial class Expansion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Profiles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ContentProfiles",
                columns: table => new
                {
                    ContentProfileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentName = table.Column<string>(nullable: false),
                    ContentMedium = table.Column<int>(nullable: false),
                    VisitsPerMonth = table.Column<int>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    PublisherProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentProfiles", x => x.ContentProfileId);
                    table.ForeignKey(
                        name: "FK_ContentProfiles_Profiles_PublisherProfileId",
                        column: x => x.PublisherProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(nullable: false),
                    TypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "AdSpaceListings",
                columns: table => new
                {
                    AdSpaceListingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdBanner = table.Column<int>(nullable: false),
                    CostPerClick = table.Column<decimal>(nullable: false),
                    CostPerView = table.Column<decimal>(nullable: false),
                    CostPerThousand = table.Column<decimal>(nullable: false),
                    ListingExpiry = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ContentProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdSpaceListings", x => x.AdSpaceListingId);
                    table.ForeignKey(
                        name: "FK_AdSpaceListings_ContentProfiles_ContentProfileId",
                        column: x => x.ContentProfileId,
                        principalTable: "ContentProfiles",
                        principalColumn: "ContentProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertiserProfileTags",
                columns: table => new
                {
                    AdvertiserProfileId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertiserProfileTags", x => new { x.AdvertiserProfileId, x.TagId });
                    table.ForeignKey(
                        name: "FK_AdvertiserProfileTags_Profiles_AdvertiserProfileId",
                        column: x => x.AdvertiserProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertiserProfileTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentProfileTags",
                columns: table => new
                {
                    ContentProfileId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentProfileTags", x => new { x.ContentProfileId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ContentProfileTags_ContentProfiles_ContentProfileId",
                        column: x => x.ContentProfileId,
                        principalTable: "ContentProfiles",
                        principalColumn: "ContentProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentProfileTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdSpacePurchases",
                columns: table => new
                {
                    AdvertiserProfileId = table.Column<int>(nullable: false),
                    AdSpaceListingId = table.Column<int>(nullable: false),
                    PaymentModel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdSpacePurchases", x => new { x.AdvertiserProfileId, x.AdSpaceListingId });
                    table.UniqueConstraint("AK_AdSpacePurchases_AdSpaceListingId_AdvertiserProfileId", x => new { x.AdSpaceListingId, x.AdvertiserProfileId });
                    table.ForeignKey(
                        name: "FK_AdSpacePurchases_AdSpaceListings_AdSpaceListingId",
                        column: x => x.AdSpaceListingId,
                        principalTable: "AdSpaceListings",
                        principalColumn: "AdSpaceListingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdSpacePurchases_Profiles_AdvertiserProfileId",
                        column: x => x.AdvertiserProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdSpaceListings_ContentProfileId",
                table: "AdSpaceListings",
                column: "ContentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertiserProfileTags_TagId",
                table: "AdvertiserProfileTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentProfiles_PublisherProfileId",
                table: "ContentProfiles",
                column: "PublisherProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentProfileTags_TagId",
                table: "ContentProfileTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdSpacePurchases");

            migrationBuilder.DropTable(
                name: "AdvertiserProfileTags");

            migrationBuilder.DropTable(
                name: "ContentProfileTags");

            migrationBuilder.DropTable(
                name: "AdSpaceListings");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ContentProfiles");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Profiles");
        }
    }
}
