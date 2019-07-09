using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPricesRBT.Model
{
    public class SalonDBData : OnPropertyChangedClass
    {
        public string _descriptonOfSalon;
        public string DescriptionOfSalon
        {
            get { return _descriptonOfSalon; }
            set { _descriptonOfSalon = value; }
        }

        public string _ipsql;
        public string IpSql
        {
            get { return _ipsql; }
            set { _ipsql = value; }
        }

        public string _domenOfSalon;
        public string DomenOfSalon
        {
            get { return _domenOfSalon; }
            set { _domenOfSalon = value; }
        }

        public string _db1Cname;
        public string DB1CName
        {
            get { return _db1Cname; }
            set { _db1Cname = value; }
        }

        public string _timezone;
        public string Timezone
        {
            get { return _timezone; }
            set { _timezone = value; }
        }

        public string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string _parentIDRRef;
        public string _ParentIDRRef
        {
            get { return _parentIDRRef; }
            set { _parentIDRRef = value;  }
        }

        public string _uploadPrices;
        public string UploadPrices
        {
            get { return _uploadPrices; }
            set
            { _uploadPrices = value; OnPropertyChanged(); }
        }

        public string _mustUploadPrices;
        public string MustUploadPrices
        {
            get { return _mustUploadPrices; }
            set { _mustUploadPrices = value; OnPropertyChanged(); }
        }

        public bool _successUpload;
        public bool SuccessUpload
        {
            get { return _successUpload; }
            set { _successUpload = value; OnPropertyChanged(); }
        }

        public string _fld5000RRef;
        public string _Fld5000RRef
        {
            get { return _fld5000RRef; }
            set { _fld5000RRef = value; }
        }

        public string _marked;
        public string _Marked
        {
            get { return _marked; }
            set { _marked = value; }
        }
    }
}
