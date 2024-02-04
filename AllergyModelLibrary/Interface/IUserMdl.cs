namespace AllergyModelLibrary.Interface
{
    public interface IUserMdl
    {
        string Password { get; set; }
        int User_Id { get; set; }
        string Recovery { get; set; }
    }
}