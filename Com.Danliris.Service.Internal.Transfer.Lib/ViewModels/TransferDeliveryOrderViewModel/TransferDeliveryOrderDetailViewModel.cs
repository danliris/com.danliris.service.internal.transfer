﻿using Com.Danliris.Service.Internal.Transfer.Lib.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferDeliveryOrderViewModel
{
    public class TransferDeliveryOrderDetailViewModel : BasicViewModel, IValidatableObject
    {
        public int DOItemId { get; set; }
        public string ETODetailId { get; set; }
        public string ITODetailId { get; set; }
        public string TRDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Grade { get; set; }
        public string ProductRemark { get; set; }
        public int RequestedQuantity { get; set; }
        public string UomId { get; set; }
        public string UomUnit { get; set; }
        public int ReceivedQuantity { get; set; }
        public int UnitReceivedQuantity { get; set; }
        public int RemainingQuantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}