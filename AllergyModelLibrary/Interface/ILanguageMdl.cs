namespace AllergyModelLibrary.Interface
{
    public interface ILanguageMdl
    {
        string AWS_Name { get; set; }
        string Full_Name { get; set; }
        int Language_Id { get; set; }
        string Tes_Name { get; set; }
        string Culture_Name { get; set; }
    }
}