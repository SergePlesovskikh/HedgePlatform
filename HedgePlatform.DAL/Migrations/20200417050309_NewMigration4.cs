using Microsoft.EntityFrameworkCore.Migrations;

namespace HedgePlatform.DAL.Migrations
{
    public partial class NewMigration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resident_Flat_FlatId",
                table: "Resident");

            migrationBuilder.DropForeignKey(
                name: "FK_Resident_Flat_FlatId1",
                table: "Resident");

            migrationBuilder.DropIndex(
                name: "IX_Resident_FlatId1",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "FlatId1",
                table: "Resident");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Counter");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Resident",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Resident",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FIO",
                table: "Resident",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Counter",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CounterTypeId",
                table: "Counter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Counter_CounterTypeId",
                table: "Counter",
                column: "CounterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counter_CounterType_CounterTypeId",
                table: "Counter",
                column: "CounterTypeId",
                principalTable: "CounterType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resident_Flat_FlatId",
                table: "Resident",
                column: "FlatId",
                principalTable: "Flat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counter_CounterType_CounterTypeId",
                table: "Counter");

            migrationBuilder.DropForeignKey(
                name: "FK_Resident_Flat_FlatId",
                table: "Resident");

            migrationBuilder.DropIndex(
                name: "IX_Counter_CounterTypeId",
                table: "Counter");

            migrationBuilder.DropColumn(
                name: "CounterTypeId",
                table: "Counter");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Resident",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "FlatId",
                table: "Resident",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "FIO",
                table: "Resident",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "FlatId1",
                table: "Resident",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Counter",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Counter",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Resident_FlatId1",
                table: "Resident",
                column: "FlatId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Resident_Flat_FlatId",
                table: "Resident",
                column: "FlatId",
                principalTable: "Flat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resident_Flat_FlatId1",
                table: "Resident",
                column: "FlatId1",
                principalTable: "Flat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
