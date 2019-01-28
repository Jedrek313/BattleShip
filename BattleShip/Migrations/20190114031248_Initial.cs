using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleShip.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    playerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    playerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.playerId);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    gameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    finished = table.Column<bool>(nullable: false),
                    playerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.gameId);
                    table.ForeignKey(
                        name: "FK_Game_Player_playerId",
                        column: x => x.playerId,
                        principalTable: "Player",
                        principalColumn: "playerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Move",
                columns: table => new
                {
                    moveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    playerMove = table.Column<bool>(nullable: false),
                    x = table.Column<int>(nullable: false),
                    y = table.Column<int>(nullable: false),
                    gameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move", x => x.moveId);
                    table.ForeignKey(
                        name: "FK_Move_Game_gameId",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ship",
                columns: table => new
                {
                    shipId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    playerShip = table.Column<bool>(nullable: false),
                    shipNumber = table.Column<int>(nullable: false),
                    x = table.Column<int>(nullable: false),
                    y = table.Column<int>(nullable: false),
                    gameId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ship", x => x.shipId);
                    table.ForeignKey(
                        name: "FK_Ship_Game_gameId",
                        column: x => x.gameId,
                        principalTable: "Game",
                        principalColumn: "gameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_playerId",
                table: "Game",
                column: "playerId");

            migrationBuilder.CreateIndex(
                name: "IX_Move_gameId",
                table: "Move",
                column: "gameId");

            migrationBuilder.CreateIndex(
                name: "IX_Ship_gameId",
                table: "Ship",
                column: "gameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Move");

            migrationBuilder.DropTable(
                name: "Ship");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
