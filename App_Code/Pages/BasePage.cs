using System;

using System.IO;
using System.Globalization;
using System.Threading;

using APB.Mercury.WebInterface.WebServiceMercury.Www;
using APB.Mercury.WebInterface.WebServiceMercury.Www.MasterPages;

using APB.MercuryFork.CMT.DataObjects;
using APB.Mercury.WebInterface.WebServiceMercury.Www.Globalization;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www.Pages
{
	/// <summary>
	/// Summary description for BasePage
	/// </summary>
	public class BasePage : System.Web.UI.Page
	{
		#region Properties

		public BaseMaster MasterPage
		{
			get
			{
				return (BaseMaster)this.Master;
			}
		}

		public bool HasValidation { get; set; }

		public bool HasGrid { get; set; }

		public bool HasAddress { get; set; }

		public bool HasPhone { get; set; }

        public bool HasMasterPageHidden { get; set; }

        public bool HasCalendar { get; set; }

        public bool HasMaskedInput { get; set; }

        public bool IsNotAuthenticated { get; set; }

        public SystemUserSession SessionUser
        {
            get
            {
                if (Session["_SessionUser"] == null)
                {
                    return null;
                }
                else
                {
                    return (SystemUserSession)Session["_SessionUser"];
                }
            }
            set
            {
                Session["_SessionUser"] = value;
            }
        }

        public string RedirectAfterLogoff
        {
            get
            {
                string lReturn;
                if (Session["RedirectAfterLogoff"] == null)
                    lReturn = "";
                else
                    lReturn = Session["RedirectAfterLogoff"].ToString();

                return lReturn;
            }
        }

		#endregion

		#region Private Methods

		private void LoadPageJsAndCSS()
		{
			string lBaseFolder;
			string lJsFolder;
			string lCssFolder;
			string lFileName;
			string lFullPath;

			lBaseFolder = Request.PhysicalApplicationPath;

			lJsFolder = "Js\\Pages\\";
			lCssFolder = "Skin\\" + this.MasterPage.SkinFolder + "\\Pages\\";

			lFileName = Request.Url.LocalPath.TrimStart('/');

			lFileName = lFileName.Substring(lFileName.IndexOf('/') + 1);

			lFileName = lFileName.Replace('/', '_');

			lFullPath = Path.Combine(Path.Combine(lBaseFolder, lJsFolder), lFileName + ".js");

			if (File.Exists(lFullPath))
			{
				this.MasterPage.AddJavaScript("Pages/" + lFileName);
			}

			lFullPath = Path.Combine(Path.Combine(lBaseFolder, lCssFolder), lFileName + ".css");

			if (File.Exists(lFullPath))
			{
				this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/Pages/" + lFileName);
			}
		}

		protected void LoadHeader()
		{
            string lUrl;

            if (this.SessionUser == null && this.IsNotAuthenticated == false)
            {

                #region Código para autenticação manual, utilizada para debugar projeto

                ////Logon manual para debugar código
                //OperationResult lResult = new OperationResult();

                //lResult = LoginHub.LogUser("ADMIN", "103", "A", LocalInstance.ValidatedSettings.WebAdminLogonSettings);

                //if (lResult.IsValid)
                //{

                //    //Session["_SessionUser"] = (ExternalUserSession)lResult.ReturnObject;
                //    Session["_SessionUser"] = (SystemUserSession)lResult.ReturnObject;

                //}

                #endregion

                Session["RedirectAfterLogin"] = Request.Url.PathAndQuery;

                if (Session["RedirectAfterLogoff"] != null)
                    lUrl = Session["RedirectAfterLogoff"].ToString();
                else
                    lUrl = this.MasterPage.BaseURL + "LogOff.aspx";

                Response.Redirect(lUrl);
                    
            }
            else
            {
                Session["RedirectAfterLogin"] = null;
            }

            this.MasterPage.PageH3 = CultureHub.GetLocalResc("PageH3");


            //Somente insere as bibliotecas JQuery se houver necessidade
            if (this.HasCalendar || this.HasMaskedInput)
            {
                this.MasterPage.AddJavaScript(@"jquery/js/jquery-1.3.2.min.js");
                this.MasterPage.AddJavaScript(@"jquery/js/jquery-ui-1.7.1.custom.min.js");
            }

            if (this.HasCalendar)
            {
                //this.MasterPage.AddJavaScript("DatePickerV2.js");
                //this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/DatePicker.css");

                this.MasterPage.AddJavaScript(@"jquery/js/jquery.datepick-pt-BR.js");
                this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/JQueryDatePicker/smoothness/jquery-ui-1.7.1.custom.css");
            }

            if (this.HasMaskedInput)
            {
                this.MasterPage.AddJavaScript("jquery/js/jquery.maskedinput-1.2.2.min.js");
            }
            


			this.MasterPage.AddJavaScript("lib/prototype.js");
			this.MasterPage.AddJavaScript("scriptaculous/scriptaculous.js");

			this.MasterPage.AddJavaScript("Base.js");
			this.MasterPage.AddJavaScript("Dialogs.js");

			this.MasterPage.AddCSS("base.css");

            this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/Style.css");
            this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/Menu.css");

			if (this.HasValidation)
				this.MasterPage.AddJavaScript("Validation.js");

			if (this.HasGrid)
			{
				//this.MasterPage.AddJavaScript("WebGrid.js");
				//this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/WebGrid.css");
                this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/DefaultGrid.css");
			}

			if (this.HasAddress)
			{
				this.MasterPage.AddJavaScript("AddressPhone.js");
				this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/AddressPhone.css");
			}

			if (this.HasPhone)
			{
				this.MasterPage.AddJavaScript("AddressPhone.js");
				this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/AddressPhone.css");
			}

            if (this.HasMasterPageHidden)
            {
                this.MasterPage.AddCSS(this.MasterPage.SkinFolder + "/MasterPageHidden.css");
            }                        


			LoadPageJsAndCSS();
		}

		#endregion

        #region Public Methods

        protected override void InitializeCulture()
        {
            String lSelectedCulture = LocalInstance.ValidatedSettings.Culture;
            String lSelectedUICulture = LocalInstance.ValidatedSettings.UICulture;

            //Page.Culture = CultureInfo.CreateSpecificCulture(lSelectedCulture);
            Page.Culture = lSelectedCulture;
                
//            Page.UICulture = new CultureInfo(lSelectedUICulture);
            Page.UICulture = lSelectedUICulture;

            base.InitializeCulture();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            
            if (!Page.IsPostBack
                && Session["BackPage"] == null)
                Session["BackPage"] = Request.Url.AbsoluteUri;
            else if (Request.UrlReferrer != null)
                Session["BackPage"] = Request.UrlReferrer.AbsoluteUri;
            
        }

        protected void GoBackPage()
        {
            string lUrl = String.Empty;

            if (Session["BackPage"] != null)
                lUrl = Session["BackPage"].ToString();

            else if (Session["RedirectAfterLogin"] != null)
                lUrl = Session["RedirectAfterLogin"].ToString();

            else
                lUrl = String.Empty;

            if (!String.IsNullOrEmpty(lUrl))
                Response.Redirect(lUrl);
        }
        #endregion
    }
}