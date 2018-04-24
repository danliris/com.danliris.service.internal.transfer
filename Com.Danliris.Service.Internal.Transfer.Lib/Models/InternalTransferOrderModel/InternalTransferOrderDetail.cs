﻿using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel
{
    public class InternalTransferOrderDetail : StandardEntity, IValidatableObject
    {
        
        
        public string ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public double Quantity { get; set; }

        public int ITOId { get; set; }

        public int TRDetailId { get; set; }

        public string UomId { get; set; }

        public string UomUnit { get; set; }

        public string ProductRemark { get; set; }

        public string Grade { get; set; }
        public string Status { get; set; }

        public virtual InternalTransferOrder InternalTransferOrder { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
