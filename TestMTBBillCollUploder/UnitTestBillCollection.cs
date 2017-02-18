using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BillCollectionUploadManager;

namespace TestMTBBillCollUploder
{
    [TestClass]
    public class UnitTestBillCollection
    {
        [TestMethod]
        public void TestUploadBillColl()
        {
            RealTimeBillCollManager rel = new RealTimeBillCollManager();
            Assert.IsTrue(rel.UpdateLogForFileUpload("sgs", false, DateTime.Now));
        }
    }
}
