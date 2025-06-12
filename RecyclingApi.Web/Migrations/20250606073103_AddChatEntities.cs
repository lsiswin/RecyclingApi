using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecyclingApi.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChatEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: false),
                    CurrentMemberCount = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdminIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WelcomeMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Announcement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AnnouncementTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequirePassword = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    AllowFileUpload = table.Column<bool>(type: "bit", nullable: false),
                    AllowedFileTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxFileSize = table.Column<long>(type: "bigint", nullable: false),
                    MessageRetentionDays = table.Column<int>(type: "int", nullable: false),
                    EnableModeration = table.Column<bool>(type: "bit", nullable: false),
                    BannedWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastActiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalMessageCount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RealName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsCustomerService = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastOnlineTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActiveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConnectionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FirstVisitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitCount = table.Column<int>(type: "int", nullable: false),
                    PageVisitStats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preferences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMuted = table.Column<bool>(type: "bit", nullable: false),
                    MutedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MuteReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsBanned = table.Column<bool>(type: "bit", nullable: false),
                    BannedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BanReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.Id);
                    table.UniqueConstraint("AK_ChatUsers_UserId", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserAvatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomerServiceId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProblem = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Solution = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    IsBotSession = table.Column<bool>(type: "bit", nullable: false),
                    BotId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActiveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMessageTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastMessageContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastMessageSender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MessageCount = table.Column<int>(type: "int", nullable: false),
                    UnreadCount = table.Column<int>(type: "int", nullable: false),
                    SatisfactionRating = table.Column<int>(type: "int", nullable: true),
                    UserFeedback = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FeedbackTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    AverageResponseTime = table.Column<int>(type: "int", nullable: true),
                    FirstResponseTime = table.Column<int>(type: "int", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserIpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArchivedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatSessions_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatSessions_ChatUsers_CustomerServiceId",
                        column: x => x.CustomerServiceId,
                        principalTable: "ChatUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ChatSessions_ChatUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ChatUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SenderAvatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    TargetUserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TargetUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReplyToMessageId = table.Column<int>(type: "int", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadByUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EditHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeleteReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    PinnedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PinnedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reactions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderIpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SenderUserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatMessages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "ChatSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "ChatUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatRoomId",
                table: "ChatMessages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_MessageId",
                table: "ChatMessages",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReplyToMessageId",
                table: "ChatMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SessionId",
                table: "ChatMessages",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_Timestamp",
                table: "ChatMessages",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_ChatRoomId",
                table: "ChatSessions",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_CustomerServiceId",
                table: "ChatSessions",
                column: "CustomerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_SessionId",
                table: "ChatSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_StartTime",
                table: "ChatSessions",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_Status",
                table: "ChatSessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_UserId",
                table: "ChatSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_ConnectionId",
                table: "ChatUsers",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_UserId",
                table: "ChatUsers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatSessions");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "ChatUsers");
        }
    }
}
