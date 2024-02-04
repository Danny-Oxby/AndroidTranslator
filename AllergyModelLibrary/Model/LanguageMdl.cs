using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("Language")]
    public class LanguageMdl : ILanguageMdl
    {
        public LanguageMdl() { }
        public LanguageMdl(string fn, string aws, string tes, string cul)
        {
            Full_Name = fn;
            AWS_Name = aws;
            Tes_Name = tes;
            Culture_Name = cul;
        }
        public LanguageMdl(int id, string fn, string aws, string tes, string cul)
        {
            Language_Id = id;
            Full_Name = fn;
            AWS_Name = aws;
            Tes_Name = tes;
            Culture_Name = cul;
        }
        [Column("Language_Id"), PrimaryKey, AutoIncrement] public int Language_Id { get; set; }
        [Column("Full_Name"), NotNull, Unique] public string Full_Name { get; set; }
        [Column("AWS_Name"), NotNull, Unique] public string AWS_Name { get; set; }
        [Column("Tesseract_Name"), NotNull, Unique] public string Tes_Name { get; set; }
        [Column("Culture_Name"), NotNull, Unique] public string Culture_Name { get; set; }

    }
}
