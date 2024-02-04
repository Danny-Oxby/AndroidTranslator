namespace AllergyModelLibrary.Interface
{
    public interface IDisplayMdl : IBaseDisplay
    {
        int Dispaly_Id { get; set; }
        int User_Id { get; set; }
    }

    public interface ILoginDisplayMdl : IBaseDisplay
    {
        int Language_Id { get; set; }
    }

    public interface IBaseDisplay
    {
        string DisplayValue { get; set; }
        string SearchName { get; set; }
    }
}