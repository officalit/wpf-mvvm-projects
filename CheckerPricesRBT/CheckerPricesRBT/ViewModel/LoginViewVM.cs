using CheckerPricesRBT.CommLibrary;
using CheckerPricesRBT.Model;
using CheckerPricesRBT.View;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CheckerPricesRBT.ViewModel
{
    public class LoginViewVM : OnPropertyChangedClass
    {
        public Logger Logger => PPModel.Logger;
        public PPModel Model => PPModel.Model;
        public SQLCentralConnection SQLCentralConnection { get { return PPModel.GlobalConnection; } }
        public RelayCommand ConnectToSQL { get; }


        public LoginViewVM()
        {
            ConnectToSQL = new RelayCommand(ConnectToSQLMetod);
        }

        private void ConnectToSQLMetod(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null)
                return;
            var password = passwordBox.Password;
            var login = _Login;

            Model.CreateConnection(login, password);
            try
            {
                using (SqlConnection zevs = new SqlConnection(SQLCentralConnection._ZevsConnection))
                {
                    zevs.Open();
                    Logger.Logbox += "Успешное подключение к серверу Zevs!";
                    Logger.Logbox += "Желаем Вам продуктивной работы!";
                    var Next = new PricesViewVM();
                    ShowWindow(Next);
                    CloseWindow();
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }

        public void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginViewWind)
                {
                    window.Close();
                    break;
                }
            }
        }

        public void ShowWindow(object viewModel)
        {
            var win = new PricesViewWindow();
            win.DataContext = viewModel;
            win.Show();
        }

        private string _login;
        public string _Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }


    }
}
