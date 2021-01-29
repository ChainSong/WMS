using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class ReceiptManagementService : BaseService
    {

        public Response<GetReceiptDetailByConditionResponse> GetReceiptByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetReceiptByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetReceiptDetailByConditionResponse> GetShelvesByIDs(string IDs)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };



            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetShelvesByIDs(IDs);

                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetReceiptDetailByConditionResponse> GetShelvesByIDs2(string IDs, string ProdName)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };



            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetShelvesByIDs2(IDs, ProdName);

                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetReceiptDetailByConditionResponse> GetReceiptByIDs(string IDs)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };



            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetReceiptByIDs(IDs);

                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptDetailByConditionResponse> GetImportReceiptByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetImportReceiptByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetReceiptDetailByConditionResponse> GetImportReceiptByCondition2(GetReceiptByConditionRequest request, string prodName)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetImportReceiptByCondition2(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount, prodName);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptDetailByConditionResponse> GetReceiptForRPTByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.GetReceiptForRPTByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptDetailByConditionResponse> GetAsnScanDiffForRPTByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;
                response.Result = accessor.GetAsnScanDiffForRPTByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptDetailByConditionResponse> ExportReceiptForRPTByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                //int RowCount;
                //if (request.PageSize > 0)
                //{
                response.Result = accessor.ExportReceiptForRPTByCondition(request.SearchCondition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                //}
                //else
                //{
                //    response.Result.PageIndex = 0;
                //    response.Result.PageCount = 0;
                //    response.Result.ReceiptCollection = accessor.GetWarehouseByConditionNoPaging(request.SearchCondition);
                //}
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptDetailByConditionResponse> ExportAsnScanDiffForRPTByCondition(GetReceiptByConditionRequest request)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.ExportAsnScanDiffForRPTByCondition(request.SearchCondition);
                response.Result.PageIndex = request.PageIndex;
   
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetReceiptDetailByConditionResponse> GetReceiptDetailByCondition(GetReceiptDetailByConditionRequest request, string ID)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiptDetailByConditionResponse request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.GetReceiptDetailByCondition(request.SearchCondition, ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetReceiptByIDResponse> ReceiptDelete(string ReceiptNumber)
        {
            Response<GetReceiptByIDResponse> response = new Response<GetReceiptByIDResponse>();
            response.Result = new GetReceiptByIDResponse();
            response.SuccessMessage = new ReceiptManagementAccessor().ReceiptDelete(ReceiptNumber);  //获取执行结果

            if (response.SuccessMessage == "")
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;

            }
            return response;

        }

        public Response<GetAsnByConditionResponse> ASNQuery(GetAsnByConditionRequest request)
        {
            Response<GetAsnByConditionResponse> response = new Response<GetAsnByConditionResponse>() { Result = new GetAsnByConditionResponse() };
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                int RowCount;

                response.Result.ASNCollection = accessor.ASNQuery(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetAsnOrReceiptOrDetailByConditionResponse> ASNDetailQuery(GetAsnOrReceiptOrDetailByConditionRequest request, long ID)
        {
            Response<GetAsnOrReceiptOrDetailByConditionResponse> response = new Response<GetAsnOrReceiptOrDetailByConditionResponse>() { Result = new GetAsnOrReceiptOrDetailByConditionResponse() };
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.ASNDetailQuery(request, ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public Response<GetAsnOrReceiptOrDetailByConditionResponse> ReceiptDetailQuery(GetAsnOrReceiptOrDetailByConditionRequest request, long ID)
        {
            Response<GetAsnOrReceiptOrDetailByConditionResponse> response = new Response<GetAsnOrReceiptOrDetailByConditionResponse>() { Result = new GetAsnOrReceiptOrDetailByConditionResponse() };
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.ReceiptDetailQuery(request, ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<GetAsnOrReceiptOrDetailByConditionResponse> ReceiptDetailAndBarCodeQuery(GetAsnOrReceiptOrDetailByConditionRequest request, long ID)
        {
            Response<GetAsnOrReceiptOrDetailByConditionResponse> response = new Response<GetAsnOrReceiptOrDetailByConditionResponse>() { Result = new GetAsnOrReceiptOrDetailByConditionResponse() };
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.ReceiptDetailAndBarCodeQuery(request, ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<string> AddReceiptAndReceiptDetail(AddReceiptAndReceiptDetailRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.Receipts == null || !request.Receipts.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ReceiptManagementAccessor().AddReceiptAndReceiptDetail(request);
                int a = 0;
                if (int.TryParse(message, out a))
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }

        public Response<string> EditReceiptAndReceiptDetail(AddReceiptAndReceiptDetailRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.Receipts == null || !request.Receipts.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ReceiptManagementAccessor().EditReceiptAndReceiptDetail(request);
                if (message == "更新成功")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }
        public Response<string> EditReceiptAndReceiptDetail_ImportPatch(AddReceiptAndReceiptDetailRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.Receipts == null || !request.Receipts.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ReceiptManagementAccessor().EditReceiptAndReceiptDetail_ImportPatch(request);
                if (message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }


        public Response<string> ReceiptStatusBack(AddReceiptAndReceiptDetailRequest request, int ToStatus)
        {
            Response<string> response = new Response<string>();
            string message = "";


            if (request == null || request.Receipts == null || !request.Receipts.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ReceiptManagementAccessor().ReceiptStatusBack(request, ToStatus);
                if (message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;

        }


        public Response<GetReceiptAndReceriptDetailsResponse> PrintShelves(string rid, int Flag)
        {
            Response<GetReceiptAndReceriptDetailsResponse> response = new Response<GetReceiptAndReceriptDetailsResponse>()
            {
                Result = new GetReceiptAndReceriptDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.PrintShelves(rid, Flag);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }
        public Response<GetReceiptAndReceriptDetailsResponse> PrintShelvesYXDR(string rid, int Flag)
        {
            Response<GetReceiptAndReceriptDetailsResponse> response = new Response<GetReceiptAndReceriptDetailsResponse>()
            {
                Result = new GetReceiptAndReceriptDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.PrintShelvesYXDR(rid, Flag);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }
        public Response<GetReceiptAndReceriptDetailsResponse> PrintShelvesYFBLD(string rid, int Flag)
        {
            Response<GetReceiptAndReceriptDetailsResponse> response = new Response<GetReceiptAndReceriptDetailsResponse>()
            {
                Result = new GetReceiptAndReceriptDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.PrintShelvesYFBLD(rid, Flag);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }
        public Response<GetReceiptAndReceriptDetailsResponse> PrintShelvesNike(string rid, int Flag)
        {
            Response<GetReceiptAndReceriptDetailsResponse> response = new Response<GetReceiptAndReceriptDetailsResponse>()
            {
                Result = new GetReceiptAndReceriptDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.PrintShelvesNike(rid, Flag);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }
        public int GetSkuTotal(string ID, string SKU, string BoxNumber, string Batchs)
        {
            int m = 0;
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                m = accessor.GetSkuTotal(ID, SKU, BoxNumber, Batchs);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }
            return m;
        }

        /// <summary>
        /// 加入库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AddInventoryAKC(string id, string UserName, out string msg)
        {
            msg = "";
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                bool result = accessor.AddInventoryAKC(id, UserName, out msg);
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }


        /// <summary>
        ///  更新体积
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateReceiptVolume(string id,string volume, string UserName, out string msg)
        {
            msg = "";
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                bool result = accessor.UpdateReceiptVolume(id,volume, UserName, out msg);
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }
        /// <summary>
        /// 回退RF装箱数据
        /// </summary>
        /// <param name="ExternReceiptNumber"></param>
        /// <returns></returns>
        public string BackCloseBox(string ExternReceiptNumber)
        {
            string msg = "";
            try
            {
                msg = new ReceiptManagementAccessor().BackClsoeBox(ExternReceiptNumber);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public Response<GetReceiptDetailByConditionResponse> GetReceiptDetailByIDS(string IDS)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.GetReceiptDetailByIDS(IDS);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        /// <summary>
        /// 下发上架任务
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public Response<string> ReceiptTask(string IDS,string Name)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new ReceiptManagementAccessor().ReceiptTask(IDS,Name);
                if (message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 根据ID查询订单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Receipt> GetRceiptInfoByIDs(string ids)
        {
            try
            {
                return new ReceiptManagementAccessor().GetRceiptInfoByIDs(ids);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public Response<GetReceiptAndReceriptDetailsResponse> PrintShelves_JT(string rid, int Flag)
        {
            Response<GetReceiptAndReceriptDetailsResponse> response = new Response<GetReceiptAndReceriptDetailsResponse>()
            {
                Result = new GetReceiptAndReceriptDetailsResponse()
            };
            //if (request == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("AddReceiptAndReceiptDetail request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                ReceiptManagementAccessor accessor = new ReceiptManagementAccessor();
                response.Result = accessor.PrintShelves_JT(rid, Flag);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }



    }
}
