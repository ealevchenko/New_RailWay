using AppSettings;
using MessageLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWSettings.Settings
{
    public static class RWDBSetting
    {
        private static bool _blog = false;

        static RWDBSetting() {

            DBSetting.InitLog(_blog);
        }
        /// <summary>
        /// Инициализация логирования ошибок
        /// </summary>
        /// <param name="blog"></param>
        public static void InitLog(bool blog)
        {
            _blog = blog;
            DBSetting.InitLog(_blog);            
        }

        /// <summary>
        /// Прочесть значение ключа уазанного сервиса из БД, если нет значения вернуть значение по умолчанию
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static T GetDBSetting<T>(string Key, service service, T def)
        {
            return DBSetting.GetDBSetting<T>(Key, (int)service, def);
        }
        /// <summary>
        /// Прочесть значение ключа уазанного сервиса из БД, если нет значения вернуть значение по умолчанию (null)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static T GetDBSetting<T>(string Key, service service)
        {
            return DBSetting.GetDBSetting<T>(Key, (int)service);
        }

        /// <summary>
        /// Записать значение в базу данных Setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool SetDBSetting<T>(this T val, string Key, service service, string description)
        {
            return val.SetDBSetting(Key, (int)service, description);
        }
        /// <summary>
        /// Записать значение в базу данных Setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool SetDBSetting<T>(this T val, string Key, service service)
        {
            return val.SetDBSetting(Key, (int)service, "");
        }

        public static bool IsSetting(this int id_service, string Key)
        {
            return DBSetting.IsSetting(id_service, Key);
        }
    }
}
