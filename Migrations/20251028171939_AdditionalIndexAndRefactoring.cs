using ElderConnectApi.Data.Common;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElderConnectApi.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalIndexAndRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Gender>(
                name: "gender",
                table: "patients",
                type: "gender",
                nullable: false,
                defaultValue: Gender.Male);

            migrationBuilder.CreateIndex(
                name: "ix_bookings_end_time",
                table: "bookings",
                column: "end_time");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_start_time",
                table: "bookings",
                column: "start_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bookings_end_time",
                table: "bookings");

            migrationBuilder.DropIndex(
                name: "ix_bookings_start_time",
                table: "bookings");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "patients");
        }
    }
}
