using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KASHOP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateCraetedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "categories");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedById",
                table: "categories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "categories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_categories_CreatedById",
                table: "categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_categories_UpdatedById",
                table: "categories",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_Users_CreatedById",
                table: "categories",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_Users_UpdatedById",
                table: "categories",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_Users_CreatedById",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_categories_Users_UpdatedById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_CreatedById",
                table: "categories");

            migrationBuilder.DropIndex(
                name: "IX_categories_UpdatedById",
                table: "categories");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedById",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
