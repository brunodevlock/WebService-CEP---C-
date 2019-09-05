using System;
using System.Data;
using APB.MercuryFork.CMT.DataObjects.TableDefinitions;
using APB.Framework.DataBase;
using APB.MercuryFork.CMT.DataObjects;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www
{
    [Serializable]
    public class SystemUserSession
    {
        #region Properties

        public string Email { get; set; }
        
        public string Name { get; set; }

        public decimal UserId { get; set; }

        public decimal PiuId { get; set; }

        public decimal Id { get; set; }

        //public decimal WebProductId { get; set; }

        public string SkinFolder { get; set; }

        //public PortalSettings Settings { get; set; }

        /// <summary>
        /// Nome cortado em 20 caracteres para poder incluir nos campos "REGUSER"
        /// </summary>
        public string TruncName
        {
            get
            {
                return (this.Name.Length > 20) ? this.Name.Substring(0, 20) : this.Name;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public SystemUserSession()
        {
        }

        public SystemUserSession(string pEmail, decimal pUserId)
        {
            this.Email  = pEmail;
            this.UserId = pUserId;
        }

        //public SystemUserSession(ConnectionInfo pConnInfo, PortalSettings pPortalSettings)
        //{

        //    DataTable lSkinTable;

        //    try
        //    {
        //        this.Settings       = pPortalSettings;
        //        //this.WebProductId   = pPortalSettings.WebProductId;
        //        this.SkinFolder     = pPortalSettings.SkinFolder;

        //    }
        //    catch (Exception lException)
        //    {
        //        throw lException;
        //    }

        //}

        #endregion
    }
}
