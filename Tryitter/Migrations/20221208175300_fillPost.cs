using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tryitter.Migrations
{
    public partial class fillPost : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Posts(Title, Description, ImageUrl, DataPost, UserId)" +
                "Values('CSharp Null-Coalescing Operator', " +
                "'The other day I was working in Markdown Monster - a WPF application - where I needed to display a list of matches from a directory search. The search supports searching the file content as well as file names, so the results would reflect how many search phrase matches there are for each matched file. But I wanted to display files that have 0 matches - ie. where only the file name matches but not any content - as an empty string rather than displaying 0.'," +
                "'https://i.pinimg.com/736x/03/1b/68/031b68882265722dede1080a200f015a.jpg'," + 
                "now()," +
                "1)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Posts");
        }
    }
}
