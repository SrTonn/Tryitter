using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tryitter.Migrations
{
    public partial class fillUser : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Users(Name, Email, Password) Values('Urbe', 'Urbe88@email.com', 'UrbeRock')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Users");
        }
    }
}
