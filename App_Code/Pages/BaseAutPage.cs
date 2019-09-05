using System;
using System.Configuration;

using APB.Mercury.WebInterface.WebServiceMercury.Www.MasterPages;
using APB.Mercury.WebInterface.WebServiceMercury.Www.Globalization;

using APB.Mercury.WebInterface.WebServiceMercury.Www;

using APB.MercuryFork.CMT.DataObjects;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www.Pages
{
    /// <summary>
	/// Summary description for BaseAutPage
	/// </summary>
	public class BaseAutPage : BasePage
	{
		#region Properties
        
        public new BaseAutMaster MasterPage
		{
			get
			{
				return (BaseAutMaster)base.MasterPage;
			}
		}

        //public ExternalUserSession SessionUser
        //{
        //    get
        //    {
        //        if (Session["_SessionUser"] == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return (ExternalUserSession)Session["_SessionUser"];
        //        }
        //    }
        //    set
        //    {
        //        Session["_SessionUser"] = value;
        //    }
        //}

		#endregion

		#region Private Methods

		protected new void LoadHeader()
		{
            string lUrl;

            //TODO: descomentar esse trecho antes de fechar a versão
            //if (this.SessionUser == null)
            //{
            //    Session["RedirectAfterLogin"] = Request.Url.PathAndQuery;

            //    if (Session["RedirectAfterLogoff"] != null)
            //        lUrl = Session["RedirectAfterLogoff"].ToString();
            //    else
            //        lUrl = this.MasterPage.BaseURL + "LogOff.aspx";

            //    Response.Redirect(lUrl);
            //}
            //else
            //{
            //    Session["RedirectAfterLogin"] = null;
            //}

			if (Request["imp"] != null)
			{
				decimal lNewID = 0;

				if(decimal.TryParse(Request["imp"], out lNewID))
				{
                    //if (LocalInstance.ValidatedSettings.AllowProviderImpersonation)
                    //{
                    //    if (Request["pass"] == DateTime.Now.Minute.ToString())
                    //    {
                    //        this.SessionUser.ProviderID = lNewID;
                    //        this.SessionUser.Impersonated = true;
                    //    }
                    //}
				}
			}

            //código utilizado abaixo para agilizar homologação
            //if (this.SessionUser.Impersonated)
            //{
            //    this.Title += " IMPERSONATING PRV_ID " + this.SessionUser.ProviderID.ToString();
            //}

			base.LoadHeader();

			this.MasterPage.PageH3 = CultureHub.GetLocalResc("PageH3");

			this.MasterPage.AddJavaScriptBodyOnLoad("PageInit();");
		}

		#endregion
	}
}