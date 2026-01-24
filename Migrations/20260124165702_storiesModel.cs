using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChampionSteps.Migrations
{
    /// <inheritdoc />
    public partial class storiesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Domain = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PersonName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: false),
                    Highlight = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    TagsCsv = table.Column<string>(type: "TEXT", maxLength: 800, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    SourceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedByName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SubmittedByEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoryMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Kind = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryMedia_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Country",
                table: "Stories",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Domain",
                table: "Stories",
                column: "Domain");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_PersonName",
                table: "Stories",
                column: "PersonName");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_SourceType",
                table: "Stories",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_Visibility",
                table: "Stories",
                column: "Visibility");

            migrationBuilder.CreateIndex(
                name: "IX_StoryMedia_StoryId",
                table: "StoryMedia",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryMedia_Url",
                table: "StoryMedia",
                column: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoryMedia");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
