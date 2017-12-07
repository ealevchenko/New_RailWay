using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLog
{
    public static class MServicesLog
    {
        /// <summary>
        /// Записать лог сервиса
        /// </summary>
        /// <param name="id_service"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static long WriteLogServices(this service service, DateTime start, DateTime stop, int code)
        {
            return ServicesLogs.WriteLogServices((int)service, start, stop, code);
        }
        /// <summary>
        /// Записать статус сервиса после выполнения
        /// </summary>
        /// <param name="id_service"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static long WriteLogStatusServices(this service service, DateTime start, DateTime stop)
        {
            return ServicesLogs.WriteLogStatusServices((int)service, start, stop);
        }
        /// <summary>
        /// Записать статус сервиса при запуске
        /// </summary>
        /// <param name="id_service"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static long WriteLogStatusServices(this service service, DateTime start)
        {
            return ServicesLogs.WriteLogStatusServices((int)service, start);
        }
        /// <summary>
        /// Записать статус сервиса при запуске
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static long WriteLogStatusServices(this service service)
        {
            return WriteLogStatusServices(service, DateTime.Now);
        }
        /// <summary>
        /// Проверка работы сервиса
        /// </summary>
        /// <param name="service"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static bool IsRunServices(this service service, int period)
        {
            return ServicesLogs.IsRunServices((int)service, period);
        }



    }
}
