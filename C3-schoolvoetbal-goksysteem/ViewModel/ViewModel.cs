using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace C3_schoolvoetbal_goksysteem.ViewModel
{
    public class ViewModel
    {
        private int id = 1;
        private string username;
        private string password;
        private string email;
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public ViewModel()
        {
            RegisterCommand = new RelayCommand(() =>
            {
                login(true);
                //ReadData(sqlite_conn);
            });
            LoginCommand = new RelayCommand(() => 
            {
                login();
            });
        }

        private void login(bool register = false)
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = CreateConnection();
            switch (register)
            {
                case true:
                    InsertData(sqlite_conn);
                    break;
                case false:

                    break;
            }
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            string DBName = ApplicationData.Current.LocalFolder.Path + @"\database.db";
            System.Diagnostics.Debug.WriteLine(DBName);
            sqlite_conn = new SQLiteConnection($"Data Source = {DBName}; Version = 3; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE 'users' ('id' INTEGER, 'username' TEXT, 'password' TEXT, 'email' TEXT, PRIMARY KEY('id' AUTOINCREMENT))";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();

        }

        static void InsertData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO users(username, password) VALUES('Kanker Piet', 'ww123'); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO users(username, password) VALUES('Kanker Piet', 'ww123'); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO users(username, password) VALUES('Kanker Piet', 'ww123'); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO users(username, password) VALUES('Kanker Piet', 'ww123'); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        static void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM users";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
            }
            conn.Close();
        }
    }
}
