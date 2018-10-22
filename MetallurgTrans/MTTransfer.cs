using EFMT.Concrete;
using MessageLog;
using EFMT.Entities;
//using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TransferRailCars;
using WebApiClient;
using RW;
using libClass;
//using EFKIS.Concrete;
//using EFKIS.Entities;

namespace MetallurgTrans
{
    /// <summary>
    /// Коды ошибок операций переноса вагонов
    /// </summary>
    public enum mtt_err : int
    {
        global_error = -1,
        not_fromPath = -2,
        not_listApproachesCars = -3,
        not_listArrivalCars = -4,
        file_error = -5,
        not_listWagonsTracking = -6
        //not_listStartArrivalSostav = -5,
    }
    /// <summary>
    /// Коды ошибок истории движения вагонов
    /// </summary>
    public enum mtt_err_arrival : int
    {
        close_car = 0,
        close_manual = -1,
        close_new_route = -2,
        close_timeout = -3,
        close_different_cargo = -4,
        close_arrival_uz = -5,
        close_arrival_station_on = -6,
    }

    public enum wtroute : int
    {
        not = 0,
        amkr = 1,
        send = 2,
        client = 3,
        ret = 4,
    }

    [Serializable()]
    public class FileApproachesSostav
    {
        public string Index
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string File
        {
            get;
            set;
        }
    }

    [Serializable()]
    public class FileArrivalSostav
    {
        public string Index
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public int Operation
        {
            get;
            set;
        }
        public string File
        {
            get;
            set;
        }

    }

    public class MTTransfer
    {
        private eventID eventID = eventID.MetallurgTrans_MTTransfer;
        protected service servece_owner = service.Null;

        private string fromPath;
        public string FromPath { get { return this.fromPath; } set { this.fromPath = value; } }
        private bool delete_file = false;
        public bool DeleteFile { get { return this.delete_file; } set { this.delete_file = value; } }
        private int day_range_approaches_cars = 30; // тайм аут по времени для вагонов на подходе
        public int DayRangeApproachesCars { get { return this.day_range_approaches_cars; } set { this.day_range_approaches_cars = value; } }
        private int day_range_approaches_cars_arrival = 5; // тайм аут по времени для вагонов на подходе прибывших на конечную станцию
        public int DayRangeApproachesCarsArrival { get { return this.day_range_approaches_cars_arrival; } set { this.day_range_approaches_cars_arrival = value; } }

        private int day_range_arrival_cars = 10; // тайм аут по времени для вагонов прибывших на УЗ
        public int DayRangeArrivalCars { get { return this.day_range_arrival_cars; } set { this.day_range_arrival_cars = value; } }
        private bool arrival_to_railway = true;
        public bool ArrivalToRailWay { get { return this.arrival_to_railway; } set { this.arrival_to_railway = value; } }

        private bool arrival_to_railcars = true;
        public bool ArrivalToRailCars { get { return this.arrival_to_railcars; } set { this.arrival_to_railcars = value; } }

        private DateTime datetime_start_new_tracking = new DateTime(2018, 01, 01, 0, 0, 0); // Время начала запроса информации по вагону которого нет в базе АМКР
        public DateTime DateTimeStartNewTracking { get { return this.datetime_start_new_tracking; } set { this.datetime_start_new_tracking = value; } }
        private string url_wagons_tracking;
        public string URLWagonsTracking { get { return this.url_wagons_tracking; } set { this.url_wagons_tracking = value; } }
        private string user_wagons_tracking;
        public string UserWagonsTracking { get { return this.user_wagons_tracking; } set { this.user_wagons_tracking = value; } }
        private string psw_wagons_tracking;
        public string PSWWagonsTracking { get { return this.psw_wagons_tracking; } set { this.psw_wagons_tracking = value; } }
        private string api_wagons_tracking;
        public string APIWagonsTracking { get { return this.api_wagons_tracking; } set { this.api_wagons_tracking = value; } }

        public MTTransfer()
        {

        }

