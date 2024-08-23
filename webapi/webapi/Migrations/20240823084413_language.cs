using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class language : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hasta");

            migrationBuilder.RenameColumn(
                name: "HastaId",
                table: "Prescription",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Prescription",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "HastaId",
                table: "Examination",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Examination",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Doctor",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "HastaId",
                table: "Diagnosis",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Diagnosis",
                newName: "DoctorId");

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activity = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Prescription",
                newName: "HastaId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Prescription",
                newName: "DoktorId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Examination",
                newName: "HastaId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Examination",
                newName: "DoktorId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Doctor",
                newName: "DoktorId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Diagnosis",
                newName: "HastaId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Diagnosis",
                newName: "DoktorId");

            migrationBuilder.CreateTable(
                name: "Hasta",
                columns: table => new
                {
                    HastaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activity = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hasta", x => x.HastaId);
                });
        }
    }
}
