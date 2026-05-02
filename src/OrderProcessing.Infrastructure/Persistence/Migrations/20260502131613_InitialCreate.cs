using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderProcessing.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrentStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "work_order_state_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ChangedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CompletedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_order_state_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_order_state_history_work_orders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "work_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_work_order_state_history_WorkOrderId_ChangedUtc",
                table: "work_order_state_history",
                columns: new[] { "WorkOrderId", "ChangedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_work_orders_ExternalReference",
                table: "work_orders",
                column: "ExternalReference",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_order_state_history");

            migrationBuilder.DropTable(
                name: "work_orders");
        }
    }
}
