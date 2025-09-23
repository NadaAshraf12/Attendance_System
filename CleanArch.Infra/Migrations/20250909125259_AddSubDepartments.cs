using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArch.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddSubDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Department",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentDepartmentId",
                table: "Department",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_Code",
                table: "Department",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentDepartmentId",
                table: "Department",
                column: "ParentDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department",
                column: "ParentDepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Department_ParentDepartmentId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_Code",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_ParentDepartmentId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "ParentDepartmentId",
                table: "Department");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Department",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
