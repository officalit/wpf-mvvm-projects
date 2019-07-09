using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckerPricesRBT.Model
{
    public class SQLCentralConnection : OnPropertyChangedClass
    {
        //private string _login;
        //public string _Login
        //{
        //    get { return _login; }
        //    set
        //    {
        //        _login = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string _password;
        //public string _Password
        //{
        //    get { return _password; }
        //    set
        //    {
        //        _password = value;
        //        OnPropertyChanged();
        //    }
        //}

        private string _zevsconn;
        public string _ZevsConnection
        {
            get { return _zevsconn; }
            set
            {
                _zevsconn = value;
                OnPropertyChanged();
            }
        }

        //public SQLCentralConnection(string zevsconn)
        //{ 
        //    _ZevsConnection = zevsconn;
        //}


    }
}
