using EFMT.Concrete;
using MessageLog;
using MT.Entities;
using RWWebAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MetallurgTrans
{
    public enum mtt_err : int
    {
            global_error = -1,
            not_fromPath = -2,
            not_listApproachesCars = -3,
            not_listArrivalCars = -4,
        //not_listStartArrivalSostav = -5,
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
        private int day_range_approaches_cars = 30;
        public int DayRangeApproachesCars { get { return this.day_range_approaches_cars; } set { this.day_range_approaches_cars = value; } }
        private int day_range_arrival_cars = 30;
        public int DayRangeArrivalCars { get { return this.day_range_arrival_cars; } set { this.day_range_arrival_cars = value; } }


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
            int? parentid = null;
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            ApproachesCars old_car = efmt.GetApproachesCarsOfNumCar(car.Num, true).FirstOrDefault();
            if (old_car==null) return null; // нет историии движения, первая операция над вагоном
            if (old_car.Arrival!=null) return null; // история закрыта, первая операция над вагоном 

            if (old_car.CargoCode == car.CargoCode) {
                if (old_car.DateOperation.Date.AddDays(day_range) > car.DateOperation)
                {
                    parentid = old_car.ID;
                } // больше допустимого интервала
            } // грузы в вагонах разные
            // закрываем старый вагон
            old_car.NumDocArrival = 0;
            old_car.Arrival = car.DateOperation;
            efmt.SaveApproachesCars(old_car); // сохранить изменение
            return parentid;
        }
        /// <summary>
        /// Возвращает список вагонов из txt-файла
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public List<ApproachesCars> TransferTXTToListApproachesCars(string file, int id_sostav)
        {
            RWReference api_reference = new RWReference();
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
                            ReferenceCargo corect_cargo_code = api_reference.GetReferenceCargoOfCodeETSNG(!String.IsNullOrWhiteSpace(array[3]) ? int.Parse(array[3]): 0);
                            ApproachesCars new_wag = new ApproachesCars()
                            {
                                ID = 0,
                                IDSostav = id_sostav,
                                CompositionIndex = array[4].ToString(),
                                Num = !String.IsNullOrWhiteSpace(array[0]) ? int.Parse(array[0]) : -1,
                                CountryCode = !String.IsNullOrWhiteSpace(array[1]) ? int.Parse(array[1].Substring(0, 2)) : -1, // Подрезка кода страны в фале 3 цифры переводим в 2 цифры
                                Weight = !String.IsNullOrWhiteSpace(array[2]) ? int.Parse(array[2]) : -1,
                                CargoCode = !String.IsNullOrWhiteSpace(array[3]) ? corect_cargo_code != null ? corect_cargo_code.etsng : 0 : -1, // скорректируем код
                                TrainNumber = !String.IsNullOrWhiteSpace(array[5]) ? int.Parse(array[5]) : -1,
                                Operation = array[6].ToString(),
                                DateOperation = !String.IsNullOrWhiteSpace(array[7]) ? DateTime.Parse(array[7], CultureInfo.CreateSpecificCulture("ru-RU")) : DateTime.Now,
                                CodeStationFrom = !String.IsNullOrWhiteSpace(array[4]) ? int.Parse(array[4].Substring(0, 4)) : -1,
                                CodeStationOn = !String.IsNullOrWhiteSpace(array[4]) ? int.Parse(array[4].Substring(9, 4)) : -1,
                                CodeStationCurrent = !String.IsNullOrWhiteSpace(array[8]) ? int.Parse(array[8]) : -1,
                                CountWagons = !String.IsNullOrWhiteSpace(array[9]) ? int.Parse(array[9]) : -1,
                                SumWeight = !String.IsNullOrWhiteSpace(array[10]) ? int.Parse(array[10]) : -1,
                                FlagCargo = !String.IsNullOrWhiteSpace(array[11]) ? int.Parse(array[11]) : -1,
                                Route = !String.IsNullOrWhiteSpace(array[12]) ? int.Parse(array[12]) : -1,
                                Owner = !String.IsNullOrWhiteSpace(array[13]) ? int.Parse(array[13]) : -1,
                                NumDocArrival = null,
                                Arrival = null, 
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
                    //TODO: Добавить сохранение в события системы
                    //if (list.Count() > 0) { mess.SaveLogEvents(trans_id, servece_owner, eventID); }
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
                e.WriteError(String.Format("Ошибка операции добавления вагонов в БД MT.Approaches, сотава на подходе :{0}", new_id),servece_owner,eventID);
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
            //TODO: Добавить сохранение в события системы
            //if (files != null && files.Count() > 0) { mess.SaveLogEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
            return files.Count();
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
            int? parentid = null;
            EFMetallurgTrans efmt = new EFMetallurgTrans();
            ArrivalCars old_car = efmt.GetArrivalCarsOfNumCar(car.Num, true).FirstOrDefault();
            if (old_car == null) return null; // нет историии движения, первая операция над вагоном
            if (old_car.Arrival != null) return null; // история закрыта, первая операция над вагоном 

            if (old_car.CargoCode == car.CargoCode)
            {
                if (old_car.DateOperation.Date.AddDays(day_range) > car.DateOperation)
                {
                    parentid = old_car.ID;
                } // больше допустимого интервала
            } // грузы в вагонах разные
            // закрываем старый вагон
            old_car.NumDocArrival = 0;
            old_car.Arrival = car.DateOperation;
            efmt.SaveArrivalCars(old_car); // сохранить изменение
            return parentid;
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
                XDocument doc = XDocument.Load(file);
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
                RWReference api_reference = new RWReference();
                XDocument doc = XDocument.Load(file);
                foreach (XElement element in doc.Element("NewDataSet").Elements("Table"))
                {
                    try
                    {
                        ReferenceCargo corect_cargo_code = api_reference.GetReferenceCargoOfCodeETSNG(!String.IsNullOrWhiteSpace((string)element.Element("IDCargo")) ? (int)element.Element("IDCargo"): 0);
                        ArrivalCars mtarr = new ArrivalCars()
                        {
                            ID = 0,
                            IDSostav = id_sostav,
                            Position = !String.IsNullOrWhiteSpace((string)element.Element("Position")) ? (int)element.Element("Position") : -1,
                            Num = !String.IsNullOrWhiteSpace((string)element.Element("CarriageNumber")) ? (int)element.Element("CarriageNumber") : -1,
                            CountryCode = !String.IsNullOrWhiteSpace((string)element.Element("CountryCode"))
                             ? ((string)element.Element("CountryCode")).Length >= 2 ? int.Parse(((string)element.Element("CountryCode")).Substring(0, 2)) : (int)element.Element("CountryCode") : -1, // Подрезка кода страны в фале 3 цифры переводим в 2 цифры 
                            Weight = !String.IsNullOrWhiteSpace((string)element.Element("Weight")) ? (int)element.Element("Weight") : -1,
                            CargoCode = !String.IsNullOrWhiteSpace((string)element.Element("IDCargo")) ? corect_cargo_code != null ? corect_cargo_code.etsng : 0 : -1,
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
        /// Перенос списка вагонов в БД MTArrival
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
                    //TODO: Добавить сохранение в события системы
                    //if (list.Count() > 0) { mess.SaveLogEvents(trans_id, servece_owner, eventID); }
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
        /// Перенести БД MTArrival вагоны состава из файла xml.
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
                    //TODO: ПЕРЕДАТЬ ОПЕРАЦИЮ НОВЫЙ СОСТАВ
                    //RWO_MT rw_operations = new RWO_MT();
                    //int IDArrival = mt_arrival.GetIDArrival(new_id);
                    //rw_operations.ArrivalSostav(new_id);
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
                        // Добавим строку
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
        /// Перености xml-файлы из указанной папки  в таблицы MTArrival
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="delete_file"></param>
        /// <returns></returns>
        public int TransferArrival(string fromPath, bool delete_file)
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
                    // защита от записи повторов
                    FileInfo fi = new FileInfo(fs.File);
                    ArrivalSostav exs_sostav = efmt.GetArrivalSostavOfFile(fi.Name);
                    if (exs_sostav == null)
                    {
                        int? ParentIDSostav = null;
                        int IDArrival = efmt.GetNextIDArrival();
                        // получить не закрытый состав
                        ArrivalSostav no_close_sostav = efmt.GetNoCloseArrivalSostav(fs.Index, fs.Date);

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
                        if (list != null & listdb != null)
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
            //TODO: Добавить сохранение в события системы
            //if (files != null && files.Count() > 0) { mess.SaveLogEvents(countError > 0 ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
            return files.Count();
        }
        /// <summary>
        /// Перености xml-файлы из папки по умолчанию  в таблицы MTArrival
        /// </summary>
        /// <returns></returns>
        public int TransferArrival()
        {
            //MT_Transfer old = new MT_Transfer(servece_owner);
            //old.DayMonitoringTrains = 3;
            //old.TransferArrival(this.fromPath, false); //TODO: УБРАТЬ СТАРЫЙ ПЕРЕНОС ПРИБЫТИЯ
            return TransferArrival(this.fromPath, this.delete_file);
        }
        #endregion
    }
}
