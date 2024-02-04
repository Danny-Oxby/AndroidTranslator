using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("Login_Text")]
    public class LoginDisplayMdl : ILoginDisplayMdl //a variation of diplay model, specificlly for the login page
    {
        [Column("ExpectedValue"), PrimaryKey, NotNull, Unique] public string SearchName { get; set; }
        [Column("TranslatedContent"), NotNull] public string DisplayValue { get; set; }
        [Column("Language_Id"), NotNull] public int Language_Id { get; set; }
    }
}
