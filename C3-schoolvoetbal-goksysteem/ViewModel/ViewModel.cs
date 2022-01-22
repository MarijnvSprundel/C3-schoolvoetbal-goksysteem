using GalaSoft.MvvmLight.Command;
using BC = BCrypt.Net.BCrypt;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace C3_schoolvoetbal_goksysteem.ViewModel
{
    public class ViewModel: INotifyPropertyChanged
    {
        public string Username { get; set; }
        private string password;
        public string Password { get; set; }
        public string Email { get; set; }

        private Visibility errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get { return errorVisibility; }
            set
            {
                Set(ref errorVisibility, value);
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                Set(ref errorMessage, value);
            }
        }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        SQLiteConnection sqlite_conn;

        

        public ViewModel()
        {
            sqlite_conn = CreateConnection();
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
            
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || (register && string.IsNullOrEmpty(Email)))
            {
                ErrorMessage = "Vul alle velden in";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            
            SQLiteDataReader selectUserReader;
            SQLiteCommand selectUserCmd;
            selectUserCmd = sqlite_conn.CreateCommand();
            selectUserCmd.CommandText = $"SELECT username, password FROM users WHERE username = '{Username}'";
            selectUserReader = selectUserCmd.ExecuteReader();
            switch (register)
            {
                case true:
                    if (selectUserReader.HasRows)
                    {
                        ErrorMessage = "Username is al in gebruik";
                        ErrorVisibility = Visibility.Visible;
                        return;
                    }
                    string passwordHash = BC.EnhancedHashPassword(Password);
                    SQLiteCommand registerCmd;
                    registerCmd = sqlite_conn.CreateCommand();
                    registerCmd.CommandText = $"INSERT INTO users(username, password, email) VALUES('{Username}', '{passwordHash}', '{Email}'); ";
                    registerCmd.ExecuteNonQuery();
                    break;
                case false:
                    if (!selectUserReader.HasRows)
                    {
                        ErrorMessage = "Account niet gevonden";
                        ErrorVisibility = Visibility.Visible;
                    }
                    string retrievedHash = "";
                    if (selectUserReader.Read())
                    {
                        retrievedHash = selectUserReader["password"].ToString();
                    }

                    bool verifyPassword = BC.EnhancedVerify(Password, retrievedHash);

                    Debug.WriteLine(verifyPassword);
                    break;
            }
        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            string DBName = ApplicationData.Current.LocalFolder.Path + @"\database.db";
            Debug.WriteLine(DBName);
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

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
