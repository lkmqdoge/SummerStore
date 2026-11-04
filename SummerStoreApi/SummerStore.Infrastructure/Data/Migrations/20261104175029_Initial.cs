using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SummerStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_products_product_id2",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_product_id2",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "product_id2",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "documents");

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "documents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "file_path",
                table: "documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "product_user",
                columns: table => new
                {
                    owned_products_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owners_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_user", x => new { x.owned_products_id, x.owners_id });
                    table.ForeignKey(
                        name: "fk_product_user_products_owned_products_id",
                        column: x => x.owned_products_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_user_users_owners_id",
                        column: x => x.owners_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_products_creator_id",
                table: "products",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_user_owners_id",
                table: "product_user",
                column: "owners_id");

            migrationBuilder.AddForeignKey(
                name: "fk_products_users_creator_id",
                table: "products",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_users_creator_id",
                table: "products");

            migrationBuilder.DropTable(
                name: "product_user");

            migrationBuilder.DropIndex(
                name: "ix_products_creator_id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "file_path",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "type",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "documents");

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "documents",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "discriminator",
                table: "documents",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "product_id2",
                table: "documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "documents",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_documents_product_id2",
                table: "documents",
                column: "product_id2");

            migrationBuilder.AddForeignKey(
                name: "fk_documents_products_product_id2",
                table: "documents",
                column: "product_id2",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
