using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using APB.MercuryFork.CMT.DataObjects.WebServiceCEP.CEP;
using System.Data;
using APB.Mercury.WebInterface.WebServiceMercury.Www;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// Summary description for WS_Auth
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WSCEP : System.Web.Services.WebService
{

    public WSCEP()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GETTOKEN(string pKEY)
    {
        string lReturn = string.Empty;

        Regex lMath = new Regex("(^[1-5]{2})([A-J]{4})([L-Z]*)([6-9]{1})");

        if (lMath.IsMatch(pKEY))
            lReturn = Crypto(pKEY);

        return lReturn;
    }

    [WebMethod]
    public DataTable GETCEP(string pCEP, string pToken, string pIsDeliveryRequest, int pIdCourier)
    {
        DataTable lResult;

        try
        {
            if (ValidaToken(pToken))
            {
                CEP lCep = new CEP();
                
                string lCleanUserZipCode = pCEP.Replace("-", "").Replace("'", "");

                if (pIsDeliveryRequest == string.Empty)
                    pIsDeliveryRequest = "0";

                if (pIsDeliveryRequest == "1")
                {
                    if (lCep.CourierCanDeliver(lCleanUserZipCode, LocalInstance.ConnectionInfo))
                        lResult = lCep.GetDeliveryAddressByZipCode(lCleanUserZipCode, Convert.ToInt32(pIsDeliveryRequest), pIdCourier, LocalInstance.ConnectionInfo);
                    else
                    {
                        lResult = new DataTable("ERRO");
                        lResult.Columns.Add("MENSAGEM");
                        DataRow lRow = lResult.NewRow();
                        lRow["MENSAGEM"] = "A Empresa courier não realiza entregas no endereço solicitado.";
                        lResult.Rows.Add(lRow);
                    }
                }
                else
                {
                    lResult = lCep.GetDeliveryAddressByZipCode(lCleanUserZipCode, Convert.ToInt32(pIsDeliveryRequest), pIdCourier, LocalInstance.ConnectionInfo);
                }
            }
            else
            {
                lResult = new DataTable("ERRO");

                lResult.Columns.Add("MENSAGEM");
                DataRow lRow = lResult.NewRow();
                lRow["MENSAGEM"] = "Token Incorreto";
                lResult.Rows.Add(lRow);
            }
        }
        catch (Exception lEx)
        {
            lResult = new DataTable("ERRO");

            lResult.Columns.Add("MENSAGEM");
            DataRow lRow = lResult.NewRow();
            lRow["MENSAGEM"] = lEx.Message;
            lResult.Rows.Add(lRow);
        }

        return lResult;

    }

    [WebMethod]
    public DataTable GETLOGRADOURO(string pEstado, string pCidade, string pRua, string pToken, string pIsDeliveryRequest, int pCourier)
    {

        DataTable lResult;

        try
        {
            if (ValidaToken(pToken))
            {
                CEP lCep = new CEP();

                if (string.IsNullOrEmpty(pIsDeliveryRequest))
                    pIsDeliveryRequest = "0";                                
                
                lResult = lCep.GetCurrierAddresses(pEstado.Replace("'", "´"), pCidade.Replace("'", "´"), pRua.Replace("'", "´"), Convert.ToInt32(pIsDeliveryRequest), pCourier, LocalInstance.ConnectionInfo);
                
            }
            else
            {
                lResult = new DataTable("ERRO");

                lResult.Columns.Add("MENSAGEM");
                DataRow lRow = lResult.NewRow();
                lRow["MENSAGEM"] = "Token Incorreto";
                lResult.Rows.Add(lRow);
            }
        }
        catch (Exception lEx)
        {
            lResult = new DataTable("ERRO");

            lResult.Columns.Add("MENSAGEM");
            DataRow lRow = lResult.NewRow();
            lRow["MENSAGEM"] = lEx.Message;
            lResult.Rows.Add(lRow);
        }

        return lResult;

    }

    private bool ValidaToken(string pToken)
    {
        bool lReturn;
        try
        {
            string[] lKey;

            Regex lMath = new Regex("(^[1-5]{2})([A-J]{4})([L-Z]*)([6-9]{1})");
            lKey = Decrypto(pToken).Split('|');

            if (!lMath.IsMatch(lKey[0]))
                lReturn = false;

            if (!Convert.ToDateTime(lKey[1]).ToString("dd/MM/yyyy").Equals(DateTime.Now.ToString("dd/MM/yyyy")))
                lReturn = false;

            lReturn = true;
        }
        catch
        {
            lReturn = false;
        }

        return lReturn;
    }

    private string Crypto(string pKey)
    {
        try
        {




            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Mode = CipherMode.ECB;

            DES.Key = ASCIIEncoding.ASCII.GetBytes("TokenCep");
            DES.Padding = PaddingMode.PKCS7;


            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            Byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(pKey + "|" + DateTime.Now.ToString("dd/MM/yyyy"));
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

        }
        catch (Exception Ex)
        {
            return "";
        }



    }

    private string Decrypto(string pToken)
    {
        DESCryptoServiceProvider DES = new DESCryptoServiceProvider();



        DES.Mode = CipherMode.ECB;

        DES.Key = ASCIIEncoding.ASCII.GetBytes("TokenCep");

        DES.Padding = PaddingMode.PKCS7;

        ICryptoTransform DESEncrypt = DES.CreateDecryptor();
        Byte[] Buffer = Convert.FromBase64String(pToken);

        return Encoding.UTF8.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));




    }



}

