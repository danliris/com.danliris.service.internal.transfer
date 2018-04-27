﻿using Com.Danliris.Service.Internal.Transfer.Lib.Helpers;
using Com.Danliris.Service.Internal.Transfer.Lib.Interfaces;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferDeliveryOrderModel;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferDeliveryOrderViewModel;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Services.TransferDeliveryOrderService
{
    public class TransferDeliveryOrderService : BasicService<InternalTransferDbContext , TransferDeliveryOrder>, IMap<TransferDeliveryOrder, TransferDeliveryOrderViewModel>
    {
        public TransferDeliveryOrderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public TransferDeliveryOrder MapToModel(TransferDeliveryOrderViewModel viewModel)
        {
            TransferDeliveryOrder model = new TransferDeliveryOrder();

            PropertyCopier<TransferDeliveryOrderViewModel, TransferDeliveryOrder>.Copy(viewModel, model);

            model.DONo = viewModel.DONo;
            model.DOdate = (DateTime)viewModel.DODate;
            model.SupplierId = viewModel.Supplier._id ?? "";
            model.SupplierCode = viewModel.Supplier.code;
            model.SupplierName = viewModel.Supplier.name;
            model.OrderDivisionId = viewModel.Division._id;
            model.OrderDivisionCode = viewModel.Division.code;
            model.OrderDivisionName = viewModel.Division.name;
            model.Remark = viewModel.Remark;
            model.IsPosted = viewModel.IsPosted;

            model.TransferDeliveryOrderItem = new List<TransferDeliveryOrderItem>();

            foreach (TransferDeliveryOrderItemViewModel transferDeliveryOrderItemViewModel in viewModel.items)
            {
                TransferDeliveryOrderItem transferDeliveryOrderItem = new TransferDeliveryOrderItem();

                PropertyCopier<TransferDeliveryOrderItemViewModel, TransferDeliveryOrderItem>.Copy(transferDeliveryOrderItemViewModel, transferDeliveryOrderItem);

                transferDeliveryOrderItem.DOId = transferDeliveryOrderItemViewModel.DOId;
                transferDeliveryOrderItem.ETOId = transferDeliveryOrderItemViewModel.ETOId;
                transferDeliveryOrderItem.ETONo = transferDeliveryOrderItemViewModel.ETONo;
                transferDeliveryOrderItem.TRId = transferDeliveryOrderItemViewModel.TRId;
                transferDeliveryOrderItem.TRNo = transferDeliveryOrderItemViewModel.TRNo;
                transferDeliveryOrderItem.ITOId = transferDeliveryOrderItemViewModel.ITOId;
                transferDeliveryOrderItem.ITONo = transferDeliveryOrderItemViewModel.ITONo;
                transferDeliveryOrderItem.UnitId = transferDeliveryOrderItemViewModel.UnitId;
                transferDeliveryOrderItem.UnitCode = transferDeliveryOrderItemViewModel.UnitCode;
                transferDeliveryOrderItem.UnitName = transferDeliveryOrderItemViewModel.UnitName;

                transferDeliveryOrderItem.transferDeliveryOrderDetail = new List<TransferDeliveryOrderDetail>();
                foreach (TransferDeliveryOrderDetailViewModel transferDeliveryOrderDetailViewModel in transferDeliveryOrderItemViewModel.details)
                {
                    TransferDeliveryOrderDetail transferDeliveryOrderDetail = new TransferDeliveryOrderDetail();
                    PropertyCopier<TransferDeliveryOrderDetailViewModel, TransferDeliveryOrderDetail>.Copy(transferDeliveryOrderDetailViewModel, transferDeliveryOrderDetail);

                    transferDeliveryOrderDetail.DOItemId = transferDeliveryOrderDetailViewModel.DOItemId;
                    transferDeliveryOrderDetail.ETODetailId = transferDeliveryOrderDetailViewModel.ETODetailId;
                    transferDeliveryOrderDetail.ITODetailId = transferDeliveryOrderDetailViewModel.ITODetailId;
                    transferDeliveryOrderDetail.TRDetailId = transferDeliveryOrderDetailViewModel.TRDetailId;
                    transferDeliveryOrderDetail.ProductId = transferDeliveryOrderDetailViewModel.ProductId;
                    transferDeliveryOrderDetail.ProductCode = transferDeliveryOrderDetailViewModel.ProductCode;
                    transferDeliveryOrderDetail.ProductName = transferDeliveryOrderDetailViewModel.ProductName;
                    transferDeliveryOrderDetail.Grade = transferDeliveryOrderDetailViewModel.Grade;
                    transferDeliveryOrderDetail.ProductRemark = transferDeliveryOrderDetailViewModel.ProductRemark;
                    transferDeliveryOrderDetail.RequestedQuantity = transferDeliveryOrderDetailViewModel.RequestedQuantity;
                    transferDeliveryOrderDetail.UomId = transferDeliveryOrderDetailViewModel.UomId;
                    transferDeliveryOrderDetail.UomUnit = transferDeliveryOrderDetailViewModel.UomUnit;
                    transferDeliveryOrderDetail.DOQuantity = transferDeliveryOrderDetailViewModel.ReceivedQuantity;
                    transferDeliveryOrderDetail.ShippingOrderQuantity = transferDeliveryOrderDetailViewModel.UnitReceivedQuantity;
                    transferDeliveryOrderDetail.RemainingQuantity = transferDeliveryOrderDetailViewModel.RemainingQuantity;

                    transferDeliveryOrderItem.transferDeliveryOrderDetail.Add(transferDeliveryOrderDetail);
                }
                model.TransferDeliveryOrderItem.Add(transferDeliveryOrderItem);
            }

            return model;
        }

        public TransferDeliveryOrderViewModel MapToViewModel(TransferDeliveryOrder model)
        {
            TransferDeliveryOrderViewModel viewModel = new TransferDeliveryOrderViewModel();
            PropertyCopier<TransferDeliveryOrder, TransferDeliveryOrderViewModel>.Copy(model, viewModel);
            viewModel.Supplier = new SupplierViewModel()
            {
                _id = model.SupplierId,
                code = model.SupplierCode,
                name = model.SupplierName
            };
            viewModel.Division = new DivisionViewModel()
            {
                _id = model.OrderDivisionId,
                code = model.OrderDivisionCode,
                name = model.OrderDivisionName
            };
            
            viewModel.DODate = model.DOdate;
            viewModel.DONo = model.DONo;
            viewModel.Remark = model.Remark;
            viewModel.IsPosted = model.IsPosted;

            viewModel.items = new List<TransferDeliveryOrderItemViewModel>();
            if (model.TransferDeliveryOrderItem != null)
            {
                foreach (TransferDeliveryOrderItem transferDeliveryOrderItem in model.TransferDeliveryOrderItem)
                {
                    TransferDeliveryOrderItemViewModel transferDeliveryOrderItemViewModel = new TransferDeliveryOrderItemViewModel();
                    PropertyCopier<TransferDeliveryOrderItem, TransferDeliveryOrderItemViewModel>.Copy(transferDeliveryOrderItem, transferDeliveryOrderItemViewModel);

                    transferDeliveryOrderItemViewModel.DOId = transferDeliveryOrderItem.DOId;
                    transferDeliveryOrderItemViewModel.ETOId = transferDeliveryOrderItem.ETOId;
                    transferDeliveryOrderItemViewModel.ETONo = transferDeliveryOrderItem.ETONo;
                    transferDeliveryOrderItemViewModel.TRId = transferDeliveryOrderItem.TRId;
                    transferDeliveryOrderItemViewModel.TRNo = transferDeliveryOrderItem.TRNo;
                    transferDeliveryOrderItemViewModel.ITOId = transferDeliveryOrderItem.ITOId;
                    transferDeliveryOrderItemViewModel.ITONo = transferDeliveryOrderItem.ITONo;
                    transferDeliveryOrderItemViewModel.UnitId = transferDeliveryOrderItem.UnitId;
                    transferDeliveryOrderItemViewModel.UnitCode = transferDeliveryOrderItem.UnitCode;
                    transferDeliveryOrderItemViewModel.UnitName = transferDeliveryOrderItem.UnitName;

                    transferDeliveryOrderItemViewModel.details = new List<TransferDeliveryOrderDetailViewModel>();
                    if (transferDeliveryOrderItem.transferDeliveryOrderDetail != null)
                    {
                        foreach (TransferDeliveryOrderDetail transferDeliveryOrderDetail in transferDeliveryOrderItem.transferDeliveryOrderDetail)
                        {
                            TransferDeliveryOrderDetailViewModel transferDeliveryOrderDetailViewModel = new TransferDeliveryOrderDetailViewModel();
                            PropertyCopier<TransferDeliveryOrderDetail, TransferDeliveryOrderDetailViewModel>.Copy(transferDeliveryOrderDetail, transferDeliveryOrderDetailViewModel);

                            transferDeliveryOrderDetailViewModel.DOItemId = transferDeliveryOrderDetail.DOItemId;
                            transferDeliveryOrderDetailViewModel.ETODetailId = transferDeliveryOrderDetail.ETODetailId;
                            transferDeliveryOrderDetailViewModel.ITODetailId = transferDeliveryOrderDetail.ITODetailId;
                            transferDeliveryOrderDetailViewModel.TRDetailId = transferDeliveryOrderDetail.TRDetailId;
                            transferDeliveryOrderDetailViewModel.ProductId = transferDeliveryOrderDetail.ProductId;
                            transferDeliveryOrderDetailViewModel.ProductCode = transferDeliveryOrderDetail.ProductCode;
                            transferDeliveryOrderDetailViewModel.ProductName = transferDeliveryOrderDetail.ProductName;
                            transferDeliveryOrderDetailViewModel.Grade = transferDeliveryOrderDetail.Grade;
                            transferDeliveryOrderDetailViewModel.ProductRemark = transferDeliveryOrderDetail.ProductRemark;
                            transferDeliveryOrderDetailViewModel.RequestedQuantity = transferDeliveryOrderDetail.RequestedQuantity;
                            transferDeliveryOrderDetailViewModel.UomId = transferDeliveryOrderDetail.UomId;
                            transferDeliveryOrderDetailViewModel.UomUnit = transferDeliveryOrderDetail.UomUnit;
                            transferDeliveryOrderDetailViewModel.ReceivedQuantity = transferDeliveryOrderDetail.DOQuantity;
                            transferDeliveryOrderDetailViewModel.UnitReceivedQuantity = transferDeliveryOrderDetail.ShippingOrderQuantity;
                            transferDeliveryOrderDetailViewModel.RemainingQuantity = transferDeliveryOrderDetail.RemainingQuantity;

                            transferDeliveryOrderItemViewModel.details.Add(transferDeliveryOrderDetailViewModel);
                        }
                    }
                    viewModel.items.Add(transferDeliveryOrderItemViewModel);
                }
            }

            return viewModel;
        }

        public override Tuple<List<TransferDeliveryOrder>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<TransferDeliveryOrder> Query = this.DbContext.TransferDeliveryOrders;

            List<string> SearchAttributes = new List<string>()
            {
                "DONo"
            };

            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "Id", "DONo", "DOdate", "Supplier", "items"
            };

            Query = Query
                .Select(o => new TransferDeliveryOrder
                {
                    Id = o.Id,
                    DONo = o.DONo,
                    DOdate = o.DOdate,
                    SupplierId = o.SupplierId,
                    SupplierCode = o.SupplierCode,
                    SupplierName = o.SupplierName,
                    TransferDeliveryOrderItem = o.TransferDeliveryOrderItem
                        .Select(
                            p => new TransferDeliveryOrderItem
                            {
                                Id = p.Id,
                                DOId = p.DOId,
                                ETONo = p.ETONo
                            }
                        )
                        .Where(
                            i => i.DOId.Equals(o.Id)
                        )
                        .ToList()
                });

            /* Order */
            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            /* Pagination */
            Pageable<TransferDeliveryOrder> pageable = new Pageable<TransferDeliveryOrder>(Query, Page - 1, Size);
            List<TransferDeliveryOrder> Data = pageable.Data.ToList<TransferDeliveryOrder>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override async Task<TransferDeliveryOrder> ReadModelById(int Id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(Id) && d._IsDeleted.Equals(false))
                .Include(d => d.TransferDeliveryOrderItem)
                    .ThenInclude(d => d.transferDeliveryOrderDetail)
                .FirstOrDefaultAsync();
        }

        public override async Task<int> CreateModel(TransferDeliveryOrder Model)
        {
            int Created = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    Model.DONo = await this.GenerateTransferDeliveryOrderNo(Model);
                    Created = await this.CreateAsync(Model);


                    transaction.Commit();
                }
                catch (ServiceValidationExeption e)
                {
                    throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
            return Created;
        }

        public override void OnCreating(TransferDeliveryOrder model)
        {
            base.OnCreating(model);

            model._CreatedAgent = "Service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;

            TransferDeliveryOrderItemService transferDeliveryOrderItemService = ServiceProvider.GetService<TransferDeliveryOrderItemService>();
            transferDeliveryOrderItemService.Username = this.Username;

            foreach (TransferDeliveryOrderItem transferDeliveryOrderItem in model.TransferDeliveryOrderItem)
            {
                transferDeliveryOrderItemService.OnCreating(transferDeliveryOrderItem);
            }
        }

        async Task<string> GenerateTransferDeliveryOrderNo(TransferDeliveryOrder model)
        {
            DateTime Now = DateTime.Now;
            string Year = Now.ToString("yy");
            string Month = Now.ToString("mm");

            string transferDeliveryOrderNo = "DO" + model.SupplierCode + Year + Month;

            var lastTransferDeliveryOrderNo = await this.DbSet.Where(w => w.DONo.StartsWith(transferDeliveryOrderNo)).OrderByDescending(o => o.DONo).FirstOrDefaultAsync();

            if (lastTransferDeliveryOrderNo == null)
            {
                return transferDeliveryOrderNo + "001";
            }
            else
            {
                int lastNo = Int32.Parse(lastTransferDeliveryOrderNo.DONo.Replace(transferDeliveryOrderNo, "")) + 1;
                return transferDeliveryOrderNo + lastNo.ToString().PadLeft(5, '0');
            }
        }



        public override void OnUpdating(int id, TransferDeliveryOrder model)
        {
            base.OnUpdating(id, model);
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override void OnDeleting(TransferDeliveryOrder model)
        {
            base.OnDeleting(model);
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        

        public class Keys
        {
        }

        // public async Task<TransferDeliveryOrder> ReadModelByQuery(string Supplier)
        //{
        //    TransferDeliveryOrder result = new TransferDeliveryOrder();
        //    result = await this.DbSet.Where(TransferDeliveryOrder => String.Equals(TransferDeliveryOrder.DONo, Supplier) && !TransferDeliveryOrder._IsDeleted).OrderByDescending(x => x._LastModifiedUtc).FirstOrDefaultAsync();
        //
        //    if (result == null)
        //    {
        // result = new TransferDeliveryOrder();
        //  return result;
        //}
        //  return result;
        //}
    }
}
