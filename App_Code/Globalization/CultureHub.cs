using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www.Globalization
{

    /// <summary>
    /// Classe para pegar mensagens globalizadas
    /// </summary>
    public static class CultureHub
    {
        #region Public Methods

        public static string GetLocalResc(string pMessageKey)
        {
            object lMsg = null;

            try
            {
                lMsg = HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path, pMessageKey);
            }
            catch (Exception)
            {
                //falta o arquivo de resources

                //HttpContext.Current.Trace.Write(
                //                                "MissingResourceError",
                //                                string.Format(
                //                                            Resources.ErrorMessages.Setup_MissingResourceFile,
                //                                            HttpContext.Current.Request.FilePath
                //                                            )
                //                                );

            }

            if (lMsg == null) lMsg = string.Format("[{0}][{1}]", HttpContext.Current.Request.Path, pMessageKey);

            return lMsg.ToString();
        }

        #endregion
    }
}