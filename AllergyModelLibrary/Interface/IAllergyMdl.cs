namespace AllergyModelLibrary.Interface
{
    public interface IAllergyMdl
    {
        int Allergy_Id { get; set; }
        string Colour { get; set; }
        string Name { get; set; }
        bool Search { get; set; }
        int User_Id { get; set; }
    }
}