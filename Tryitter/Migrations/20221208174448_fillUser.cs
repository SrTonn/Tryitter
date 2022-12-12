using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tryitter.Migrations
{
    public partial class fillUser : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Users(Name, Email, Password, Admin) Values('Urbe', 'Urbe88@email.com', 'UrbeRock', true),"
                + "('Joel', 'Joel@email.com', 'Joel123', false)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Users");
        }
    }
}
