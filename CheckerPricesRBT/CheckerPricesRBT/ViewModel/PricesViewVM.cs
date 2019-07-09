using CheckerPricesRBT.CommLibrary;
using CheckerPricesRBT.Model;
using CheckerPricesRBT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CheckerPricesRBT.ViewModel
{
    public class PricesViewVM : OnPropertyChangedClass
    {
        public ParsedPrice ParsedPrice => PPModel.ParsedPrice;
        public Logger Logger => PPModel.Logger;
        public PPModel Model => PPModel.Model;
        private ObservableCollection<SalonDBData> _salons;
        public ObservableCollection<SalonDBData> Salons { get => _salons; set { SetProperty(ref _salons, value); } }

        public RelayCommand OpenUploadViewCommand { get; }
        //public RelayCommand Execute { get; }
        public IAsyncCommand Execute { get; private set; }

        public PricesViewVM()
        {
            OpenUploadViewCommand = new RelayCommand(OpenUploadViewMetod);
            //Execute = new RelayCommand(ExecuteMetod);
            Model.PropertyChanged += Model_PropertyChanged;
            Load();
            Execute = AsyncCommand.Create(token => Model.CheckerPrices(ParsedPrice.DateTimePrices, token)); 
        }


        async void Load()
        {
            await Model.LoadDepartmentData();
            await Model.LoadSalonDBData();
            await Model.LoadSQLSalonConnectionData();
        }

        static List<string> modelNamePropertyChanged = new List<string>() { "DepartamenListDownload", "SalonDBDataListDownload", "LogPassListDownload", "ParsingPriceListCompleted" };
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
                _propertyChanged(e.PropertyName);
                void _propertyChanged(string propertyName)
                {
                    switch (propertyName)
                    {
                        case "DepartamenListDownload":
                        Logger.Logbox += "Список подразделений загружен успешно!";
                        break;

                        case "SalonDBDataListDownload":
                        Logger.Logbox += "Список салонов загружен успешно!";
                        ServerListFromZevs = Model.GetServerList();
                        break;

                        case "LogPassListDownload":
                        Logger.Logbox += "Список логинов-паролей салонов загружен успешно!";
                        break;

                        case "ParsingPriceListCompleted":
                        Logger.Logbox += "Прайс-лист распарсен успешно!";
                        Model.InsertParsed();
                        break;
                    }
                }
        }


        //public async void ExecuteMetod(object parameter)
        //{
        //    await Model.CheckerPrices(ParsedPrice.DateTimePrices);
        //}

        public void OpenUploadViewMetod(object parameter)
        {
            var Next = new PricesUploadVM();
            ShowWindow(Next);

        }

        public void ShowWindow(object viewModel)
        {
            var win = new PricesUploadViewWind();
            win.DataContext = viewModel;
            win.ShowDialog();
        }

        ObservableCollection<SalonDBData> _serverlistfromzevs;
        public ObservableCollection<SalonDBData> ServerListFromZevs
        {
            get { return _serverlistfromzevs; }
            private set { _serverlistfromzevs = value; OnPropertyChanged(); ViewServerListFromZevs.Source = ServerListFromZevs; }
        }
        public CollectionViewSource ViewServerListFromZevs
        {
            get { return _viewServerListFromZevs; }
            private set { _viewServerListFromZevs = value; OnPropertyChanged(); }

        }
        private CollectionViewSource _viewServerListFromZevs = new CollectionViewSource() { };


        private ListSortDirection _sortDirection;
        private RelayCommand _sortCommand;
        public RelayCommand Сортировка { get { return _sortCommand ?? (_sortCommand = new RelayCommand(Метод_сортировки_при_клике_на_столбец)); } }

        private void Метод_сортировки_при_клике_на_столбец(object parameter)
        {
            _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                          ListSortDirection.Descending :
                                          ListSortDirection.Ascending;
            ViewServerListFromZevs.SortDescriptions.Clear();
            ViewServerListFromZevs.SortDescriptions.Add(new SortDescription(parameter.ToString(), _sortDirection));
        }

        ObservableCollection<ParsedPrice> _parsingPrices;
        public ObservableCollection<ParsedPrice> ParsingPricesList
        {
            get { return _parsingPrices; }
            private set { _parsingPrices = value; OnPropertyChanged(); }
        }

        private string _filtrSearch;
        public string FiltrSearch
        {
            get { return _filtrSearch; }
            set
            {
                _filtrSearch = value;
                OnPropertyChanged("FiltrSearch");
                AddFiltering();
            }
        }

        private void AddFiltering()
        {
            ViewServerListFromZevs.Filter += new FilterEventHandler(ShowOnlyBargainsFilter);
        }

        private void ShowOnlyBargainsFilter(object sender, FilterEventArgs e)
        {
            SalonDBData salon = e.Item as SalonDBData;
            if (salon != null)
            {
                if (salon.DescriptionOfSalon.Contains(FiltrSearch))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

    }
}