        public MTTransfer(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region TransferApproaches Перенос составов на подходах
        /// <summary>
        /// Получить parent_id
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public int? GetParentID(ApproachesCars car, int day_range)
        {
            try
            {
                int? parentid = null;
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                ApproachesCars old_car = efmt.GetApproachesCarsOfNumCar(car.Num, true).FirstOrDefault();
                if (old_car == null) return null; // нет историии движения, первая операция над вагоном
                if (old_car.Arrival != null) return null; // история закрыта, первая операция над вагоном 

                if (old_car.CargoCode == car.CargoCode)
                {
                    // входит в диапазон времени
                    if (old_car.DateOperation.Date.AddDays(day_range) > car.DateOperation)
                    {
                        old_car.NumDocArrival = (int)mtt_err_arrival.close_car;
                        // предыдущий состав не прибыл на станцию назначения
                        if (old_car.CodeStationOn != old_car.CodeStationCurrent)
                        {
                            parentid = old_car.ID;
                        }
                        else
                        {
                            // новый состав стоит еще на станции что и предыдущий
                            if (old_car.CodeStationCurrent == car.CodeStationCurrent)
                            {
                                parentid = old_car.ID;
                            }
                            else
                            {
                                // вагон начал движение по новому маршруту
                                old_car.NumDocArrival = (int)mtt_err_arrival.close_new_route;
                            }
                        }
                    }
                    else
                    {
                        // больше допустимого интервала
                        old_car.NumDocArrival = (int)mtt_err_arrival.close_timeout;
                    }
                }
                else
                {
                    // грузы в вагонах разные
                    old_car.NumDocArrival = (int)mtt_err_arrival.close_different_cargo;
                }
                // закрываем старый вагон
                old_car.Arrival = car.DateOperation;
                efmt.SaveApproachesCars(old_car); // сохранить изменение
                return parentid;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetParentID(car={0}, day_range={1})", car.GetFieldsAndValue(), day_range), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Возвращает список вагонов из txt-файла
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public List<ApproachesCars> TransferTXTToListApproachesCars(string file, int id_sostav)
        {
            List<ApproachesCars> list = new List<ApproachesCars>();
            int count = 0;
            int error = 0;
            try
            {
                StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
                string input = null;
                while ((input = sr.ReadLine()) != null)
                {
                    count++;
                    try
                    {
                        string[] array = input.Split(';');
                        if (array.Count() >= 14)
                        {
                            //Reference api_reference = new Reference();
                            EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();

                            int code_cargo = -1;
                            int code_station_from = -1;
                            int code_station_on = -1;
                            if (!String.IsNullOrWhiteSpace(array[3]))
                            {
                                EFReference.Entities.Cargo corect_cargo_code = ef_reference.GetCargoOfCodeETSNG(int.Parse(array[3]));
                                code_cargo = corect_cargo_code != null ? corect_cargo_code.code_etsng : int.Parse(array[3]);
                            }
                            if (!String.IsNullOrWhiteSpace(array[4]))
                            {
                                int codefrom = int.Parse(array[4].Substring(0, 4));
                                int codeon = int.Parse(array[4].Substring(9, 4));
                                EFReference.Entities.Stations corect_station_from = ef_reference.GetCorrectStationsOfCode(codefrom, false);
                                code_station_from = corect_station_from != null ? corect_station_from.code : codefrom;
                                EFReference.Entities.Stations corect_station_on = ef_reference.GetCorrectStationsOfCode(codeon, false);
                                code_station_on = corect_station_on != null ? corect_station_on.code : codeon;
                            }
                            ApproachesCars new_wag = new ApproachesCars()
                            {
                                ID = 0,
                                IDSostav = id_sostav,
                                CompositionIndex = array[4].ToString(),
                                Num = !String.IsNullOrWhiteSpace(array[0]) ? int.Parse(array[0]) : -1,
                                CountryCode = !String.IsNullOrWhiteSpace(array[1]) ? int.Parse(array[1].Substring(0, 2)) : -1, // Подрезка кода страны в фале 3 цифры переводим в 2 цифры
                                Weight = !String.IsNullOrWhiteSpace(array[2]) ? int.Parse(array[2]) : -1,
                                CargoCode = code_cargo, // скорректируем код
                                TrainNumber = !String.IsNullOrWhiteSpace(array[5]) ? int.Parse(array[5]) : -1,
                                Operation = array[6].ToString(),
                                DateOperation = !String.IsNullOrWhiteSpace(array[7]) ? DateTime.Parse(array[7], CultureInfo.CreateSpecificCulture("ru-RU")) : DateTime.Now,
                                CodeStationFrom = code_station_from,
                                CodeStationOn = code_station_on,
                                CodeStationCurrent = !String.IsNullOrWhiteSpace(array[8]) ? int.Parse(array[8]) : -1,
                                CountWagons = !String.IsNullOrWhiteSpace(array[9]) ? int.Parse(array[9]) : -1,
                                SumWeight = !String.IsNullOrWhiteSpace(array[10]) ? int.Parse(array[10]) : -1,
                                FlagCargo = !String.IsNullOrWhiteSpace(array[11]) ? int.Parse(array[11]) : -1,
                                Route = !String.IsNullOrWhiteSpace(array[12]) ? int.Parse(array[12]) : -1,
                                Owner = !String.IsNullOrWhiteSpace(array[13]) ? int.Parse(array[13]) : -1,
                                NumDocArrival = null,
                                Arrival = null,
                                UserName = null,
                            };
                            // Получить parent_id
                            new_wag.ParentID = GetParentID(new_wag, this.day_range_approaches_cars);
                            list.Add(new_wag);
                        }
                        else
                        {
                            error++;
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка выполнения переноса вагона метода:TransferTXTToMTApproachesWagons, файл:{0}, строка:{1}", file, input), servece_owner, eventID);
                        error++;
                    }
                }
                sr.Close();
                string mess = String.Format("В файле {0} определенно: {1} вагонов, добавлено в список : {2}, пропущено по ошибке : {3}", file, count, list.Count(), error);
                mess.WriteInformation(servece_owner, eventID);
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка выполнения метода:TransferTXTToMTApproachesWagons(file={0}, id_sostav={1}), файл:{0})", file, id_sostav), servece_owner, eventID);
                return null;
            }
            return list;
        }
        /// <summary>
        /// Перенос списка вагонов в БД MTApproaches
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int TransferListApproachesCarsToDB(List<ApproachesCars> list)
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            if (list == null) return this.eventID.GetEventIDErrorCode((int)mtt_err.not_listApproachesCars);
            try
            {
                int error = 0;
                int trans = 0;
                string trans_id = "";

                if (list.Count() > 0)
                {
                    foreach (ApproachesCars apc in list)
                    {
                        int res = efmt.SaveApproachesCars(apc);
                        if (res > 0) { trans++; }
                        if (res < 0) { error++; }
                        trans_id = trans_id + res.ToString() + "; ";
                    }
                    string mess = String.Format("В списке определенно: {0} вагонов, перенесено в БД MT.Approaches : {1}, пропущено по ошибке : {2}", list.Count(), trans, error);
                    mess.WriteInformation(servece_owner, this.eventID);
                    if (list.Count() > 0) { mess.WriteEvents(trans_id, servece_owner, eventID); }
                    return trans;
                }
                else return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferListApproachesCarsToDB(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенести БД MTApproaches вагоны состава из файла txt.
        /// </summary>
        /// <param name="new_id"></param>
        /// <param name="file"></param>
        /// <param name="countCopy"></param>
        /// <param name="countError"></param>
        /// <returns></returns>
        protected bool SaveApproachesWagons(int new_id, string file, ref int countCopy, ref int countError)
        {
            try
            {
                int count_wagons = 0;
                if (new_id > 0)
                {
                    // Переносим вагоны
                    count_wagons = TransferListApproachesCarsToDB(TransferTXTToListApproachesCars(file, new_id));
                    if (count_wagons > 0) { countCopy++; }
                    if (count_wagons < 0) { countError++; } // Счетчик ошибок при переносе
                }
                if (new_id < 0) { countError++; } // Счетчик ошибок при переносе
                if (count_wagons > 0 & new_id > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка операции добавления вагонов в БД MT.Approaches, сотава на подходе :{0}", new_id), servece_owner, eventID);
            }
            return false;
        }
        /// <summary>
        /// Получить список FileApproachesSostav
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        protected List<FileApproachesSostav> GetFileApproachesSostav(string[] files)
        {
            List<FileApproachesSostav> listfs = new List<FileApproachesSostav>();
            foreach (string file in files)
            {
                try
                {
                    if (!String.IsNullOrEmpty(file))
                    {
                        FileInfo fi = new FileInfo(file);
                        string index = fi.Name.Substring(5, 13);
                        DateTime date = DateTime.Parse(fi.Name.Substring(19, 4) + "-" + fi.Name.Substring(23, 2) + "-" + fi.Name.Substring(25, 2) + " " + fi.Name.Substring(27, 2) + ":" + fi.Name.Substring(29, 2) + ":00");
                        // Добавим строку
                        listfs.Add(new FileApproachesSostav()
                        {
                            Index = index,
                            Date = date,
                            File = file
                        });
                    }
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка формирования строки списка файлов состава List<FileApproachesSostav>, файл:{0}", file), servece_owner, eventID);
                }
            }
            return listfs;
        }
        /// <summary>
        /// Перености txt-файлы из указанной папки  в таблицы MTApproaches
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="delete_file"></param>
        /// <returns></returns>
        public int TransferApproaches(string fromPath, bool delete_file)
        {
            try
            {
                if (!Directory.Exists(fromPath))
                {
                    String.Format("Указанного пути {0} с txt-файлами для переноса в БД MT.Approaches.. не существует.", fromPath).WriteError(servece_owner, this.eventID);
                    return this.eventID.GetEventIDErrorCode((int)mtt_err.not_fromPath);
                }
                int countCopy = 0;
                int countExist = 0;
                int countError = 0;
                int countDelete = 0;
                string[] files = Directory.GetFiles(fromPath, "*.txt");
                if (files == null | files.Count() == 0) { return 0; }
                String.Format("Определенно {0} txt-файлов для копирования", files.Count()).WriteInformation(servece_owner, this.eventID);
                List<FileApproachesSostav> list_sostav = GetFileApproachesSostav(files);
                var listFileSostavs = from c in list_sostav.OrderBy(c => c.Date).ThenBy(c => c.Index)
                                      select new { c.Index, c.Date, c.File };
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                // Пройдемся по списку
                foreach (var fs in listFileSostavs)
                {
                    try
                    {
                        Console.WriteLine("Переносим файл {0}", fs.File);
                        // защита от записи повторов
                        FileInfo fi = new FileInfo(fs.File);
                        ApproachesSostav exs_sostav = efmt.GetApproachesSostavOfFile(fi.Name);
                        if (exs_sostav == null)
                        {
                            int? ParentIDSostav = null;
                            // получить не закрытый состав
                            ApproachesSostav no_close_sostav = efmt.GetNoCloseApproachesSostav(fs.Index, fs.Date);
                            if (no_close_sostav != null)
                            {
                                ParentIDSostav = no_close_sostav.ID;
                                // Закрыть состав
                                no_close_sostav.Close = DateTime.Now;
                                efmt.SaveApproachesSostav(no_close_sostav);
                            }
                            ApproachesSostav new_sostav = new ApproachesSostav()
                            {
                                ID = 0,
                                FileName = fi.Name,
                                CompositionIndex = fs.Index,
                                DateTime = fs.Date,
                                Create = DateTime.Now,
                                Close = null,
                                Approaches = null,
                                ParentID = ParentIDSostav

                            };

                            int new_id = efmt.SaveApproachesSostav(new_sostav);
                            if (delete_file & SaveApproachesWagons(new_id, fs.File, ref  countCopy, ref  countError))
                            {
                                File.Delete(fs.File);
                                countDelete++;
                            }
                        }
                        else
                        {
                            // Проверка сравниваем количество если совподает удаляем файл, иначе добавляем новые вагоны и удаляем файл
                            List<ApproachesCars> list = TransferTXTToListApproachesCars(fs.File, exs_sostav.ID);
                            List<ApproachesCars> listdb = efmt.GetApproachesCarsOfSostav(exs_sostav.ID).ToList();
                            if (list != null & listdb != null)
                            {
                                if (list.Count() != listdb.Count())
                                {
                                    efmt.DeleteApproachesCarsOfSostav(exs_sostav.ID);
                                    if (delete_file & SaveApproachesWagons(exs_sostav.ID, fs.File, ref  countCopy, ref  countError))
                                    {
                                        File.Delete(fs.File);
                                        countDelete++;
                                    }
                                }
                                else
                                {
                                    // Файл перенесен ранеее, удалим его если это требуется
                                    if (delete_file)
                                    {
                                        File.Delete(fs.File);
                                        countDelete++;
                                    }
                                }
                                countExist++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка переноса txt-файла в БД MT.Approaches, файл {0}", fs.File), servece_owner, eventID);
                        countError++;
                    }
                }
                string mess = String.Format("Перенос txt-файлов в БД MT.Approaches выполнен, определено для переноса {0} txt-файлов, перенесено {1}, были перенесены ранее {2}, ошибки при переносе {3}, удаленно {4}.", files.Count(), countCopy, countExist, countError, countDelete);
                mess.WriteInformation(servece_owner, this.eventID);
                if (files != null && files.Count() > 0) { mess.WriteEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return files.Count();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferApproaches(fromPath={0}, delete_file={1})", fromPath, delete_file), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перености txt-файлы из папки по умолчанию в таблицы MTApproaches
        /// </summary>
        /// <returns></returns>
        public int TransferApproaches()
        {
            return TransferApproaches(this.fromPath, this.delete_file);
        }
        #endregion

        #region TransferArrival Перенос составов на станциях УЗ

        public int? GetParentID(ArrivalCars car, int day_range)
        {
            try
            {
                int? parentid = null;
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                ArrivalCars old_car = efmt.GetArrivalCarsOfNumCar(car.Num, true).FirstOrDefault();
                if (old_car == null) return null; // нет историии движения, первая операция над вагоном
                if (old_car.Arrival != null) return null; // история закрыта, первая операция над вагоном 

                if (old_car.CargoCode == car.CargoCode)
                {
                    if (old_car.DateOperation.Date.AddDays(day_range) > car.DateOperation)
                    {

                        if (old_car.CompositionIndex == car.CompositionIndex |
                                (old_car.CompositionIndex != car.CompositionIndex &
                                efmt.IsConsigneeSend(false, old_car.Consignee, mtConsignee.AMKR) &
                                efmt.IsConsigneeSend(true, car.Consignee, mtConsignee.AMKR)))
                        { // Продолжаем цепочку вагонов если равны CompositionIndex или (CompositionIndex не равны но следующий код досылки и входит в диапазон времени)
                            parentid = old_car.ID;
                            old_car.NumDocArrival = (int)mtt_err_arrival.close_car;
                            old_car.Arrival = car.DateOperation;
                        }
                        else
                        {
                            // вагон начал движение по новому маршруту
                            old_car.NumDocArrival = (int)mtt_err_arrival.close_new_route;
                            old_car.Arrival = car.DateOperation;
                        }
                    }
                    else
                    {
                        // больше допустимого интервала
                        old_car.NumDocArrival = (int)mtt_err_arrival.close_timeout;
                        old_car.Arrival = DateTime.Now;
                    }
                }
                else
                {
                    // грузы в вагонах разные
                    old_car.NumDocArrival = (int)mtt_err_arrival.close_different_cargo;
                    old_car.Arrival = car.DateOperation;
                }
                // закрываем старый вагон
                efmt.SaveArrivalCars(old_car); // сохранить изменение
                return parentid;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetParentID(car={0}, day_range={1})", car.GetFieldsAndValue(), day_range), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Получить тип операции над составом
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected int GetOperationToXml(string file)
        {
            try
            {
                XDocument doc;
                try
                {
                    doc = XDocument.Load(file);
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка считывания файла:{0}", file), servece_owner, eventID);
                    return (int)mtt_err.file_error;
                }

                foreach (XElement element in doc.Element("NewDataSet").Elements("Table"))
                {
                    string opr = (string)element.Element("Operation");
                    if (String.IsNullOrEmpty(opr)) return (int)mtOperation.not;
                    if (opr.Trim().ToUpper() == "ПРИБ") return (int)mtOperation.coming;
                    if (opr.Trim().ToUpper() == "ТСП") return (int)mtOperation.tsp;
                }
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка определения операции файл:{0}", file), servece_owner, eventID);
            }
            return (int)mtOperation.not;

        }
        /// <summary>
        /// Возвращает список вагонов из xml-файла
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public List<ArrivalCars> TransferXMLToListArrivalCars(string file, int id_sostav)
        {
            List<ArrivalCars> list = new List<ArrivalCars>();
            int count = 0;
            int error = 0;
            try
            {
                //Reference api_reference = new Reference();
                EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();
                XDocument doc;
                try
                {
                    doc = XDocument.Load(file);
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка считывания файла:{0}", file), servece_owner, eventID);
                    return null;
                }
                foreach (XElement element in doc.Element("NewDataSet").Elements("Table"))
                {
                    try
                    {
                        int code_cargo = -1;
                        if (!String.IsNullOrWhiteSpace((string)element.Element("IDCargo")))
                        {
                            EFReference.Entities.Cargo corect_cargo_code = ef_reference.GetCargoOfCodeETSNG((int)element.Element("IDCargo"));
                            code_cargo = corect_cargo_code != null ? corect_cargo_code.code_etsng : (int)element.Element("IDCargo");
                        }
                        ArrivalCars mtarr = new ArrivalCars()
                        {
                            ID = 0,
                            IDSostav = id_sostav,
                            Position = !String.IsNullOrWhiteSpace((string)element.Element("Position")) ? (int)element.Element("Position") : -1,
                            Num = !String.IsNullOrWhiteSpace((string)element.Element("CarriageNumber")) ? (int)element.Element("CarriageNumber") : -1,
                            CountryCode = !String.IsNullOrWhiteSpace((string)element.Element("CountryCode"))
                             ? ((string)element.Element("CountryCode")).Length >= 2 ? int.Parse(((string)element.Element("CountryCode")).Substring(0, 2)) : (int)element.Element("CountryCode") : -1, // Подрезка кода страны в фале 3 цифры переводим в 2 цифры 
                            Weight = !String.IsNullOrWhiteSpace((string)element.Element("Weight")) ? (int)element.Element("Weight") : -1,
                            CargoCode = code_cargo,
                            Cargo = !String.IsNullOrWhiteSpace((string)element.Element("Cargo")) ? (string)element.Element("Cargo") : "?",
                            StationCode = !String.IsNullOrWhiteSpace((string)element.Element("IDStation")) ? (int)element.Element("IDStation") : -1,
                            Station = !String.IsNullOrWhiteSpace((string)element.Element("Station")) ? (string)element.Element("Station") : "?",
                            Consignee = !String.IsNullOrWhiteSpace((string)element.Element("Consignee")) ? (int)element.Element("Consignee") : -1,
                            Operation = !String.IsNullOrWhiteSpace((string)element.Element("Operation")) ? (string)element.Element("Operation") : "?",
                            CompositionIndex = !String.IsNullOrWhiteSpace((string)element.Element("CompositionIndex")) ? (string)element.Element("CompositionIndex") : "?",
                            DateOperation = !String.IsNullOrWhiteSpace((string)element.Element("DateOperation")) ? DateTime.Parse((string)element.Element("DateOperation"), CultureInfo.CreateSpecificCulture("ru-RU")) : DateTime.Now,
                            TrainNumber = !String.IsNullOrWhiteSpace((string)element.Element("TrainNumber")) ? (int)element.Element("TrainNumber") : -1,
                            NumDocArrival = null,
                            Arrival = null,
                            UserName = null,

                        };
                        // Получить parent_id
                        mtarr.ParentID = GetParentID(mtarr, this.day_range_arrival_cars);
                        list.Add(mtarr);
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка выполнения переноса вагона метода:TransferXMLToListArrivalCars, файл:{0}, вагон:{1}", file, element.Element("CarriageNumber")), servece_owner, eventID);
                        error++;
                    }
                }
                string mess = String.Format("В файле {0} определенно: {1} вагонов, добавлено в список : {2}, пропущено по ошибке : {3}", file, count, list.Count(), error);
                mess.WriteInformation(servece_owner, eventID);
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка выполнения метода:TransferXMLToListArrivalCars(file={0}, id_sostav={1}), файл:{0})", file, id_sostav), servece_owner, eventID);
                return null;
            }
            return list;
        }
        /// <summary>
        /// Перенос списка вагонов в БД Arrival
        /// </summary>
        /// <param name="list"></param>
        /// <param name="set_arrival"></param>
        /// <returns></returns>
        public int TransferListArrivalCarsToDB(List<ArrivalCars> list)
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            if (list == null) return this.eventID.GetEventIDErrorCode((int)mtt_err.not_listArrivalCars);
            try
            {
                int error = 0;
                int trans = 0;
                string trans_id = "";

                if (list.Count() > 0)
                {
                    foreach (ArrivalCars apc in list)
                    {
                        int res = efmt.SaveArrivalCars(apc);
                        if (res > 0) { trans++; }
                        if (res < 0) { error++; }
                        trans_id = trans_id + res.ToString() + "; ";

                    }
                    string mess = String.Format("В списке определенно: {0} вагонов, перенесено в БД MT.Arrival : {1}, пропущено по ошибке : {2}", list.Count(), trans, error);
                    mess.WriteInformation(servece_owner, this.eventID);
                    if (list.Count() > 0) { mess.WriteEvents(trans_id, servece_owner, eventID); }
                    return trans;
                }
                else return 0;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferListArrivalCarsToDB(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенести БД Arrival вагоны состава из файла xml.
        /// </summary>
        /// <param name="new_id"></param>
        /// <param name="file"></param>
        /// <param name="countCopy"></param>
        /// <param name="countError"></param>
        /// <returns></returns>
        protected bool SaveArrivalWagons(int new_id, string file, ref int countCopy, ref int countError)
        {
            try
            {
                int count_wagons = 0;
                if (new_id > 0)
                {
                    // Переносим вагоны
                    count_wagons = TransferListArrivalCarsToDB(TransferXMLToListArrivalCars(file, new_id));
                    if (count_wagons > 0) { countCopy++; }
                    if (count_wagons < 0) { countError++; } // Счетчик ошибок при переносе
                }
                if (new_id < 0) { countError++; } // Счетчик ошибок при переносе
                if (count_wagons > 0 & new_id > 0)
                {
                    if (arrival_to_railcars)
                    {
                        ////TODO: Убрать старый метод переноса составов в прибытие, добаить новый переносить из службы КИС
                        TRailCars trc = new TRailCars();
                        int res = trc.ArrivalToRailCars(new_id);
                    }
                    if (arrival_to_railway)
                    {
                        int res = AddBufferArrivalSostav(new_id);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка операции добавления вагонов в БД MT.Arrival, сотава на станции УЗ КР :{0}", new_id), servece_owner, eventID);
            }
            return false;
        }
        /// <summary>
        /// Получить список FileArrivalSostav
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        protected List<FileArrivalSostav> GetFileArrivalSostav(string[] files)
        {
            List<FileArrivalSostav> listfs = new List<FileArrivalSostav>();
            foreach (string file in files)
            {
                try
                {
                    if (!String.IsNullOrEmpty(file))
                    {
                        FileInfo fi = new FileInfo(file);
                        string index = fi.Name.Substring(5, 13);
                        DateTime date = DateTime.Parse(fi.Name.Substring(19, 4) + "-" + fi.Name.Substring(23, 2) + "-" + fi.Name.Substring(25, 2) + " " + fi.Name.Substring(27, 2) + ":" + fi.Name.Substring(29, 2) + ":00");
                        int operation = GetOperationToXml(file);
                        // Добавим строку если определилась операция
                        listfs.Add(new FileArrivalSostav()
                        {
                            Index = index,
                            Date = date,
                            Operation = operation,
                            File = file
                        });

                    }
                }
                catch (Exception e)
                {
                    e.WriteError(String.Format("Ошибка формирования строки списка файлов состава List<FileArrivalSostav>, файл:{0}", file), servece_owner, eventID);
                }
            }
            return listfs;
        }
        /// <summary>
        /// Перености xml-файлы из указанной папки  в таблицы Arrival
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="delete_file"></param>
        /// <returns></returns>
        public int TransferArrival(string fromPath, bool delete_file)
        {
            try
            {
                if (!Directory.Exists(fromPath))
                {
                    String.Format("Указанного пути {0} с xml-файлами для переноса в БД MT.Arrival.. не существует.", fromPath).WriteError(servece_owner, this.eventID);
                    return this.eventID.GetEventIDErrorCode((int)mtt_err.not_fromPath);
                }
                int countCopy = 0;
                int countExist = 0;
                int countError = 0;
                int countDelete = 0;
                string[] files = Directory.GetFiles(fromPath, "*.xml");
                if (files == null | files.Count() == 0) { return 0; }
                String.Format("Определенно {0} xml-файлов для копирования", files.Count()).WriteInformation(servece_owner, this.eventID);
                List<FileArrivalSostav> list_sostav = GetFileArrivalSostav(files);
                var listFileSostavs = from c in list_sostav.OrderBy(c => c.Date).ThenBy(c => c.Index).ThenBy(c => c.Operation)
                                      select new { c.Index, c.Date, c.Operation, c.File };
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                // Пройдемся по списку
                foreach (var fs in listFileSostavs)
                {
                    try
                    {
                        Console.WriteLine("Переносим файл {0}", fs.File);
                        XDocument doc = XDocument.Load(fs.File);
                        // защита от записи повторов
                        FileInfo fi = new FileInfo(fs.File);
                        ArrivalSostav exs_sostav = efmt.GetArrivalSostavOfFile(fi.Name);
                        if (exs_sostav == null)
                        {
                            int? ParentIDSostav = null;
                            int IDArrival = efmt.GetNextIDArrival();
                            // получить не закрытый состав
                            ArrivalSostav no_close_sostav = efmt.GetNoCloseArrivalSostav(fs.Index, fs.Date, this.day_range_arrival_cars);

                            if (no_close_sostav != null)
                            {
                                ParentIDSostav = no_close_sostav.ID;
                                IDArrival = no_close_sostav.IDArrival;
                                // Закрыть состав
                                no_close_sostav.Close = DateTime.Now;
                                efmt.SaveArrivalSostav(no_close_sostav);
                            }
                            ArrivalSostav new_sostav = new ArrivalSostav()
                            {
                                ID = 0,
                                IDArrival = IDArrival,
                                FileName = fi.Name,
                                CompositionIndex = fs.Index,
                                DateTime = fs.Date,
                                Create = DateTime.Now,
                                Close = null,
                                Arrival = null,
                                ParentID = ParentIDSostav,
                                Operation = fs.Operation,

                            };

                            int new_id = efmt.SaveArrivalSostav(new_sostav);
                            if (delete_file & SaveArrivalWagons(new_id, fs.File, ref  countCopy, ref  countError))
                            {
                                File.Delete(fs.File);
                                countDelete++;
                            }
                        }
                        else
                        {
                            // Проверка сравниваем количество если совподает удаляем файл, иначе добавляем новые вагоны и удаляем файл
                            List<ArrivalCars> list = TransferXMLToListArrivalCars(fs.File, exs_sostav.ID);
                            List<ArrivalCars> listdb = efmt.GetArrivalCarsOfSostav(exs_sostav.ID).ToList();
                            if (list != null && listdb != null)
                            {
                                if (list.Count() != listdb.Count())
                                {
                                    efmt.DeleteArrivalCarsOfSostav(exs_sostav.ID);
                                    if (delete_file & SaveArrivalWagons(exs_sostav.ID, fs.File, ref  countCopy, ref  countError))
                                    {
                                        File.Delete(fs.File);
                                        countDelete++;
                                    }
                                }
                                else
                                {
                                    // Файл перенесен ранеее, удалим его если это требуется
                                    if (delete_file)
                                    {
                                        File.Delete(fs.File);
                                        countDelete++;
                                    }
                                }
                                countExist++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка переноса xml-файла в БД MT.Arrival, файл {0}", fs.File), servece_owner, eventID);
                        countError++;
                    }
                }
                string mess = String.Format("Перенос xml-файлов в БД MT.Arrival выполнен, определено для переноса {0} xml-файлов, перенесено {1}, были перенесены ранее {2}, ошибки при переносе {3}, удаленно {4}.", files.Count(), countCopy, countExist, countError, countDelete);
                mess.WriteInformation(servece_owner, this.eventID);
                if (files != null && files.Count() > 0) { mess.WriteEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return files.Count();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrival(fromPath={0}, delete_file={1})", fromPath, delete_file), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перености xml-файлы из папки по умолчанию  в таблицы Arrival
        /// </summary>
        /// <returns></returns>
        public int TransferArrival()
        {
            return TransferArrival(this.fromPath, this.delete_file);
        }
        #endregion

        #region TransferWagonsTracking Перенос вагонов из Web.Api МетТранса
        /// <summary>
        /// Добавить список изменений по вагону
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int TransferWagonsTracking(List<WagonsTrackingMT> list)
        {
            if (list == null || list.Count() == 0) return 0;
            RWReference rw_ref = new RWReference(true);
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            int countCopy = 0;
            int countError = 0;
            int res = 0;
            //string trans_id = "";
            try
            {
                foreach (WagonsTrackingMT wt in list.OrderBy(l => l.dt))
                {
                    try
                    {
                        int id_cargo = 0;
                        if (wt.kgr != null && wt.kgr > 0)
                        {
                            id_cargo = rw_ref.GetIDReferenceCargoOfCorrectCodeETSNG((int)wt.kgr);
                        }
                        res = efmt.SaveWagonsTracking(new WagonsTracking()
                        {
                            id = 0,
                            nvagon = wt.nvagon,
                            st_disl = wt.st_disl,
                            nst_disl = wt.nst_disl,
                            kodop = wt.kodop,
                            nameop = wt.nameop,
                            full_nameop = wt.full_nameop,
                            dt = wt.dt,
                            st_form = wt.st_form,
                            nst_form = wt.nst_form,
                            idsost = wt.idsost,
                            nsost = wt.nsost,
                            st_nazn = wt.st_nazn,
                            nst_nazn = wt.nst_nazn,
                            ntrain = wt.ntrain,
                            st_end = wt.st_end,
                            nst_end = wt.nst_end,
                            kgr = wt.kgr,
                            nkgr = wt.nkgr,
                            id_cargo = id_cargo,
                            kgrp = wt.kgrp,
                            ves = wt.ves,
                            updated = wt.updated,
                            kgro = wt.kgro,
                            km = wt.km,
                        });
                        if (res >= 0)
                        {
                            countCopy++;
                        }
                        else
                        {
                            countError++;
                        }
                        //trans_id+= res.ToString() + "; ";
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка переноса информации по вагону {0} дата операции {1} в БД MT.WagonsTracking", wt.nvagon, wt.dt), servece_owner, eventID);
                        countError++;
                    }
                }
                string mess = String.Format("Вагон №{0}, Определенно: {1} новых операций, перенесено в БД MT.WagonsTracking : {2}, пропущено по ошибке : {3}", (list != null && list.Count() > 0 ? list[0].nvagon : -1), list.Count(), countCopy, countError);
                mess.WriteInformation(servece_owner, this.eventID);
                if (list != null && list.Count() > 0) { mess.WriteEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return countError == 0 ? countCopy : -1;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferWagonsTracking(list={0})", list), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенести информацию слежения за вагонами
        /// </summary>
        /// <returns></returns>
        public int TransferWagonsTracking(DateTime datetime_start_new_tracking, string url, string user, string psw, string api)
        {
            int countCopy = 0;
            int countSkip = 0;
            int countError = 0;
            int res = 0;
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            try
            {
                WebApiClientMetallurgTrans client = new WebApiClientMetallurgTrans(url, user, psw, api);
                List<WagonsTrackingMT> list_tracking = client.GetWagonsTracking();
                if (list_tracking == null) return (int)mtt_err.not_listWagonsTracking;
                foreach (WagonsTrackingMT wt in list_tracking)
                {
                    try
                    {
                        List<WagonsTracking> list = efmt.GetWagonsTrackingOfNumCars(wt.nvagon).ToList();
                        WagonsTracking wt_old = list.OrderByDescending(t => t.dt).FirstOrDefault();
                        if (wt_old == null)
                        {
                            // Обновляем информацию переносим все
                            List<WagonsTrackingMT> list_new = client.GetWagonsTracking(wt.nvagon, datetime_start_new_tracking);
                            if (list_new == null || list_new.Count() == 0)
                            {
                                list_new.Add(wt);
                            }
                            res = TransferWagonsTracking(list_new);
                            if (res >= 0)
                            {
                                countCopy++;
                            }
                            else
                            {
                                countError++;
                            }
                        }
                        else
                        {
                            if (wt_old.dt != wt.dt)
                            {
                                // Обновляем информацию добавим новое
                                List<WagonsTrackingMT> list_new = client.GetWagonsTracking(wt.nvagon, ((DateTime)wt_old.dt).AddSeconds(1));
                                res = TransferWagonsTracking(list_new);
                                if (res >= 0)
                                {
                                    countCopy++;
                                }
                                else
                                {
                                    countError++;
                                }
                            }
                            else
                            {
                                countSkip++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.WriteError(String.Format("Ошибка переноса информации по вагону {0} дата операции {1} в БД MT.WagonsTracking", wt.nvagon, wt.dt), servece_owner, eventID);
                        countError++;
                    }
                }
                string mess = String.Format("Перенос информации о слежении за вагонами в БД MT.WagonsTracking выполнен, доступна информация о {0} вагонах, перенесено новых данных {1}, пропущено {2}, ошибки при переносе {3}."
                    , list_tracking.Count(), countCopy, countSkip, countError);
                mess.WriteInformation(servece_owner, this.eventID);
                if (list_tracking != null && list_tracking.Count() > 0) { mess.WriteEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return list_tracking.Count();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferWagonsTracking()"), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенести информацию слежения за вагонами
        /// </summary>
        /// <returns></returns>
        public int TransferWagonsTracking()
        {
            return TransferWagonsTracking(this.datetime_start_new_tracking, this.url_wagons_tracking, this.user_wagons_tracking, this.psw_wagons_tracking, this.api_wagons_tracking);
        }
        #endregion

        #region WTCycle Формирование циклограмм движения вагонов по данным МетТранса (TransferWagonsTracking)
        /// <summary>
        /// Проверка строка WagonsTracking это начало цикла
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsStartCycle(WagonsTracking car)
        {
            return IsArrivalAMKR(car);
        }

        /// <summary>
        /// Начало вагон на АМКР (Старт нового цикла)
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsArrivalAMKR(WagonsTracking car)
        {
            return car.st_disl == 46700 & (car.nameop == "ОТОТ" | car.nameop == "ВЫГ2" | car.nameop == "ВЫГРН") & (car.kgrp == 7932 | car.kgrp == 3437 | car.kgrp == 6302) ? true : false;
        }
        /// <summary>
        ///  Отправка вагона клиенту из АМКР
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsAMKRSendingClient(WagonsTracking car)
        {
            return (car.st_disl == 46700 | car.st_disl == 46720) & (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН") & car.st_end != 46700 ? true : false;
        }
        /// <summary>
        ///  Отправка вагона клиенту от клиента (пустой вагон после выгрузкиотроавлен на загрузку к другому клиенту)
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsClientSendingClient(WagonsTracking car, int client_code)
        {
            return (client_code != 46700 & car.st_disl != 46720) && (car.st_disl == client_code) && (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН") && (car.st_end != client_code && car.st_end != 46700 && car.st_end != 46720) ? true : false;
        }
        /// <summary>
        /// Начало вагон у клиента
        /// </summary>
        /// <param name="car"></param>
        /// <param name="client_code"></param>
        /// <returns></returns>
        public bool IsArrivalClient(WagonsTracking car, int client_code)
        {
            return car.st_disl != 46700 & (car.nameop == "ОТОТ" | car.nameop == "ВЫГРН") & car.st_disl == client_code ? true : false;
        }
        /// <summary>
        /// Начало возвращения вагона от клиента на АМКР
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsReturnAMKR(WagonsTracking car)
        {
            return car.st_end == 46700 & (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН") ? true : false;
        }

        /// <summary>
        /// Перенос и формирование циклограммы по всем вагонам
        /// </summary>
        /// <returns></returns>
        public int TransferWTCycle()
        {
            try
            {
                int transfer = 0;
                int transfer_car = 0;
                int error = 0;
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                // выбираю список всех уникальных вагононов который были на контроле
                List<int> cars = efmt.WagonsTracking.ToList().Select(c => c.nvagon).Distinct().ToList();
                // Формирование циклограммы по всем вагонам списка 
                foreach (int car in cars)
                {
                    int res_tr = TransferWTCycle(car);
                    if (res_tr > 0) { transfer++; transfer_car += res_tr; }
                    if (res_tr < 0) { error++; }
                    if (res_tr == 0) { transfer++; }
                }
                string mess = String.Format("Коррекция данных БД MT.WagonsTracking выполнена, найдено для коррекции {0} вагонов, скорректировано {1} групп вагонов, общее количество коррекций {2}, ошибки при коррекции {3}."
                    , cars != null ? cars.Count() : 0, transfer, transfer_car, error);
                mess.WriteInformation(servece_owner, this.eventID);
                if (cars != null && cars.Count() > 0) { mess.WriteEvents(error > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return transfer;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferWTCycle()"), servece_owner, eventID);
                return -1;
            }
        }
        /// <summary>
        /// Перенос и формирование циклограммы по указаному вагону 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int TransferWTCycle(int num)
        {
            try
            {
                int transfer = 0;
                int error = 0;
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                int cycle = 0;
                wtroute route = wtroute.not;
                int station_sending = 0;
                int station_from = 0;
                int? kode_cargo_out = null;
                List<WagonsTracking> list_wt_cars;
                // Получим оследнюю запись циклограммы
                WTCycle last_Cycle = efmt.GetWTCycleOfNumCar(num).OrderByDescending(c => c.id).FirstOrDefault();
                if (last_Cycle == null)
                {
                    // Последней записи нет переносим все
                    list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).OrderBy(t => t.dt).ToList();
                }
                else
                {
                    list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).Where(t => t.id > last_Cycle.id_wt).OrderBy(t => t.dt).ToList();
                    cycle = last_Cycle.cycle;
                    route = (wtroute)last_Cycle.route;
                    station_sending = last_Cycle.station_end;
                    station_from = last_Cycle.station_from;
                    kode_cargo_out = last_Cycle.WagonsTracking.kgrp;
                }
                // Переносим двнные
                foreach (WagonsTracking car in list_wt_cars)
                {
                    if (car.kgrp != null)
                    {
                        // Кто грузополучатель?
                        if (car.kgrp == 7932 | car.kgrp == 3437 | car.kgrp == 6302)
                        {
                            // Грузополучатель АМКР
                            // Первый цикл
                            if (cycle == 0 && car.kgrp > 0 && route == wtroute.not)
                            {
                                // возврат
                                route = wtroute.ret;
                                station_sending = 46700;
                            }
                            // Вагон возвращается на АМКР?
                            if (car.st_end == 46700 && (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН"))
                            {
                                //Вагон следует на АМКР
                                if (route == wtroute.client)
                                {
                                    // возврат
                                    route = wtroute.ret;
                                    station_from = (int)car.st_disl;
                                    station_sending = 46700;
                                }
                                else
                                {
                                    if (route == wtroute.send)
                                    {
                                        // возврат
                                        route = wtroute.ret;
                                        station_sending = 46700;

                                    }
                                    // возврат вагона на АМКР а route != wtroute.client
                                }
                            }
                            // Вагон прибыл на АМКР?
                            if (car.st_disl == 46700 & (car.nameop == "ОТОТ" | car.nameop == "ВЫГ2" | car.nameop == "ВЫГРН"))
                            {
                                // Вагон прибыл на АМКР
                                if (route == wtroute.ret | route == wtroute.not)
                                {
                                    // Прибыл новый цикл
                                    cycle++;
                                    route = wtroute.amkr;
                                    station_sending = 46700;
                                    station_from = station_sending;
                                }
                                else
                                {
                                    if (route == wtroute.client)
                                    {
                                        cycle++;
                                        route = wtroute.amkr;
                                        station_sending = 46700;
                                        station_from = station_sending;
                                    }
                                    // Вагон прибыл на АМКР а route != wtroute.ret
                                }
                            }
                            // Проверим изменения на код грузополучателя (Направление изменилось?)
                            if (car.kgrp > 0 && kode_cargo_out > 0 && car.nameop != "ОКОТ" && car.kgrp != kode_cargo_out)
                            {
                                // возврат
                                route = wtroute.ret;
                                station_from = (int)car.st_disl;
                                station_sending = 46700;
                            }
                        }
                        else
                        {
                            // Грузополучатель Клиент 
                            // Первый цикл
                            if (cycle == 0 && car.kgrp > 0 && route == wtroute.not)
                            {
                                // отправка клиенту
                                route = wtroute.send;
                                station_from = (int)car.st_disl;
                                station_sending = (int)car.st_end;
                                //kode_cargo_out = car.kgrp;
                            }

                            // Вагон движится клиенту из АМКР?
                            if ((car.st_disl == 46700 | car.st_disl == 46720) & (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН") & car.st_end != 46700)
                            {
                                // Вагон движется к клиенту
                                if (route == wtroute.amkr)
                                {
                                    // отправка клиенту
                                    route = wtroute.send;
                                    //station_from = 46700;
                                    station_from = (int)car.st_disl;
                                    station_sending = (int)car.st_end;
                                    //kode_cargo_out = car.kgrp;
                                }
                                else
                                {
                                    if (route == wtroute.ret)
                                    {
                                        // Нет фиксации зашел на АМКР (будет выполнена начало нового цикла и отправка клиенту)
                                        cycle++;
                                        route = wtroute.send;
                                        //station_from = 46700;
                                        station_from = (int)car.st_disl;
                                        station_sending = (int)car.st_end;
                                    }
                                    // Отправка клиенту из АМКР а route != wtroute.amkr
                                }
                            }
                            // Вагон движится клиенту от клиета?
                            if ((station_sending != 46700 & car.st_disl != 46720) && (car.st_disl == station_sending) && (car.nameop == "ОДПВ" | car.nameop == "ПГР2" | car.nameop == "ПОГРН") && (car.st_end != station_sending && car.st_end != 46700 && car.st_end != 46720))
                            {
                                // Вагон движится клиенту от клиета
                                if (route == wtroute.client)
                                {
                                    // отправка опять клиенту
                                    route = wtroute.send;
                                    //station_from = station_sending;
                                    station_from = (int)car.st_disl;
                                    station_sending = (int)car.st_end;
                                }
                                else
                                {
                                    // Отправка клиенту другому клиенту а route != wtroute.client
                                }

                            }
                            // Вагон прибыл к клиенту?
                            if (car.st_disl != 46700 && (car.nameop == "ОТОТ" | car.nameop == "ВЫГРН") && car.st_disl == car.st_end)
                            {
                                // Вагон прибыл к клиенту
                                if (route == wtroute.send)
                                {
                                    // прибыл клиенту
                                    route = wtroute.client;
                                    station_from = station_sending;
                                }
                                else
                                {
                                    // Если wtroute.client пропускаем (за ОТОТ может прийти ВЫГРН)
                                    // прибытие клиенту а route != wtroute.send
                                }
                            }
                            // Проверим изменения на код грузополучателя (Направление изменилось?)
                            if (car.kgrp > 0 && kode_cargo_out > 0 && car.nameop != "ОКОТ" && car.kgrp != kode_cargo_out)
                            {
                                // Отправка из АМКР
                                if (kode_cargo_out == 7932 | kode_cargo_out == 3437 | kode_cargo_out == 6302)
                                {
                                    // Отправка из АМКР
                                    // отправка клиенту
                                    route = wtroute.send;
                                    //station_from = 46700;
                                    station_from = (int)car.st_disl;
                                    station_sending = (int)car.st_end;

                                }
                                else
                                {
                                    // Отправка от клиента клиенту   
                                    // отправка опять клиенту
                                    route = wtroute.send;
                                    station_from = (int)car.st_disl;
                                    station_sending = (int)car.st_end;

                                }
                            }
                        }
                        // Сохранить грузополучателя если опреация не ОКОТ (на этой операции не меняется станция получателя)
                        if (car.nameop != "ОКОТ") { kode_cargo_out = car.kgrp; }// Всегда актуальный код грузополучателя
                    }
                    else
                    {
                        // Обработать эти операции
                        // ВУ23	Перечисление в неиспр. вагоны
                        // ПРМ	Прием на дорогу
                        // ВУ36	Возвращение в рабочий парк
                    }
                    // Сохраним если это не нулевой цикл или если нулевой тогда код грузополучателя должен быть определен

                    if (cycle > 0 || (cycle == 0 && car.kgrp > 0))
                    {
                        int res = efmt.SaveWTCycle(new WTCycle()
                        {
                            id = 0,
                            id_wt = car.id,
                            cycle = cycle,
                            station_end = station_sending,
                            station_from = station_from,
                            route = (int)route
                        });
                        if (res > 0) { transfer++; } else { error++; }
                    }
                }
                return transfer;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferWTCycle(num={0})", num), servece_owner, eventID);
                return -1;
            }
        }
        public int TransferWTCycle_Station(int num)
        {
            try
            {
                int transfer = 0;
                int error = 0;
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                int cycle = 0;
                wtroute route = wtroute.not;
                int station_end = 0;
                int station_from = 0;
                int shipper = 0;
                int consignee = 0;
                int kode_cargo_consignee = 0; // Код грузополучатель
                int kode_cargo_shipper = 0; // Код грузоотправитель
                List<WagonsTracking> list_wt_cars;
                // Получим оследнюю запись циклограммы
                WTCycle last_Cycle = efmt.GetWTCycleOfNumCar(num).OrderByDescending(c => c.id).FirstOrDefault();
                if (last_Cycle == null)
                {
                    // Последней записи нет переносим все
                    list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).OrderBy(t => t.dt).ToList();
                }
                else
                {
                    list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).Where(t => t.id > last_Cycle.id_wt).OrderBy(t => t.dt).ToList();
                    cycle = last_Cycle.cycle;
                    route = (wtroute)last_Cycle.route;
                    station_end = last_Cycle.station_end;
                    station_from = last_Cycle.station_from;
                    shipper = last_Cycle.Shipper != null ? (int)last_Cycle.Shipper : 0;
                    consignee = last_Cycle.Consignee != null ? (int)last_Cycle.Consignee : 0;
                    kode_cargo_consignee = last_Cycle.WagonsTracking.kgrp != null ? (int)last_Cycle.WagonsTracking.kgrp : 0;
                    kode_cargo_shipper = last_Cycle.WagonsTracking.kgro != null ? (int)last_Cycle.WagonsTracking.kgro : 0;
                }
                // Переносим двнные
                foreach (WagonsTracking car in list_wt_cars)
                {
                    // Определим цепочку следования грузополучатель грузоприемник
                    kode_cargo_consignee = car.kgrp > 0 ? (int)car.kgrp : kode_cargo_consignee;
                    kode_cargo_shipper = car.kgro > 0 ? (int)car.kgro : kode_cargo_shipper;                  
                    
                    // Конечная станция не равна null и "в регулировании"
                    if (car.st_end > 0)
                    {
                        // Конечная станция Кривой Рог
                        if (car.st_end == 46700 || car.st_end == 46720)
                        {
                            // Конечная станция Кривой Рог
                            // Изменилось направление с клиента на Кривой Рог
                            if (car.st_end != station_end )
                            {
                                // Отправлен на АМКР (считаем время отправки)
                                if (route == wtroute.client || route == wtroute.not)
                                {
                                    // возврат
                                    route = wtroute.ret;
                                    station_from = (int)car.st_disl;
                                    station_end = (int)car.st_end;
                                }
                                else
                                {
                                    // Алгоритм неопределил "вагон у клиента"  
                                    if (route == wtroute.send)
                                    {
                                        // возврат
                                        route = wtroute.ret;
                                        station_from = (int)car.st_disl;
                                        station_end = (int)car.st_end;

                                    }
                                    // Сообщение "Возврат вагона на АМКР а route != wtroute.client"
                                } // {end Отправлен на АМКР}
                            }
                            else
                            {
                                // Прибыл на АМКР?
                                if (car.st_disl == car.st_end & (car.nameop == "ОТОТ" | car.nameop == "ВЫГ2" | car.nameop == "ВЫГРН"))
                                {
                                    // Прибыл на АМКР (считаем время прибытия)
                                    if (route == wtroute.ret | route == wtroute.not)
                                    {
                                        // Прибыл новый цикл
                                        cycle++;
                                        route = wtroute.amkr;
                                        station_from = (int)car.st_disl;
                                        station_end = (int)car.st_end;
                                    }
                                    else
                                    {
                                        // Алгоритм неопределил "вагон возвращается"  
                                        if (route == wtroute.client)
                                        {
                                            cycle++;
                                            route = wtroute.amkr;
                                            station_from = (int)car.st_disl;
                                            station_end = (int)car.st_end;
                                        }
                                        // Сообщение "Вагон прибыл на АМКР а route != wtroute.ret"
                                    }
                                } // {end Прибыл на АМКР?}
                            } // {end Изменилось направление с клиента на Кривой Рог}
                        }
                        else
                        {
                            // Конечная станция Клиент                        
                            // Изменилось направление с Кривой Рог на клиент?
                            if (car.st_end != station_end)
                            {
                                // Вагон движется к клиенту
                                // Откуда движется вагон от АМКР или Клиента
                                if (car.st_disl == 46700 || car.st_disl == 46720)
                                {
                                    // Вагон движется к клиенту от АМКР (считаем время отправки)
                                    if (route == wtroute.amkr || route == wtroute.not)
                                    {
                                        // отправка клиенту
                                        route = wtroute.send;
                                        station_from = (int)car.st_disl;
                                        station_end = (int)car.st_end;
                                    }
                                    else
                                    {
                                        // Алгоритм неопределил "вагон на АМКР" 
                                        if (route == wtroute.ret)
                                        {
                                            cycle++;
                                            route = wtroute.send;
                                            station_from = (int)car.st_disl;
                                            station_end = (int)car.st_end;
                                        }
                                        // Сообщение "Отправка клиенту из АМКР а route != wtroute.amkr"
                                    }
                                }
                                else
                                {
                                    // Вагон движется к клиенту от клиента (считаем время отправки)                                
                                    if (route == wtroute.client || route == wtroute.not)
                                    {
                                        // отправка опять клиенту
                                        route = wtroute.send;
                                        station_from = (int)car.st_disl;
                                        station_end = (int)car.st_end;
                                    }
                                    else
                                    {
                                        // Алгоритм неопределил "вагон у клиента" 
                                        if (route == wtroute.send)
                                        {
                                            station_from = (int)car.st_disl;
                                            station_end = (int)car.st_end;
                                        }

                                        // Сообщение "Отправка клиенту другому клиенту а route != wtroute.client"
                                    }

                                }

                            }
                            else {
                                // Вагон прибыл к клиенту?
                                if ((car.st_disl != 46700 || car.st_disl != 46720) && (car.nameop == "ОТОТ" | car.nameop == "ВЫГРН"))
                                {
                                    // Вагон прибыл к клиенту
                                    if (route == wtroute.send || route == wtroute.not)
                                    {
                                        // прибыл клиенту
                                        route = wtroute.client;
                                        station_from = (int)car.st_disl;
                                        station_end = (int)car.st_end;
                                    }
                                    else
                                    {
                                        // Алгоритм неопределил "вагон направлен клиенту" 
                                        if (route == wtroute.amkr)
                                        {
                                            route = wtroute.client;
                                            station_from = (int)car.st_disl;
                                            station_end = (int)car.st_end;
                                        }
                                        // Если wtroute.client пропускаем (за ОТОТ может прийти ВЫГРН)
                                        // Сообщение "Вагон прибыл к клиенту а route != wtroute.send"
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Обработать эти операции
                        //ВЫГРН,РАСФ,ТСП,ВЫГРО,ОКОТ,ПРМ,ФОРМ,-НЕИС,ОСВ,ВУ36
                    }
                    // Сохраним если это не нулевой цикл или если нулевой тогда код грузополучателя должен быть определен

                    if (cycle > 0 || (cycle == 0 && car.st_end > 0))
                    {
                        int res = efmt.SaveWTCycle(new WTCycle()
                        {
                            id = 0,
                            id_wt = car.id,
                            cycle = cycle,
                            station_end = station_end,
                            station_from = station_from,
                            Shipper = kode_cargo_shipper,
                            Consignee = kode_cargo_consignee,
                            route = (int)route
                        });
                        if (res > 0) { transfer++; } else { error++; }
                    }
                }
                return transfer;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferWTCycle_Station(num={0})", num), servece_owner, eventID);
                return -1;
            }
        }

        //public int TransferWTCycle(int num)
        //{
        //    try
        //    {
        //        int transfer = 0;
        //        int error = 0;
        //        EFMetallurgTrans efmt = new EFMetallurgTrans();
        //        int cycle = 0;
        //        wtroute route = wtroute.not;
        //        int station_sending = 0;
        //        int station_from = 0;
        //        int? kode_cargo_out = null;
        //        List<WagonsTracking> list_wt_cars;
        //        // Получим оследнюю запись циклограммы
        //        WTCycle last_Cycle = efmt.GetWTCycleOfNumCar(num).OrderByDescending(c => c.id).FirstOrDefault();
        //        if (last_Cycle == null)
        //        {
        //            // Последней записи нет переносим все
        //            list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).OrderBy(t => t.dt).ToList();
        //        }
        //        else
        //        {
        //            list_wt_cars = efmt.GetWagonsTrackingOfNumCars(num).Where(t => t.id > last_Cycle.id_wt).OrderBy(t => t.dt).ToList();
        //            cycle = last_Cycle.cycle;
        //            route = (wtroute)last_Cycle.route;
        //            station_sending = last_Cycle.station_end;
        //            station_from = last_Cycle.station_from;
        //            kode_cargo_out = last_Cycle.WagonsTracking.kgrp;
        //        }
        //        // Переносим двнные
        //        foreach (WagonsTracking car in list_wt_cars)
        //        {
        //            if (IsStartCycle(car))
        //            { // Это старт цикла
        //                cycle++;
        //                route = wtroute.amkr;
        //                station_sending = 46700;
        //                station_from = station_sending;
        //            }
        //            // Проверка на отправку вагона клиенту из АМКР
        //            if (IsAMKRSendingClient(car))
        //            {
        //                if (route == wtroute.amkr)
        //                {
        //                    // отправка клиенту
        //                    route = wtroute.send;
        //                    station_from = 46700;
        //                    station_sending = (int)car.st_end;
        //                }
        //                else
        //                {
        //                    if (route == wtroute.ret)
        //                    {
        //                        // Нет фиксации зашел на АМКР (будет выполнена начало нового цикла и отправка клиенту)
        //                        cycle++;
        //                        route = wtroute.send;
        //                        station_from = 46700;
        //                        station_sending = (int)car.st_end;
        //                    }
        //                    // Отправка клиенту из АМКР а route != wtroute.amkr
        //                }
        //            }
        //            // Проверка на прибытие клиенту вагона
        //            if (IsArrivalClient(car, station_sending))
        //            {
        //                if (route == wtroute.send)
        //                {
        //                    // прибыл клиенту
        //                    route = wtroute.client;
        //                    station_from = station_sending;
        //                }
        //                else
        //                {
        //                    // Если wtroute.client пропускаем (за ОТОТ может прийти ВЫГРН)
        //                    // прибытие клиенту а route != wtroute.send
        //                }
        //            }
        //            // Проверка на отправку вагона клиенту от клиента
        //            if (IsClientSendingClient(car, station_sending))
        //            {
        //                if (route == wtroute.client)
        //                {
        //                    // отправка опять клиенту
        //                    route = wtroute.send;
        //                    station_from = station_sending;
        //                    station_sending = (int)car.st_end;
        //                }
        //                else
        //                {
        //                    // Отправка клиенту другому клиенту а route != wtroute.client
        //                }
        //            }
        //            // Проверка на возврат вагона на АМКР
        //            if (IsReturnAMKR(car))
        //            {
        //                if (route == wtroute.client)
        //                {
        //                    // возврат
        //                    route = wtroute.ret;
        //                    station_sending = 46700;
        //                }
        //                else
        //                {
        //                    if (route == wtroute.send)
        //                    {
        //                        // возврат
        //                        route = wtroute.ret;
        //                        station_sending = 46700;
        //                    }

        //                    // возврат вагона на АМКР а route != wtroute.client
        //                }
        //            }
        //            // Проверка на прибытие вагона на АМКР
        //            if (IsArrivalAMKR(car))
        //            {
        //                if (route == wtroute.ret)
        //                {
        //                    // возврат
        //                    route = wtroute.amkr;
        //                    station_sending = 46700;
        //                    station_from = station_sending;
        //                }
        //                else
        //                {
        //                    // Если wtroute.amkr пропускаем (начало цыкла было определено выше)
        //                    // Вагон прибыл на АМКР а route != wtroute.ret
        //                }
        //            }
        //            //if (route == wtroute.amkr & IsAMKRSendingClient(car))
        //            //{ // отправка клиенту
        //            //    route = wtroute.send;
        //            //    station_from = 46700;
        //            //    station_sending = (int)car.st_end;
        //            //}
        //            //if (route == wtroute.send & IsArrivalClient(car, station_sending))
        //            //{ // прибыл клиенту
        //            //    route = wtroute.client;
        //            //    station_from = station_sending;
        //            //}
        //            //if (route == wtroute.client & IsReturnAMKR(car))
        //            //{ // возврат
        //            //    route = wtroute.ret;
        //            //    station_sending = 46700;

        //            //}
        //            //if (route == wtroute.ret & IsArrivalAMKR(car))
        //            //{ // возврат
        //            //    route = wtroute.amkr;
        //            //    station_sending = 46700;
        //            //    station_from = station_sending;
        //            //}
        //            // Сохраним
        //            if (cycle > 0)
        //            {
        //                int res = efmt.SaveWTCycle(new WTCycle()
        //                {
        //                    id = 0,
        //                    id_wt = car.id,
        //                    cycle = cycle,
        //                    station_end = station_sending,
        //                    station_from = station_from,
        //                    route = (int)route
        //                });
        //                if (res > 0) { transfer++; } else { error++; }
        //            }
        //        }
        //        return transfer;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("TransferWTCycle(num={0})", num), servece_owner, eventID);
        //        return -1;
        //    }
        //}
        #endregion

        #region CloseApproaches Автозакрытие вагонов на подходах
        /// <summary>
        /// проверить все вагоны
        /// </summary>
        /// <returns></returns>
        public int CloseApproachesCars()
        {
            try
            {
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                int close = 0;
                int skip = 0;
                int error = 0;

                List<ApproachesCars> list = new List<ApproachesCars>();
                DateTime dt = DateTime.Now.AddDays(-1 * this.day_range_approaches_cars_arrival);
                list = efmt.GetNoCloseApproachesCars().Where(c => c.DateOperation < dt).OrderBy(c => c.DateOperation).ToList();
                foreach (ApproachesCars car in list.ToList())
                {
                    //ApproachesCars car_close = car;
                    int res = CloseApproachesCar(car);
                    if (res > 0) { close++; }
                    if (res == 0) { skip++; }
                    if (res < 0) { error++; }
                }
                string mess = String.Format("Коррекция вагонов на подходах БД MT.Approaches - выполнена, определено {0} не закрытых вагонов, закрыто автоматически {1}, пропущено {2}, ошибки закрытия {3}.",
                    list != null ? list.Count() : 0, close, skip, error);
                mess.WriteInformation(servece_owner, this.eventID);
                if (list != null && list.Count() > 0) { mess.WriteEvents(error > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return close;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseApproachesCars()"), servece_owner, eventID);
                return -1;
            }
        }

        public int CloseApproachesCar(ApproachesCars car)
        {
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            int result = 0;
            try
            {
                //-----------------------------------------------------------------------------
                // Проверим есть продолжение истории с вагоном в таблице на подходах
                ApproachesCars car_next = efmt.GetApproachesCarsOfNextCar(car.Num, car.DateOperation);
                if (car_next != null)
                {
                    if (car.CargoCode == car_next.CargoCode)
                    {
                        // входит в диапазон времени
                        if (car.DateOperation.Date.AddDays(this.day_range_approaches_cars) > car_next.DateOperation)
                        {
                            car.NumDocArrival = (int)mtt_err_arrival.close_car;
                            // предыдущий состав не прибыл на станцию назначения
                            if (car.CodeStationOn != car.CodeStationCurrent)
                            {
                                //parentid = car.ID;
                            }
                            else
                            {
                                // новый состав стоит еще на станции что и предыдущий
                                if (car.CodeStationCurrent == car_next.CodeStationCurrent)
                                {
                                    //parentid = car.ID;
                                }
                                else
                                {
                                    // вагон начал движение по новому маршруту
                                    car.NumDocArrival = (int)mtt_err_arrival.close_new_route;
                                }
                            }
                        }
                        else
                        {
                            // больше допустимого интервала
                            car.NumDocArrival = (int)mtt_err_arrival.close_timeout;
                        }
                    }
                    else
                    {
                        // грузы в вагонах разные
                        car.NumDocArrival = (int)mtt_err_arrival.close_different_cargo;
                    }
                    car.Arrival = car_next.DateOperation;
                    Console.WriteLine("Вагон:{0}, дата операции:{1}, индекс:{2}, найдено продолжение на подходах, код:{3}", car.Num, car.DateOperation, car.CompositionIndex, car.NumDocArrival);
                    return efmt.SaveApproachesCars(car); // сохранить изменение
                }
                //-----------------------------------------------------------------------------
                // Проверим есть продолжение истории с вагоном в таблице прибыл на УЗ
                ArrivalCars car_arrival = efmt.GetArrivalCarsOfNextCar(car.Num, car.DateOperation);
                if (car_arrival != null)
                {

                    int num = car_arrival.NumDocArrival > 0 ? (int)car_arrival.NumDocArrival : (int)mtt_err_arrival.close_arrival_uz;
                    DateTime dt_oper = car_arrival.DateOperation;
                    car.NumDocArrival = num;
                    car.Arrival = dt_oper;
                    Console.WriteLine("Вагон:{0}, дата операции:{1}, индекс:{2}, найдено продолжение в прибитии, код:{3}", car.Num, car.DateOperation, car.CompositionIndex, car.NumDocArrival);
                    return efmt.SaveApproachesCars(car); // сохранить изменение         
                }
                //-----------------------------------------------------------------------------
                // Проверим вагон дошол до станции назначения, если да закроем его через 2 суток
                if (car.CodeStationOn == car.CodeStationCurrent & car.DateOperation.AddDays(this.day_range_approaches_cars_arrival) < DateTime.Now)
                {
                    car.NumDocArrival = (int)mtt_err_arrival.close_arrival_station_on;
                    car.Arrival = DateTime.Now;
                    Console.WriteLine("Вагон:{0}, дата операции:{1}, индекс:{2}, прибыл на станцию (3 дня прошло -закрываю), код:{3}", car.Num, car.DateOperation, car.CompositionIndex, car.NumDocArrival);
                    return efmt.SaveApproachesCars(car); // сохранить изменение                        
                }
                //-----------------------------------------------------------------------------
                // TODO:Отключить Проверим вагон есть в системе КИС

                //EFWagons ef_wag = new EFWagons();
                //List<PromNatHist> list = ef_wag.GetNatHistOfVagonGreater(car.Num, car.DateOperation).ToList();

                //if (list != null && list.Count() > 0) { 
                //    car.NumDocArrival = (int)mtt_err_arrival.close_arrival_station_on;
                //    car.Arrival = list[0].DAT_VVOD != null ? list[0].DAT_VVOD : DateTime.Now;
                //    Console.WriteLine("Вагон:{0}, дата операции:{1}, индекс:{2}, прибыл на станцию (3 дня прошло -закрываю), код:{3}", car.Num, car.DateOperation, car.CompositionIndex, car.NumDocArrival);
                //    return efmt.SaveApproachesCars(car); // сохранить изменение                 
                //}
                Console.WriteLine("Вагон:{0}, дата операции:{1}, индекс:{2} - оставлен до выяснения", car.Num, car.DateOperation, car.CompositionIndex);
                return result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CloseApproachesCar(car={0})", car), servece_owner, eventID);
                return -1;
            }

        }
        #endregion

        #region BufferArrivalSostav Перенос составов на станциях УЗ Кривого Рога из МТ в Railway
        /// <summary>
        /// Добавить строку в таблицу-буффер переноса составов на путь отправки на АМКР станции УЗ.
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public int AddBufferArrivalSostav(int id_sostav)
        {
            try
            {
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                ArrivalSostav sostav = efmt.GetArrivalSostav(id_sostav);
                // Закрыть старые прибытия
                List<BufferArrivalSostav> list_arrival = efmt.GetBufferArrivalSostavOfIDArrival(sostav.IDArrival).ToList(); // Получим список прибытий
                if (list_arrival != null)
                {
                    // Получим список не закрытых старых составов принадлежащих прибытию
                    List<BufferArrivalSostav> list_arrival_no_close = list_arrival.Where(b => b.id_sostav < sostav.ID & b.close == null).ToList();
                    // Закроем
                    foreach (BufferArrivalSostav bas in list_arrival_no_close)
                    {
                        bas.close = DateTime.Now;
                        bas.close_user = System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        bas.close_comment = "По составу пришло новое ТСП";

                        int res_close = efmt.SaveBufferArrivalSostav(bas);

                    }
                }
                // Создадим новое прибытие
                BufferArrivalSostav new_bas = new BufferArrivalSostav()
                {
                    id = 0,
                    datetime = sostav.DateTime,
                    id_sostav = sostav.ID,
                    id_arrival = sostav.IDArrival,
                    count_wagons = sostav.ArrivalCars != null ? (int?)sostav.ArrivalCars.Count() : null,
                    list_wagons = efmt.GetListArrivalCarsToString(sostav.ArrivalCars.ToList())
                };
                return efmt.SaveBufferArrivalSostav(new_bas);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("AddBufferArrivalSostav(id_sostav={0})", id_sostav), servece_owner, eventID);
                return -1;
            }
        }

        public int TransferArrivalSostavToRailWay()
        {

            EFMetallurgTrans efmt = new EFMetallurgTrans();
            // Выбераем все составы которые не закрылись
            List<BufferArrivalSostav> list_no_close = efmt.GetBufferArrivalSostavOfNotClose().OrderBy(s => s.datetime).ToList();
            foreach (BufferArrivalSostav bas in list_no_close)
            {

                int res = TransferArrivalSostavToRailWay(bas);
            }
            return 0;
        }

        public int TransferArrivalSostavToRailWay(BufferArrivalSostav bas)
        {
            try
            {
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                RWOperations rw_operations = new RWOperations(this.servece_owner);
                int count = 0;
                int transfer = 0;
                int skip = 0;
                int tsp = 0;
                int tsp_error = 0;
                int error = 0;
                string message = null;
                //int close = 0;

                // Получить новый и предыдущий состав
                ArrivalSostav sostav_new = ef_mt.GetArrivalSostav(bas.id_sostav);
                ArrivalSostav sostav_old = sostav_new.ParentID != null ? ef_mt.GetArrivalSostav((int)sostav_new.ParentID) : null;
                // Получить новый и предыдущий список вагонов в составе
                List<ArrivalCars> list_new_car = sostav_new.ArrivalCars != null ? sostav_new.ArrivalCars.ToList() : new List<ArrivalCars>();
                List<ArrivalCars> list_old_car = sostav_old != null && sostav_old.ArrivalCars != null ? sostav_old.ArrivalCars.ToList() : new List<ArrivalCars>();
                // Провести анализ спсиков и убрать существующие вагоны
                ef_mt.RemoveMatchingArrivalCars(ref list_new_car, ref list_old_car);
                // Сделать ТСП по УЗ вагонов которые были оцеплены на станции УЗ
                foreach (ArrivalCars car_old in list_old_car)
                {
                    int res_tsp = rw_operations.TSPOnUZ(car_old);
                    tsp += res_tsp > 0 ? 1 : 0;
                    tsp_error += res_tsp < 0 ? 1 : 0;
                }


                // Получим список отцепленных вагонов по последнему ТСП
                //List<int> not_nums = ef_mt.GetNotCarsOfOldArrivalSostav(sostav);

                //EFReference.Concrete.EFReference ef_reference = new EFReference.Concrete.EFReference();

                //int codeon = int.Parse(sostav.CompositionIndex.Substring(9, 4));
                //EFReference.Entities.Stations corect_station_on = ef_reference.GetCorrectStationsOfCode(codeon, false);
                //int code_station_on = corect_station_on != null ? corect_station_on.code : codeon;





                //EFReference.Entities.Stations station_in = ef_reference.GetStationsOfCode(int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10);
                //int codecs_in = station_in != null ? (int)station_in.code_cs : int.Parse(arr_car.CompositionIndex.Substring(9, 4)) * 10;
                ////EFReference.Entities.Stations station_from = ef_reference.GetStationsOfCode(int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10);
                ////int? codecs_from = station_from != null ? station_from.code_cs : int.Parse(sost.CompositionIndex.Substring(0, 4)) * 10;
                //Stations station = rw_ref.GetStationsUZ(codecs_in, true);
                //Ways way = ef_rw.GetWaysOfArrivalUZ(station.id);

                // Переставить отцепленные вагоны с пути "Прибытие на АМКР" на путь "Отправка на УЗ"
                //int result_close = 0;
                //if (not_nums != null && not_nums.Count() > 0)
                //{
                //    foreach(int num in not_nums){
                //        int result = rw_operations.OperationArrivalUZToSendingUZ(code_station_on, num);
                //    }



                //    //List<Cars> list_not_cars = ef_rw.GetCarsOfArrivalNum(sostav.IDArrival, not_nums.ToArray());
                //    ////List<CarOperations> list_operations = ExecOperation(list_not_cars, OperationClose, new OperationClose(sostav.DateTime));
                //    //List<CarOperations> list_operations = list_not_cars.CloseOperations(sostav.DateTime, true);
                //    //result_close = SaveChanges(list_operations);
                //}
                // Поставим новые
                // List<Cars> list_result = new List<Cars>();
                //foreach (ArrivalCars car in sostav.ArrivalCars.ToList())
                //{
                //    DateTime dt_start = DateTime.Now;
                //    //message += car.Num.ToString() + " - ";
                //    //Cars car_new = SetCarsToRailWay(car);
                //    //if (car_new != null)
                //    //{
                //    //    int res = ef_rw.SaveCarsNoDetect(car_new);
                //    //    if (res > 0)
                //    //    {
                //    //        transfer++;
                //    //        message += res.ToString();
                //    //    }
                //    //    else { error++; message += res.ToString(); }
                //    //}
                //    //else
                //    //{
                //    //    skip++;
                //    //    message += "null";
                //    //}
                //    message += "; ";
                //    TimeSpan ts = DateTime.Now - dt_start;
                //    Console.WriteLine(String.Format("Перенос вагона №{0}, время выполнения: {1}:{2}:{3}({4})", car.Num, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds));
                //}
                //count = sostav.ArrivalCars != null ? sostav.ArrivalCars.Count() : 0;
                //string mess = String.Format("Перенос состава из базы данных [MT.Arrival] (id состава: {0}, id прибытия {1}, индекс: {2}, дата операции: {3}) в систему RailWay. Определенно для переноса {4} вагона(ов), перенесено {5}, пропущено {6}, ошибок переноса {7}, закрыто по ТСП {8}.",
                //    sostav.ID, sostav.IDArrival, sostav.CompositionIndex, sostav.DateTime, count, transfer, skip, error, result_close);
                //mess.WriteInformation(servece_owner, eventID);
                //if (error > 0) { mess.WriteEvents(message, servece_owner, eventID); }
                return transfer;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("TransferArrivalSostavToRailWay(bas={0})", bas.GetFieldsAndValue()), servece_owner, eventID);
                return -1;
            }
        }


        #endregion

        #region АДМИНИСТРИРОВАНИЕ Коррекция данных
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        public void CorrectCloseArrivalSostav(int interval)
        {
            try
            {
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                List<int> list = efmt.GetArrivalSostav().OrderBy(s => s.IDArrival).Select(s => s.IDArrival).Distinct().ToList();
                if (list == null || list.Count() == 0) return;
                foreach (int id in list)
                {
                    CorrectCloseArrivalSostav(id, interval);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CorrectCloseArrivalSostav(interval={0})", interval), servece_owner, eventID);
            }
            return;
        }
        /// <summary>
        /// Коррекция наследования операций над составами по группе составов с кодом прибытия
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public void CorrectCloseArrivalSostav(int id_arrival, int interval)
        {
            try
            {
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                List<ArrivalSostav> list = efmt.GetArrivalSostavOfIDArrival(id_arrival).ToList();
                if (list == null || list.Count() <= 1) return;

                foreach (ArrivalSostav arr_first in list.Where(c => c.ParentID == null))
                {
                    //ArrivalSostav car = list.Where(c => c.ParentID == arr_first.ID).FirstOrDefault();
                    CorrectCloseArrivalSostav(arr_first, ref list, interval);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CorrectCloseArrivalSostav(id_arrival={0}, interval={1})", id_arrival, interval), servece_owner, eventID);
            }
            return;

        }
        /// <summary>
        /// Рекурсиный проход по всем составам 
        /// </summary>
        /// <param name="car"></param>
        /// <param name="list"></param>
        /// <param name="interval"></param>
        public void CorrectCloseArrivalSostav(ArrivalSostav car, ref List<ArrivalSostav> list, int interval)
        {
            try
            {
                EFMetallurgTrans efmt = new EFMetallurgTrans();
                ArrivalSostav car_next = list.Where(c => c.ParentID == car.ID).FirstOrDefault();
                if (car_next != null)
                {
                    DateTime date = car.DateTime.AddDays(interval);
                    if (car_next.DateTime > date)
                    {
                        car_next.ParentID = null;
                        efmt.SaveArrivalSostav(car_next);
                        return;
                    }
                    CorrectCloseArrivalSostav(car_next, ref list, interval);
                }
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CorrectCloseArrivalSostav(car={0}, list={1}, interval={2})", car.GetFieldsAndValue(), list, interval), servece_owner, eventID);
            }
            return;
        }

        #endregion




    }
}
