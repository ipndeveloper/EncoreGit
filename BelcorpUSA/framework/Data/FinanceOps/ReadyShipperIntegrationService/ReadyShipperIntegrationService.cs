using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace ReadyShipperIntegrationService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ReadyShipperIntegrationService : IReadyShipperIntegration
    {
        //LinqToSql data context
        NetstepsDataContext dc = NetstepsDataContext.DB;

        public ReadyShipperIntegrationService() { }

        /// <summary>
        /// Returns a string object containing the text of a true order xml to import into ready shipper
        /// </summary>
        /// <returns>string of true order xml</returns>
        public string ImportOrdersIntoReadyShipper(string userName, string password)
        {
            if (userName != ConfigurationManager.AppSettings["readyShipperUserName"].ToString() || password != ConfigurationManager.AppSettings["readyShipperPassword"].ToString())
            {
                dc.UspLogisticsCommunicationInsert("An unsuccessful login attempt was made by username: " + userName + " password: " + password);
                return "Incorrect Login Credentials";
            }
            try
            {
                ordercollection col = OrderObjectMapping.MapOrderCollectionFromView(dc);
                //ordercollection col = GetOrderCollectionObject(GetXmlStringFromFilePath(ConfigurationManager.AppSettings["TodaysOrdersPath"].ToString(), dc));
                string xml = GetXmlFromOrderCollectionObject(col);
                xml = xml.Remove(0, 1);
                xml = xml.Replace("utf-16\"?>", "utf-8\"?>");

                if (ConfigurationManager.AppSettings["LogAllActivity"].ToString().ToUpper().Equals("TRUE"))
                {
                    dc.UspLogisticsCommunicationInsert("ImportOrdersIntoReadyShipper called with the following xml returned: " + xml);
                }

                return xml;
            }
            catch (Exception ex)
            {
                dc.UspErrorLogsInsert(DateTime.Now, String.Empty, "ImportOrdersIntoReadyShipper method, ReadyShipperIntegrationService class: ", ex.Message);
                return "There was an error encountered: " + ex.Message;
            }
        }

        /// <summary>
        /// Takes in a string object containing the text of a true order xml exported from Ready Shipper to update or database.
        /// </summary>
        /// /// <param name="trueOrderXml">string of xml data of true order xml exported from Ready Shipper</param>
        /// <returns>void</returns>
        public void ExportOrdersFromReadyShipper(string trueOrderXml, string userName, string password)
        {
            if (userName != ConfigurationManager.AppSettings["readyShipperUserName"].ToString() || password != ConfigurationManager.AppSettings["readyShipperPassword"].ToString())
            {
                dc.UspLogisticsCommunicationInsert("An unsuccessful login attempt was made by username: " + userName + " password: " + password);
                return;
            }
            try
            {
                ordercollection col = GetOrderCollectionObject(trueOrderXml);
                if (ConfigurationManager.AppSettings["LogAllActivity"].ToString().ToUpper().Equals("TRUE"))
                {
                    dc.UspLogisticsCommunicationInsert("ExportOrdersFromReadyShipper method called with the following xml as an input: " + trueOrderXml);
                }
                OrderShipmentsUpdate.UpdateOrderShipments(col, dc);
            }
            catch (Exception ex)
            {
                dc.UspErrorLogsInsert(DateTime.Now, String.Empty, "ExportOrdersFromReadyShipper method, ReadyShipperIntegrationService class: ", ex.Message);
            }
        }

        /// <summary>
        /// Returns an ordercollection xsd class object based on the xml data passed
        /// </summary>
        /// <param name="xml">xml data of Order Collection</param>
        /// <returns></returns>
        internal ordercollection GetOrderCollectionObject(string xml)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                // serialise to object
                XmlSerializer serializer = new XmlSerializer(typeof(ordercollection));
                stream = new StringReader(xml); // read xml data
                reader = new XmlTextReader(stream);  // create reader
                // covert reader to object
                return (ordercollection)serializer.Deserialize(reader);
            }
            catch(Exception ex)
            {
                dc.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetOrderCollectionObject method, ReadyShipperIntegrationService class: ", ex.Message);
                return null;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (reader != null) reader.Close();
            }
        }

        
        /// <summary>
        /// Returns the xml as string based on ordercollection object values
        /// </summary>
        /// <param name="ordercollection">object that would be converted into xml</param>
        /// <returns></returns>
        internal string GetXmlFromOrderCollectionObject(ordercollection trueOrder)
        {
            MemoryStream stream = null;
            TextWriter writer = null;
            try
            {
                stream = new MemoryStream(); // read xml in memory
                writer = new StreamWriter(stream, Encoding.UTF8);
                // get serialise object
                XmlSerializer serializer = new XmlSerializer(typeof(ordercollection));



                //XmlWriterSettings settings = new XmlWriterSettings { Encoding = Encoding.Unicode };


                serializer.Serialize(writer, trueOrder); // read object
                /*
                using (XmlWriter writerb = XmlWriter.Create(memoryStream2, settings))
                {
                    _loan = (Model.Loan)xs2.Deserialize(memoryStream2);
                }
                */




                int count = (int)stream.Length; // saves object in memory stream
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                // copy stream contents in byte array
                stream.Read(arr, 0, count);
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(true, false);
                //UnicodeEncoding utf = new UnicodeEncoding(); // convert byte array to string
                return encoding.GetString(arr).Trim();
                //return utf.GetString(arr).Trim();
            }
            catch(Exception ex)
            {
                dc.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetXmlFromOrderCollectionObject method, ReadyShipperIntegrationService class: ", ex.Message);
                return string.Empty;
            }
            finally
            {
                if (stream != null) stream.Close();
                if (writer != null) writer.Close();
            }
        }

        public static string GetXmlStringFromFilePath(string strFile, NetstepsDataContext dc)
        {
            try
            {
                // Load the xml file into XmlDocument object.
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strFile);
                // Now create StringWriter object to get data from xml document.
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                xmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                dc.UspErrorLogsInsert(DateTime.Now, String.Empty, "GetXmlStringFromFilePath method, ReadyShipperIntegrationService class: ", ex.Message);
                return String.Empty; 
            }
        }
    }
}
