using System;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;

using APB.MercuryFork.CMT.DataObjects;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www
{

    /// <summary>
    /// Configurações do site que foram validadas
    /// </summary>
    public class ValidatedSettings
    {

        #region Global

        #endregion

        #region Properties

        public string ApplicationServerMode { get; set; }

        public string ApplicationName { get; set; }

        public string ApplicationSymbol { get; set; }

        public string ActiveConnectionString { get; set; }

        public string SiteVersionNumber { get; set; }

        public string ExternalUser { get; set; }

        public decimal PageSize { get; set; }

        public string Culture { get; set; }

        public string UICulture { get; set; }

        public string DBCulture { get; set; }

        public string DateOutPut { get; set; }

        public string DateTimeOutPut { get; set; }
        
        public string FolderExeServiceIntegCmtBv { get; set; }

        public string FolderConfigServiceIntegCmtBv { get; set; }

        public string LogoImageBillet { get; set; }

        public string URLBilletImages { get; set; }

        public string UploadPathImportFinger { get; set; }

        #endregion

        #region Constructors

        public ValidatedSettings()
        {
        }

        #endregion

        #region Public Methods

        public OperationResult LoadValidatedSettings()
        {
            OperationResult     lReturn = new OperationResult();
            AppSettingsSection  lSection;

            string lSetting;
            decimal lDecimalValue;

            //iniciar configurações dos logons de estudante e comum


            //	ApplicationServerMode -----------------------------------------------------------------

            lReturn.Trace("Config [ApplicationServerMode] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["ApplicationServerMode"];

            if (lSetting == null || lSetting == "" || (lSetting.ToLower() != "internal" && lSetting.ToLower() != "web"))
            {
                lReturn.Trace("Config [ApplicationServerMode] - Nulo ou vazio, revertendo para valor padrão");

                lSetting = "Internal";
            }

            lReturn.Trace("Config [ApplicationServerMode]:[{0}]", lSetting);

            this.ApplicationServerMode = lSetting;


            //	ApplicationName -----------------------------------------------------------------

            lReturn.Trace("Config [ApplicationName] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["ApplicationName"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [ApplicationName] - Nulo ou vazio, revertendo para valor padrão");

                lReturn.InvalidField("ApplicationName", "NullOrInvalid", lSetting);
            }

            lReturn.Trace("Config [ApplicationName]:[{0}]", lSetting);

            this.ApplicationName = lSetting;


            //	ApplicationSymbol -----------------------------------------------------------------

            lReturn.Trace("Config [ApplicationSymbol] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["ApplicationSymbol"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [ApplicationSymbol] - Nulo ou vazio, revertendo para valor padrão");

                lReturn.InvalidField("ApplicationSymbol", "NullOrInvalid", lSetting);
            }
            else if (lSetting.Length > 3)
            {
                lReturn.Trace("Config [ApplicationSymbol] - Valor com mais de três caracteres");

                lReturn.InvalidField("ApplicationSymbol", "InvalidSize", lSetting);

                lSetting = string.Empty;
            }

            lReturn.Trace("Config [ApplicationSymbol]:[{0}]", lSetting);

            this.ApplicationSymbol = lSetting;


            //	ActiveConnectionString -----------------------------------------------------------------

            lReturn.Trace("Config [ActiveConnectionString] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["ActiveConnectionString"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [ActiveConnectionString] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("ActiveConnectionString", "NullOrInvalid", lSetting);
            }
            else
            {
                if (ConfigurationManager.ConnectionStrings[lSetting] == null && this.ApplicationServerMode.ToLower() == "internal")
                {
                    lReturn.Trace("Config [ActiveConnectionString] - Connection string [{0}] inexistente, configuração inválida!", lSetting);

                    lReturn.InvalidField("ActiveConnectionString", "Invalid", lSetting);
                }
                else
                {
                    lReturn.Trace("Config [ActiveConnectionString]:[{0}]", lSetting);

                    this.ActiveConnectionString = lSetting;
                }
            }


            //	SiteVersionNumber -----------------------------------------------------------------

            lReturn.Trace("Config [SiteVersionNumber] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["SiteVersionNumber"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [SiteVersionNumber] - Nulo ou vazio, revertendo para valor padrão");

                lSetting = "AA.MM.NN";
            }

            lReturn.Trace("Config [SiteVersionNumber]:[{0}]", lSetting);

            this.SiteVersionNumber = lSetting;




            //	DBCulture -----------------------------------------------------------------

            lReturn.Trace("Config [DBCulture] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["DBCulture"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [DBCulture] - Nulo ou vazio, revertendo para valor padrão");

                lSetting = "en-US";
            }
            else
            {
                lReturn.Trace("Config [DBCulture] - Testando cultura");

                try
                {
                    CultureInfo lTestCulture = new CultureInfo(lSetting);

                    lReturn.Trace("Config [DBCulture] - Teste de cultura ok!");
                }
                catch
                {
                    lReturn.Trace("Config [DBCulture] - Teste de cultura falhou, revertendo para valor padrão");

                    lSetting = "en-US";
                }
            }

            lReturn.Trace("Config [DBCulture]:[{0}]", lSetting);

            this.DBCulture = lSetting;


            //	DateOutPut -----------------------------------------------------------------

            lReturn.Trace("Config [DateOutPut] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["DateOutPut"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [DateOutPut] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("DateOutPut", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [DateOutPut]:[{0}]", lSetting);

                this.DateOutPut = lSetting;
            }


            //	DateTimeOutPut -----------------------------------------------------------------

            lReturn.Trace("Config [DateTimeOutPut] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["DateTimeOutPut"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [DateTimeOutPut] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("DateTimeOutPut", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [DateTimeOutPut]:[{0}]", lSetting);

                this.DateTimeOutPut = lSetting;
            }


            //	ExternalUser -----------------------------------------------------------------

            lReturn.Trace("Config [ExternalUser] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["ExternalUser"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [ExternalUser] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("ExternalUser", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [ExternalUser]:[{0}]", lSetting);

                this.ExternalUser = lSetting;
            }
     

			//	PageSize -----------------------------------------------------------------

			lReturn.Trace("Config [PageSize] - Pegando valor");

			lSetting = ConfigurationManager.AppSettings["PageSize"];

			if (lSetting == null || lSetting == "" || !decimal.TryParse(lSetting, out lDecimalValue))
			{
				lReturn.Trace("Config [PageSize] - Nulo, configuração inválida!");

				lReturn.InvalidField("PageSize", "NullOrInvalid", lSetting);
			}
			else
			{
				lReturn.Trace("Config [PageSize]:[{0}]", lSetting);

				this.PageSize = lDecimalValue;
			}

            
            //	Culture -----------------------------------------------------------------

            lReturn.Trace("Config [Culture] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["Culture"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [Culture] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("Culture", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [Culture]:[{0}]", lSetting);

                this.Culture = lSetting;
            }


            //	UICulture -----------------------------------------------------------------

            lReturn.Trace("Config [UICulture] - Pegando valor");

            lSetting = lSetting = ConfigurationManager.AppSettings["UICulture"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [UICulture] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("UICulture", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [UICulture]:[{0}]", lSetting);

                this.UICulture = lSetting;
            }

            //	FolderExeServiceIntegCmtBv -----------------------------------------------------------------

            lReturn.Trace("Config [FolderExeServiceIntegCmtBv] - Pegando valor");

            lSetting = lSetting = ConfigurationManager.AppSettings["FolderExeServiceIntegCmtBv"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [FolderExeServiceIntegCmtBv] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("FolderExeServiceIntegCmtBv", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [FolderExeServiceIntegCmtBv]:[{0}]", lSetting);

                this.FolderExeServiceIntegCmtBv = lSetting;
            }

            //	FolderConfigServiceIntegCmtBv -----------------------------------------------------------------

            lReturn.Trace("Config [FolderConfigServiceIntegCmtBv] - Pegando valor");

            lSetting = lSetting = ConfigurationManager.AppSettings["FolderConfigServiceIntegCmtBv"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [FolderConfigServiceIntegCmtBv] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("FolderConfigServiceIntegCmtBv", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [FolderConfigServiceIntegCmtBv]:[{0}]", lSetting);

                this.FolderConfigServiceIntegCmtBv = lSetting;
            }

            //	URLBilletImages --------------------------------------------------------------

            lReturn.Trace("Config [LogoImageBillet] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["LogoImageBillet"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [LogoImageBillet] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("LogoImageBillet", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [LogoImageBillet]:[{0}]", lSetting);

                this.LogoImageBillet = lSetting;
            }

            //	URLBilletImages --------------------------------------------------------------

            lReturn.Trace("Config [URLBilletImages] - Pegando valor");

            lSetting = ConfigurationManager.AppSettings["URLBilletImages"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [URLBilletImages] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("URLBilletImages", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [URLBilletImages]:[{0}]", lSetting);

                this.URLBilletImages = lSetting;
            }

            //	UploadPathImportFinger -----------------------------------------------------------------

            lReturn.Trace("Config [UploadPathImportFinger] - Pegando valor");

            lSetting = lSetting = ConfigurationManager.AppSettings["UploadPathImportFinger"];

            if (lSetting == null || lSetting == "")
            {
                lReturn.Trace("Config [UploadPathImportFinger] - Nulo ou vazio, configuração inválida!");

                lReturn.InvalidField("UploadPathImportFinger", "NullOrInvalid", lSetting);
            }
            else
            {
                lReturn.Trace("Config [UploadPathImportFinger]:[{0}]", lSetting);

                this.UploadPathImportFinger = lSetting;
            }

            return lReturn;

        }

        #endregion
    }
}