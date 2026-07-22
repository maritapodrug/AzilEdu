using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AzilEdu.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Donors",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "DonorStatusId", "DonorTypeId", "Email", "FirstName", "LastName", "Notes", "OrganizationName", "Phone" },
                values: new object[,]
                {
                    { 1, "Ilica 42", "Zagreb", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, "stjepan.blazevic@gmail.com", "Stjepan", "Blažević", "Redoviti donator hrane", "", "091 555 1234" },
                    { 2, "Avenija Dubrovnik 15", "Zagreb", new DateTime(2023, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "donacije@zooplus.hr", "", "", "Donira hranu i opremu kvartalno", "Zooplus Hrvatska d.o.o.", "01 234 5678" },
                    { 3, "Vukovarska 88", "Split", new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "karin.simic@outlook.com", "Karin", "Šimić", "Donira povremeno, kontaktirati pred kampanje", "", "095 999 4455" },
                    { 4, "Tratinska 10", "Zagreb", new DateTime(2024, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, "info@prijatelji-zivotinja.hr", "", "", "Suradnja na edukacijskim projektima", "Udruga Prijatelji životinja", "01 987 6543" },
                    { 5, "Maksimirska 60", "Zagreb", new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "boris.kralj@gmail.com", "Boris", "Kralj", "Novi donator, uplatio prvu donaciju", "", "092 111 3344" },
                    { 6, "Riva 22", "Pula", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, "marketing@petcenter.hr", "", "", "Donira opremu i igračke", "Petcenter d.o.o.", "052 345 6789" },
                    { 7, "Korzo 5", "Rijeka", new DateTime(2022, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "nikolina.djukic@yahoo.com", "Nikolina", "Đukić", "Prestala donirati 2024.", "", "098 222 7788" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "EmployeeNumber", "EmployeePositionId", "EmployeeStatusId", "FirstName", "HireDate", "LastName", "Notes", "Phone" },
                values: new object[,]
                {
                    { 1, "renata.stefanec@azil.hr", "AZ-001", 1, 1, "Renata", new DateTime(2019, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Štefanec", "Odgovorna za smještaj i ishranu životinja", "091 700 1001" },
                    { 2, "ivan.posavec@azil.hr", "AZ-002", 2, 1, "Dr. Ivan", new DateTime(2020, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Posavec", "Stalni veterinar, radi pon-pet", "091 700 1002" },
                    { 3, "sandra.filipovic@azil.hr", "AZ-003", 3, 1, "Sandra", new DateTime(2021, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filipović", "Koordinira raspored volontera", "091 700 1003" },
                    { 4, "goran.tkalcic@azil.hr", "AZ-004", 4, 1, "Goran", new DateTime(2018, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tkalčić", "Vodi administraciju i financije", "091 700 1004" },
                    { 5, "mirela.vukovic@azil.hr", "AZ-005", 1, 2, "Mirela", new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vuković", "Brine o mačkama i malim životinjama", "091 700 1005" },
                    { 6, "davor.knezevic@azil.hr", "AZ-006", 1, 1, "Davor", new DateTime(2023, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Knežević", "Zamjenski djelatnik, radi na terenu", "091 700 1006" }
                });

            migrationBuilder.InsertData(
                table: "Volunteers",
                columns: new[] { "Id", "AvailableFrom", "Email", "FirstName", "LastName", "Notes", "Phone", "Skills", "VolunteerStatusId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ana.horvat@email.com", "Ana", "Horvat", "Dostupna vikendom", "091 234 5678", "Briga o psima, šetanje", 2 },
                    { 2, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "marko.peric@email.com", "Marko", "Perić", "Fotograf volonter", "098 765 4321", "Fotografija životinja, društvene mreže", 2 },
                    { 3, new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "ivana.kovac@email.com", "Ivana", "Kovač", "Može pomagati pri veterinarskim pregledima", "095 111 2233", "Veterinarski tehničar", 2 },
                    { 4, null, "tomislav.babic@email.com", "Tomislav", "Babić", "Privremeno nedostupan zbog posla", "092 444 5566", "Edukacija životinja, dresura", 3 },
                    { 5, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "petra.novak@email.com", "Petra", "Novak", "Novi volonter, u uvođenju", "099 888 7766", "Administracija, pisanje molbi", 1 },
                    { 6, new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "luka.maric@email.com", "Luka", "Marić", "Može prevoziti životinje na veterinara", "091 321 6549", "Transport životinja, vozač", 2 },
                    { 7, null, "maja.juric@email.com", "Maja", "Jurić", "Završila volontiranje", "098 654 3210", "Šivanje, izrada igračaka za životinje", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Volunteers",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
