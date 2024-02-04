namespace AllergyModelLibrary.Interface
{
    public interface ILogMdl
    {
        string Issue_Date { get; set; }
        int Issue_ID { get; set; }
        string Issue_Location { get; set; }
        string Issue_Parameters { get; set; }
        int Issue_Type { get; set; }
        int User_Id { get; set; }
    }
}