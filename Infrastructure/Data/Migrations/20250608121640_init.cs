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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStamp = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    NetworkType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLMNId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lac = table.Column<int>(type: "int", nullable: true),
                    Tac = table.Column<int>(type: "int", nullable: true),
                    Rac = table.Column<int>(type: "int", nullable: true),
                    CellId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARFCN = table.Column<int>(type: "int", nullable: true),
                    FrequencyBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualFrequencyMhz = table.Column<double>(type: "float", nullable: true),
                    SignalStrength = table.Column<int>(type: "int", nullable: false),
                    RSRP = table.Column<int>(type: "int", nullable: true),
                    RSRQ = table.Column<int>(type: "int", nullable: true),
                    RSCP = table.Column<int>(type: "int", nullable: true),
                    RXLEV = table.Column<int>(type: "int", nullable: true),
                    ECNO = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    NetworkInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
