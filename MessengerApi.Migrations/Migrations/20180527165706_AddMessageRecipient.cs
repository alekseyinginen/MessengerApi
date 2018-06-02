using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MessengerApi.Migrations.Migrations
{
    public partial class AddMessageRecipient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageRecipientId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlackList",
                table: "ClientProfiles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MessageRecipient",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRecipient_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageRecipientId",
                table: "Messages",
                column: "MessageRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecipient_ApplicationUserId",
                table: "MessageRecipient",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageRecipient_MessageRecipientId",
                table: "Messages",
                column: "MessageRecipientId",
                principalTable: "MessageRecipient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageRecipient_MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageRecipient");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "BlackList",
                table: "ClientProfiles");
        }
    }
}
