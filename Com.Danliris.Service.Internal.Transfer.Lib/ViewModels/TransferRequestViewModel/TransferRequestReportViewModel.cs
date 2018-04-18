using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferRequestViewModel
{
    public class TransferRequestReportViewModel
    {
        public string trNo { get; set; }
        public DateTime trDate { get; set; }
        public DateTime requestedArrivalDate { get; set; }
        public string unitName { get; set; }
        public string divisionName { get; set; }
        public string categoryName { get; set; }
        public bool isPosted { get; set; }
        public bool isCanceled { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public double quantity { get; set; }
        public string uom { get; set; }
        public string grade { get; set; }
        public string status { get; set; }
        public DateTime _updatedDate { get; set; }
    }
}
