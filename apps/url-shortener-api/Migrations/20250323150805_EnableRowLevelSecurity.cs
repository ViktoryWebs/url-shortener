using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortenerApi.Migrations
{
    /// <inheritdoc />
    public partial class EnableRowLevelSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.\"Urls\" ENABLE ROW LEVEL SECURITY");
            migrationBuilder.Sql(@"
                CREATE POLICY user_access_policy
                ON public.""Urls""
                FOR SELECT USING (auth.uid() = ""CreatedByUserId"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP POLICY user_access_policy ON public.\"Urls\";");
        }
    }
}
