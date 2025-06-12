using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    NetworkType = table.Column<string>(type: "text", nullable: false),
                    PLMNId = table.Column<string>(type: "text", nullable: true),
                    Lac = table.Column<int>(type: "integer", nullable: true),
                    Tac = table.Column<int>(type: "integer", nullable: true),
                    Rac = table.Column<int>(type: "integer", nullable: true),
                    CellId = table.Column<string>(type: "text", nullable: true),
                    ARFCN = table.Column<int>(type: "integer", nullable: true),
                    FrequencyBand = table.Column<string>(type: "text", nullable: true),
                    ActualFrequencyMhz = table.Column<double>(type: "double precision", nullable: true),
                    SignalStrength = table.Column<int>(type: "integer", nullable: false),
                    RSRP = table.Column<int>(type: "integer", nullable: true),
                    RSRQ = table.Column<int>(type: "integer", nullable: true),
                    RSCP = table.Column<int>(type: "integer", nullable: true),
                    RXLEV = table.Column<int>(type: "integer", nullable: true),
                    ECNO = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                    NetworkInformationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_NetworkMeasurements_NetworkInformationId",
                        column: x => x.NetworkInformationId,
                        principalTable: "NetworkMeasurements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NetworkInformationId",
                table: "Users",
                column: "NetworkInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "NetworkMeasurements");
        }
    }
}
