using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MessengerApi.Migrations.Migrations
{
    public partial class RemoveBlackListFromClientProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecipient_AspNetUsers_ApplicationUserId",
                table: "MessageRecipient");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageRecipient_MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageRecipient",
                table: "MessageRecipient");

            migrationBuilder.DropColumn(
                name: "BlackList",
                table: "ClientProfiles");

            migrationBuilder.RenameTable(
                name: "MessageRecipient",
                newName: "Recipients");

            migrationBuilder.RenameIndex(
                name: "IX_MessageRecipient_ApplicationUserId",
                table: "Recipients",
                newName: "IX_Recipients_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recipients",
                table: "Recipients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Recipients_MessageRecipientId",
                table: "Messages",
                column: "MessageRecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_AspNetUsers_ApplicationUserId",
                table: "Recipients",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Recipients_MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_AspNetUsers_ApplicationUserId",
                table: "Recipients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recipients",
                table: "Recipients");

            migrationBuilder.RenameTable(
                name: "Recipients",
                newName: "MessageRecipient");

            migrationBuilder.RenameIndex(
                name: "IX_Recipients_ApplicationUserId",
                table: "MessageRecipient",
                newName: "IX_MessageRecipient_ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "BlackList",
                table: "ClientProfiles",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageRecipient",
                table: "MessageRecipient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecipient_AspNetUsers_ApplicationUserId",
                table: "MessageRecipient",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageRecipient_MessageRecipientId",
                table: "Messages",
                column: "MessageRecipientId",
                principalTable: "MessageRecipient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
