using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bormech.Server.Liblary.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tanktype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TankCertificationType",
                table: "TankCertifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TankCertificationType",
                table: "TankCertifications");
        }
    }
}
