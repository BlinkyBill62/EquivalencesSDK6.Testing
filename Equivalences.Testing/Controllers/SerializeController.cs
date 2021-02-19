using Algenta.Colectica.Model.Ddi;
using Algenta.Colectica.Model.Ddi.Serialization;
using Algenta.Colectica.Model.Utility;
using ColecticaSdkMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace EquivalencesSDK6.Controllers
{
    public class Deserializer : Controller
    {
        string xml;
        // GET: Serialize
        public ActionResult Deserialize()
        {
            SerializeModel model = new SerializeModel();
            model.Xml = "";
            return View(model);
        }     

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Deserialize(SerializeModel model, string command, HttpPostedFileBase postedFile)
        {

            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    if (fileExtension != ".xml")
                    {
                        return View(model);
                    }
                    string row;
                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        row = sreader.ReadLine();
                    }
                    model.Xml = row;
                    xml = row;
                    return View(model);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }

            switch (command)
            {
                case "Process":
                    PhysicalInstance oInfoDTO = new PhysicalInstance();
                    model.XmlValues = (PhysicalInstance)XMLToObject(xml, oInfoDTO);
                    return View(model);
                default:
                    break;
            }
            return View(model);
        }

        public Object XMLToObject(string XMLString, Object oObject)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(oObject.GetType());
            oObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            return oObject;
        }

        
    }
}