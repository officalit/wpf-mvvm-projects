using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPricesRBT.Model
{
    public class DepartmentData : OnPropertyChangedClass
    {
        private string _ddref;
        public string _IDRRef
        {
            get { return _ddref; }
            set
            {
                _ddref = value;
            }
        }

        private string _code;
        public string _Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }

        private string _fld2788RRef;
        public string _Fld2788RRef
        {
            get { return _fld2788RRef; }
            set
            {
                _fld2788RRef = value;
            }
        }


    }
}
