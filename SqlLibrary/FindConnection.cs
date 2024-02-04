using SQLite;
using System.IO;

namespace SqlLibrary
{
    public class FindConnection
    {
        private static SQLiteConnection _connection;
        internal static SQLiteConnection CreateConnection() //this should only be avaible in this library
        {
            if (_connection == null)
            {
                //string dbPath = Path.Combine(Environment.CurrentDirectory, "AllergyData.db");
                //return new SQLiteConnection(Path.Combine(Environment.CurrentDirectory, "AllergyData.db"));
                string documentsDirectoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var dbPath = Path.Combine(documentsDirectoryPath, "AllergyData.db");

                //string dbPath = "C:\\Users\\oxbyd\\OneDrive - Sheffield Hallam University\\B\\SqlLibrary\\bin\\Debug\\netstandard2.0\\AllergyData.db";
                _connection = new SQLiteConnection(dbPath);
            }

            return _connection;
        }
        internal static bool TableNotExist(string tablename)//not check should only be needed until after sign-in stage/table creation
        {
            var temp = _connection.ExecuteScalar<bool>("SELECT count(*) FROM sqlite_master WHERE type='table' AND name=?", tablename);
            return !temp;
        }

        internal static int CountTables() 
        {
            SQLiteConnection localconnection = CreateConnection();
            var temp = localconnection.ExecuteScalar<int>("SELECT count(*) FROM sqlite_master WHERE type='table'");
            return temp;
        }
    }
}
