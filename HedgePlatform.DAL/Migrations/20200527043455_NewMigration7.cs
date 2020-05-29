using Microsoft.EntityFrameworkCore.Migrations;

namespace HedgePlatform.DAL.Migrations
{
    public partial class NewMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteResult_VoteOptionId",
                table: "VoteResult");

            migrationBuilder.CreateIndex(
                name: "IX_VoteResult_VoteOptionId",
                table: "VoteResult",
                column: "VoteOptionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteResult_VoteOptionId",
                table: "VoteResult");

            migrationBuilder.CreateIndex(
                name: "IX_VoteResult_VoteOptionId",
                table: "VoteResult",
                column: "VoteOptionId");
        }
    }
}
