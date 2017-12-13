using MessageLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamir.SharpSsh;

namespace MetallurgTrans
{
    public enum sftp_client_error : int
    {
        clobal_error = -1,
        not_fromFilePaths = -2,
        coincide_fromFilePaths_toDirPath = -3,
        not_connect = -4,
        null_client_sftp = - 5,
    }
    
    [Serializable()]
    public class connectSFTP
    {
        public string Host
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public string User
        {
            get;
            set;
        }
        public string PSW
        {
            get;
            set;
        }
    }

    public class SFTPClient
    {
        private eventID eventID = eventID.MetallurgTrans_SFTPClient;
        protected service servece_owner = service.Null;
        private Sftp client_sftp;
        private connectSFTP connect_SFTP;
        private string _fromPathsHost;  // Путь для чтения файлов из host
        public string fromPathsHost { get { return this._fromPathsHost; } set { this._fromPathsHost = value; } }
        private string _toPathsHost;    // Путь для записи файлов в host
        public string toPathsHost { get { return this._toPathsHost; } set { this._toPathsHost = value; } }
        private string _FileFiltrHost = "*.*";  // Фильтр файлов из host
        public string FileFiltrHost { get { return this._FileFiltrHost; } set { this._FileFiltrHost = value; } }
        private string _fromDirPath;   // Путь для чтения файлов для загрузки в host
        public string fromDirPath { get { return this._fromDirPath; } set { this._fromDirPath = value; } }
        private string _toTMPDirPath = Path.GetTempPath();     // Путь к временной папки для записи файлов из host для дальнейшей обработки
        public string toTMPDirPath { get { return this._toTMPDirPath; } set { this._toTMPDirPath = value; } }
        private string _toDirPath = null; // Путь для записи файлов из host для постоянного хранения
        public string toDirPath { get { return this._toDirPath; } set { this._toDirPath = value; } }
        private string _FileFiltr = "*.*";  // Фильтр файлов для загрузки в host
        public string FileFiltr { get { return this._FileFiltr; } set { this._FileFiltr = value; } }
        private bool _DeleteFileHost = false; // Признак удаления файлов после копирования из host
        public bool DeleteFileHost { get { return this._DeleteFileHost; } set { this._DeleteFileHost = value; } }
        private bool _DeleteFileDir = false; // Признак удаления файлов после копирования из папки
        public bool DeleteFileDir { get { return this._DeleteFileDir; } set { this._DeleteFileDir = value; } }
        private bool _RewriteFile = false;  // Признак перезаписи файлов в директории приемнике если совподает название файда
        public bool RewriteFile { get { return this._RewriteFile; } set { this._RewriteFile = value; } }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public SFTPClient(connectSFTP con_sftp)
        {
            this.connect_SFTP = con_sftp;
        }

        public SFTPClient(string Host, int Port, string User, string PSW)
        {
            this.connect_SFTP = new connectSFTP() {  Host = Host, Port = Port, User = User, PSW=PSW};
        }

        public SFTPClient(connectSFTP con_sftp, service servece_owner)
        {
            this.connect_SFTP = con_sftp;
            this.servece_owner = servece_owner;
        }

        public SFTPClient(string Host, int Port, string User, string PSW , service servece_owner)
        {
            this.connect_SFTP = new connectSFTP() { Host = Host, Port = Port, User = User, PSW = PSW }; 
            this.servece_owner = servece_owner;
        }

