using CheckerPricesRBT.CommLibrary;
using CheckerPricesRBT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPricesRBT.ViewModel
{
    public class SalonDBViewVM : OnPropertyChangedClass
    {
        public Logger Logger => PPModel.Logger;
        public PPModel Model => PPModel.Model;
        public SQLCentralConnection SQLCentralConnection { get { return PPModel.GlobalConnection; } }
        public RelayCommand ConnectToSQL { get; }
    }
}
