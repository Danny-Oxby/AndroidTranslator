using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("Setting")]
    public class SettingMdl : ISettingMdl
    {
        [Column("Setting_Id"), PrimaryKey, AutoIncrement] public int Setting_ID { get; set; }
        [Column("User_Id"), NotNull] public int User_Id { get; set; }
        [Column("Language_Id"), NotNull] public int Language_Id { get; set; }
        [Column("Font_Type"), NotNull] public int Font_Type { get; set; }
        [Column("Font_Size"), NotNull] public int Font_Size { get; set; }
        [Column("Match_Full_Word"), NotNull] public bool Full_Word { get; set; }
        [Column("Terms_And_Condition_Accepted"), NotNull] public bool TC { get; set; }
        [Column("Hide_Warning"), NotNull] public bool Warning_State { get; set; }

    }
}
