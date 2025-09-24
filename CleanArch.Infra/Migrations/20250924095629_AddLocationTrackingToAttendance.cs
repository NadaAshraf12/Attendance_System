using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArch.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationTrackingToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckInDeviceInfo",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckInIpAddress",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckInLatitude",
                table: "AttendanceRecords",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckInLongitude",
                table: "AttendanceRecords",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutDeviceInfo",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutIpAddress",
                table: "AttendanceRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLatitude",
                table: "AttendanceRecords",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CheckOutLongitude",
                table: "AttendanceRecords",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInDeviceInfo",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckInIpAddress",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckInLatitude",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckInLongitude",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckOutDeviceInfo",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckOutIpAddress",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckOutLatitude",
                table: "AttendanceRecords");

            migrationBuilder.DropColumn(
                name: "CheckOutLongitude",
                table: "AttendanceRecords");
        }
    }
}
