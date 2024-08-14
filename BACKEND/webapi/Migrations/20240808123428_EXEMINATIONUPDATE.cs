using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class EXEMINATIONUPDATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Examination_Doctor_DoktorId",
                table: "Examination");

            migrationBuilder.DropForeignKey(
                name: "FK_Examination_Hasta_HastaId",
                table: "Examination");

            migrationBuilder.DropIndex(
                name: "IX_Examination_DoktorId",
                table: "Examination");

            migrationBuilder.DropIndex(
                name: "IX_Examination_HastaId",
                table: "Examination");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Examination_DoktorId",
                table: "Examination",
                column: "DoktorId");

            migrationBuilder.CreateIndex(
                name: "IX_Examination_HastaId",
                table: "Examination",
                column: "HastaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Examination_Doctor_DoktorId",
                table: "Examination",
                column: "DoktorId",
                principalTable: "Doctor",
                principalColumn: "DoktorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Examination_Hasta_HastaId",
                table: "Examination",
                column: "HastaId",
                principalTable: "Hasta",
                principalColumn: "HastaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
