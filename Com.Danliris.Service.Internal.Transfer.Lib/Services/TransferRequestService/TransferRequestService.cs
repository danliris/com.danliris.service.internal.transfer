using Com.Danliris.Service.Internal.Transfer.Lib.Helpers;
using Com.Danliris.Service.Internal.Transfer.Lib.Interfaces;
using Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels;
using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferRequestViewModel;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
<<<<<<< HEAD
using System.IO;
using System.Data;
=======
using Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel;
>>>>>>> upstream/dev

namespace Com.Danliris.Service.Internal.Transfer.Lib.Services.TransferRequestService
{
    public class TransferRequestService : BasicService<InternalTransferDbContext, TransferRequest>, IMap<TransferRequest, TransferRequestViewModel>
    {
        public TransferRequestService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<TransferRequest>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<TransferRequest> Query = this.DbContext.TransferRequests;

            List<string> SearchAttributes = new List<string>()
            {
                "TRNo","DivisionName","CategoryName","UnitName"
            };

            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "TRNo", "_CreatedUtc", "RequestedArrivalDate", "TRDate", "IsPosted","IsCanceled","Unit","Category"
            };

            Query = Query
                .Select(tr => new TransferRequest
                {
                    Id = tr.Id,
                    TRNo = tr.TRNo,
                    _CreatedUtc = tr._CreatedUtc,
                    DivisionName=tr.DivisionName,
                    TRDate=tr.TRDate,
                    CategoryName = tr.CategoryName,
                    RequestedArrivalDate = tr.RequestedArrivalDate,
                    _LastModifiedUtc = tr._LastModifiedUtc,
                    UnitName= tr.UnitName,
                    IsPosted=tr.IsPosted,
                    IsCanceled=tr.IsCanceled
                });

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<TransferRequest> pageable = new Pageable<TransferRequest>(Query, Page - 1, Size);
            List<TransferRequest> Data = pageable.Data.ToList<TransferRequest>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public  Tuple<List<TransferRequest>, int, Dictionary<string, string>, List<string>> ReadModelPosted(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            IQueryable<TransferRequest> Query = this.DbContext.TransferRequests;

            List<string> SearchAttributes = new List<string>()
            {
                "TRNo"
            };

            
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            List<string> SelectedFields = new List<string>()
            {
                "Id", "trNo","_CreatedUtc", "requestedArrivalDate","remark", "trDate", "IsPosted","IsCanceled","unitId","unitCode","unitName","divisionId","divisionCode","divisionName","categoryId","categoryCode","categoryName","details"
            };

            Query = Query
                .Select(tr => new TransferRequest
                {
                    Id = tr.Id,
                    TRNo = tr.TRNo,
                    _CreatedUtc = tr._CreatedUtc,
                    DivisionId=tr.DivisionId,
                    DivisionCode=tr.DivisionCode,
                    DivisionName = tr.DivisionName,
                    TRDate = tr.TRDate,
                    Remark=tr.Remark,
                    CategoryId=tr.CategoryId,
                    CategoryCode=tr.CategoryCode,
                    CategoryName = tr.CategoryName,
                    RequestedArrivalDate = tr.RequestedArrivalDate,
                    _LastModifiedUtc = tr._LastModifiedUtc,
                    UnitId=tr.UnitId,
                    UnitCode=tr.UnitCode,
                    UnitName = tr.UnitName,
                    IsPosted = tr.IsPosted,
                    IsCanceled = tr.IsCanceled,
                    TransferRequestDetails=tr.TransferRequestDetails
                }).Where(s=>s.IsPosted== true && !(from data in this.DbContext.InternalTransferOrders select data.TRId).Contains(s.Id));

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<TransferRequest> pageable = new Pageable<TransferRequest>(Query, Page - 1, Size);
            List<TransferRequest> Data = pageable.Data.ToList<TransferRequest>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
        public TransferRequest MapToModel(TransferRequestViewModel viewModel)
        {
            TransferRequest model = new TransferRequest();

            PropertyCopier<TransferRequestViewModel, TransferRequest>.Copy(viewModel, model);

            model.TRDate = viewModel.trDate;
            model.RequestedArrivalDate = viewModel.requestedArrivalDate;
            model.UnitId = viewModel.unit._id;
            model.UnitCode = viewModel.unit.code;
            model.UnitName = viewModel.unit.name;
            model.DivisionId = viewModel.unit.divisionId;
            model.DivisionCode = viewModel.unit.divisionCode;
            model.DivisionName = viewModel.unit.divisionName;
            model.CategoryCode = viewModel.category.code;
            model.CategoryId = viewModel.category._id;
            model.CategoryName = viewModel.category.name;
            model.Remark = viewModel.remark;
            model.TRNo = viewModel.trNo;
            model.IsPosted = viewModel.isPosted;
            model.IsCanceled = viewModel.isCanceled;

            model.TransferRequestDetails = new List<TransferRequestDetail>();

            foreach (TransferRequestDetailViewModel transferRequestDetailViewModel in viewModel.details)
            {
                TransferRequestDetail transferRequestDetail = new TransferRequestDetail();

                PropertyCopier<TransferRequestDetailViewModel, TransferRequestDetail>.Copy(transferRequestDetailViewModel, transferRequestDetail);
                
                transferRequestDetail.ProductId = transferRequestDetailViewModel.product._id;
                transferRequestDetail.ProductCode = transferRequestDetailViewModel.product.code;
                transferRequestDetail.ProductName = transferRequestDetailViewModel.product.name;
                transferRequestDetail.Quantity = transferRequestDetailViewModel.quantity;
                transferRequestDetail.UomId = transferRequestDetailViewModel.uom._id;
                transferRequestDetail.UomUnit = transferRequestDetailViewModel.uom.unit;
                transferRequestDetail.ProductRemark = transferRequestDetailViewModel.productRemark;
                transferRequestDetail.Grade = transferRequestDetailViewModel.grade;
                transferRequestDetail.Status = transferRequestDetailViewModel.status;

                model.TransferRequestDetails.Add(transferRequestDetail);
            }

            return model;
        }

        public TransferRequestViewModel MapToViewModel(TransferRequest model)
        {
            TransferRequestViewModel viewModel = new TransferRequestViewModel();
            viewModel.trDate = model.TRDate;
            viewModel.remark = model.Remark;
            viewModel.unitId = model.UnitId;
            viewModel.unitName = model.UnitName;
            viewModel.unitCode = model.UnitCode;
            viewModel.categoryId = model.CategoryId;
            viewModel.categoryCode = model.CategoryCode;
            viewModel.categoryName = model.CategoryName;
            viewModel.divisionId = model.DivisionId;
            viewModel.divisionCode = model.DivisionCode;
            viewModel.divisionName = model.DivisionName;
            viewModel.requestedArrivalDate = model.RequestedArrivalDate;

            PropertyCopier<TransferRequest, TransferRequestViewModel>.Copy(model, viewModel);

            UnitViewModel Unit = new UnitViewModel()
            {
                _id = model.UnitId,
                code = model.UnitCode,
                name = model.UnitName,
                divisionId=model.DivisionId,
                divisionCode=model.DivisionCode,
                divisionName=model.DivisionName
            };

            CategoryViewModel Category = new CategoryViewModel()
            {
                _id = model.CategoryId,
                code = model.CategoryName,
                name = model.CategoryName,
            };

            viewModel.trNo = model.TRNo;
            viewModel.unit = Unit;
            viewModel.category = Category;
            viewModel.remark = model.Remark;
            viewModel.trDate = model.TRDate;
            viewModel.requestedArrivalDate = model.RequestedArrivalDate;
            viewModel.isPosted = model.IsPosted;
            viewModel.isCanceled = model.IsCanceled;

            viewModel.details = new List<TransferRequestDetailViewModel>();
            if (model.TransferRequestDetails != null)
            {
                foreach (TransferRequestDetail transferRequestDetail in model.TransferRequestDetails)
                {
                    TransferRequestDetailViewModel transferRequestDetailViewModel = new TransferRequestDetailViewModel();
                    PropertyCopier<TransferRequestDetail, TransferRequestDetailViewModel>.Copy(transferRequestDetail, transferRequestDetailViewModel);

                    UomViewModel Uom = new UomViewModel()
                    {
                        _id = transferRequestDetail.UomId,
                        unit = transferRequestDetail.UomUnit
                    };
                    transferRequestDetailViewModel.uom = Uom;
                    transferRequestDetailViewModel.quantity = transferRequestDetail.Quantity;
                    transferRequestDetailViewModel.grade = transferRequestDetail.Grade;
                    ProductViewModel Product = new ProductViewModel()
                    {
                        _id = transferRequestDetail.ProductId,
                        code = transferRequestDetail.ProductCode,
                        name = transferRequestDetail.ProductName
                    };
                    transferRequestDetailViewModel.product = Product;
                    transferRequestDetailViewModel.productRemark = transferRequestDetail.ProductRemark;
                    transferRequestDetailViewModel.status = transferRequestDetail.Status;
                    transferRequestDetailViewModel.grade = transferRequestDetail.Grade;

                    viewModel.details.Add(transferRequestDetailViewModel);
                }
            }

            return viewModel;
        }

        async Task<string> CustomCodeGenerator(TransferRequest model)
        {
            DateTime Now = DateTime.Now;
            string Year = Now.ToString("yy");

            string transferRequestNo = "TR" + model.UnitCode + Year;

            var lastTransferRequestNo = await this.DbSet.Where(w => w.TRNo.StartsWith(transferRequestNo)).OrderByDescending(o => o.TRNo).FirstOrDefaultAsync();

            if (lastTransferRequestNo == null)
            {
                return transferRequestNo + "00001";
            }
            else
            {
                int lastNo = Int32.Parse(lastTransferRequestNo.TRNo.Replace(transferRequestNo, "")) + 1;
                return transferRequestNo + lastNo.ToString().PadLeft(5, '0');
            }
        }

        public override async Task<TransferRequest> ReadModelById(int id)
        {
            return await this.DbSet
                .Where(d => d.Id.Equals(id) && d._IsDeleted.Equals(false))
                .Include(d => d.TransferRequestDetails)
                .FirstOrDefaultAsync();
        }

        public override async Task<int> CreateModel(TransferRequest Model)
        {
            int Created = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    Model.TRNo = await this.CustomCodeGenerator(Model);
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
        public override void OnCreating(TransferRequest model)
        {
            if (model.TransferRequestDetails.Count > 0)
            {
                TransferRequestDetailService transferRequestDetailService = this.ServiceProvider.GetService<TransferRequestDetailService>();

                transferRequestDetailService.Username = this.Username;
                foreach (TransferRequestDetail transferRequestDetail in model.TransferRequestDetails)
                {
                    transferRequestDetailService.OnCreating(transferRequestDetail);
                }
            }

            base.OnCreating(model);
            model._CreatedAgent = "Service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override void OnUpdating(int id, TransferRequest model)
        {
            base.OnUpdating(id, model);
            model._LastModifiedAgent = "Service";
            model._LastModifiedBy = this.Username;
        }

        public override async Task<int> UpdateModel(int Id, TransferRequest Model)
        {
            TransferRequestDetailService transferRequestDetailService = this.ServiceProvider.GetService<TransferRequestDetailService>();
            transferRequestDetailService.Username = this.Username;
            

            int Updated = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    HashSet<int> transferRequestDetails = new HashSet<int>(transferRequestDetailService.DbSet
                        .Where(w => w.TransferRequestId.Equals(Id))
                        .Select(s => s.Id));
                    Updated = await this.UpdateAsync(Id, Model);


                    foreach (int transferRequestDetail in transferRequestDetails)
                    {
                        TransferRequestDetail model = Model.TransferRequestDetails.FirstOrDefault(prop => prop.Id.Equals(transferRequestDetail));
                        if (model == null)
                        {
                            await transferRequestDetailService.DeleteModel(transferRequestDetail);
                        }
                        else
                        {
                            await transferRequestDetailService.UpdateModel(transferRequestDetail, model);
                        }
                    }

                    foreach (TransferRequestDetail transferRequestDetail in Model.TransferRequestDetails)
                    {
                        if (transferRequestDetail.Id.Equals(0))
                        {
                            await transferRequestDetailService.CreateModel(transferRequestDetail);
                        }
                    }


                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }

            return Updated;
        }

        public bool TRPost(List<int> Ids)
        {
            bool IsSuccessful = false;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var mdn = this.DbSet.Where(m => Ids.Contains(m.Id)).ToList();
                    mdn.ForEach(m => { m.IsPosted = true; m._LastModifiedUtc = DateTime.UtcNow; m._LastModifiedAgent = "Service"; m._LastModifiedBy = this.Username; });
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

        public bool TRUnpost(int Id)
        {
            bool IsSuccessful = false;

            using (var Transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    var transfer = this.DbSet.FirstOrDefault(tr => tr.Id == Id && tr._IsDeleted==false);
                    transfer.IsPosted = false;
                    transfer._LastModifiedUtc = DateTime.UtcNow;
                    transfer._LastModifiedAgent = "Service";
                    transfer._LastModifiedBy = this.Username;
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

        public override void OnDeleting(TransferRequest model)
        {
            base.OnDeleting(model);
            model._DeletedAgent = "Service";
            model._DeletedBy = this.Username;
        }

        public override async Task<int> DeleteModel(int Id)
        {
            TransferRequestDetailService transferRequestDetailService = this.ServiceProvider.GetService<TransferRequestDetailService>();

            int Deleted = 0;
            using (var transaction = this.DbContext.Database.BeginTransaction())
            {
                try
                {
                    TransferRequest Model = await this.ReadModelById(Id);
                    Deleted = await this.DeleteAsync(Id);

                    HashSet<int> transferRequestDetails = new HashSet<int>(transferRequestDetailService.DbSet
                        .Where(p => p.TransferRequestId.Equals(Id))
                        .Select(p => p.Id));

                    transferRequestDetailService.Username = this.Username;

                    foreach (int transferRequestDetail in transferRequestDetails)
                    {
                        await transferRequestDetailService.DeleteModel(transferRequestDetail);
                    }

                    
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return Deleted;
        }

        public IQueryable<TransferRequestReportViewModel> GetReportQuery(string trNo, string unitId, string status, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var Query = (from a in DbContext.TransferRequests
                         join b in DbContext.TransferRequestDetails on a.Id equals b.TransferRequestId
                         where a._IsDeleted == false
                             && a.TRNo == (string.IsNullOrWhiteSpace(trNo) ? a.TRNo : trNo)
                             && a.UnitId == (string.IsNullOrWhiteSpace(unitId) ? a.UnitId : unitId)
                             && b.Status == (string.IsNullOrWhiteSpace(status) ? b.Status : status)
                             && a.TRDate.AddHours(offset).Date >= DateFrom.Date
                             && a.TRDate.AddHours(offset).Date <= DateTo.Date
                         select new TransferRequestReportViewModel
                         {
                             trNo = a.TRNo,
                             trDate = a.TRDate,
                             uom = b.UomUnit,
                             unitName = a.UnitName,
                             divisionName = a.DivisionName,
                             categoryName = a.CategoryName,
                             requestedArrivalDate = a.RequestedArrivalDate,
                             productName = b.ProductName,
                             productCode = b.ProductCode,
                             grade = b.Grade,
                             quantity = b.Quantity,
                             isPosted = a.IsPosted,
                             isCanceled = a.IsCanceled,
                             status=b.Status,
                             _updatedDate=a._LastModifiedUtc
                         });

            return Query;
        }

        public Tuple<List<TransferRequestReportViewModel>, int> GetReport(string trNo, string unitId, string status, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset)
        {
            var Query = GetReportQuery(trNo, unitId, status, dateFrom, dateTo, offset);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            if (OrderDictionary.Count.Equals(0))
            {
                Query = Query.OrderByDescending(b => b._updatedDate);
            }
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];

                //Query = Query.OrderBy(string.Concat(Key, " ", OrderType));
            }

            Pageable<TransferRequestReportViewModel> pageable = new Pageable<TransferRequestReportViewModel>(Query, page - 1, size);
            List<TransferRequestReportViewModel> Data = pageable.Data.ToList<TransferRequestReportViewModel>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData);
        }

        public MemoryStream GenerateExcel(string trNo, string unitId, string status, DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            var Query = GetReportQuery(trNo, unitId, status, dateFrom, dateTo, offset);
            Query = Query.OrderByDescending(b => b._updatedDate);
            DataTable result = new DataTable();
            result.Columns.Add(new DataColumn() { ColumnName = "Nomor TR", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal TR", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Unit", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kategori", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Kode Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Nama Barang", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Jumlah", DataType = typeof(double) });
            result.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal diminta datang TR", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Tanggal diminta datang TO Eksternal", DataType = typeof(String) });
            result.Columns.Add(new DataColumn() { ColumnName = "Status", DataType = typeof(String) });
            if (Query.ToArray().Count() == 0)
                result.Rows.Add("", "", "", "", "", "", "", "", "", "", ""); // to allow column name to be generated properly for empty data as template
            else
                foreach (var item in Query)
                    result.Rows.Add((item.trNo), item.trDate, item.unitName, item.categoryName, item.productCode, item.productName, item.quantity, item.uom, item.requestedArrivalDate, item.status);

            return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") }, true);
        }
    }
}
