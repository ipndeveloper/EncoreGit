using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace NetSteps.BirthdayAlert.Host
{
    partial class ServiceBirthday : ServiceBase
    {

        private Timer t = null;

        public ServiceBirthday()
        {
            InitializeComponent();
            t = new Timer(60000);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);

            if(!System.Diagnostics.EventLog.SourceExists("MySourceBirthdayAlert"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MySourceBirthdayAlert","MyLog");
            }
             
            eventLogBirthdayAlert.Source = "MySourceBirthdayAlert";
            eventLogBirthdayAlert.Log = "MyLog";

        }

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {



            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", "Exception:" + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
            t.Start(); //Inciamos el Timer
            eventLogBirthdayAlert.WriteEntry("Se inicio el servicio de BirthdayAlert");
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
            t.Stop();
            eventLogBirthdayAlert.WriteEntry("Se detuvo el servicio de BirthdayAlert");
        }
    }
}
