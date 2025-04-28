using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGP.Infrastucture.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResultadoLaboratorioEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConsultorioId",
                table: "ResultadosLaboratorio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ResultadosLaboratorio",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsultorioId",
                table: "ResultadosLaboratorio");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ResultadosLaboratorio");
        }
    }
}
