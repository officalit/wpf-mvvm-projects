using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckerPricesRBT.Model
{
    public class ParsedPrice : OnPropertyChangedClass
    {
        private DateTime _dateTimePrices = DateTime.Now.AddDays(-1);
        public DateTime DateTimePrices
        {
            get { return _dateTimePrices; }
            set
            {
                _dateTimePrices = value;
                OnPropertyChanged();

            }
        }

        public string _codeDepartment;
        public string _DepartmentCode
        {
            get { return _codeDepartment; }
            set { _codeDepartment = value; }
        }

        public string _uploadPrices;
        public string UploadPrices
        {
            get { return _uploadPrices; }
            set
            {
                _uploadPrices = value;
            }
        }
    }
}
