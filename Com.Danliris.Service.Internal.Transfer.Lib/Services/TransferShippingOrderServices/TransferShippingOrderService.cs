﻿using Com.Danliris.Service.Internal.Transfer.Lib.Helpers;
using Com.Danliris.Service.Internal.Transfer.Lib.Interfaces;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferShippingOrderModel;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferShippingOrderViewModels;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferDeliveryOrderModel;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Services.TransferShippingOrderServices
{
    public class TransferShippingOrderService : BasicService<InternalTransferDbContext, TransferShippingOrder>, IMap<TransferShippingOrder, TransferShippingOrderViewModel>
    {
        public TransferShippingOrderService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public TransferShippingOrderViewModel MapToViewModel(TransferShippingOrder model)
        {
            TransferShippingOrderViewModel viewModel = new TransferShippingOrderViewModel();
            PropertyCopier<TransferShippingOrder, TransferShippingOrderViewModel>.Copy(model, viewModel);

            viewModel.Supplier = new SupplierViewModel()
            {
                _id = model.SupplierId,
                code = model.SupplierCode,
                name = model.SupplierName
            };

            viewModel.TransferShippingOrderItems = new List<TransferShippingOrderItemViewModel>();
            if (model.TransferShippingOrderItems != null)
            {
                foreach (TransferShippingOrderItem shippingOrderItem in model.TransferShippingOrderItems)
                {
                    TransferShippingOrderItemViewModel shippingOrderItemViewModel = new TransferShippingOrderItemViewModel();
                    PropertyCopier<TransferShippingOrderItem, TransferShippingOrderItemViewModel>.Copy(shippingOrderItem, shippingOrderItemViewModel);

                    shippingOrderItemViewModel.TransferShippingOrderDetails = new List<TransferShippingOrderDetailViewModel>();
                    if (shippingOrderItem.TransferShippingOrderDetails != null)
                    {
                        foreach (TransferShippingOrderDetail shippingOrderDetail in shippingOrderItem.TransferShippingOrderDetails)
                        {
                            TransferShippingOrderDetailViewModel shippingOrderDetailViewModel = new TransferShippingOrderDetailViewModel();
                            PropertyCopier<TransferShippingOrderDetail, TransferShippingOrderDetailViewModel>.Copy(shippingOrderDetail, shippingOrderDetailViewModel);

                            TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.SingleOrDefault(d => d.Id == shippingOrderDetailViewModel.TRDetailId);
                            if (transferRequestDetail != null)
                            {
                                int TRId = transferRequestDetail.TransferRequestId;
                                shippingOrderDetailViewModel.TRNo = this.DbContext.TransferRequests.SingleOrDefault(d => d.Id == TRId).TRNo;
                            }

                            shippingOrderDetailViewModel.Product = new ProductViewModel
                            {
                                _id = shippingOrderDetail.ProductId,
                                code = shippingOrderDetail.ProductCode,
                                name = shippingOrderDetail.ProductName
                            };
                            shippingOrderDetailViewModel.Uom = new UomViewModel
                            {
                                _id = shippingOrderDetail.UomId,
                                unit = shippingOrderDetail.UomUnit
                            };

                            shippingOrderItemViewModel.TransferShippingOrderDetails.Add(shippingOrderDetailViewModel);
                        }
                    }

                    viewModel.TransferShippingOrderItems.Add(shippingOrderItemViewModel);
                }
            }

            return viewModel;
        }

        public TransferShippingOrder MapToModel(TransferShippingOrderViewModel viewModel)
        {
            TransferShippingOrder model = new TransferShippingOrder();
            PropertyCopier<TransferShippingOrderViewModel, TransferShippingOrder>.Copy(viewModel, model);

            model.SupplierId = viewModel.Supplier._id;
            model.SupplierCode = viewModel.Supplier.code;
            model.SupplierName = viewModel.Supplier.name;

            model.TransferShippingOrderItems = new List<TransferShippingOrderItem>();
            foreach (TransferShippingOrderItemViewModel shippingOrderItemViewModel in viewModel.TransferShippingOrderItems)
            {
                TransferShippingOrderItem shippingOrderItem = new TransferShippingOrderItem();
                PropertyCopier<TransferShippingOrderItemViewModel, TransferShippingOrderItem>.Copy(shippingOrderItemViewModel, shippingOrderItem);

                shippingOrderItem.TransferShippingOrderDetails = new List<TransferShippingOrderDetail>();
                foreach (TransferShippingOrderDetailViewModel shippingOrderDetailViewModel in shippingOrderItemViewModel.TransferShippingOrderDetails)
                {
                    TransferShippingOrderDetail shippingOrderDetail = new TransferShippingOrderDetail();
                    PropertyCopier<TransferShippingOrderDetailViewModel, TransferShippingOrderDetail>.Copy(shippingOrderDetailViewModel, shippingOrderDetail);

                    shippingOrderDetail.ProductId = shippingOrderDetailViewModel.Product._id;
                    shippingOrderDetail.ProductCode = shippingOrderDetailViewModel.Product.code;
                    shippingOrderDetail.ProductName = shippingOrderDetailViewModel.Product.name;
                    shippingOrderDetail.UomId = shippingOrderDetailViewModel.Uom._id;
                    shippingOrderDetail.UomUnit = shippingOrderDetailViewModel.Uom.unit;

                    shippingOrderItem.TransferShippingOrderDetails.Add(shippingOrderDetail);
                }

                model.TransferShippingOrderItems.Add(shippingOrderItem);
            }

            return model;
        }

        public override async Task<TransferShippingOrder> ReadModelById(int Id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(Id) && d._IsDeleted.Equals(false))
                .Include(d => d.TransferShippingOrderItems)
                    .ThenInclude(d => d.TransferShippingOrderDetails)
                .FirstOrDefaultAsync();
        }


        public override Tuple<List<TransferShippingOrder>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<TransferShippingOrder> Query = this.DbContext.TransferShippingOrders;

            List<string> SearchAttributes = new List<string>()
                {
                    // Model
                    "SONo", "SupplierName", "TransferShippingOrderItems.DONo"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
                {
                    // ViewModel
                    "Id", "SONo", "SODate", "Supplier", "TransferShippingOrderItems", "IsPosted"
                };
            Query = Query
                .Select(result => new TransferShippingOrder
                {
                    // Model
                    Id = result.Id,
                    SONo = result.SONo,
                    SODate = result.SODate,
                    SupplierName = result.SupplierName,
                    IsPosted = result.IsPosted,
                    Remark = result.Remark,
                    _LastModifiedUtc = result._LastModifiedUtc,
                    TransferShippingOrderItems = result.TransferShippingOrderItems
                        .Select(
                            p => new TransferShippingOrderItem
                            {
                                Id = p.Id,
                                SOId = p.SOId,
                                TRId = p.TRId,
                                TRNo = p.TRNo,
                                ITOId = p.ITOId,
                                ITONo = p.ITONo,
                                ETOId = p.ETOId,
                                ETONo = p.ETONo,
                                DOId = p.DOId,
                                DONo = p.DONo,
                                TransferShippingOrderDetails = p.TransferShippingOrderDetails
                            }
                        )
                        .Where(
                            i => i.SOId.Equals(result.Id)
                        )
                        .ToList()

                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<TransferShippingOrder> pageable = new Pageable<TransferShippingOrder>(Query, Page - 1, Size);
            List<TransferShippingOrder> Data = pageable.Data.ToList<TransferShippingOrder>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        async Task<string> CustomCodeGenerator(TransferShippingOrder model)
        {
            DateTime Now = DateTime.Now;
            string Year = Now.ToString("yy");
            string Month = Now.ToString("MM");

            string SONo = "SO" + model.SupplierCode + Year + Month;

            var lastSONo = await this.DbSet.Where(w => w.SONo.StartsWith(SONo)).OrderByDescending(o => o.SONo).FirstOrDefaultAsync();

            int Padding = 3;

            if (lastSONo == null)
            {
                return SONo + "1".PadLeft(Padding, '0');
            }
            else
            {
                int lastNo = Int32.Parse(lastSONo.SONo.Replace(SONo, "")) + 1;
                return SONo + lastNo.ToString().PadLeft(Padding, '0');
            }
        }

        public override async Task<int> CreateModel(TransferShippingOrder Model)
        {
            int Created = 0;
            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    Model.SONo = await this.CustomCodeGenerator(Model);
                    Created = await this.CreateAsync(Model);

                    foreach (var item in Model.TransferShippingOrderItems)
                    {
                        foreach (var detail in item.TransferShippingOrderDetails)
                        {
                            TransferDeliveryOrderDetail transferDeliveryOrderDetail = this.DbContext.TransferDeliveryOrderDetails.FirstOrDefault(s => s.Id == detail.DODetailId);
                            transferDeliveryOrderDetail.ShippingOrderQuantity += (int)detail.DeliveryQuantity;
                            transferDeliveryOrderDetail.RemainingQuantity -= (int)detail.DeliveryQuantity;

                            TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.FirstOrDefault(s => s.Id == detail.TRDetailId);
                            transferRequestDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                            transferRequestDetail._LastModifiedUtc = DateTime.UtcNow;
                            transferRequestDetail._LastModifiedAgent = "Service";
                            transferRequestDetail._LastModifiedBy = this.Username;

                            InternalTransferOrderDetail internalTransferOrderDetail = this.DbContext.InternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ITODetailId);
                            internalTransferOrderDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                            internalTransferOrderDetail._LastModifiedUtc = DateTime.UtcNow;
                            internalTransferOrderDetail._LastModifiedAgent = "Service";
                            internalTransferOrderDetail._LastModifiedBy = this.Username;
                        }
                    }
                    this.DbContext.SaveChanges();

                    Transaction.Commit();
                }
                catch (ServiceValidationExeption e)
                {
                    throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
            }

            return Created;
        }

        public override void OnCreating(TransferShippingOrder model)
        {
            base.OnCreating(model);

            model._CreatedAgent = "Service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;

            TransferShippingOrderItemService shippingOrderItemService = ServiceProvider.GetService<TransferShippingOrderItemService>();
            shippingOrderItemService.Username = this.Username;

            foreach (TransferShippingOrderItem shippingOrderItem in model.TransferShippingOrderItems)
            {
                shippingOrderItemService.OnCreating(shippingOrderItem);
            }
        }

        public override async Task<int> UpdateModel(int Id, TransferShippingOrder Model)
        {
            int Updated = 0;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    TransferShippingOrderItemService shippingOrderItemService = ServiceProvider.GetService<TransferShippingOrderItemService>();
                    shippingOrderItemService.Username = this.Username;
                    TransferShippingOrderDetailService shippingOrderDetailService = ServiceProvider.GetService<TransferShippingOrderDetailService>();
                    shippingOrderDetailService.Username = this.Username;

                    HashSet<int> ShippingOrderItemIds = new HashSet<int>(
                        this.DbContext.TransferShippingOrderItems
                            .Where(p => p.SOId.Equals(Id))
                            .Select(p => p.Id)
                        );

                    foreach (int itemId in ShippingOrderItemIds)
                    {
                        HashSet<int> ShippingOrderDetailIds = new HashSet<int>(
                            this.DbContext.TransferShippingOrderDetails
                                .Where(p => p.SOItemId.Equals(itemId))
                                .Select(p => p.Id)
                            );

                        TransferShippingOrderItem shippingOrderItemNew = Model.TransferShippingOrderItems.FirstOrDefault(p => p.Id.Equals(itemId));

                        if (shippingOrderItemNew == null)
                        {
                            TransferShippingOrderItem item = this.DbContext.TransferShippingOrderItems
                                .Include(d => d.TransferShippingOrderDetails)
                                .FirstOrDefault(p => p.Id.Equals(itemId));

                            if (item != null)
                            {
                                foreach (var detail in item.TransferShippingOrderDetails)
                                {
                                    TransferDeliveryOrderDetail transferDeliveryOrderDetail = this.DbContext.TransferDeliveryOrderDetails.FirstOrDefault(s => s.Id == detail.DODetailId);
                                    transferDeliveryOrderDetail.ShippingOrderQuantity -= (int)detail.DeliveryQuantity;
                                    transferDeliveryOrderDetail.RemainingQuantity += (int)detail.DeliveryQuantity;

                                    TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.FirstOrDefault(s => s.Id == detail.TRDetailId);
                                    transferRequestDetail._LastModifiedUtc = DateTime.UtcNow;
                                    transferRequestDetail._LastModifiedAgent = "Service";
                                    transferRequestDetail._LastModifiedBy = this.Username;

                                    InternalTransferOrderDetail internalTransferOrderDetail = this.DbContext.InternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ITODetailId);
                                    internalTransferOrderDetail._LastModifiedUtc = DateTime.UtcNow;
                                    internalTransferOrderDetail._LastModifiedAgent = "Service";
                                    internalTransferOrderDetail._LastModifiedBy = this.Username;

                                    if (transferDeliveryOrderDetail.RemainingQuantity == transferDeliveryOrderDetail.DOQuantity)
                                    {
                                        ExternalTransferOrderDetail externalTransferOrderDetail = this.DbContext.ExternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ETODetailId);

                                        transferRequestDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                        internalTransferOrderDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                    }
                                    else
                                    {
                                        transferRequestDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                        internalTransferOrderDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                    }
                                }
                            }

                            foreach (int detailId in ShippingOrderDetailIds)
                            {
                                await shippingOrderDetailService.DeleteModel(detailId);
                            }

                            await shippingOrderItemService.DeleteModel(itemId);
                        }
                        else
                        {
                            await shippingOrderItemService.UpdateModel(itemId, shippingOrderItemNew);

                            foreach (int detailId in ShippingOrderDetailIds)
                            {
                                TransferShippingOrderDetail shippingOrderDetailNew = shippingOrderItemNew.TransferShippingOrderDetails.FirstOrDefault(p => p.Id.Equals(detailId));

                                if (shippingOrderDetailNew == null)
                                {
                                    await shippingOrderDetailService.DeleteModel(detailId);
                                }
                                else
                                {
                                    await shippingOrderDetailService.UpdateModel(detailId, shippingOrderDetailNew);

                                    TransferShippingOrderDetail shippingOrderDetailOld = this.DbContext.TransferShippingOrderDetails.FirstOrDefault(p => p.Id.Equals(detailId));

                                    TransferDeliveryOrderDetail transferDeliveryOrderDetail = this.DbContext.TransferDeliveryOrderDetails.FirstOrDefault(s => s.Id == shippingOrderDetailNew.DODetailId);
                                    transferDeliveryOrderDetail.ShippingOrderQuantity = transferDeliveryOrderDetail.ShippingOrderQuantity - (int)shippingOrderDetailOld.DeliveryQuantity + (int)shippingOrderDetailNew.DeliveryQuantity;
                                    transferDeliveryOrderDetail.RemainingQuantity = transferDeliveryOrderDetail.RemainingQuantity + (int)shippingOrderDetailOld.DeliveryQuantity - (int)shippingOrderDetailNew.DeliveryQuantity;

                                    TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.FirstOrDefault(s => s.Id == shippingOrderDetailNew.TRDetailId);
                                    transferRequestDetail._LastModifiedUtc = DateTime.UtcNow;
                                    transferRequestDetail._LastModifiedAgent = "Service";
                                    transferRequestDetail._LastModifiedBy = this.Username;

                                    InternalTransferOrderDetail internalTransferOrderDetail = this.DbContext.InternalTransferOrderDetails.FirstOrDefault(s => s.Id == shippingOrderDetailNew.ITODetailId);
                                    internalTransferOrderDetail._LastModifiedUtc = DateTime.UtcNow;
                                    internalTransferOrderDetail._LastModifiedAgent = "Service";
                                    internalTransferOrderDetail._LastModifiedBy = this.Username;

                                    if (transferDeliveryOrderDetail.RemainingQuantity == transferDeliveryOrderDetail.DOQuantity)
                                    {
                                        ExternalTransferOrderDetail externalTransferOrderDetail = this.DbContext.ExternalTransferOrderDetails.FirstOrDefault(s => s.Id == shippingOrderDetailNew.ETODetailId);

                                        transferRequestDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                        internalTransferOrderDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                    }
                                    else
                                    {
                                        transferRequestDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                        internalTransferOrderDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                    }
                                }
                            }
                        }
                    }

                    Updated = await this.UpdateAsync(Id, Model);

                    foreach (TransferShippingOrderItem item in Model.TransferShippingOrderItems)
                    {
                        if (item.Id == 0)
                        {
                            await shippingOrderItemService.CreateModel(item);

                            foreach (var detail in item.TransferShippingOrderDetails)
                            {
                                TransferDeliveryOrderDetail transferDeliveryOrderDetail = this.DbContext.TransferDeliveryOrderDetails.FirstOrDefault(s => s.Id == detail.DODetailId);
                                transferDeliveryOrderDetail.ShippingOrderQuantity += (int)detail.DeliveryQuantity;
                                transferDeliveryOrderDetail.RemainingQuantity -= (int)detail.DeliveryQuantity;

                                TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.FirstOrDefault(s => s.Id == detail.TRDetailId);
                                transferRequestDetail._LastModifiedUtc = DateTime.UtcNow;
                                transferRequestDetail._LastModifiedAgent = "Service";
                                transferRequestDetail._LastModifiedBy = this.Username;

                                InternalTransferOrderDetail internalTransferOrderDetail = this.DbContext.InternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ITODetailId);
                                internalTransferOrderDetail._LastModifiedUtc = DateTime.UtcNow;
                                internalTransferOrderDetail._LastModifiedAgent = "Service";
                                internalTransferOrderDetail._LastModifiedBy = this.Username;

                                if (transferDeliveryOrderDetail.RemainingQuantity == transferDeliveryOrderDetail.DOQuantity)
                                {
                                    ExternalTransferOrderDetail externalTransferOrderDetail = this.DbContext.ExternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ETODetailId);

                                    transferRequestDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                    internalTransferOrderDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                }
                                else
                                {
                                    transferRequestDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                    internalTransferOrderDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                }
                            }
                        }
                    }

                    this.DbContext.SaveChanges();

                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
            }

            return Updated;
        }

        public override void OnUpdating(int id, TransferShippingOrder model)
        {
            base.OnUpdating(id, model);

            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            int Deleted = 0;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    TransferShippingOrderItemService transferShippingOrderItemService = ServiceProvider.GetService<TransferShippingOrderItemService>();
                    transferShippingOrderItemService.Username = this.Username;
                    TransferShippingOrderDetailService transferShippingOrderDetailService = ServiceProvider.GetService<TransferShippingOrderDetailService>();
                    transferShippingOrderDetailService.Username = this.Username;

                    HashSet<int> TransferShippingOrderItemIds = new HashSet<int>(
                        this.DbContext.TransferShippingOrderItems
                            .Where(p => p.SOId.Equals(Id))
                            .Select(p => p.Id)
                        );

                    foreach (int itemId in TransferShippingOrderItemIds)
                    {
                        HashSet<int> TransferShippingOrderDetailIds = new HashSet<int>(
                            this.DbContext.TransferShippingOrderDetails
                                .Where(p => p.SOItemId.Equals(itemId))
                                .Select(p => p.Id)
                            );

                        foreach (int detailId in TransferShippingOrderDetailIds)
                        {
                            await transferShippingOrderDetailService.DeleteModel(detailId);

                            TransferShippingOrderItem item = this.DbContext.TransferShippingOrderItems
                                .Include(d => d.TransferShippingOrderDetails)
                                .FirstOrDefault(p => p.Id.Equals(itemId));

                            if (item != null)
                            {
                                foreach (var detail in item.TransferShippingOrderDetails)
                                {
                                    TransferDeliveryOrderDetail transferDeliveryOrderDetail = this.DbContext.TransferDeliveryOrderDetails.FirstOrDefault(s => s.Id == detail.DODetailId);
                                    transferDeliveryOrderDetail.ShippingOrderQuantity -= (int)detail.DeliveryQuantity;
                                    transferDeliveryOrderDetail.RemainingQuantity += (int)detail.DeliveryQuantity;

                                    TransferRequestDetail transferRequestDetail = this.DbContext.TransferRequestDetails.FirstOrDefault(s => s.Id == detail.TRDetailId);
                                    transferRequestDetail._LastModifiedUtc = DateTime.UtcNow;
                                    transferRequestDetail._LastModifiedAgent = "Service";
                                    transferRequestDetail._LastModifiedBy = this.Username;

                                    InternalTransferOrderDetail internalTransferOrderDetail = this.DbContext.InternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ITODetailId);
                                    internalTransferOrderDetail._LastModifiedUtc = DateTime.UtcNow;
                                    internalTransferOrderDetail._LastModifiedAgent = "Service";
                                    internalTransferOrderDetail._LastModifiedBy = this.Username;

                                    if (transferDeliveryOrderDetail.RemainingQuantity == transferDeliveryOrderDetail.DOQuantity)
                                    {
                                        ExternalTransferOrderDetail externalTransferOrderDetail = this.DbContext.ExternalTransferOrderDetails.FirstOrDefault(s => s.Id == detail.ETODetailId);

                                        transferRequestDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                        internalTransferOrderDetail.Status = externalTransferOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian ke Unit Pengirim" : "Barang Sudah dikirim semua ke Unit Pengirim";
                                    }
                                    else
                                    {
                                        transferRequestDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                        internalTransferOrderDetail.Status = transferDeliveryOrderDetail.RemainingQuantity > 0 ? "Barang Sudah dikirim sebagian" : "Barang Sudah dikirim semua";
                                    }
                                }
                            }
                        }

                        await transferShippingOrderItemService.DeleteModel(itemId);
                    }

                    Deleted = await this.DeleteAsync(Id);

                    this.DbContext.SaveChanges();
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
            }

            return Deleted;
        }

        public override void OnDeleting(TransferShippingOrder model)
        {
            base.OnDeleting(model);

            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
            model._DeletedAgent = "Service";
            model._DeletedBy = this.Username;
        }

        public bool SOPost(List<int> Ids)
        {
            bool IsSuccessful = false;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var listData = this.DbSet
                        .Where(m => Ids.Contains(m.Id))
                        .Include(d => d.TransferShippingOrderItems)
                            .ThenInclude(d => d.TransferShippingOrderDetails)
                        .ToList();
                    listData.ForEach(data => {
                        data.IsPosted = true;
                        data._LastModifiedUtc = DateTime.UtcNow;
                        data._LastModifiedAgent = "Service";
                        data._LastModifiedBy = this.Username;

                        foreach (var item in data.TransferShippingOrderItems)
                        {
                            item._LastModifiedUtc = DateTime.UtcNow;
                            item._LastModifiedAgent = "Service";
                            item._LastModifiedBy = this.Username;
                            foreach (var detail in item.TransferShippingOrderDetails)
                            {
                                detail._LastModifiedUtc = DateTime.UtcNow;
                                detail._LastModifiedAgent = "Service";
                                detail._LastModifiedBy = this.Username;
                            }
                        }
                    });

                    this.DbContext.SaveChanges();

                    IsSuccessful = true;
                    Transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Transaction.Rollback();
                    throw;
                }
            }

            return IsSuccessful;
        }

        public bool SOUnpost(int Id)
        {
            bool IsSuccessful = false;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var data = this.DbSet
                        .Include(d => d.TransferShippingOrderItems)
                            .ThenInclude(d => d.TransferShippingOrderDetails)
                        .FirstOrDefault(so => so.Id == Id && so._IsDeleted == false);
                    data.IsPosted = false;
                    data._LastModifiedUtc = DateTime.UtcNow;
                    data._LastModifiedAgent = "Service";
                    data._LastModifiedBy = this.Username;

                    foreach (var item in data.TransferShippingOrderItems)
                    {
                        item._LastModifiedUtc = DateTime.UtcNow;
                        item._LastModifiedAgent = "Service";
                        item._LastModifiedBy = this.Username;

                        foreach (var detail in item.TransferShippingOrderDetails)
                        {
                            detail._LastModifiedUtc = DateTime.UtcNow;
                            detail._LastModifiedAgent = "Service";
                            detail._LastModifiedBy = this.Username;
                        }
                    }

                    this.DbContext.SaveChanges();

                    IsSuccessful = true;
                    Transaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Transaction.Rollback();
                    throw;
                }
            }

            return IsSuccessful;
        }

        public bool CheckIdIsUsedByUnitReceiptNotes(int Id)
        {
            //HashSet<int> TransferDeliveryOrderItemIds = new HashSet<int>(this.DbContext.UnitReceiptNotesService.Select(p => p.SOId));
            HashSet<int> TransferDeliveryOrderItemIds = new HashSet<int>();

            return TransferDeliveryOrderItemIds.Contains(Id);
        }

    }
}
