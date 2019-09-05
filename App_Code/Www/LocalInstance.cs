using System;
using System.Configuration;
using System.Globalization;

using APB.Framework.DataBase;
using APB.MercuryFork.CMT.DataObjects;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www
{
    /// <summary>
    /// Summary description for Instance
    /// </summary>
    public static class LocalInstance
    {
        #region Properties        

        private static ConnectionStringSettings _ActiveConnection = null;

        private static ConnectionInfo _ConnectionInfo = null;

        public static ConnectionInfo ConnectionInfo
        {
            get
            {
                if (_ConnectionInfo == null)
                    LoadConnectionInfo();

                return _ConnectionInfo;
            }
        }

        private static ConnectionInfo _CEPConnectionInfo = null;

        public static ConnectionInfo CEPConnectionInfo
        {
            get
            {
                if (_CEPConnectionInfo == null)
                    LoadConnectionInfo();

                return _CEPConnectionInfo;
            }
        }

        private static ValidatedSettings _ValidatedSettings = null;

        public static ValidatedSettings ValidatedSettings
        {
            get
            {
                if (_ValidatedSettings == null)
                    LoadValidatedSettings();

                return _ValidatedSettings;
            }
        }

        public static string SessionIdWS
        {
            get
            {
                return "WSPRODATA";
            }
        }


        //private static ValidatedSystemParameters _ValidatedSystemParameters = null;

        //public static ValidatedSystemParameters ValidatedSystemParameters
        //{
        //    get
        //    {
        //        if (_ValidatedSystemParameters == null)
        //            LoadValidatedSystemParameters();

        //        return _ValidatedSystemParameters;
        //    }
        //}

        #endregion

        #region Private Methods

        private static void ConfigureConnection(ConnectionStringSettings pConnectionSettings, CultureInfo pDBCulture, out ConnectionInfo pInfo)
        {
            pInfo = new ConnectionInfo(
                                        pConnectionSettings.ConnectionString,
                                        (pConnectionSettings.ProviderName.ToLower() == "oracle") ? 0 : 1,
                                        (LocalInstance.ValidatedSettings.ApplicationServerMode.ToLower() == "web") ? 0 : 1,
                                        pDBCulture
                                        );

            //pInfo.HomologModeOn = LocalInstance.ValidatedSettings.HomologModeOn;

            //TODO: Tem um jeito melhorzinho? - luciano
            string[] lConnectionValues;
            string lDBItem;
            string lDBName = "";

            lConnectionValues = pInfo.ConnectionString.Split(';');

            try
            {
                if (pInfo.DataBaseType == 0)
                {
                    lDBItem = "data source";
                }
                else
                {
                    lDBItem = "initial catalog";
                }

                foreach (string lValue in lConnectionValues)
                {
                    if (lValue.ToLower().Contains(lDBItem))
                    {
                        lDBName = lValue.Split('=')[1].Trim();

                        break;
                    }
                }
            }
            catch { }

            pInfo.DataBaseName = lDBName;
        }

        #endregion

        #region Public Methods

        public static void LoadConnectionInfo()
        {
            CultureInfo lDBCulture = System.Globalization.CultureInfo.CurrentUICulture;

            ConnectionStringSettings lCEPDataBaseSettings;

            if (ConfigurationManager.AppSettings["DBCulture"] != null)
            {
                try
                {
                    CultureInfo lCulture = new CultureInfo(LocalInstance.ValidatedSettings.DBCulture);

                    lDBCulture = lCulture;
                }
                catch { }
            }

            //Banco da aplicação:

            _ActiveConnection = ConfigurationManager.ConnectionStrings[LocalInstance.ValidatedSettings.ActiveConnectionString];

            ConfigureConnection(_ActiveConnection, lDBCulture, out _ConnectionInfo);

            //Banco de CEP:

            lCEPDataBaseSettings = new ConnectionStringSettings();

            lCEPDataBaseSettings = ConfigurationManager.ConnectionStrings["CEP"];

            if (lCEPDataBaseSettings != null)
                ConfigureConnection(lCEPDataBaseSettings, lDBCulture, out _CEPConnectionInfo);
        }


        public static OperationResult LoadValidatedSettings()
        {
            _ValidatedSettings = new ValidatedSettings();

            return _ValidatedSettings.LoadValidatedSettings();
        }

        //public static OperationResult LoadValidatedSystemParameters()
        //{
        //    _ValidatedSystemParameters = new ValidatedSystemParameters();

        //    return _ValidatedSystemParameters.LoadValidatedSystemParameters();
        //}

        #endregion
    }
}