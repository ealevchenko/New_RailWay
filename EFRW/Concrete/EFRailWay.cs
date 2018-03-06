﻿using EFRW.Abstract;
using MessageLog;
using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFRW.Entities;
using System.Data.Entity;

namespace EFRW.Concrete
{
    public class Option {
        public int value { get; set; }
        public string text { get; set; }
    }
    
    public class EFRailWay : IRailWay
    {
        private eventID eventID = eventID.EFRW_EFRailWay;

        protected EFDbContext context = new EFDbContext();

        // Перечисление типов отправки составов на другую станцию
        public enum typeSendTransfer : int { railway = 0, kis_output = 1, kis_input = 2, railway_buffer =3 }
        // Перечисление типов стороны (четная, нечетная)
        public enum Side : int { odd = 1, even = 0}

        public Database Database
        {
            get { return context.Database; }
        }

        public List<Option> GetTypeSendTransfer()
        {
            List<Option> list = new List<Option>();
            try
            {
                foreach (typeSendTransfer type in Enum.GetValues(typeof(typeSendTransfer)))
                {
                    list.Add(new Option() { value = (int)type, text = type.ToString() });
                }
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetTypeSendTransfer()"), eventID);
                return null;
            }
        }

        public string GetTypeSendTransfer(int type)
        {
            try
            {
                return ((typeSendTransfer)type).ToString();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetTypeSendTransfer(type={0})", type), eventID);
                return null;
            }
        }

        public List<Option> GetSide()
        {
            List<Option> list = new List<Option>();
            try
            {
                foreach (Side side in Enum.GetValues(typeof(Side))) {
                    list.Add(new Option() { value = (int)side, text = side.ToString() });
                }
                return list;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSide()"), eventID);
                return null;
            }
        }

        public string GetSide(int side)
        {
            try
            {
                return ((Side)side).ToString();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSide(side={0})", side), eventID);
                return null;
            }
        }

        #region СТАНЦИИ, ПЕРЕГОНЫ, ПУТИ
        #region Stations
        public IQueryable<Stations> Stations
        {
            get { return context.Stations; }
        }

        public IQueryable<Stations> GetStations()
        {
            try
            {
                return Stations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStations()"), eventID);
                return null;
            }
        }

        public Stations GetStations(int id)
        {
            try
            {
                return GetStations().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStations(id={0})",id), eventID);
                return null;
            }
        }

