using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SummerStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_images_products_product_id",
                table: "product_images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_product_images",
                table: "product_images");

            migrationBuilder.DropIndex(
                name: "ix_product_images_product_id_sort_order",
                table: "product_images");

            migrationBuilder.DropColumn(
                name: "image",
                table: "product_images");

            migrationBuilder.RenameTable(
                name: "product_images",
                newName: "documents");

            migrationBuilder.AlterColumn<int>(
                name: "sort_order",
                table: "documents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "documents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "product_id2",
                table: "documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<long>(
                name: "size",
                table: "documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "pk_documents",
                table: "documents",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_product_id",
                table: "documents",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_product_id2",
                table: "documents",
                column: "product_id2");

            migrationBuilder.AddForeignKey(
                name: "fk_documents_products_product_id",
                table: "documents",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_documents_products_product_id2",
                table: "documents",
                column: "product_id2",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_documents_products_product_id",
                table: "documents");

            migrationBuilder.DropForeignKey(
                name: "fk_documents_products_product_id2",
                table: "documents");

            migrationBuilder.DropPrimaryKey(
                name: "pk_documents",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_product_id",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "ix_documents_product_id2",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "discriminator",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "name",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "product_id2",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "size",
                table: "documents");

            migrationBuilder.RenameTable(
                name: "documents",
                newName: "product_images");

            migrationBuilder.AlterColumn<int>(
                name: "sort_order",
                table: "product_images",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "product_images",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "product_images",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "pk_product_images",
                table: "product_images",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_product_images_product_id_sort_order",
                table: "product_images",
                columns: new[] { "product_id", "sort_order" });

            migrationBuilder.AddForeignKey(
                name: "fk_product_images_products_product_id",
                table: "product_images",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
