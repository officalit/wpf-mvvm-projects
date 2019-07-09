using CheckerPricesRBT.CommLibrary;
using CheckerPricesRBT.Model;
using CheckerPricesRBT.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckerPricesRBT.ViewModel
{
    public class PricesUploadVM
    {
        public PPModel Model => PPModel.Model;
        public ParsedPrice ParsedPrice => PPModel.ParsedPrice;
        public RelayCommand SaveCommand { get; }
        

        public PricesUploadVM()
        {
            SaveCommand = new RelayCommand(CloseWindow);
        }


        public void CloseWindow(object paramater)
        {
            Model.ParsedPrices();
            foreach (Window window in Application.Current.Windows)
            {
                if (window is PricesUploadViewWind)
                {
                    window.Close();
                    break;
                }
            }

        }
    }
}
