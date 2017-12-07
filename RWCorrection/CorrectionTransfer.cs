using EFKIS.Concrete;
using EFKIS.Entities;
using EFMT.Concrete;
using EFRC.Concrete;
using EFRC.Entities;
using EFMT.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWCorrection
{
    public class CorrectionTransfer
    {
        public CorrectionTransfer() { }
        /// <summary>
        /// Скорректировать запись в справочнике входящие поставки (если id состав = -xx)
        /// </summary>
        /// <param name="id"></param>
        public int CorrSAPIncSupply(int id){
            try
            {
                EFSAP ef_sap = new EFSAP();
                EFRailCars ef_rc = new EFRailCars();
                EFTKIS ef_kis = new EFTKIS();
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();

                SAPIncSupply sap_string = ef_sap.GetSAPIncSupply(id);
                if (sap_string == null) return -2;
                if (sap_string.IDMTSostav > 0) return -3;
                // Получим номер документа и вес
                int num = sap_string.CarriageNumber;
                decimal? width = sap_string.WeightDoc;
                int oldsostav = sap_string.IDMTSostav;
                int natur = int.Parse(sap_string.CompositionIndex.Substring(2, 4));
                BufferArrivalSostav bas = ef_kis.GetBufferArrivalSostavOfNatur(natur);
                if (bas == null) return -4;
                DateTime dt_amkr = bas.datetime;
                int newidsostav = 0;

                // Сделать отметку на МТ о принятии вагона
                int res_close_arrival = ef_mt.CloseArrivalCarsOfDocWeight(natur, num, dt_amkr, width);
                if (res_close_arrival <= 0)
                {
                    res_close_arrival = ef_mt.CloseArrivalCarsOfDocDay(natur, num, dt_amkr, 1);
                }
                int res_close_approaches = ef_mt.CloseApproachesCarsOfDocWeight(natur, num, dt_amkr, width);
                if (res_close_approaches <= 0)
                {
                    res_close_approaches = ef_mt.CloseApproachesCarsOfDocDay(natur, num, dt_amkr, 1);
                }
                ArrivalCars mt_list = ef_mt.GetArrivalCarsToNatur(natur, num, dt_amkr, 15);
                if (mt_list == null) return -6;
                newidsostav = mt_list.IDSostav;
                string CompositionIndex = mt_list.CompositionIndex;

                List<VAGON_OPERATIONS> vag_list_new = ef_rc.GetVagonsOperations(newidsostav, num).OrderByDescending(v => v.id_oper).ToList();
                List<VAGON_OPERATIONS> vag_list = ef_rc.GetVagonsOperations(oldsostav, num).OrderByDescending(v=>v.id_oper).ToList();
                if (vag_list != null && vag_list.Count() > 0) {
                    foreach (VAGON_OPERATIONS vag in vag_list_new)
                    {
                        ef_rc.DeleteVAGON_OPERATIONS(vag.id_oper);
                    }                      
                }

                foreach (VAGON_OPERATIONS vag in vag_list)
                {
                    if (vag.IDSostav == oldsostav & vag.num_vagon == num)
                    {
                        vag.IDSostav = newidsostav;
                        ef_rc.SaveVAGON_OPERATIONS(vag);
                    }
                }

                sap_string.IDMTSostav = newidsostav;
                sap_string.CompositionIndex = CompositionIndex;
                int res = ef_sap.SaveSAPIncSupply(sap_string);
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

        }

        public int CorrSAPIncSupply(){
            try
            {
                EFSAP ef_sap = new EFSAP();

                List<SAPIncSupply> sap_list = ef_sap.GetSAPIncSupply().Where(s=>s.IDMTSostav<-64).ToList();
                foreach (SAPIncSupply s in sap_list) {
                    Console.WriteLine("Коррекция {0} - результат {1}",s.ID,CorrSAPIncSupply(s.ID));
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

        }

        public int CorrSAPIncSupplyOfNum(int num)
        {

            try
            {

                EFSAP ef_sap = new EFSAP();
                EFRailCars ef_rc = new EFRailCars();
                EFTKIS ef_kis = new EFTKIS();
                EFMetallurgTrans ef_mt = new EFMetallurgTrans();
                List<SAPIncSupply> list_sap = ef_sap.GetSAPIncSupply().Where(s => s.CarriageNumber == num).OrderByDescending(c => c.ID).ToList();
                if (list_sap == null || list_sap.Count() < 2) return -2;
                Console.WriteLine("1 -> Время {0}, Индекс {1}, Состав {2}", list_sap[0].DateTime, list_sap[0].CompositionIndex, list_sap[0].IDMTSostav);
                Console.WriteLine("2 -> Время {0}, Индекс {1}, Состав {2}", list_sap[1].DateTime, list_sap[1].CompositionIndex, list_sap[1].IDMTSostav);
                Console.Write("Меняем 2 на 1 ?");
                string key = Console.ReadLine();
                if (key == "y")
                {

                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public int CorrSAPIncSupplyArrivalRC(int station, int train)
        {
            try
            {
                EFRailCars ef_rc = new EFRailCars();
                List<VAGON_OPERATIONS> vag_list_arr = ef_rc.GetVAGON_OPERATIONS().Where(v => v.st_lock_id_stat == station & v.st_lock_train == train & v.is_hist==0).OrderBy(v => v.num_vag_on_way).ToList();
                foreach (VAGON_OPERATIONS v in vag_list_arr) {
                    Console.WriteLine("Вагон {0} - результат {1}", v.num_vagon, CorrSAPIncSupplyOfNum((int)v.num_vagon));
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

        }
    }
}
