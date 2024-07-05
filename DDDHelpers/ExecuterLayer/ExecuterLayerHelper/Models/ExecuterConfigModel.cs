using System;
using System.Collections.Generic;
using System.Text;

namespace ExecuterLayerHelper.Models
{
    public class ExecuterConfigModel
    {
        #region Private Members
        private string _Rabbit_User_FILE;
        private string _Rabbit_Password_FILE;

        private string _Database_User_FILE;
        private string _Database_Password_FILE;
        #endregion

        #region C'tor
        public ExecuterConfigModel(string rabbitServer, string rabbitUserFILE, string rabbitPasswordFILE,
            string databaseServer, string databasePort, string databaseName, string databaseUserFILE, string databasePasswordFILE,
            string directoryUploadPicture, string directoryUploadVideo, string directoryConvertVideo,
            string endpointNameService, string endpointNameExecuter)
        {
            Rabbit_Server = rabbitServer;
            _Rabbit_User_FILE = rabbitUserFILE;
            _Rabbit_Password_FILE = rabbitPasswordFILE;
            Endpointname_Service = endpointNameService;
            Endpointname_Executer = endpointNameExecuter;

            Database_Server = databaseServer;
            DatabasePort = databasePort;
            Database_Name = databaseName;
            _Database_User_FILE = databaseUserFILE;
            _Database_Password_FILE = databasePasswordFILE;

            DirectoryUploadPicture = directoryUploadPicture;
            DirectoryUploadVideo = directoryUploadVideo;
            DirectoryConvertVideo = directoryConvertVideo;
        }
        #endregion

        #region Public Properties
        #region RabbitMQ
        public string Rabbit_Server { get; private set; }

        public string Rabbit_User {
            get
            {
                return System.IO.File.ReadAllText(_Rabbit_User_FILE);
            }
        }

        public string Rabbit_Password {
            get
            {
                return System.IO.File.ReadAllText(_Rabbit_Password_FILE);
            }
        }

        public string Endpointname_Service { get; private set; }

        public string Endpointname_Executer { get; private set; }
        #endregion

        #region Database
        public string Database_Server { get; private set; }

        public string DatabasePort { get; private set; }

        public string Database_Name { get; private set; }

        public string Database_User {
            get
            {
                return System.IO.File.ReadAllText(_Database_User_FILE);
            }
        }

        public string Database_Password {
            get
            {
                return System.IO.File.ReadAllText(_Database_Password_FILE);
            }
        }
        #endregion

        #region Directories
        public string DirectoryUploadPicture { get; private set; }

        public string DirectoryUploadVideo { get; private set; }

        public string DirectoryConvertVideo { get; private set; }
        #endregion
        #endregion
    }
}
