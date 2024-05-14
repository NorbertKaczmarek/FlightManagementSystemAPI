using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerLotu = table.Column<int>(type: "int", nullable: false),
                    DataWylotu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MiejsceWylotu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiejscePrzylotu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypSamolotu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
