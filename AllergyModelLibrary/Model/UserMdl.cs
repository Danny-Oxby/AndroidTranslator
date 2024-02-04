using AllergyModelLibrary.Interface;
using SQLite;

namespace AllergyModelLibrary.Model
{
    [Table("User")] // matches the name of the DB table
    public class UserMdl : IUserMdl
    {
        public UserMdl(){}
        public UserMdl(string _recovery, string _passowrd)
        {
            Recovery = _recovery;
            Password = _passowrd;
        }

        // assigned to the next property only
        [Column("User_Id"), PrimaryKey, AutoIncrement] public int User_Id { get; set; } //PK's dont need not null 
        [Column("Username")] public string Recovery { get; set; } //column specific is only needed is the prop and column name don't match
        [Column("Password")] public string Password { get; set; }
    }
}
