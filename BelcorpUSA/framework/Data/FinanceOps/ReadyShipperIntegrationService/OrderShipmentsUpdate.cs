using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ReadyShipperIntegrationService
{
    public static class OrderShipmentsUpdate
    {
        internal static void UpdateOrderShipments(ordercollection col, NetstepsDataContext dc)
        {
            int count = col.order.Count();
            foreach (order o in col.order)
            {
                //string valuesForProc = "";
                string trackingNumber = "";
                if (o.ship_data != null && o.ship_data.box != null)
                {
                    //int numberOfShipments = o.ship_data.box.Count();
                    //int orderShipmentIndex = 1;
                    string delimiter = "";
                    //string carriageReturn = "|";
                    foreach (box b in o.ship_data.box)
                    {
                        trackingNumber += delimiter + b.tracking_number;
                        delimiter = ", ";
                        /*
                        if (numberOfShipments == orderShipmentIndex)
                            carriageReturn = String.Empty;
                        
                        foreach (item i in b.item)
                        {
                            valuesForProc += orderShipmentIndex.ToString()
                                + delimiter 
                                + i.id 
                                + delimiter 
                                + i.quantity 
                                + delimiter 
                                + DateTime.Now.ToShortDateString() // place holder until XSD has a shipped date field.
                                + delimiter
                                + b.tracking_number
                                + carriageReturn;
                        }
                        orderShipmentIndex++;
                        */ 
                    }
                }

                dc.uspLogisticsRecordTrackingNumberByOrderID(Convert.ToInt32(o.id.primary_id), trackingNumber);
                
                // update the database with new order shipment information.
                //dc.UspUpdateOrderShipments(Convert.ToInt32(ConfigurationManager.AppSettings["modifiedByUserID"]),
                //    Convert.ToInt32(o.id.primary_id), valuesForProc);
            }
            
        }
    }
}
