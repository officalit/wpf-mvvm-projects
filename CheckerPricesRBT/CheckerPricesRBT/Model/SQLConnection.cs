using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPricesRBT.Model
{
    public class SQLConnection
    {

        private string _login;
        public string _Login
        {
            get { return _login; }
            set
            {
                _login = value;
            }
        }

        private string _password;
        public string _Password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }

        private string _db;
        public string DB
        {
            get { return _db; }
            set
            {
                _db = value;
            }
        }
    }
}
