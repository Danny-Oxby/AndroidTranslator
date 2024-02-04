namespace AllergyModelLibrary.Interface
{
    public interface ISettingMdl
    {
        int Font_Size { get; set; }
        int Font_Type { get; set; }
        bool Full_Word { get; set; }
        int Language_Id { get; set; }
        int Setting_ID { get; set; }
        bool TC { get; set; }
        int User_Id { get; set; }
        bool Warning_State { get; set; }
    }
}