        public int SaveStations(Stations Stations)
        {
            Stations dbEntry;
            try
            {
                if (Stations.id == 0)
                {
                    dbEntry = new Stations()
                    {
                        id = 0,
                        name_ru = Stations.name_ru ,
                        name_en = Stations.name_en ,
                        view = Stations.view ,
                        exit_uz = Stations.exit_uz ,
                        station_uz = Stations.station_uz ,
                        id_rs = Stations.id_rs ,
                        id_kis = Stations.id_kis, 
                        default_side = Stations.default_side,  
                        code_uz = Stations.code_uz, 
                        Ways = Stations.Ways, 
                        CarOperations = Stations.CarOperations, 
                        StationsNodes = Stations.StationsNodes, 
                        StationsNodes1 = Stations.StationsNodes1 
                    };
                    context.Stations.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Stations.Find(Stations.id);
                    if (dbEntry != null)
                    {
                        dbEntry.name_ru = Stations.name_ru;
                        dbEntry.name_en = Stations.name_en;
                        dbEntry.view = Stations.view;
                        dbEntry.exit_uz = Stations.exit_uz;
                        dbEntry.station_uz = Stations.station_uz;
                        dbEntry.id_rs = Stations.id_rs;
                        dbEntry.id_kis = Stations.id_kis;
                        dbEntry.default_side = Stations.default_side;
                        dbEntry.code_uz = Stations.code_uz;
                        dbEntry.Ways = Stations.Ways;
                        dbEntry.CarOperations = Stations.CarOperations; 
                        dbEntry.StationsNodes = Stations.StationsNodes;
                        dbEntry.StationsNodes1 = Stations.StationsNodes1;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveStations(Stations={0})", Stations.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Stations DeleteStations(int id)
        {
            Stations dbEntry = context.Stations.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Stations.Remove(dbEntry);

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteStations(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Показать станции отсортированные по признаку просмотр, уз
        /// </summary>
        /// <param name="view"></param>
        /// <param name="uz"></param>
        /// <returns></returns>
        public IQueryable<Stations> GetStationsOfSelect(bool view, bool uz)
        {
            try
            {
                return GetStations().Where(s => s.view == view & s.station_uz == uz);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfSelect(view={0}, uz={1})", view, uz), eventID);
                return null;
            }
        }
        /// <summary>
        /// Показать станции АМКР по которым можно производить операции
        /// </summary>
        /// <returns></returns>
        public IQueryable<Stations> GetStationsOfViewAMKR()
        {
            try
            {
                return GetStationsOfSelect(true, false);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfViewAMKR()"), eventID);
                return null;
            }
        }

        public Stations GetStationsOfKis(int id_kis)
        {
            try
            {
                return GetStations().Where(s => s.id_kis == id_kis).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfKis(id_kis={0})", id_kis), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить страку станции по коду УЗ
        /// </summary>
        /// <param name="code_uz"></param>
        /// <returns></returns>
        public Stations GetStationsOfCodeUZ(int code_uz)
        {
            try
            {
                return GetStations().Where(s => s.code_uz == code_uz).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsOfCodeUZ(code_uz={0})", code_uz), eventID);
                return null;
            }
        }

        #endregion

        #region StationsNodes
        public IQueryable<StationsNodes> StationsNodes
        {
            get { return context.StationsNodes; }
        }

        public IQueryable<StationsNodes> GetStationsNodes()
        {
            try
            {
                return StationsNodes;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsNodes()"), eventID);
                return null;
            }
        }

        public StationsNodes GetStationsNodes(int id)
        {
            try
            {
                return GetStationsNodes().Where(n => n.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetStationsNodes(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveStationsNodes(StationsNodes StationsNodes)
        {
            StationsNodes dbEntry;
            try
            {
                if (StationsNodes.id == 0)
                {
                    dbEntry = new StationsNodes()
                    {
                        id = 0,  
                        nodes = StationsNodes.nodes ,   
                        id_station_from = StationsNodes.id_station_from , 
                        side_station_from = StationsNodes.side_station_from , 
                        id_station_on = StationsNodes.id_station_on , 
                        side_station_on = StationsNodes.side_station_on , 
                        transfer_type = StationsNodes.transfer_type, 
                        Stations = StationsNodes.Stations, 
                        Stations1 = StationsNodes.Stations1
                    };
                    context.StationsNodes.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.StationsNodes.Find(StationsNodes.id);
                    if (dbEntry != null)
                    {
                        dbEntry.nodes = StationsNodes.nodes;                       
                        dbEntry.id_station_from = StationsNodes.id_station_from; 
                        dbEntry.side_station_from = StationsNodes.side_station_from; 
                        dbEntry.id_station_on = StationsNodes.id_station_on; 
                        dbEntry.side_station_on = StationsNodes.side_station_on; 
                        dbEntry.transfer_type = StationsNodes.transfer_type;
                        dbEntry.Stations = StationsNodes.Stations;
                        dbEntry.Stations1 = StationsNodes.Stations1;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveStationsNodes(StationsNodes={0})", StationsNodes.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public StationsNodes DeleteStationsNodes(int id)
        {
            StationsNodes dbEntry = context.StationsNodes.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.StationsNodes.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteStationsNodes(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить список перегонов для отправки
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IQueryable<StationsNodes> GetSendStationsNodes(int id_station)
        {
            try
            {
                return GetStationsNodes().Where(n => n.id_station_from == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetSendStationsNodes(id_station={0})", id_station), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список перегонов по прибытию
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public IQueryable<StationsNodes> GetArrivalStationsNodes(int id_station)
        {
            try
            {
                return GetStationsNodes().Where(n => n.id_station_on == id_station);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalStationsNodes(id_station={0})", id_station), eventID);
                return null;
            }
        }

        public IQueryable<StationsNodes> GetStationsNodes(int id_station_from, int id_station_on, typeSendTransfer st)
        {
            try
            {
                return GetStationsNodes().Where(n => n.id_station_from == id_station_from & n.id_station_on == id_station_on & n.transfer_type == (int)st);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetArrivalStationsNodes(id_station_from={0}, id_station_on={1}, transfer_type={2})", id_station_from, id_station_on, st), eventID);
                return null;
            }
        }
        /// <summary>
        /// Проверка соответствия станций к правилам по коду станций RailWay
        /// </summary>
        /// <param name="id_station_from"></param>
        /// <param name="id_station_on"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public bool IsRulesTransfer(int id_station_from, int id_station_on, typeSendTransfer st) { 
            List<StationsNodes> list = GetStationsNodes(id_station_from, id_station_on, st).ToList();
            return list != null && list.Count() > 0 ? true : false;
        }
        /// <summary>
        /// Проверка соответствия станций к правилам по коду станций КИС
        /// </summary>
        /// <param name="id_station_from_kis"></param>
        /// <param name="id_station_on_kis"></param>
        /// <param name="st"></param>
        /// <returns></returns>
        public bool IsRulesTransferOfKis(int id_station_from_kis, int id_station_on_kis, typeSendTransfer st)
        {

            Stations id_station_from = GetStationsOfKis(id_station_from_kis);
            Stations id_station_on = GetStationsOfKis(id_station_on_kis);
            return IsRulesTransfer((id_station_from != null ? id_station_from.id : 0), (id_station_on != null ? id_station_on.id : 0), st);
        }

        #endregion

        #region Ways
        public IQueryable<Ways> Ways
        {
            get { return context.Ways; }
        }

        public IQueryable<Ways> GetWays()
        {
            try
            {
                return Ways;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays()"), eventID);
                return null;
            }
        }

        public Ways GetWays(int id)
        {
            try
            {
                return GetWays().Where(w=> w.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWays(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveWays(Ways Ways)
        {
            Ways dbEntry;
            try
            {
                if (Ways.id == 0)
                {
                    dbEntry = new Ways()
                    {
                        id = 0,
                        id_station = Ways.id_station,
                        num = Ways.num,
                        name_ru = Ways.name_ru,
                        name_en = Ways.name_en,
                        position = Ways.position,
                        capacity = Ways.capacity,
                        id_car_status = Ways.id_car_status,
                        tupik = Ways.tupik,
                        dissolution = Ways.dissolution,
                        defrosting = Ways.defrosting,
                        overturning = Ways.overturning,
                        pto = Ways.pto,
                        cleaning = Ways.cleaning,
                        rest = Ways.rest,
                        id_rc = Ways.id_rc,
                        CarOperations = Ways.CarOperations,
                        Stations = Ways.Stations
                    };
                    context.Ways.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Ways.Find(Ways.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_station = Ways.id_station;
                        dbEntry.num = Ways.num;
                        dbEntry.name_ru = Ways.name_ru;
                        dbEntry.name_en = Ways.name_en;
                        dbEntry.position = Ways.position;
                        dbEntry.capacity = Ways.capacity;
                        dbEntry.id_car_status = Ways.id_car_status;
                        dbEntry.tupik = Ways.tupik;
                        dbEntry.dissolution = Ways.dissolution;
                        dbEntry.defrosting = Ways.defrosting;
                        dbEntry.overturning = Ways.overturning;
                        dbEntry.pto = Ways.pto;
                        dbEntry.cleaning = Ways.cleaning;
                        dbEntry.rest = Ways.rest;
                        dbEntry.id_rc = Ways.id_rc;
                        dbEntry.CarOperations = Ways.CarOperations;
                        dbEntry.Stations = Ways.Stations;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveWays(Ways={0})", Ways.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public Ways DeleteWays(int id)
        {
            Ways dbEntry = context.Ways.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Ways.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteWays(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить путь для отправки на АМКР из станции УЗ
        /// </summary>
        /// <param name="id_station"></param>
        /// <returns></returns>
        public Ways GetWaysOfArrivalUZ(int id_station)
        {
            try
            {
                return GetWays().Where(w => w.id_station == id_station & w.num == "1").FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetWaysOfArrivalUZ(id_station={0})", id_station), eventID);
                return null;
            }
        }

        #endregion

        #endregion 

        #region ВАГОНЫ (СОСТОЯНИЕ, ОПЕРАЦИИ, ВХОД. ПОСТАВКИ, ИСХ. ПОСТАВКИ)
        #region Cars
        public IQueryable<Cars> Cars
        {
            get { return context.Cars; }
        }

        public IQueryable<Cars> GetCars()
        {
            try
            {
                return Cars;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCars()"), eventID);
                return null;
            }
        }

        public Cars GetCars(int id)
        {
            try
            {
                return GetCars().Where(с => с.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCars(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCars(Cars Cars)
        {
            Cars dbEntry;
            try
            {
                if (Cars.id == 0)
                {
                    dbEntry = new Cars()
                    {
                        id = 0,
                        id_sostav = Cars.id_sostav,
                        id_arrival = Cars.id_arrival,
                        num = Cars.num,
                        dt_uz = Cars.dt_uz,
                        dt_inp_amkr = Cars.dt_inp_amkr,
                        dt_out_amkr = Cars.dt_out_amkr,
                        natur = Cars.natur,
                        dt_user = Cars.dt_user != null ? Cars.dt_user : DateTime.Now,
                        user = Cars.user != null ? Cars.user : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        ReferenceCars = Cars.ReferenceCars,
                        CarOperations = Cars.CarOperations,
                        CarsInpDelivery = Cars.CarsInpDelivery,
                        CarsOutDelivery = Cars.CarsOutDelivery
                    };
                    context.Cars.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.Cars.Find(Cars.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_sostav = Cars.id_sostav;
                        dbEntry.id_arrival = Cars.id_arrival;
                        dbEntry.num = Cars.num;
                        dbEntry.dt_uz = Cars.dt_uz;
                        dbEntry.dt_inp_amkr = Cars.dt_inp_amkr;
                        dbEntry.dt_out_amkr = Cars.dt_out_amkr;
                        dbEntry.natur = Cars.natur;
                        dbEntry.dt_user = Cars.dt_user != null ? Cars.dt_user : DateTime.Now;
                        dbEntry.user = Cars.user != null ? Cars.user : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        //dbEntry.user = Cars.user;
                        //dbEntry.dt_user = Cars.dt_user;
                        dbEntry.ReferenceCars = Cars.ReferenceCars;
                        dbEntry.CarOperations = Cars.CarOperations;
                        dbEntry.CarsInpDelivery = Cars.CarsInpDelivery;
                        dbEntry.CarsOutDelivery = Cars.CarsOutDelivery;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCars(Cars={0})", Cars.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }
        ///// <summary>
        ///// Сохранить список вагонов, вернуть количество сохраненых без ошибок
        ///// </summary>
        ///// <param name="Cars"></param>
        ///// <returns></returns>
        //public int SaveCars(List<Cars> Cars)
        //{
        //    try
        //    {
        //        int result =0;
        //        foreach (Cars car in Cars) {
        //            int res = SaveCars(car);
        //            if (res > 0) { result++; }
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        e.WriteErrorMethod(String.Format("SaveCars(Cars={0})", Cars), eventID);
        //        return -1;
        //    }

        //}

        public Cars DeleteCars(int id)
        {
            Cars dbEntry = context.Cars.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.Cars.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCars(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить вагоны пренадлежащие указаному id операции состава
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public IQueryable<Cars> GetCarsOfSostav(int id_sostav) {
            try
            {
                return GetCars().Where(с => с.id_sostav == id_sostav);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfSostav(id_sostav={0})", id_sostav), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить вагоны пренадлежащие указаному id прибытия состава
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <returns></returns>
        public IQueryable<Cars> GetCarsOfArrival(int id_arrival) {
            try
            {
                return GetCars().Where(с => с.id_arrival == id_arrival);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfArrival(id={0})", id_arrival), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть вагон по id sostav и номеру вагона
        /// </summary>
        /// <param name="id_sostav"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Cars GetCarsOfSostavNum(int id_sostav, int num)
        {
            try
            {
                return GetCars().Where(с => с.id_sostav == id_sostav & с.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfSostavNum(id_sostav={0}, num={1})", id_sostav, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Вернуть вагон по id прибытия и номеру вагона
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Cars GetCarsOfArrivalNum(int id_arrival, int num)
        {
            try
            {
                return GetCars().Where(с => с.id_arrival == id_arrival & с.num == num).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfArrivalNum(id_arrival={0}, num={1})", id_arrival, num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список вагонов пренадлежащих указаному id прибытия
        /// </summary>
        /// <param name="id_arrival"></param>
        /// <param name="nums"></param>
        /// <returns></returns>
        public List<Cars> GetCarsOfArrivalNum(int id_arrival, int[] nums) {
            try
            {
                List<Cars> list_result = new List<Cars>();
                List<Cars> list_arrivals = GetCarsOfArrival(id_arrival).ToList();
                if (list_arrivals == null || list_arrivals.Count() == 0) return list_result;
                foreach (int num in nums) {
                    Cars car = list_arrivals.Find(c => c.num == num);
                    if (car != null) { list_result.Add(car); }
                }
                return list_result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOfArrivalNum(id_arrival={0}, nums={1})", id_arrival, nums), eventID);
                return null;
            }
        }

        #endregion

        #region CarOperations
        public IQueryable<CarOperations> CarOperations
        {
            get { return context.CarOperations; }
        }

        public IQueryable<CarOperations> GetCarOperations()
        {
            try
            {
                return CarOperations;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarOperations()"), eventID);
                return null;
            }
        }

        public CarOperations GetCarOperations(int id)
        {
            try
            {
                return GetCarOperations().Where(o => o.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarOperations(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarOperations(CarOperations CarOperations)
        {
            CarOperations dbEntry;
            try
            {
                if (CarOperations.id == 0)
                {
                    dbEntry = new CarOperations()
                    {
                        id = 0,
                        id_car = CarOperations.id_car, 
                        id_car_conditions = CarOperations.id_car_conditions, 
                        id_car_status = CarOperations.id_car_status, 
                        id_station = CarOperations.id_station, 
                        dt_inp_station = CarOperations.dt_inp_station, 
                        dt_out_station = CarOperations.dt_out_station, 
                        id_way = CarOperations.id_way, 
                        dt_inp_way = CarOperations.dt_inp_way, 
                        dt_out_way = CarOperations.dt_out_way, 
                        position = CarOperations.position, 
                        send_id_station = CarOperations.send_id_station, 
                        send_id_overturning = CarOperations.send_id_overturning, 
                        send_id_shop = CarOperations.send_id_shop, 
                        send_dt_inp_way = CarOperations.send_dt_inp_way, 
                        send_dt_out_way = CarOperations.send_dt_out_way, 
                        send_id_position = CarOperations.send_id_position, 
                        send_train1 = CarOperations.send_train1, 
                        send_train2 = CarOperations.send_train2, 
                        send_side = CarOperations.send_side,
                        edit_user = CarOperations.edit_user != null ? CarOperations.edit_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName,
                        edit_dt = CarOperations.edit_dt != null ? CarOperations.edit_dt : DateTime.Now,
                        parent_id = CarOperations.parent_id, 
                        CarConditions = CarOperations.CarConditions, 
                        Cars = CarOperations.Cars, 
                        CarStatus = CarOperations.CarStatus, 
                        Stations = CarOperations.Stations, 
                        Ways = CarOperations.Ways,  
                    };
                    context.CarOperations.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarOperations.Find(CarOperations.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_car = CarOperations.id_car;
                        dbEntry.id_car_conditions = CarOperations.id_car_conditions; 
                        dbEntry.id_car_status = CarOperations.id_car_status;
                        dbEntry.id_station = CarOperations.id_station;
                        dbEntry.dt_inp_station = CarOperations.dt_inp_station; 
                        dbEntry.dt_out_station = CarOperations.dt_out_station; 
                        dbEntry.id_way = CarOperations.id_way;
                        dbEntry.dt_inp_way = CarOperations.dt_inp_way;
                        dbEntry.dt_out_way = CarOperations.dt_out_way; 
                        dbEntry.position = CarOperations.position;
                        dbEntry.send_id_station = CarOperations.send_id_station;
                        dbEntry.send_id_overturning = CarOperations.send_id_overturning; 
                        dbEntry.send_id_shop = CarOperations.send_id_shop;
                        dbEntry.send_dt_inp_way = CarOperations.send_dt_inp_way;
                        dbEntry.send_dt_out_way = CarOperations.send_dt_out_way;
                        dbEntry.send_id_position = CarOperations.send_id_position;
                        dbEntry.send_train1 = CarOperations.send_train1;
                        dbEntry.send_train2 = CarOperations.send_train2;
                        dbEntry.send_side = CarOperations.send_side;
                        dbEntry.edit_user = CarOperations.edit_user != null ? CarOperations.edit_user : System.Environment.UserDomainName + @"\" + System.Environment.UserName;
                        dbEntry.edit_dt = CarOperations.edit_dt != null ? CarOperations.edit_dt : DateTime.Now;
                        //dbEntry.edit_user = CarOperations.edit_user;
                        //dbEntry.edit_dt = CarOperations.edit_dt; 
                        dbEntry.parent_id = CarOperations.parent_id; 
                        dbEntry.CarConditions = CarOperations.CarConditions;
                        dbEntry.Cars = CarOperations.Cars;
                        dbEntry.CarStatus = CarOperations.CarStatus; 
                        dbEntry.Stations = CarOperations.Stations;
                        dbEntry.Ways = CarOperations.Ways;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarOperations(CarOperations={0})", CarOperations.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarOperations DeleteCarOperations(int id)
        {
            CarOperations dbEntry = context.CarOperations.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarOperations.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarOperations(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Получить все операции по указаному вагону
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public IQueryable<CarOperations> GetCarOperationsOfCar(int id_car) {
            try
            {
                return GetCarOperations().Where(o => o.id_car == id_car).OrderByDescending(o => o.id);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarOperationsOfCar(id_car={0})", id_car), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить текущую операцию
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public CarOperations GetCurrentCarOperationsOfCar(int id_car) {
            try
            {
                return GetCarOperationsOfCar(id_car).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCurrentCarOperationsOfCar(id_car={0})", id_car), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить список операций над указаным номером вагона
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IQueryable<CarOperations> GetCarOperationsOfNumCar(int num) {
            try
            {
                return GetCarOperations().Where(o => o.Cars.num == num);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarOperationsOfNumCar(num={0})", num), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить последнюю операцию над указаным номером вагона
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public CarOperations GetLastCarOperationsOfNumCar(int num) {
            try
            {
                return GetCarOperationsOfNumCar(num).OrderByDescending(o=>o.id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarOperationsOfNumCar(num={0})", num), eventID);
                return null;
            }
        }
        #endregion

        #region CarConditions
        public IQueryable<CarConditions> CarConditions
        {
            get { return context.CarConditions; }
        }

        public IQueryable<CarConditions> GetCarConditions()
        {
            try
            {
                return CarConditions;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarConditions()"), eventID);
                return null;
            }
        }

        public CarConditions GetCarConditions(int id)
        {
            try
            {
                return GetCarConditions().Where(c => c.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarConditions(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarConditions(CarConditions CarConditions)
        {
            CarConditions dbEntry;
            try
            {
                if (CarConditions.id == 0)
                {
                    dbEntry = new CarConditions()
                    {
                        id = 0,
                        name_ru = CarConditions.name_ru,
                        name_en = CarConditions.name_en,
                        CarOperations = CarConditions.CarOperations
                    };
                    context.CarConditions.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarConditions.Find(CarConditions.id);
                    if (dbEntry != null)
                    {
                        dbEntry.name_ru = CarConditions.name_ru;
                        dbEntry.name_en = CarConditions.name_en;
                        dbEntry.CarOperations = CarConditions.CarOperations;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarConditions(CarConditions={0})", CarConditions.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarConditions DeleteCarConditions(int id)
        {
            CarConditions dbEntry = context.CarConditions.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarConditions.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarConditions(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region CarStatus
        public IQueryable<CarStatus> CarStatus
        {
            get { return context.CarStatus; }
        }

        public IQueryable<CarStatus> GetCarStatus()
        {
            try
            {
                return CarStatus;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarStatus()"), eventID);
                return null;
            }
        }

        public CarStatus GetCarStatus(int id)
        {
            try
            {
                return GetCarStatus().Where(s => s.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarStatus(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarStatus(CarStatus CarStatus)
        {
            CarStatus dbEntry;
            try
            {
                if (CarStatus.id == 0)
                {
                    dbEntry = new CarStatus()
                    {
                        id = 0,
                        status_ru = CarStatus.status_ru,
                        status_en = CarStatus.status_en,
                        order = CarStatus.order,
                        id_status_next = CarStatus.id_status_next,
                        CarOperations = CarStatus.CarOperations
                    };
                    context.CarStatus.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarStatus.Find(CarStatus.id);
                    if (dbEntry != null)
                    {
                        dbEntry.status_ru = CarStatus.status_ru;
                        dbEntry.status_en = CarStatus.status_en;
                        dbEntry.order = CarStatus.order;
                        dbEntry.id_status_next = CarStatus.id_status_next;
                        dbEntry.CarOperations = CarStatus.CarOperations;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarStatus(CarStatus={0})", CarStatus.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarStatus DeleteCarStatus(int id)
        {
            CarStatus dbEntry = context.CarStatus.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarStatus.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarStatus(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #region CarsInpDelivery
        public IQueryable<CarsInpDelivery> CarsInpDelivery
        {
            get { return context.CarsInpDelivery; }
        }

        public IQueryable<CarsInpDelivery> GetCarsInpDelivery()
        {
            try
            {
                return CarsInpDelivery;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInpDelivery()"), eventID);
                return null;
            }
        }

        public CarsInpDelivery GetCarsInpDelivery(int id)
        {
            try
            {
                return GetCarsInpDelivery().Where(d => d.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInpDelivery(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarsInpDelivery(CarsInpDelivery CarsInpDelivery)
        {
            CarsInpDelivery dbEntry;
            try
            {
                if (CarsInpDelivery.id == 0)
                {
                    dbEntry = new CarsInpDelivery()
                    {
                        id = 0,
                        id_car = CarsInpDelivery.id_car,
                        datetime = CarsInpDelivery.datetime,
                        composition_index = CarsInpDelivery.composition_index,
                        id_arrival = CarsInpDelivery.id_arrival,
                        num_car = CarsInpDelivery.num_car,
                        position = CarsInpDelivery.position,
                        num_nakl_sap = CarsInpDelivery.num_nakl_sap,
                        country_code = CarsInpDelivery.country_code,
                        id_country = CarsInpDelivery.id_country,
                        weight_cargo = CarsInpDelivery.weight_cargo,
                        num_doc_reweighing_sap = CarsInpDelivery.num_doc_reweighing_sap,
                        dt_doc_reweighing_sap = CarsInpDelivery.dt_doc_reweighing_sap,
                        weight_reweighing_sap = CarsInpDelivery.weight_reweighing_sap,
                        dt_reweighing_sap = CarsInpDelivery.dt_reweighing_sap,
                        post_reweighing_sap = CarsInpDelivery.post_reweighing_sap,
                        cargo_code = CarsInpDelivery.cargo_code,
                        id_cargo = CarsInpDelivery.id_cargo,
                        material_code_sap = CarsInpDelivery.material_code_sap,
                        material_name_sap = CarsInpDelivery.material_name_sap,
                        station_shipment = CarsInpDelivery.station_shipment,
                        station_shipment_code_sap = CarsInpDelivery.station_shipment_code_sap,
                        station_shipment_name_sap = CarsInpDelivery.station_shipment_name_sap, 
                        consignee = CarsInpDelivery.consignee,
                        shop_code_sap = CarsInpDelivery.shop_code_sap,
                        shop_name_sap = CarsInpDelivery.shop_name_sap,
                        new_shop_code_sap = CarsInpDelivery.new_shop_code_sap,
                        new_shop_name_sap = CarsInpDelivery.new_shop_name_sap,
                        permission_unload_sap = CarsInpDelivery.permission_unload_sap,
                        step1_sap = CarsInpDelivery.step1_sap,
                        step2_sap = CarsInpDelivery.step2_sap,
                        Cars = CarsInpDelivery.Cars,
                        ReferenceCargo = CarsInpDelivery.ReferenceCargo, 
                    };
                    context.CarsInpDelivery.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarsInpDelivery.Find(CarsInpDelivery.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_car = CarsInpDelivery.id_car;
                        dbEntry.datetime = CarsInpDelivery.datetime;
                        dbEntry.composition_index = CarsInpDelivery.composition_index;
                        dbEntry.id_arrival = CarsInpDelivery.id_arrival;
                        dbEntry.num_car = CarsInpDelivery.num_car;
                        dbEntry.position = CarsInpDelivery.position;
                        dbEntry.num_nakl_sap = CarsInpDelivery.num_nakl_sap;
                        dbEntry.country_code = CarsInpDelivery.country_code;
                        dbEntry.id_country = CarsInpDelivery.id_country;
                        dbEntry.weight_cargo = CarsInpDelivery.weight_cargo;
                        dbEntry.num_doc_reweighing_sap = CarsInpDelivery.num_doc_reweighing_sap;
                        dbEntry.dt_doc_reweighing_sap = CarsInpDelivery.dt_doc_reweighing_sap;
                        dbEntry.weight_reweighing_sap = CarsInpDelivery.weight_reweighing_sap;
                        dbEntry.dt_reweighing_sap = CarsInpDelivery.dt_reweighing_sap;
                        dbEntry.post_reweighing_sap = CarsInpDelivery.post_reweighing_sap;
                        dbEntry.cargo_code = CarsInpDelivery.cargo_code;
                        dbEntry.id_cargo = CarsInpDelivery.id_cargo;
                        dbEntry.material_code_sap = CarsInpDelivery.material_code_sap;
                        dbEntry.material_name_sap = CarsInpDelivery.material_name_sap;
                        dbEntry.station_shipment = CarsInpDelivery.station_shipment;
                        dbEntry.station_shipment_code_sap = CarsInpDelivery.station_shipment_code_sap;
                        dbEntry.station_shipment_name_sap = CarsInpDelivery.station_shipment_name_sap;
                        dbEntry.consignee = CarsInpDelivery.consignee;
                        dbEntry.shop_code_sap = CarsInpDelivery.shop_code_sap;
                        dbEntry.shop_name_sap = CarsInpDelivery.shop_name_sap;
                        dbEntry.new_shop_code_sap = CarsInpDelivery.new_shop_code_sap;
                        dbEntry.new_shop_name_sap = CarsInpDelivery.new_shop_name_sap;
                        dbEntry.permission_unload_sap = CarsInpDelivery.permission_unload_sap;
                        dbEntry.step1_sap = CarsInpDelivery.step1_sap;
                        dbEntry.step2_sap = CarsInpDelivery.step2_sap;
                        dbEntry.Cars = CarsInpDelivery.Cars;
                        dbEntry.ReferenceCargo = CarsInpDelivery.ReferenceCargo;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarsInpDelivery(CarsInpDelivery={0})", CarsInpDelivery.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarsInpDelivery DeleteCarsInpDelivery(int id)
        {
            CarsInpDelivery dbEntry = context.CarsInpDelivery.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarsInpDelivery.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarsInpDelivery(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        /// <summary>
        /// Вернуть входящую поставку по id вагона
        /// </summary>
        /// <param name="id_car"></param>
        /// <returns></returns>
        public CarsInpDelivery GetCarsInpDeliveryOfCar(int id_car)
        {
            try
            {
                return GetCarsInpDelivery().Where(d => d.id_car == id_car).OrderByDescending(d => d.id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInpDeliveryOfCar(id_car={0})", id_car), eventID);
                return null;
            }
        }
        /// <summary>
        /// Получить строку входящей поставки по номену вагона и кода прибытия
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id_arrival"></param>
        /// <returns></returns>
        public CarsInpDelivery GetCarsInpDeliveryOfNumArrival(int num, int id_arrival)
        {
            try
            {
                return GetCarsInpDelivery().Where(d => d.num_car == num & d.id_arrival == id_arrival).OrderByDescending(d => d.id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsInpDeliveryOfNumArrival(num={0}, id_arrival={1})", num, id_arrival), eventID);
                return null;
            }
        }

        #endregion

        #region CarsOutDelivery
        public IQueryable<CarsOutDelivery> CarsOutDelivery
        {
            get { return context.CarsOutDelivery; }
        }

        public IQueryable<CarsOutDelivery> GetCarsOutDelivery()
        {
            try
            {
                return CarsOutDelivery;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOutDelivery()"), eventID);
                return null;
            }
        }

        public CarsOutDelivery GetCarsOutDelivery(int id)
        {
            try
            {
                return GetCarsOutDelivery().Where(d => d.id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCarsOutDelivery(id={0})", id), eventID);
                return null;
            }
        }

        public int SaveCarsOutDelivery(CarsOutDelivery CarsOutDelivery)
        {
            CarsOutDelivery dbEntry;
            try
            {
                if (CarsOutDelivery.id == 0)
                {
                    dbEntry = new CarsOutDelivery()
                    {
                        id = 0,
                        id_car = CarsOutDelivery.id_car,
                        num_nakl_sap = CarsOutDelivery.num_nakl_sap,
                        id_tupik = CarsOutDelivery.id_tupik,
                        id_country_out = CarsOutDelivery.id_country_out,
                        id_station_out = CarsOutDelivery.id_station_out,
                        note = CarsOutDelivery.note,
                        cargo_code = CarsOutDelivery.cargo_code,
                        id_cargo = CarsOutDelivery.id_cargo,
                        weight_cargo = CarsOutDelivery.weight_cargo,
                        num_doc_reweighing_sap = CarsOutDelivery.num_doc_reweighing_sap,
                        dt_doc_reweighing_sap = CarsOutDelivery.dt_doc_reweighing_sap,
                        weight_reweighing_sap = CarsOutDelivery.weight_reweighing_sap,
                        dt_reweighing_sap = CarsOutDelivery.dt_reweighing_sap,
                        post_reweighing_sap = CarsOutDelivery.post_reweighing_sap,
                        Cars = CarsOutDelivery.Cars,
                        ReferenceCargo = CarsOutDelivery.ReferenceCargo,
                        ReferenceCountry = CarsOutDelivery.ReferenceCountry, 
                        ReferenceStation = CarsOutDelivery.ReferenceStation
                    };
                    context.CarsOutDelivery.Add(dbEntry);
                }
                else
                {
                    dbEntry = context.CarsOutDelivery.Find(CarsOutDelivery.id);
                    if (dbEntry != null)
                    {
                        dbEntry.id_car = CarsOutDelivery.id_car;
                        dbEntry.num_nakl_sap = CarsOutDelivery.num_nakl_sap;
                        dbEntry.id_tupik = CarsOutDelivery.id_tupik;
                        dbEntry.id_country_out = CarsOutDelivery.id_country_out;
                        dbEntry.id_station_out = CarsOutDelivery.id_station_out;
                        dbEntry.note = CarsOutDelivery.note;
                        dbEntry.cargo_code = CarsOutDelivery.cargo_code;
                        dbEntry.id_cargo = CarsOutDelivery.id_cargo;
                        dbEntry.weight_cargo = CarsOutDelivery.weight_cargo;
                        dbEntry.num_doc_reweighing_sap = CarsOutDelivery.num_doc_reweighing_sap;
                        dbEntry.dt_doc_reweighing_sap = CarsOutDelivery.dt_doc_reweighing_sap;
                        dbEntry.weight_reweighing_sap = CarsOutDelivery.weight_reweighing_sap;
                        dbEntry.dt_reweighing_sap = CarsOutDelivery.dt_reweighing_sap;
                        dbEntry.post_reweighing_sap = CarsOutDelivery.post_reweighing_sap;
                        dbEntry.Cars = CarsOutDelivery.Cars;
                        dbEntry.ReferenceCargo = CarsOutDelivery.ReferenceCargo;
                        dbEntry.ReferenceCountry = CarsOutDelivery.ReferenceCountry;
                        dbEntry.ReferenceStation = CarsOutDelivery.ReferenceStation;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("SaveCarsOutDelivery(CarsOutDelivery={0})", CarsOutDelivery.GetFieldsAndValue()), eventID);
                return -1;
            }
            return dbEntry.id;
        }

        public CarsOutDelivery DeleteCarsOutDelivery(int id)
        {
            CarsOutDelivery dbEntry = context.CarsOutDelivery.Find(id);
            if (dbEntry != null)
            {
                try
                {
                    context.CarsOutDelivery.Remove(dbEntry);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    e.WriteErrorMethod(String.Format("DeleteCarsOutDelivery(id={0})", id), eventID);
                    return null;
                }
            }
            return dbEntry;
        }
        #endregion

        #endregion




    }
}
