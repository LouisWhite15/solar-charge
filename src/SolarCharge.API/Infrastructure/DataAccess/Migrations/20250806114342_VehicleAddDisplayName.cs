using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarCharge.API.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VehicleAddDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Vehicles");
        }
    }
}
