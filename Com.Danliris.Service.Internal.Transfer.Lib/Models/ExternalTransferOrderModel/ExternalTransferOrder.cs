﻿using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel
{
    public class ExternalTransferOrder : StandardEntity, IValidatableObject
    {
        public string ETONo { get; set; }
        public string OrderDivisionId { get; set; }
        public string OrderDivisionCode { get; set; }
        public string OrderDivisionName { get; set; }
        public string DeliveryDivisionId { get; set; }
        public string DeliveryDivisionCode { get; set; }
        public string DeliveryDivisionName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyDescription { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyRate { get; set; }
        public string Remark { get; set; }
        public bool IsPosted { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsClosed { get; set; }

        public virtual ICollection<ExternalTransferOrderItem> ExternalTransferOrderItems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}