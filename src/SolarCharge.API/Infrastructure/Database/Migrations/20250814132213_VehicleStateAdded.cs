using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarCharge.API.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VehicleStateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChargeState",
                table: "Vehicles",
                newName: "State");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Vehicles",
                newName: "ChargeState");
        }
    }
}
