using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventAttendeeJoinEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendees_Attendees_AttendeesId",
                table: "EventAttendees");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendees_Events_RegisteredEventsId",
                table: "EventAttendees");

            migrationBuilder.RenameColumn(
                name: "RegisteredEventsId",
                table: "EventAttendees",
                newName: "AttendeeId");

            migrationBuilder.RenameColumn(
                name: "AttendeesId",
                table: "EventAttendees",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendees_RegisteredEventsId",
                table: "EventAttendees",
                newName: "IX_EventAttendees_AttendeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendees_Attendees_AttendeeId",
                table: "EventAttendees",
                column: "AttendeeId",
                principalTable: "Attendees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendees_Events_EventId",
                table: "EventAttendees",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendees_Attendees_AttendeeId",
                table: "EventAttendees");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendees_Events_EventId",
                table: "EventAttendees");

            migrationBuilder.RenameColumn(
                name: "AttendeeId",
                table: "EventAttendees",
                newName: "RegisteredEventsId");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "EventAttendees",
                newName: "AttendeesId");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendees_AttendeeId",
                table: "EventAttendees",
                newName: "IX_EventAttendees_RegisteredEventsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendees_Attendees_AttendeesId",
                table: "EventAttendees",
                column: "AttendeesId",
                principalTable: "Attendees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendees_Events_RegisteredEventsId",
                table: "EventAttendees",
                column: "RegisteredEventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
