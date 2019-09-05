using System;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.SessionState;

using APB.Mercury.WebInterface.WebServiceMercury.Www;
using APB.Mercury.WebInterface.WebServiceMercury.Www.Globalization;
using System.Web.UI.WebControls;

using APB.MercuryFork.CMT.DataObjects;

namespace APB.Mercury.WebInterface.WebServiceMercury.Www.MasterPages
{	
	/// <summary>
	/// Classe base para páginas Master
	/// </summary>
	public class BaseMaster : MasterPage
	{

		#region Properties


        private string _PageH3 = "Propriedade PageH3 do Master";

        public string PageH3
        {
            get { return _PageH3; }
            set { _PageH3 = value; }
        }

		/// <summary>
		/// Retorna a Url da aplicação
		/// </summary>
		public string BaseURL
		{
			get
			{
				try
				{
					return string.Format("http://{0}{1}/", HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
										(VirtualFolder.Equals("/")) ? string.Empty : VirtualFolder);
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Recupera a cultura corrente configurada no Web.config
		/// </summary>
		public string UICurrentCulture
		{
			get
			{
				return System.Globalization.CultureInfo.CurrentCulture.Name;
			}
		}

		/// <summary>
		/// Pasta raíz na qual a aplicação se localiza
		/// </summary>
		/// <returns></returns>
		protected string BaseApplicationPath
		{
			get
			{
				return Server.MapPath(string.Empty);
			}
		}

		/// <summary>
		/// Pasta de skin. Deve ser Skin/[ClientAlias do web.config]
		/// </summary>
		public string SkinFolder
		{
			get
			{
                object lSkinFolder;
                string lLocalFolder;

                lSkinFolder = Session["_SkinFolder"];

                if (lSkinFolder == null)
                {
                    //lSkinFolder = LocalInstance.ValidatedSettings.ClientAlias;
                    SystemUserSession lExternalUserSession;

                    if (Session["_SessionUser"] != null)
                    {
                        lExternalUserSession = (SystemUserSession)Session["_SessionUser"];
                        lSkinFolder = lExternalUserSession.SkinFolder;
                    }
                    else
                    {
                        lSkinFolder = "";
                    }
                    
                    lLocalFolder = Server.MapPath("~/Skin//" + lSkinFolder.ToString());

                    if (!System.IO.Directory.Exists(lLocalFolder))
                    {
                        //TraceWrite(
                        //            "ExpectedConfigError",
                        //            Resources.ErrorMessages.Config_BadSkinFolder,
                        //            lSkinFolder,
                        //            lLocalFolder
                        //            );

                        lSkinFolder = "Default";
                    }

                    Session["_SkinFolder"] = lSkinFolder;
                }

                return lSkinFolder.ToString();
			}
		}

		/// <summary>
		/// Pasta de configuração. Deve ser App_Config/[ClientAlias do web.config]
		/// </summary>
        //public string ConfigFolder
        //{
        //    get
        //    {
        //        object lConfigFolder;
        //        string lLocalFolder;

        //        lConfigFolder = Session["_ConfigFolder"];

        //        if (lConfigFolder == null)
        //        {
        //            lConfigFolder = LocalInstance.ValidatedSettings.ClientAlias;

        //            lConfigFolder = System.IO.Path.Combine("App_Config", lConfigFolder.ToString());

        //            lLocalFolder = Server.MapPath(lConfigFolder.ToString());

        //            if (!System.IO.Directory.Exists(lLocalFolder))
        //            {
        //                TraceWrite(
        //                            "ExpectedConfigError",
        //                            Resources.ErrorMessages.Config_BadSkinFolder,
        //                            lConfigFolder,
        //                            lLocalFolder
        //                            );

        //                lConfigFolder = "Default";
        //            }

        //            Session["_ConfigFolder"] = lConfigFolder;
        //        }

        //        return lConfigFolder.ToString();
        //    }
        //}

		/// <summary>
		/// Retorna o diretório virtual onde o projeto está localizado
		/// </summary>
		private static string VirtualFolder
		{
			get { return HttpContext.Current.Request.ApplicationPath; }
		}

        /// <summary>
        /// Retorna o item do menu esquerdo que está espandido
        /// </summary>
        public LeftMenuItems SelectedMenuItem
        {
            get
            {
                return _SelectedMenuItem;
            }
            set
            {
                _SelectedMenuItem = value;
            }
        }

        private LeftMenuItems _SelectedMenuItem = LeftMenuItems.None;

        /// <summary>
        /// Retorna o item do menu esquerdo que está selecionado
        /// </summary>
        public LeftMenuSubItems SelectedMenuSubItem
        {
            get
            {
                return _SelectedMenuSubItem;
            }
            set
            {
                _SelectedMenuSubItem = value;
            }
        }

        private LeftMenuSubItems _SelectedMenuSubItem = LeftMenuSubItems.None;

		#endregion

		#region Public Methods

		public void AddCSS(string pCss)
		{
			AddCSS(pCss, "screen");
		}

		/// <summary>
		/// Adiciona um link para um arquivo de css, com base na pasta ~/skin/
		/// </summary>
		/// <param name="pCss">Caminho para o arquivo, se não tiver a extensão ela é adicionada.</param>
		/// <param name="pMedia">Valor do atributo media da tag link.</param>
		public void AddCSS(string pCss, string pMedia)
		{
			if (!pCss.EndsWith(".css")) pCss += ".css";

			Literal lNewCss = new Literal();

			lNewCss.Text = "\n<link href=\"" + this.BaseURL + "Skin/" + pCss + "\"  rel=\"stylesheet\" type=\"text/css\" media=\"" + pMedia + "\" />";
			
			Page.Header.Controls.Add(lNewCss);
		}

		/// <summary>
		/// Adiciona um link para um arquivo de js, com base na pasta ~/js/
		/// </summary>
		/// <param name="pJavaScriptFileName">Caminho para o arquivo, se não tiver a extensão ela é adicionada.</param>
		public void AddJavaScript(string pJavaScriptFileName)
		{
			if (!pJavaScriptFileName.EndsWith(".js")) pJavaScriptFileName += ".js";

			Literal lNewJs = new Literal();

			lNewJs.Text = "\n<script type=\"text/javascript\" src=\"" + this.BaseURL + "Js/" + pJavaScriptFileName + "\"></script>";
			
			Page.Header.Controls.Add(lNewJs);
		}

		/// <summary>
		/// Adiciona javascript dentro do corpo de uma função que será carregada no PageLoad
		/// </summary>
		/// <param name="pJavaScriptFunctionBody">Corpo da função que irá rodar no page load</param>
		public void AddJavaScriptBodyOnLoad(string pJavaScriptFunctionBody)
		{
			Literal lNewJs = new Literal();

			string lFunctionUniqueID = Guid.NewGuid().ToString().Replace('-', '_');

			lNewJs.Text = "\n<script type=\"text/javascript\">    \r\n\r\n";
			lNewJs.Text += "function AfterPageLoad_" + lFunctionUniqueID + "() {  \r\n";
			lNewJs.Text += "//essa funcao foi incluida via AddJavaScriptBodyOnLoad do master. \r\n\r\n";
			lNewJs.Text += pJavaScriptFunctionBody;
			lNewJs.Text += "\r\n    }  \r\n\r\n";
			lNewJs.Text += "addLoadEvent(AfterPageLoad_" + lFunctionUniqueID + ");  \r\n\r\n";
			lNewJs.Text += "</script>   \r\n\r\n";

			Page.Header.Controls.Add(lNewJs);
		}

		/// <summary>
		/// Adiciona um alerta javascript no page_load
		/// </summary>
		/// <param name="pMessages">Mensagem para mostrar</param>
		/// <param name="pType">0: Mensagem de erro; 1: Mensagem de ajuda</param>
		public void JavaScriptAlert(string pMessages, byte pType)
		{
			JavaScriptAlert(new string[]{pMessages}, pType);
		}

		/// <summary>
		/// Adiciona um alerta javascript no page_load
		/// </summary>
		/// <param name="pMessages">Array de mensagens para mostrar</param>
		/// <param name="pType">0: Mensagem de erro; 1: Mensagem de ajuda</param>
		public void JavaScriptAlert(string[] pMessages, byte pType)
		{
			string lMessages = "";

			foreach (string lMsg in pMessages)
			{
				lMessages += string.Format("'{0}',", lMsg.Replace("\n", "<br>").Replace("\r\n", "<br>").Replace(@"\n", "<br>").Replace('\'', ' ').Replace('"', ' ').Replace('\'', ' '));
			}

			lMessages =	lMessages.TrimEnd(',');

			lMessages = lMessages.Replace("\n", "").Replace("\r\n", "").Replace(@"\n", "").Replace('\r', ' ');

			string lFunc = "pDialogs_ShowErrorMessage";

			if(pType == 1) lFunc = "pDialogs_ShowHelpMessage";

			AddJavaScriptBodyOnLoad(string.Format("{0}(new Array({1}));", lFunc, lMessages));
		}

		/// <summary>
		/// Adiciona uma mensagem ao trace da página, com suporte a string. format
		/// </summary>
		/// <param name="pTraceCategory">Categoria da mensagem do Trace</param>
		/// <param name="pBaseMessage">Mensagem a ser exibida (enum com suporte a localização no CultureHub)</param>
		/// <param name="pParams">Outros parâmetros para o string.format que é feito na mensagem pBaseMessage</param>
		public void TraceWrite(string pCategory, string pMessage, params object[] pParams)
		{
			if (Trace.IsEnabled)
			{
				Page.Trace.Write(pCategory, string.Format(pMessage, pParams));
			}
		}

		#endregion

	}

}
