using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyLearn.Migrations
{
    /// <inheritdoc />
    public partial class AddPYQSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PYQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedToPYQAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PYQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PYQs_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PYQs_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PYQQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PYQId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Part = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PYQQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PYQQuestions_PYQs_PYQId",
                        column: x => x.PYQId,
                        principalTable: "PYQs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PYQQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PYQQuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PYQQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PYQQuestionOptions_PYQQuestions_PYQQuestionId",
                        column: x => x.PYQQuestionId,
                        principalTable: "PYQQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 13, 9, 3, 33, 362, DateTimeKind.Utc).AddTicks(5037));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 13, 9, 3, 33, 362, DateTimeKind.Utc).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 13, 9, 3, 33, 362, DateTimeKind.Utc).AddTicks(5775));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 13, 9, 3, 33, 362, DateTimeKind.Utc).AddTicks(5776));

            migrationBuilder.CreateIndex(
                name: "IX_PYQQuestionOptions_PYQQuestionId",
                table: "PYQQuestionOptions",
                column: "PYQQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PYQQuestions_PYQId",
                table: "PYQQuestions",
                column: "PYQId");

            migrationBuilder.CreateIndex(
                name: "IX_PYQs_CourseId",
                table: "PYQs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PYQs_ExamId",
                table: "PYQs",
                column: "ExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PYQQuestionOptions");

            migrationBuilder.DropTable(
                name: "PYQQuestions");

            migrationBuilder.DropTable(
                name: "PYQs");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 31, 5, 59, 12, 559, DateTimeKind.Utc).AddTicks(2864));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 31, 5, 59, 12, 559, DateTimeKind.Utc).AddTicks(3432));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 31, 5, 59, 12, 559, DateTimeKind.Utc).AddTicks(3434));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 31, 5, 59, 12, 559, DateTimeKind.Utc).AddTicks(3435));
        }
    }
}
