using CheckerPricesRBT.CommLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CheckerPricesRBT.Model
{
    public class PPModel : OnPropertyChangedClass
    {
        public static DepartmentData Department { get; } = new DepartmentData();
        public static ParsedPrice ParsedPrice { get; } = new ParsedPrice();
        public static SalonDBData SalonDB { get; } = new SalonDBData();
        public static Logger Logger { get; } = new Logger();
        public static SQLCentralConnection GlobalConnection { get; } = new SQLCentralConnection();
        public static PPModel Model { get; } = new PPModel();

        ObservableCollection<DepartmentData> departmentlist;
        public ObservableCollection<DepartmentData> GetDepartmentList()
        {
            return departmentlist;
        }
        ObservableCollection<SalonDBData> serverlistfromzevs;
        public ObservableCollection<SalonDBData> GetServerList()
        {
            return serverlistfromzevs;
        }
        ObservableCollection<SQLConnection> logpasslist;
        public ObservableCollection<SQLConnection> GetLogPassList()
        {
            return logpasslist;
        }
        ObservableCollection<ParsedPrice> parsedpricelist;
        public ObservableCollection<ParsedPrice> GetParsedPriceList()
        {
            return parsedpricelist;
        }


        public void CreateConnection(string login, string password)
        {
            GlobalConnection._ZevsConnection = @"Data Source=deleted; Initial Catalog=deleted; User ID=" + login + "; Password=" + password;
        }

        public async Task LoadDepartmentData()
        {
            departmentlist = new ObservableCollection<DepartmentData>();
            await Task.Delay(1000);

            using (SqlConnection connection = new SqlConnection(GlobalConnection._ZevsConnection))
            {
                connection.Open();
                string query = "SELECT _IDRRef, _Fld2788RRef, _Code from _Reference59";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["_Fld2788RRef"] != DBNull.Value)
                            {
                                var x = new DepartmentData();
                                x._IDRRef = String.Concat("0x") + String.Concat(Array.ConvertAll((byte[])reader["_IDRRef"], u => u.ToString("X2")));
                                x._Fld2788RRef = String.Concat("0x") + String.Concat(Array.ConvertAll((byte[])reader["_Fld2788RRef"], u => u.ToString("X2")));
                                x._Code = (string)reader["_Code"];
                                departmentlist.Add(x);
                            }
                        }
                    }
                }
            }
            OnPropertyChanged("DepartamenListDownload");
        }

        public async Task LoadSalonDBData()
        {
            serverlistfromzevs = new ObservableCollection<SalonDBData>();
            await Task.Delay(2000);

            using (SqlConnection connection = new SqlConnection(GlobalConnection._ZevsConnection))
            {
                connection.Open();
                string query = "SELECT _IDRRef, _Description, _Fld140, _Fld2843, _Fld141, _Code, _Fld3200, _Marked, _Fld5000RRef  from _Reference58";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var x = new SalonDBData();
                            var time = Convert.ToInt32((decimal)reader["_Fld3200"]);
                            x._parentIDRRef = String.Concat("0x") + String.Concat(Array.ConvertAll((byte[])reader["_IDRRef"], u => u.ToString("X2")));
                            x._descriptonOfSalon = (string)reader["_Description"];
                            x._ipsql = (string)reader["_Fld140"];
                            x._domenOfSalon = (string)reader["_Fld2843"];
                            x._db1Cname = (string)reader["_Fld141"];
                            x.Code = (string)reader["_Code"];
                            x._timezone = DateTime.Now.AddHours(-5 + time).ToShortTimeString();
                            x._fld5000RRef = String.Concat("0x") + String.Concat(Array.ConvertAll((byte[])reader["_Fld5000RRef"], u => u.ToString("X2")));
                            x._Marked = String.Concat(Array.ConvertAll((byte[])reader["_Marked"], u => u.ToString("X2")));
                            serverlistfromzevs.Add(x);
                        }
                    }
                }
            }

            OnPropertyChanged("SalonDBDataListDownload");
        }

        public async Task LoadSQLSalonConnectionData()
        {
            logpasslist = new ObservableCollection<SQLConnection>();
            await Task.Delay(3000);

            using (SqlConnection connection = new SqlConnection(GlobalConnection._ZevsConnection))
            {
                connection.Open();
                string query = "Select DB,Login,Password from int_databases_all";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var x = new SQLConnection();
                            x.DB = (string)reader["DB"];
                            x._Login = (string)reader["Login"];
                            x._Password = (string)reader["Password"];
                            logpasslist.Add(x);
                        }
                    }
                }
            }

            OnPropertyChanged("LogPassListDownload");
        }

        public async Task ParsedPrices()
        {
            //await Task.Delay(1000);
                parsedpricelist = new ObservableCollection<ParsedPrice>();
                try
                {
                    using (TextReader io = new StringReader(ParsedPrice.UploadPrices))
                    {
                        string line;
                        while ((line = await io.ReadLineAsync()) != null)
                        {
                            try
                            {
                                var Price = new ParsedPrice();
                                string[] s = Regex.Split(line, " ");
                                Price._DepartmentCode = s[1];
                                Price.UploadPrices = s[3];
                                parsedpricelist.Add(Price);
                            }
                            catch (Exception e)
                            {
                                Logger.Logbox += "Ошибка парсинга: " + e.Message;
                            }
                        }
                    }
                    OnPropertyChanged("ParsingPriceListCompleted");
                }

                catch (Exception e)
                {
                    //Logger.Logbox += "Ошибка в методе ParsedPrices() " + e.Message;
                }
 
        }

        public async Task InsertParsed()
        {
            //await Task.Delay(1000);
            int i = 0;
            try
            {
                foreach (var item in parsedpricelist)
                {
                    try
                    {
                        var department = departmentlist.FirstOrDefault(x => x._Code.Replace(" ", string.Empty).ToLower() == item._DepartmentCode.ToLower());
                        var salon = serverlistfromzevs.FirstOrDefault(x => x._ParentIDRRef == department._Fld2788RRef);
                        string olo = department._Code;
                        olo = olo.Replace(" ", string.Empty);
                        var mustuploadprices = parsedpricelist.FirstOrDefault(x => x._DepartmentCode.ToLower() == olo.ToLower());
                        salon.MustUploadPrices = mustuploadprices.UploadPrices;
                    }

                    catch (Exception e)
                    {
                        //Logger.Logbox += "Ошибка реплейса: " + e.Message;
                    }
                    i++;
                }
            }

            catch (Exception e)
            {
                if (ParsedPrice.UploadPrices == null)
                {
                    Logger.Logbox += "Прайс-лист пуст! Необходимо загрузить прайс-лист перед тем запустить опрос!";
                }

                else
                {
                    Logger.Logbox += "Ошибка в методе InsertParsed(). Описание ошибки: " + e.Message;
                }

            }

        }

        public async Task CheckerPrices(DateTime datetime, CancellationToken token = new CancellationToken())
        {
            await Task.Delay(1000).ConfigureAwait(false);
            int i = 0;
            try
            {
                foreach (var item in parsedpricelist)
                {

                    //if (i == 1) break;
                    try
                    {
                        var department = departmentlist.FirstOrDefault(x => x._Code.Replace(" ", string.Empty).ToLower() == item._DepartmentCode.ToLower());
                        var salon = serverlistfromzevs.FirstOrDefault(x => x._ParentIDRRef == department._Fld2788RRef);

                        var loggpass = logpasslist.FirstOrDefault(x => x.DB.ToLower() == salon.DB1CName.ToLower());
                        //var department = departmentlist.FirstOrDefault(x => x._Fld2788RRef == serverlistfromzevs.ElementAt(i)._ParentIDRRef);

                        string dep_idrref = department._IDRRef;
                        using (SqlConnection connection = new SqlConnection(@"Data Source=" + salon.IpSql + "; Initial Catalog=" + salon.DB1CName + "; User ID=" + loggpass._Login + "; Password=" + loggpass._Password + ""))
                        {
                            await connection.OpenAsync(token).ConfigureAwait(false);

                            if (salon._Marked != "0x00")
                            {
                                if (salon._Fld5000RRef == "0x92E8D5AD729B8B764259D1003B596758")
                                {
                                    string query = "Select COUNT(*) as prices from _InfoReg503 where _Period>=CONVERT(datetime, '" + datetime.ToString("yyyyMMdd") + " 18:00:00" + "') and _Fld2419RRef = " + dep_idrref + " ";
                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        using (var reader = await command.ExecuteReaderAsync(token).ConfigureAwait(false))
                                        {
                                            while (await reader.ReadAsync(token).ConfigureAwait(false))
                                            {
                                                salon.UploadPrices = Convert.ToString((int)reader["prices"]);
                                                Logger.Logbox += "Успешно: " + salon.DB1CName;
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    string query = "Select COUNT(*) as prices from _InfoRg503 where _Period>=CONVERT(datetime, '" + datetime.ToString("yyyyMMdd") + " 18:00:00" + "') and _Fld2419RRef = " + dep_idrref + " ";
                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        using (var reader = await command.ExecuteReaderAsync(token).ConfigureAwait(false))
                                        {
                                            while (await reader.ReadAsync(token).ConfigureAwait(false))
                                            {
                                                salon.UploadPrices = Convert.ToString((int)reader["prices"]);
                                                Logger.Logbox += "Успешно: " + salon.DB1CName;
                                            }
                                        }
                                    }
                                }
                            }



                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Logbox += "Ошибка обращения к салону: " + serverlistfromzevs.ElementAt(i).DB1CName + ". Описание ошибки: " + e.Message.ToString();
                    }

                    i++;

                }
                Logger.Logbox += "Опрос завершён!";
            }

            catch (Exception e)
            {
                if (parsedpricelist == null)
                {
                    Logger.Logbox += "Прайс-лист пуст! Необходимо загрузить прайс-лист перед тем запустить опрос!";
                }
                else
                {
                    Logger.Logbox += "Ошибка в методе CheckerPrice() " + e.Message;
                }
            }
            
        }
    }
}
