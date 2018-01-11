using EFRW.Abstract;
using MessageLog;
using libClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFRW.Entities;

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
        public enum Side : int { odd = 0, even = 1}

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
                        id_kis = Stations.id_kis
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
                        transfer_type = StationsNodes.transfer_type 
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

        #endregion






    }
}
