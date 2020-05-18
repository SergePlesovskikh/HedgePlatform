using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HedgePlatform.DAL.Migrations
{
    public partial class NewMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Resident");

            migrationBuilder.AddColumn<bool>(
                name: "Chairman",
                table: "Resident",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Owner",
                table: "Resident",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhoneId",
                table: "Resident",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResidentStatus",
                table: "Resident",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Check",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Phone = table.Column<string>(nullable: false),
                    CheckCode = table.Column<int>(nullable: false),
                    SendTime = table.Column<DateTime>(nullable: false),
                    token = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Check", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Header = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    DateMessage = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(nullable: false),
                    Psw = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteOption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoteId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteOption_Message_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uid = table.Column<string>(nullable: false),
                    PhoneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Phone_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoteResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoteOptionId = table.Column<int>(nullable: false),
                    ResidentId = table.Column<int>(nullable: false),
                    DateVote = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteResult_Resident_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Resident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoteResult_VoteOption_VoteOptionId",
                        column: x => x.VoteOptionId,
                        principalTable: "VoteOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resident_PhoneId",
                table: "Resident",
                column: "PhoneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_PhoneId",
                table: "Session",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteOption_VoteId",
                table: "VoteOption",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteResult_ResidentId",
                table: "VoteResult",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteResult_VoteOptionId",
                table: "VoteResult",
                column: "VoteOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resident_Phone_PhoneId",
                table: "Resident",
                column: "PhoneId",
                principalTable: "Phone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resident_Phone_PhoneId",
                table: "Resident");

            migrationBuilder.DropTable(
                name: "Check");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "VoteResult");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "VoteOption");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Resident_PhoneId",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "Chairman",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "PhoneId",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "ResidentStatus",
                table: "Resident");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Resident",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
