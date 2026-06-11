using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Registration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BuildingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FlatNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Cairo" },
                    { 2, true, "Giza" },
                    { 3, true, "Alexandria" },
                    { 4, true, "Qalyubia" },
                    { 5, true, "Sharqia" },
                    { 6, true, "Dakahlia" },
                    { 7, true, "Beheira" },
                    { 8, true, "Minya" },
                    { 9, true, "Qena" },
                    { 10, true, "Sohag" },
                    { 11, true, "Assiut" },
                    { 12, true, "Faiyum" },
                    { 13, true, "Gharbia" },
                    { 14, true, "Monufia" },
                    { 15, true, "Beni Suef" },
                    { 16, true, "Damietta" },
                    { 17, true, "Port Said" },
                    { 18, true, "Ismailia" },
                    { 19, true, "Suez" },
                    { 20, true, "Aswan" },
                    { 21, true, "Luxor" },
                    { 22, true, "Red Sea" },
                    { 23, true, "New Valley" },
                    { 24, true, "Matrouh" },
                    { 25, true, "North Sinai" },
                    { 26, true, "South Sinai" },
                    { 27, true, "Kafr El Sheikh" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "GovernorateId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 101, 1, true, "Nasr City" },
                    { 102, 1, true, "Maadi" },
                    { 103, 1, true, "Heliopolis" },
                    { 104, 1, true, "Downtown Cairo" },
                    { 105, 1, true, "Helwan" },
                    { 106, 1, true, "Shubra" },
                    { 107, 1, true, "New Cairo" },
                    { 108, 1, true, "Madinaty" },
                    { 201, 2, true, "Dokki" },
                    { 202, 2, true, "Mohandessin" },
                    { 203, 2, true, "6th of October" },
                    { 204, 2, true, "Haram" },
                    { 205, 2, true, "Imbaba" },
                    { 206, 2, true, "Faisal" },
                    { 207, 2, true, "Sheikh Zayed" },
                    { 208, 2, true, "Badrasheen" },
                    { 301, 3, true, "Sidi Gaber" },
                    { 302, 3, true, "Smouha" },
                    { 303, 3, true, "Miami" },
                    { 304, 3, true, "Montaza" },
                    { 305, 3, true, "Borg El Arab" },
                    { 306, 3, true, "Agami" },
                    { 307, 3, true, "Sidi Bishr" },
                    { 308, 3, true, "Mansheya" },
                    { 401, 4, true, "Banha" },
                    { 402, 4, true, "Shubra El-Kheima" },
                    { 403, 4, true, "Qalyub" },
                    { 404, 4, true, "Qaha" },
                    { 405, 4, true, "Khanka" },
                    { 406, 4, true, "Tukh" },
                    { 407, 4, true, "Kafr Shukr" },
                    { 408, 4, true, "Obour" },
                    { 501, 5, true, "Zagazig" },
                    { 502, 5, true, "10th of Ramadan City" },
                    { 503, 5, true, "Belbeis" },
                    { 504, 5, true, "Abu Hammad" },
                    { 505, 5, true, "Faqous" },
                    { 506, 5, true, "Minya Al Qamh" },
                    { 507, 5, true, "Husseiniya" },
                    { 508, 5, true, "Diyarb Negm" },
                    { 601, 6, true, "Mansoura" },
                    { 602, 6, true, "Talkha" },
                    { 603, 6, true, "Mit Ghamr" },
                    { 604, 6, true, "Dikirnis" },
                    { 605, 6, true, "Aga" },
                    { 606, 6, true, "Belqas" },
                    { 607, 6, true, "Sherbin" },
                    { 608, 6, true, "Mit Salsil" },
                    { 701, 7, true, "Damanhur" },
                    { 702, 7, true, "Kafr El Dawwar" },
                    { 703, 7, true, "Rashid" },
                    { 704, 7, true, "Edku" },
                    { 705, 7, true, "Abu Hummus" },
                    { 706, 7, true, "Itay El Baroud" },
                    { 707, 7, true, "Hosh Issa" },
                    { 708, 7, true, "Kom Hamada" },
                    { 801, 8, true, "Minya" },
                    { 802, 8, true, "Mallawi" },
                    { 803, 8, true, "Beni Mazar" },
                    { 804, 8, true, "Maghagha" },
                    { 805, 8, true, "Samalut" },
                    { 806, 8, true, "Matai" },
                    { 807, 8, true, "Abu Qurqas" },
                    { 808, 8, true, "Deir Mawas" },
                    { 901, 9, true, "Qena" },
                    { 902, 9, true, "Naqada" },
                    { 903, 9, true, "Qus" },
                    { 904, 9, true, "Nag Hammadi" },
                    { 905, 9, true, "Dishna" },
                    { 906, 9, true, "Abu Tesht" },
                    { 907, 9, true, "Farshut" },
                    { 908, 9, true, "Qift" },
                    { 1001, 10, true, "Sohag" },
                    { 1002, 10, true, "Akhmim" },
                    { 1003, 10, true, "Girga" },
                    { 1004, 10, true, "Tahta" },
                    { 1005, 10, true, "Tama" },
                    { 1006, 10, true, "Maragha" },
                    { 1007, 10, true, "El Balyana" },
                    { 1008, 10, true, "Dar El Salam" },
                    { 1101, 11, true, "Assiut" },
                    { 1102, 11, true, "Dairut" },
                    { 1103, 11, true, "Manfalut" },
                    { 1104, 11, true, "Abnub" },
                    { 1105, 11, true, "Abu Tig" },
                    { 1106, 11, true, "Sahel Selim" },
                    { 1107, 11, true, "Al Ghanayem" },
                    { 1108, 11, true, "Al Badari" },
                    { 1201, 12, true, "Faiyum" },
                    { 1202, 12, true, "Ibsheway" },
                    { 1203, 12, true, "Senuris" },
                    { 1204, 12, true, "Tamiya" },
                    { 1205, 12, true, "Itsa" },
                    { 1206, 12, true, "Yousef El Seddik" },
                    { 1301, 13, true, "Tanta" },
                    { 1302, 13, true, "El Mahalla El Kubra" },
                    { 1303, 13, true, "Kafr El Zayat" },
                    { 1304, 13, true, "Zefta" },
                    { 1305, 13, true, "Samanoud" },
                    { 1306, 13, true, "Qutour" },
                    { 1307, 13, true, "Basyoun" },
                    { 1308, 13, true, "El Santa" },
                    { 1401, 14, true, "Shibin El Kom" },
                    { 1402, 14, true, "Sadat City" },
                    { 1403, 14, true, "Menouf" },
                    { 1404, 14, true, "Ashmoun" },
                    { 1405, 14, true, "Quesna" },
                    { 1406, 14, true, "Tala" },
                    { 1407, 14, true, "Berket El Sab" },
                    { 1408, 14, true, "El Bagour" },
                    { 1501, 15, true, "Beni Suef" },
                    { 1502, 15, true, "Al Wasta" },
                    { 1503, 15, true, "Nasser" },
                    { 1504, 15, true, "Ihnasia" },
                    { 1505, 15, true, "Biba" },
                    { 1506, 15, true, "Al Fashn" },
                    { 1507, 15, true, "Sumusta" },
                    { 1601, 16, true, "Damietta" },
                    { 1602, 16, true, "New Damietta" },
                    { 1603, 16, true, "Faraskur" },
                    { 1604, 16, true, "Kafr Saad" },
                    { 1605, 16, true, "Zarqa" },
                    { 1606, 16, true, "Ras El Bar" },
                    { 1607, 16, true, "Kafr El Batikh" },
                    { 1701, 17, true, "Port Said" },
                    { 1702, 17, true, "Port Fouad" },
                    { 1703, 17, true, "Al Dawahy" },
                    { 1704, 17, true, "Al Zohour" },
                    { 1705, 17, true, "Al Arab" },
                    { 1706, 17, true, "Al Manakh" },
                    { 1801, 18, true, "Ismailia" },
                    { 1802, 18, true, "Fayed" },
                    { 1803, 18, true, "Qantara Sharq" },
                    { 1804, 18, true, "Qantara Gharb" },
                    { 1805, 18, true, "Tal El Kebir" },
                    { 1806, 18, true, "Abu Suwir" },
                    { 1901, 19, true, "Suez" },
                    { 1902, 19, true, "Ataqah" },
                    { 1903, 19, true, "Arbaeen" },
                    { 1904, 19, true, "Faisal" },
                    { 1905, 19, true, "Ganayen" },
                    { 2001, 20, true, "Aswan" },
                    { 2002, 20, true, "Kom Ombo" },
                    { 2003, 20, true, "Edfu" },
                    { 2004, 20, true, "Daraw" },
                    { 2005, 20, true, "Nasr El Nuba" },
                    { 2006, 20, true, "Abu Simbel" },
                    { 2007, 20, true, "Kalabsha" },
                    { 2101, 21, true, "Luxor" },
                    { 2102, 21, true, "Esna" },
                    { 2103, 21, true, "Armant" },
                    { 2104, 21, true, "Tod" },
                    { 2105, 21, true, "Qurna" },
                    { 2106, 21, true, "Bayadeyah" },
                    { 2201, 22, true, "Hurghada" },
                    { 2202, 22, true, "Safaga" },
                    { 2203, 22, true, "El Qoseir" },
                    { 2204, 22, true, "Marsa Alam" },
                    { 2205, 22, true, "Ras Ghareb" },
                    { 2206, 22, true, "Shalateen" },
                    { 2301, 23, true, "Kharga" },
                    { 2302, 23, true, "Dakhla" },
                    { 2303, 23, true, "Farafra" },
                    { 2304, 23, true, "Paris" },
                    { 2305, 23, true, "Balat" },
                    { 2401, 24, true, "Marsa Matrouh" },
                    { 2402, 24, true, "El Hammam" },
                    { 2403, 24, true, "El Dabaa" },
                    { 2404, 24, true, "Sidi Barrani" },
                    { 2405, 24, true, "Siwa" },
                    { 2406, 24, true, "El Negila" },
                    { 2501, 25, true, "Arish" },
                    { 2502, 25, true, "Sheikh Zuweid" },
                    { 2503, 25, true, "Rafah" },
                    { 2504, 25, true, "Bir al-Abd" },
                    { 2505, 25, true, "Nakhl" },
                    { 2506, 25, true, "Hasana" },
                    { 2601, 26, true, "Sharm El Sheikh" },
                    { 2602, 26, true, "Dahab" },
                    { 2603, 26, true, "Tor Sinai" },
                    { 2604, 26, true, "Nuweiba" },
                    { 2605, 26, true, "Saint Catherine" },
                    { 2606, 26, true, "Taba" },
                    { 2701, 27, true, "Kafr El Sheikh" },
                    { 2702, 27, true, "Desouk" },
                    { 2703, 27, true, "Fuwwah" },
                    { 2704, 27, true, "Qallin" },
                    { 2705, 27, true, "Sidi Salem" },
                    { 2706, 27, true, "Baltim" },
                    { 2707, 27, true, "Hamoul" },
                    { 2708, 27, true, "Mutubas" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_GovernorateId",
                table: "Addresses",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_RegistrationId",
                table: "Addresses",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovernorateId",
                table: "Cities",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_GovernorateId_Name",
                table: "Cities",
                columns: new[] { "GovernorateId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name",
                table: "Governorates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Registrations_MobileNumber",
                table: "Registrations",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Registrations_NormalizedEmail",
                table: "Registrations",
                column: "NormalizedEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Governorates");
        }
    }
}
