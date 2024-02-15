using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DayFlags.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Realms",
                columns: table => new
                {
                    RealmId = table.Column<Guid>(type: "uuid", nullable: false),
                    Owner = table.Column<Guid>(type: "uuid", nullable: true),
                    Label = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realms", x => x.RealmId);
                });

            migrationBuilder.CreateTable(
                name: "FlagGroups",
                columns: table => new
                {
                    FlagGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlagGroupKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SingleFlagPerDay = table.Column<bool>(type: "boolean", nullable: false),
                    RealmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagGroups", x => x.FlagGroupId);
                    table.ForeignKey(
                        name: "FK_FlagGroups_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlagTypes",
                columns: table => new
                {
                    FlagTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlagTypeKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RealmId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlagGroupId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagTypes", x => x.FlagTypeId);
                    table.ForeignKey(
                        name: "FK_FlagTypes_FlagGroups_FlagGroupId",
                        column: x => x.FlagGroupId,
                        principalTable: "FlagGroups",
                        principalColumn: "FlagGroupId");
                    table.ForeignKey(
                        name: "FK_FlagTypes_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "RealmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayFlags",
                columns: table => new
                {
                    FlagId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlagTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Creator = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayFlags", x => x.FlagId);
                    table.ForeignKey(
                        name: "FK_DayFlags_FlagTypes_FlagTypeId",
                        column: x => x.FlagTypeId,
                        principalTable: "FlagTypes",
                        principalColumn: "FlagTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayFlags_Date",
                table: "DayFlags",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_DayFlags_FlagTypeId",
                table: "DayFlags",
                column: "FlagTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlagGroups_FlagGroupKey",
                table: "FlagGroups",
                column: "FlagGroupKey");

            migrationBuilder.CreateIndex(
                name: "IX_FlagGroups_RealmId",
                table: "FlagGroups",
                column: "RealmId");

            migrationBuilder.CreateIndex(
                name: "IX_FlagTypes_FlagGroupId",
                table: "FlagTypes",
                column: "FlagGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FlagTypes_FlagTypeKey",
                table: "FlagTypes",
                column: "FlagTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_FlagTypes_RealmId",
                table: "FlagTypes",
                column: "RealmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayFlags");

            migrationBuilder.DropTable(
                name: "FlagTypes");

            migrationBuilder.DropTable(
                name: "FlagGroups");

            migrationBuilder.DropTable(
                name: "Realms");
        }
    }
}
