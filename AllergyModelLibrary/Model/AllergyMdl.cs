using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("Allergy")]
    public class AllergyMdl : IAllergyMdl
    {
        public AllergyMdl() { }
        public AllergyMdl(string _name, int _user, string _colour, bool _search)
        {
            Name = _name;
            User_Id = _user;
            Colour = _colour;
            Search = _search;
        }
        [Column("Allergy_Id"), PrimaryKey, AutoIncrement, NotNull, Unique] public int Allergy_Id { get; set; }
        [Column("Allergy_Name"), NotNull] public string Name { get; set; }
        [Column("User_Id"), NotNull] public int User_Id { get; set; }
        [Column("Colour"), NotNull] public string Colour { get; set; }
        [Column("Search"), NotNull] public bool Search { get; set; }

    }
}
