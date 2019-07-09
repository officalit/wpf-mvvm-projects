using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckerPricesRBT.Model
{
    public class Logger : OnPropertyChangedClass
    {
        private string logbox;
        public string Logbox
        {
            get { return logbox; }
            set { logbox = value +  " " + DateTime.Now.ToString() + "\n"; OnPropertyChanged(); }
        }
        
    }
}
