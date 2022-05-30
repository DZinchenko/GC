using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GC.Adapters.EF.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberPlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxDepth = table.Column<double>(type: "float", nullable: false),
                    Volume = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxDepth = table.Column<double>(type: "float", nullable: true),
                    Volume = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    DevEUI = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarSensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarSensors_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinSensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DevEUI = table.Column<int>(type: "int", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinSensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinSensors_Places_BinId",
                        column: x => x.BinId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Distance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Place1Id = table.Column<int>(type: "int", nullable: false),
                    Place2Id = table.Column<int>(type: "int", nullable: false),
                    distanceKm = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Distance_Places_Place1Id",
                        column: x => x.Place1Id,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Distance_Places_Place2Id",
                        column: x => x.Place2Id,
                        principalTable: "Places",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CarAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    AssignmentTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarAssignments_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AssignmentTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarSensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarSensorId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarSensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarSensorReadings_CarSensors_CarSensorId",
                        column: x => x.CarSensorId,
                        principalTable: "CarSensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BinSensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BinSensorId = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinSensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinSensorReadings_BinSensors_BinSensorId",
                        column: x => x.BinSensorId,
                        principalTable: "BinSensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PathParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InPathId = table.Column<int>(type: "int", nullable: false),
                    PathId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    CompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PathParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PathParts_Paths_PathId",
                        column: x => x.PathId,
                        principalTable: "Paths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PathParts_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinSensorReadings_BinSensorId",
                table: "BinSensorReadings",
                column: "BinSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_BinSensors_BinId",
                table: "BinSensors",
                column: "BinId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarAssignments_CarId",
                table: "CarAssignments",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarAssignments_UserId",
                table: "CarAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSensorReadings_CarSensorId",
                table: "CarSensorReadings",
                column: "CarSensorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSensors_CarId",
                table: "CarSensors",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distance_Place1Id",
                table: "Distance",
                column: "Place1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Distance_Place2Id",
                table: "Distance",
                column: "Place2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PathParts_PathId",
                table: "PathParts",
                column: "PathId");

            migrationBuilder.CreateIndex(
                name: "IX_PathParts_PlaceId",
                table: "PathParts",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Paths_UserId",
                table: "Paths",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinSensorReadings");

            migrationBuilder.DropTable(
                name: "CarAssignments");

            migrationBuilder.DropTable(
                name: "CarSensorReadings");

            migrationBuilder.DropTable(
                name: "Distance");

            migrationBuilder.DropTable(
                name: "PathParts");

            migrationBuilder.DropTable(
                name: "BinSensors");

            migrationBuilder.DropTable(
                name: "CarSensors");

            migrationBuilder.DropTable(
                name: "Paths");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
