using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("Display_Text")]
    public class DisplayMdl : IDisplayMdl
    {
        [Column("Dispaly_Id"), PrimaryKey, AutoIncrement, NotNull, Unique] public int Dispaly_Id { get; set; }
        [Column("ExpectedValue"), NotNull] public string SearchName { get; set; }
        [Column("User_Id"), NotNull] public int User_Id { get; set; } //FK
        [Column("TranslatedContent"), NotNull] public string DisplayValue { get; set; }
    }
}
