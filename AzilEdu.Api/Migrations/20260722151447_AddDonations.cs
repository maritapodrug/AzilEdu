using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzilEdu.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DonationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: true),
                    ItemName = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    DonorId = table.Column<int>(type: "INTEGER", nullable: false),
                    DonationTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DonationStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_DonationStatuses_DonationStatusId",
                        column: x => x.DonationStatusId,
                        principalTable: "DonationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donations_DonationTypes_DonationTypeId",
                        column: x => x.DonationTypeId,
                        principalTable: "DonationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donations_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "DonationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Evidentirana" },
                    { 2, "Potvrđena" },
                    { 3, "Iskorištena" },
                    { 4, "Otkazana" }
                });

            migrationBuilder.InsertData(
                table: "DonationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Novčana" },
                    { 2, "Hrana" },
                    { 3, "Oprema" },
                    { 4, "Lijekovi" },
                    { 5, "Usluga" }
                });

            migrationBuilder.InsertData(
                table: "Donations",
                columns: new[] { "Id", "Amount", "DonationDate", "DonationStatusId", "DonationTypeId", "DonorId", "EstimatedValue", "ItemName", "Notes", "Quantity" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 1, 750m, "Suha hrana za pse", "Donirao 50 kg hrane", 50m },
                    { 2, null, new DateTime(2024, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 2, 1500m, "Kavezi i povodci", "Oprema za prihvat novih životinja", 10m },
                    { 3, 500m, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1, null, "", "Novčana donacija za veterinara", null },
                    { 4, null, new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 5, 4, 800m, "Edukacija volontera", "Udruga organizirala edukaciju", null },
                    { 5, null, new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 3, 400m, "Mokra hrana za mačke", "Doneseno osobno", 100m },
                    { 6, null, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 6, 600m, "Igračke i posteljine", "Sezonska donacija opreme", 20m },
                    { 7, 200m, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 5, null, "", "Prva donacija novog donatora", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonationStatusId",
                table: "Donations",
                column: "DonationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonationTypeId",
                table: "Donations",
                column: "DonationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "DonationStatuses");

            migrationBuilder.DropTable(
                name: "DonationTypes");
        }
    }
}
