using Newtonsoft.Json;
using STSTRAVRAYS.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;

namespace STSTRAVRAYS.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult SessionExp()
        {
            return View();
        }

        public ActionResult Redirect()
        {
            return View();
        }

        public ActionResult Comingsoon()
        {
            return View();
        }

        public ActionResult Error() {
            return View();
        }

        public ActionResult EmergencyUpdate() 
        {
            return View();            
        }

        #region EDIT KEY AND XMl
        public ActionResult Admin()
        {
            Session.Clear();
            return View();
        }

        public string webconfiglogin(string user, string pass, string Message)
        {
            string result = string.Empty;
            try
            {
                if (user == ConfigurationManager.AppSettings["KeyEditUserName"].ToString() &&
                    pass == ConfigurationManager.AppSettings["KeyEditPassword"].ToString())
                {
                    Session.Add("AdminLogin", "1");
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            catch (Exception ex)
            {
                return "2";
            }
        }

        [CustomFilterAdmin]
        public ActionResult EditKey()
        {
            DataTable _dsT = new DataTable();
            _dsT = ReadKeyFile();

            var Appkeylist = (from App in _dsT.AsEnumerable()
                              select new WebconfigValuesLst
                              {
                                  Keys = App["Key"].ToString(),
                                  Values = App["Value"].ToString(),
                              }).ToList();

            ViewBag.WebConfigString = JsonConvert.SerializeObject(Appkeylist);// JsonConvert.SerializeObject(Appkeylist);

            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Web.config"));

            ViewBag.WebConfigXML = doc.InnerXml.ToString();
            return View("~/Views/Redirect/EditKey.cshtml");
        }

        [CustomFilterAdmin]
        private DataTable ReadKeyFile()
        {
            Configuration objConfig;
            objConfig = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings"); ;
            DataTable dt = new DataTable();
            dt.Columns.Add("Key");
            dt.Columns.Add("Value");
            foreach (string strKey in objAppsettings.Settings.AllKeys)
            {
                if (strKey != "KeyEditUserName" && strKey != "KeyEditPassword")
                {
                    DataRow dr = dt.NewRow();
                    dr["Key"] = strKey;
                    dr["Value"] = objConfig.AppSettings.Settings[strKey].Value;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        [CustomFilterAdmin]
        public ActionResult UpdateKey(string newConfig)
        {
            ArrayList _result = new ArrayList();

            _result.Add("");
            _result.Add("");
            _result.Add("");

            string Stu = string.Empty;
            string error = string.Empty;
            string result = string.Empty;

            string[] _listofKeys = null;
            try
            {
                _listofKeys = newConfig.TrimEnd('*').Split('*');

                Configuration objConfig = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                if (objAppsettings != null)
                {
                    string[] KeyValue = null;
                    for (int i = 0; i < _listofKeys.Count(); i++)
                    {
                        KeyValue = _listofKeys[i].Split('~');
                        objAppsettings.Settings[KeyValue[0].ToString()].Value = KeyValue[1].ToString();
                        objConfig.Save();
                    }

                }
                Stu = "01";
                error = "";
                result = "Web Config updated successfully.";
            }
            catch (Exception ex)
            {
                Stu = "00";
                error = "";
                result = "Unable to update Web Config file. Please do it manually.";

            }
            return Json(new { status = Stu, Error = error, Result = result });
        }

        [CustomFilterAdmin]
        public ActionResult AddNewKeys(string NewConfig)
        {
            string Stu = string.Empty;
            string error = string.Empty;
            string result = string.Empty;

            string[] _listofKeys = null;
            try
            {
                _listofKeys = NewConfig.TrimEnd('*').Split('*');

                Configuration config;
                config = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection appsettings;
                appsettings = (AppSettingsSection)config.GetSection("appSettings");
                if (appsettings != null)
                {
                    string[] KeyValue = null;
                    for (int i = 0; i < _listofKeys.Count(); i++)
                    {
                        KeyValue = _listofKeys[i].Split('~');
                        appsettings.Settings.Add(KeyValue[0].ToString(), KeyValue[1].ToString());
                        config.Save();
                    }
                }

                Stu = "01";
                error = "";
                result = "Web Config inserted successfully.";
            }
            catch (Exception ex)
            {
                Stu = "00";
                error = "";
                result = "Unable to insert Web Config file. Please do it manually.";

            }


            return Json(new { status = Stu, Error = error, Result = result });
        }

        [CustomFilterAdmin]
        public ActionResult EditXML()
        {
            try
            {
                string[] strfiles = Directory.GetFiles(Server.MapPath("~/XML"));
                ViewBag.strfiles = string.Join("~SPLITFILE~", strfiles);
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/Redirect/EditXML.cshtml");
        }

        [CustomFilterAdmin]
        public ActionResult UpdateXML(string newxmlqry, string filename)
        {
            var stu = string.Empty;
            var msg = string.Empty;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(newxmlqry);
                xDoc.Save(Server.MapPath("~/XML/" + filename.Trim()));
                stu = "01";
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = ex.ToString();
            }
            return Json(new { status = stu, errmsg = msg });
        }
        #endregion
    }
}
