using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace BillCollectionUploader
{
    public partial class MtbBillCollectionUploader : ServiceBase
    {
        string sSource;
        string sLog;
        string sEvent;
        BillCollectionUploadManager.RealTimeBillCollManager _writeFileToFtpLocation=null;
        public MtbBillCollectionUploader()
        {
            InitializeComponent();

            sSource = "MTB **********";
            sLog = "Application";
            sEvent = "Sample Event";

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
        }

        protected override void OnStart(string[] args)
        {
            _writeFileToFtpLocation = new BillCollectionUploadManager.RealTimeBillCollManager();
            EventLog.WriteEntry(sSource, "MTB Service Started", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry(sSource, "MTB Service End", EventLogEntryType.Information);
        }

        protected override void  OnContinue()
        {
            //_writeFileToFtpLocation.Restart();
 	         base.OnContinue();
        }
        protected override void  OnPause()
        {
            //_writeFileToFtpLocation.Pause();
 	         base.OnPause();
        }
       
       
    }
}