        /// <summary>
        /// Подключение к серверу SFTP
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {

            this.client_sftp = new Sftp(this.connect_SFTP.Host, this.connect_SFTP.User);
            this.client_sftp.Password = this.connect_SFTP.PSW;
            try
            {
                this.client_sftp.Connect(this.connect_SFTP.Port);
            }
            catch (Exception e)
            {
                e.WriteError(String.Format("Ошибка подключения sftp-клиента, Host:{0} ", this.connect_SFTP.Host),servece_owner, eventID);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Закрыть соединение к серверу SFTP
        /// </summary>
        public void Close()
        {
            //"TransferArrival -4 close connect".WriteInformation(servece_owner, eventID.Test);
            this.client_sftp.Close();

        }
        /// <summary>
        /// Проверка наличия файла
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected bool ExistFile(string file)
        {
            FileInfo fInfo = new FileInfo(file);
            return fInfo.Exists;
        }

        public int CopySFTPFile(string fromFilePaths, string fromFileFiltr, string toTMPDirPath, string toDirPath, bool fromDeleteFile, bool toRewriteFile)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(fromFilePaths) | String.IsNullOrWhiteSpace(toTMPDirPath))
                {
                    String.Format("Метод SFTPClient.CopySFTPFile() :Не определен путь копирования fromFilePaths:{0}, toDirPath:{1}.", fromFilePaths, toDirPath).WriteError(servece_owner, this.eventID);
                    return this.eventID.GetEventIDErrorCode((int)sftp_client_error.not_fromFilePaths);
                }
                if (toDirPath == toTMPDirPath)
                {
                    String.Format("Метод SFTPClient.CopySFTPFile() :Путь для постоянного хранения перенесённых файлов toDirPath:{0}, совпадает с временным хранилищем для обработки toTMPDirPath:{1}.", toDirPath, toTMPDirPath).WriteError(servece_owner, this.eventID);
                    return this.eventID.GetEventIDErrorCode((int)sftp_client_error.coincide_fromFilePaths_toDirPath);                    
                }
                //"TransferArrival -4.1".WriteInformation(servece_owner, eventID.Test);
                if (this.client_sftp == null) return this.eventID.GetEventIDErrorCode((int)sftp_client_error.null_client_sftp);
                string[] listfromFile = this.client_sftp.GetFileList(fromFilePaths + "//" + fromFileFiltr);
                //"TransferArrival -4.2".WriteInformation(servece_owner, eventID.Test);
                if (listfromFile == null || listfromFile.Count() == 0)
                {
                    //ServicesEventLog.LogInformation(String.Format("На сервере SFTP отсутствуют файлы для копирования"), this.eventID);
                    //"TransferArrival -4.2 - files - 0".WriteInformation(servece_owner, eventID.Test);
                    return 0;
                }
                //("TransferArrival -4.2 - files - " + listfromFile.Count().ToString()).WriteInformation(servece_owner, eventID.Test);
                int count = 0;
                int cdel = 0;
                foreach (string file in listfromFile)
                {
                    // Если указана папка перенос в постоянное хранилище
                    if (!String.IsNullOrWhiteSpace(toDirPath))
                    {
                        if (this.client_sftp == null) return this.eventID.GetEventIDErrorCode((int)sftp_client_error.null_client_sftp);
                        client_sftp.Get(fromFilePaths + "//" + file, toDirPath + "\\");
                    }
                    // Переносим во временное хранилище
                    if ((toRewriteFile) | (!toRewriteFile & !ExistFile(toTMPDirPath + "\\" + file)))
                    {
                        if (this.client_sftp == null) return this.eventID.GetEventIDErrorCode((int)sftp_client_error.null_client_sftp);
                        client_sftp.Get(fromFilePaths + "//" + file, toTMPDirPath + "\\");
                        count++;
                    }
                    // Удалим файлы из host
                    if (fromDeleteFile)
                    {
                        if (this.client_sftp == null) return this.eventID.GetEventIDErrorCode((int)sftp_client_error.null_client_sftp);
                        client_sftp.Rm(fromFilePaths + "//" + file);
                        cdel++;
                    }
                }
                //"TransferArrival -4.3".WriteInformation(servece_owner, eventID.Test);
                string mess = String.Format("На сервере SFTP:{0} найдено {1} файлов, перенесено {2}", connect_SFTP.Host, listfromFile.Count(), count);
                if (fromDeleteFile) { mess = String.Format(mess + ", удаленно {0}", cdel); }
                mess.WriteInformation(servece_owner, this.eventID);
                if (listfromFile != null && listfromFile.Count() > 0) { mess.WriteEvents(listfromFile.Count() != count ? EventStatus.Error : EventStatus.Ok, servece_owner, eventID); }
                return count;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopySFTPFile(fromFilePaths={0}, fromFileFiltr={1}, toTMPDirPath={2}, toDirPath={3}, fromDeleteFile={4}, toRewriteFile={5})", 
                    fromFilePaths, fromFileFiltr,toTMPDirPath,toDirPath,fromDeleteFile,toRewriteFile), this.servece_owner, eventID);
                return this.eventID.GetEventIDErrorCode((int)sftp_client_error.clobal_error);
            }
        }
        /// <summary>
        /// Копировать из SFTP в указаную папку
        /// </summary>
        /// <returns></returns>
        public int CopySFTPFile()
        {
            return CopySFTPFile(this._fromPathsHost, this._FileFiltrHost, this._toTMPDirPath, this._toDirPath, this._DeleteFileHost, this._RewriteFile);
        }
        /// <summary>
        /// Полное копирование из SFTP в указаную папку 
        /// </summary>
        /// <param name="fromFilePaths"></param>
        /// <param name="fromFileFiltr"></param>
        /// <param name="toDirPath"></param>
        /// <param name="fromDeleteFile"></param>
        /// <param name="toRewriteFile"></param>
        /// <returns></returns>
        public int CopyToDir(string fromFilePaths, string fromFileFiltr, string toTMPDirPath, string toDirPath, bool fromDeleteFile, bool toRewriteFile)
        {
            try
            {
                int res = 0;
                if (Connect())
                {
                    res = CopySFTPFile(fromFilePaths, fromFileFiltr, toTMPDirPath, toDirPath, fromDeleteFile, toRewriteFile);
                    Close();
                }
                else
                {
                    res = this.eventID.GetEventIDErrorCode((int)sftp_client_error.not_connect);
                }
                return res;
            }
            catch (Exception e)
            {
                e.WriteErrorMethod(String.Format("CopyToDir(fromFilePaths={0}, fromFileFiltr={1}, toTMPDirPath={2}, toDirPath={3}, fromDeleteFile={4}, toRewriteFile={5})",
                    fromFilePaths, fromFileFiltr, toTMPDirPath, toDirPath, fromDeleteFile, toRewriteFile), this.servece_owner, eventID);
                return this.eventID.GetEventIDErrorCode((int)sftp_client_error.clobal_error);
            }
        }
        /// <summary>
        /// Полное копирование из SFTP в указаную папку 
        /// </summary>
        /// <returns></returns>
        public int CopyToDir()
        {
            return CopyToDir(this._fromPathsHost, this._FileFiltrHost, this._toTMPDirPath, this._toDirPath, this._DeleteFileHost, this._RewriteFile);
        }
    }
}
