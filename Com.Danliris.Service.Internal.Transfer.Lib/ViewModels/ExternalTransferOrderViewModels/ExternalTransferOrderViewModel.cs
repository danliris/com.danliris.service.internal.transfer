﻿using Com.Danliris.Service.Internal.Transfer.Lib.Helpers;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel;
using Com.Danliris.Service.Internal.Transfer.Lib.Services.ExternalTransferOrderServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.ExternalTransferOrderViewModels
{
    public class ExternalTransferOrderViewModel : BasicViewModel, IValidatableObject
    {
        public string ETONo { get; set; }
        public DivisionViewModel OrderDivision { get; set; }
        public DivisionViewModel DeliveryDivision { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public string Remark { get; set; }
        public bool IsPosted { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsClosed { get; set; }

        public List<ExternalTransferOrderItemViewModel> ExternalTransferOrderItems { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DeliveryDivision == null || string.IsNullOrWhiteSpace(this.DeliveryDivision._id))
                yield return new ValidationResult("DeliveryDivision is required", new List<string> { "DeliveryDivision" });

            if (this.OrderDivision == null || string.IsNullOrWhiteSpace(this.OrderDivision._id))
                yield return new ValidationResult("OrderDivision is required", new List<string> { "OrderDivision" });

            if (this.OrderDate == null || this.OrderDate == DateTime.MinValue)
                yield return new ValidationResult("OrderDate is required", new List<string> { "OrderDate" });

            if (this.DeliveryDate == null || this.DeliveryDate == DateTime.MinValue)
                yield return new ValidationResult("DeliveryDate is required", new List<string> { "DeliveryDate" });
            else if (this.DeliveryDate < this.OrderDate)
                yield return new ValidationResult("DeliveryDate should not be less than OrderDate", new List<string> { "DeliveryDate" });

            int itemErrorCount = 0;
            int detailErrorCount = 0;

            if (ExternalTransferOrderItems == null || ExternalTransferOrderItems.Count.Equals(0))
            {
                yield return new ValidationResult("External Transfer Order Item is required", new List<string> { "ExternalTransferOrderItemsCount" });
            }
            else
            {
                string externalTransferOrderItemError = "[";

                foreach (ExternalTransferOrderItemViewModel Item in ExternalTransferOrderItems)
                {
                    externalTransferOrderItemError += "{ ";

                    if (string.IsNullOrWhiteSpace(Item.ITONo))
                    {
                        itemErrorCount++;
                        externalTransferOrderItemError += "InternalTransferOrder: 'TransferRequest is required', ";
                    }
                    else
                    {
                        ExternalTransferOrderItemService externalTransferOrderItemService = (ExternalTransferOrderItemService)validationContext.GetService(typeof(ExternalTransferOrderItemService));
                        //List<ExternalTransferOrderItem> itemsData = externalTransferOrderItemService.ReadModel(Filter: "{ InternalTransferOrderNo : '" + Item.InternalTransferOrderNo + "' }").Item1;
                        //itemsData = itemsData.Where(w => w.ExternalTransferOrderId != this.Id).ToList();
                        List<ExternalTransferOrderItem> itemsData = externalTransferOrderItemService.DbSet.Where(
                                w => w.ETOId != this.Id && w.ITONo.Equals(Item.ITONo)
                            )
                            .ToList();
                        if (itemsData.Count > 0)
                        {
                            itemErrorCount++;
                            externalTransferOrderItemError += "InternalTransferOrder: 'TransferRequest is already used', ";
                        }

                    }

                    string externalTransferOrderDetailError = "[";

                    foreach (ExternalTransferOrderDetailViewModel Detail in Item.ExternalTransferOrderDetails)
                    {
                        externalTransferOrderDetailError += "{ ";

                        //if (Detail.DefaultUom.unit.Equals(Detail.DealUom.unit) && Detail.DefaultQuantity == Detail.DealQuantity && Detail.Convertion != 1)
                        if (Detail.DefaultUom.unit.Equals(Detail.DealUom.unit) && Detail.Convertion != 1)
                        {
                            detailErrorCount++;
                            externalTransferOrderDetailError += "Convertion: 'Convertion should be 1', ";
                        }

                        if (Detail.Price <= 0)
                        {
                            detailErrorCount++;
                            externalTransferOrderDetailError += "Price: 'Price should be more than 0', ";
                        }

                        externalTransferOrderDetailError += " }, ";
                    }

                    externalTransferOrderDetailError += "]";

                    if (detailErrorCount > 0)
                    {
                        itemErrorCount++;
                        externalTransferOrderItemError += string.Concat("ExternalTransferOrderDetails: ", externalTransferOrderDetailError);
                    }

                    externalTransferOrderItemError += " }, ";
                }

                externalTransferOrderItemError += "]";

                if (itemErrorCount > 0)
                    yield return new ValidationResult(externalTransferOrderItemError, new List<string> { "ExternalTransferOrderItems" });
            }
        }
    }
}