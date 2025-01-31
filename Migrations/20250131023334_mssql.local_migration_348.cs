using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tessenger.Server.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_348 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    fusername = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    sendusername = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentfiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    status = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reactemoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    replymessid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    voicerecoardfile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDn",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uniquename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDn", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    fusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statusaccess = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blocked = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    members = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    admin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isadminfullaccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupusername = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    sendusername = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    documentfiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    seensusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reactemoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reactemojiusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    poloid = table.Column<long>(type: "bigint", nullable: false),
                    replymessid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    voicerecoardfile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupInformation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    profileimage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupInformation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GroupRequest",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    getrequstedgroupuser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendrequstedgroupuser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gettime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendtime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRequest", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Information",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    highlighttext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aboutme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactnum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    curredu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    facebooklink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instragramlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkdinlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    githublink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    youtubelink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    whatsappnum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tiktoklink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    redditlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    snapchartlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    twitterlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pinterestlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    websitelink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    website2link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    website3link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    profileimage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentationemail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentationphonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentationauthenticatorapp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentationsecurityquestions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentationsecuritykey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Information", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "Polo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    groupusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senderusername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    topics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    getrequsteduser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendrequsteduser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gettime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendtime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    password = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "DocumentDn");

            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropTable(
                name: "GroupInformation");

            migrationBuilder.DropTable(
                name: "GroupRequest");

            migrationBuilder.DropTable(
                name: "Information");

            migrationBuilder.DropTable(
                name: "Polo");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
