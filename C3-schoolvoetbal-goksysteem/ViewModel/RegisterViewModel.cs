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
using C3_schoolvoetbal_goksysteem.Model;
using C3_schoolvoetbal_goksysteem.View;
using Windows.UI.Xaml.Controls;

namespace C3_schoolvoetbal_goksysteem.ViewModel
{
    public class RegisterViewModel
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

        SQLiteConnection sqlite_conn;



        public RegisterViewModel()
        {
            sqlite_conn = CreateConnection();
            RegisterCommand = new RelayCommand(() =>
            {
                register();
                //ReadData(sqlite_conn);
            });
        }

        private void register()
        {

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email))
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
            User.LoggedIn = true;
            User.Username = Username;

            Frame RootFrame = new Frame();
            RootFrame.Navigate(typeof(MainPageDeluxe));

            Window.Current.Content = RootFrame;
            Window.Current.Activate();
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
