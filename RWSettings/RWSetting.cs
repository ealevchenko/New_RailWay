using AppSettings;
using MessageLog;
using RWSettings.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWSettings
{
    
    public static class RWSetting
    {
        private static bool _blog = false;
        private static eventID eventID = eventID.RWSettings_RWSetting;

        static RWSetting()
        { 
            RWDBSetting.InitLog(_blog);
        }
        /// <summary>
        /// Инициализация логирования ошибок
        /// </summary>
        /// <param name="blog"></param>
        public static void InitLog(bool blog)
        {
            _blog = blog;
            RWDBSetting.InitLog(_blog);            
        }

        #region GetSetting
        /// <summary>
        /// Прочесть значение ключа уазанного сервиса из БД, если нет значения вернуть значение по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetDBSetting<T>(string Key, service service, T def)
        {
            try
            {
                return (T)RWDBSetting.GetDBSetting<T>(Key, service, def);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDBSetting<T>(Key={0}, service={1}, def={2})", Key, service, def), eventID);
                return default(T);
            }
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
            try
            {
                return (T)RWDBSetting.GetDBSetting<T>(Key, service);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDBSetting<T>(Key={0}, service={1})", Key, service), eventID);
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetCongigSetting<T>(string Key, T def)
        {
            try
            {
                return (T)ConfigSetting.GetStringConfigurationManagerDefault(Key, def);
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetCongigSetting<T>(Key={0}, def={1})", Key, def), eventID);
                return def;
            }
        }       
        /// <summary>
        /// Прчитать значение настройки сначало с БД, если нет с web&app.config, если нет принять значение по умолчанию
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="id_project"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T GetDB_Config_DefaultSetting<T>(string Key, service service, T def, bool write_db)
        {
            try
            {
                object result;
                if (!service.IsSetting(Key))
                {
                    result = ConfigSetting.GetStringConfigurationManagerDefault(Key, def);
                    if (write_db) result.SetDBSetting(Key, (int)service);
                }
                else { 
                    result = RWDBSetting.GetDBSetting<T>(Key, service); 
                }

                return (T)result;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("GetDB_Config_DefaultSetting<T>(Key={0}, service={1}, def={2}, write_db={3})", Key, service, def, write_db), eventID);
                return def;
            }
        }
        #endregion

        #region SetSetting
        /// <summary>
        /// Записать значение ключа указанного сервиса в БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool SetDBSetting<T>(this T val, string Key, service service, string description)
        {
            return RWDBSetting.SetDBSetting(val, Key, service, description);
        }
        /// <summary>
        /// Записать значение ключа указанного сервиса в БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="Key"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool SetDBSetting<T>(this T val, string Key, service service)
        {
            return RWDBSetting.SetDBSetting(val, Key, service);
        }
        /// <summary>
        /// Записать значение ключа указанного сервиса в БД
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool SetCongigSetting<T>(this T val, string Key)
        {
            return ConfigSetting.SetStringConfigurationManager(val, Key);
        }
        #endregion

        public static bool IsSetting(this service service, string Key)
        {
            return RWDBSetting.IsSetting((int)service, Key);
        }
    }
}
