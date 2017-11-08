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

namespace MetallurgTrans
{
    public enum mtt_err : int
    {
            global_error = -1,
            not_fromPath = -2,
            not_listApproachesCars = -3,
        //not_listArrivalCars = -4,
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

        public MTTransfer()
        {

        }

        public MTTransfer(service servece_owner)
        {
            this.servece_owner = servece_owner;
        }

        #region TransferApproaches
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
                            ReferenceCargo corect_cargo_code = api_reference.GetReferenceCargoOfCodeETSNG(int.Parse(array[3]));
                            ApproachesCars new_wag = new ApproachesCars()
                            {
                                ID = 0,
                                IDSostav = id_sostav,
                                CompositionIndex = array[4].ToString(),
                                Num = !String.IsNullOrWhiteSpace(array[0]) ? int.Parse(array[0]) : -1,
                                CountryCode = !String.IsNullOrWhiteSpace(array[1]) ? int.Parse(array[1].Substring(0, 2)) : -1, // Подрезка кода страны в фале 3 цифры переводим в 2 цифры
                                Weight = !String.IsNullOrWhiteSpace(array[2]) ? int.Parse(array[2]) : -1,
                                CargoCode = !String.IsNullOrWhiteSpace(array[3]) ? corect_cargo_code != null ? corect_cargo_code.etsng : -1 : -1, // скорректируем код
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
                                Arrival = null
                            };
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
                    e.WriteError(String.Format("Формироания строки списка файлов состава List<FileApproachesSostav>, файл:{0}", file), servece_owner, eventID);
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
            string mess = String.Format("Перенос txt-файлов в БД MTApproaches выполнен, определено для переноса {0} txt-файлов, перенесено {1}, были перенесены ранее {2}, ошибки при переносе {3}, удаленно {4}.", files.Count(), countCopy, countExist, countError, countDelete);
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

    }
}
