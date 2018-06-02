using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MessengerApi.Migrations.Migrations
{
    public partial class AddGroupAndGroupUserEntitiesDeleateRecipier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Recipients_MessageRecipientId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.RenameColumn(
                name: "MessageRecipientId",
                table: "Messages",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageRecipientId",
                table: "Messages",
                newName: "IX_Messages_GroupId");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ApplicationUserId",
                table: "Groups",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_ApplicationUserId",
                table: "GroupUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Groups_GroupId",
                table: "Messages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Groups_GroupId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Messages",
                newName: "MessageRecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_GroupId",
                table: "Messages",
                newName: "IX_Messages_MessageRecipientId");

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipients_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_ApplicationUserId",
                table: "Recipients",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Recipients_MessageRecipientId",
                table: "Messages",
                column: "MessageRecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
