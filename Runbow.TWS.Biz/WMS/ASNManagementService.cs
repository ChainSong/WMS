using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.ASNs;
using System.Data;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Entity.WMS.Receipt;

namespace Runbow.TWS.Biz.WMS
{
    public class ASNManagementService : BaseService
    {
        /// <summary>
        /// 根据单号查扫描情况
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public Response<GetASNByConditionResponse> GetASNScanByAsnNumber(string AsnNumber)
        {
            Response<GetASNByConditionResponse> response = new Response<GetASNByConditionResponse>() { Result = new GetASNByConditionResponse() };
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result = accessor.GetASNScanByAsnNumber(AsnNumber);

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
        /// 检查箱差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="ScanBoxNumber"></param>
        /// <returns></returns>
        public string CheckDiff(string AsnNumber, string ScanBoxNumber)
        {
            string response = "";
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response = accessor.CheckDiff(AsnNumber, ScanBoxNumber);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }
        public string ClearAsnBoxNumber(string AsnNumber, string ScanBoxNumber)
        {
            string response = "";
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response = accessor.ClearAsnBoxNumber(AsnNumber, ScanBoxNumber);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }
        public List<ASNDetail> CheckDiffReturn(string AsnNumber, string ScanBoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                lists = accessor.CheckDiffReturn(AsnNumber, ScanBoxNumber);
            }
            catch (Exception ex)
            {
                
            }

            return lists;
        }
        public List<ASNDetail> GetAsnScanBoxSum(string AsnNumber, string ScanBoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                lists = accessor.GetAsnScanBoxSum(AsnNumber, ScanBoxNumber);
            }
            catch (Exception ex)
            {

            }

            return lists;
        }
        public string AsnScanQtyUpdate(string AsnNumber, string str2, string SKU, string Creator)
        {
            string message = "";
            try
            {
                message = new ASNManagementAccessor().AsnScanQtyUpdate(AsnNumber, str2, SKU,  Creator);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        /// <summary>
        /// 根据单号查明细--ASN扫描
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public Response<GetASNDetailByConditionResponse> GetASNDetailForScanByAsnNumber(string AsnNumber)
        {
            Response<GetASNDetailByConditionResponse> response = new Response<GetASNDetailByConditionResponse>() { Result = new GetASNDetailByConditionResponse() };
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result = accessor.GetASNDetailForScanByAsnNumber(AsnNumber);

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
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetASNByConditionResponse> GetASNByCondition(GetASNByConditionRequest request)
        {
            Response<GetASNByConditionResponse> response = new Response<GetASNByConditionResponse>() { Result = new GetASNByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.ASNCollection = accessor.GetASNByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
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
        ///入库单订单状态统计查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetASNByConditionResponse> GetASNStatusByCondition(GetASNByConditionRequest request)
        {
            Response<GetASNByConditionResponse> response = new Response<GetASNByConditionResponse>() { Result = new GetASNByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNStatusByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result.ASNCollection = accessor.GetASNStatusByCondition(request.SearchCondition);

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
        /// 根据状态查询明细
        /// </summary>
        /// <param name="search"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response<GetASNByConditionResponse> SearchReceiptOrderTotal(ASNSearchCondition search, int type)
        {
            Response<GetASNByConditionResponse> response = new Response<GetASNByConditionResponse>() { Result = new GetASNByConditionResponse() };

            if (search == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNStatusByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                response.Result.ASNCollection = accessor.SearchReceiptOrderTotal(search, type);
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
        ///查询
        public Response<GetASNDetailByConditionResponse> GetASNDetailByConditionResponse(GetASNByConditionRequest request)
        {
            Response<GetASNDetailByConditionResponse> response = new Response<GetASNDetailByConditionResponse>() { Result = new GetASNDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNDetailByConditionResponse request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                int RowCount;
                response.Result = accessor.GetASNandasndetailByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetASNDetailByConditionResponse> GetExportAsnandDetailByCondition(GetASNByConditionRequest request)
        {
            Response<GetASNDetailByConditionResponse> response = new Response<GetASNDetailByConditionResponse>() { Result = new GetASNDetailByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetExportAsnandDetailByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                response.Result = accessor.GetExportAsnandDetailByCondition(request.SearchCondition);
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



        //导出选中
        public Response<GetASNDetailByConditionResponse> GetReceiptByIDs(string IDs)
        {
            Response<GetASNDetailByConditionResponse> response = new Response<GetASNDetailByConditionResponse>() { Result = new GetASNDetailByConditionResponse() };



            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
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

        ///查询上传数据
        public Response<GetReceiptDetailByConditionResponse> GetConfirmAsnAndAsnDetail(string strwhere)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };


            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result = accessor.GetConfirmAsnAndAsnDetail(strwhere);


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

        ///查询上传数据
        public Response<GetReceiptDetailByConditionResponse> GetConfirmAsnAndAsnDetailByNikeCE(string strwhere)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };


            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result = accessor.GetConfirmAsnAndAsnDetailByNikeCE(strwhere);


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

        ///查询取消入库单数据
        public Response<GetASNDetailByConditionResponse> GetCancelAsnAndAsnDetail(string strwhere)
        {
            Response<GetASNDetailByConditionResponse> response = new Response<GetASNDetailByConditionResponse>() { Result = new GetASNDetailByConditionResponse() };


            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();

                response.Result = accessor.GetCancelAsnAndAsnDetail(strwhere);


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



        //明细单条的查看或者编辑
        public Response<ASNAndASNDetail> GetASNInfos(GetASNByConditionRequest request)
        {
            Response<ASNAndASNDetail> response = new Response<ASNAndASNDetail>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                response.Result = accessor.GetASNInfos(request.ID);
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

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> AddasnAndasnDetail(AddASNandASNDetailRequest request)
        {
            Response<string> response = new Response<string>();

            string message = "";
            if (request == null || request.asn == null || !request.asn.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddasnAndasnDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                response.Result = ex.ToString();
                response.IsSuccess = false;
                return response;
            }
            try
            {
                //判断明细中同一外部单号中是否存在相同的sku   如果存在就提示
                DataTable dt = request.asnDetails.ToDataTable();
                if (request.asn.Select(c => c.CustomerID).ToArray().FirstOrDefault() != 71)
                {//永兴判断
                    var dt1 = from t in dt.AsEnumerable()
                              group t by new { t1 = t.Field<string>("ExternReceiptNumber"), t2 = t.Field<string>("SKU"), UPC = t.Field<string>("UPC"), t3 = t.Field<string>("BoxNumber"), t4 = t.Field<string>("BatchNumber"), t5 = t.Field<string>("Unit"), t6 = t.Field<string>("Specifications") } into m
                              select new
                              {
                                  SKU = m.Select(p => p.Field<string>("SKU")).First(),
                                  UPC = m.Select(p => p.Field<string>("UPC")).First(),
                                  ExternReceiptNumber = m.Select(p => p.Field<string>("ExternReceiptNumber")).First(),
                                  BoxNumber = m.Select(p => p.Field<string>("BoxNumber")).First(),
                                  BatchNumber = m.Select(p => p.Field<string>("BatchNumber")).First(),
                                  Unit = m.Select(p => p.Field<string>("Unit")).First(),
                                  Specifications = m.Select(p => p.Field<string>("Specifications")).First(),
                                  count = m.Count(),
                              };

                    var dr1 = dt1.Where(c => c.count > 1);
                    var results = "";
                    if (request.asn.Select(c => c.CustomerID).ToArray().FirstOrDefault() != 74 && request.asn.Select(c => c.CustomerID).ToArray().FirstOrDefault() != 75)
                    {
                        //if (dr1.Count() > 0)
                        //{
                        //    foreach (var item in dr1)
                        //    {
                        //        results += "<p><font color='#FF0000'>外部单号" + item.ExternReceiptNumber + "中的SKU" + item.SKU + "存在重复值！" + "</font></p>";
                        //    }
                        //    response.Result = results;
                        //    response.IsSuccess = false;
                        //    return response;
                        //}
                    }
                }//永兴判断
                message = new ASNManagementAccessor().AddasnAndasnDetail(request);

                if (message.Contains("添加成功") || message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Result = ex.ToString();
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public int ExternKeyCheck(string keys, string flag, long CustomerID)
        {
            int i = 0;
            try
            {


                i = new ASNManagementAccessor().ExternKeyCheck(keys, flag, CustomerID);


            }
            catch (Exception ex)
            {
                LogError(ex);

            }
            return i;
        }
        /// <summary>
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="orderkey"></param>
        /// <returns></returns>
        public Response<GetOrderByConditionResponse> OrderKeyCheck(string orderkey)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };
            try
            {
                response.Result = new ASNManagementAccessor().OrderKeyCheck(orderkey);


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
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="orderkey"></param>
        /// <returns></returns>
        public Response<GetOrderByConditionResponse> OrderNumbersKeyCheck(List<OrderNumbers> orders)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };
            try
            {
                response.Result = new ASNManagementAccessor().OrderNumbersKeyCheck(orders);
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
        /// 导入包装信息
        /// </summary>
        /// <param name="packageinfo"></param>
        /// <returns></returns>
        public Response<string> ImportPackageInfo(AddPackageAndDetailRequest Requset)
        {
            Response<string> response = new Response<string>();
            string error = string.Empty;
            try
            {
                error = new ASNManagementAccessor().ImportPackageInfo(Requset);
                if (error == "1")
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = error;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.SuccessMessage = ex.Message;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        /// <summary>
        /// 导入包装信息
        /// </summary>
        /// <param name="packageinfo"></param>
        ///  <param name="Proc">调用不同存储过程</param>
        /// <returns></returns>
        public Response<string> ImportPackageInfo(AddPackageAndDetailRequest Requset, string Proc)
        {
            Response<string> response = new Response<string>();
            string error = string.Empty;
            try
            {
                error = new ASNManagementAccessor().ImportPackageInfo(Requset, Proc);
                if (error == "1")
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = error;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.SuccessMessage = ex.Message;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<bool> UpdateasnAndasnDetail(AddASNandASNDetailRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.asn == null || !request.asn.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateasnAndasnDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                if (new ASNManagementAccessor().UpdateasnAndasnDetail(request))
                {
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        /// <summary>
        /// 批量转入库单
        /// </summary>
        /// <param name="ASNIDs"></param>
        /// <returns></returns>
        public Response<bool> InsertIntoReceiptAndReceiptDetails(string ASNIDs)
        {
            Response<bool> response = new Response<bool>();
            if (ASNIDs == null || ASNIDs == "")
            {
                ArgumentNullException ex = new ArgumentNullException("InsertIntoReceiptAndReceiptDetails request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                if (new ASNManagementAccessor().InsertIntoReceiptAndReceiptDetails(ASNIDs))
                {
                    //response.IsSuccess = true;
                }
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
        //生成上架库位
        public Response<bool> CreateShelfLocation(string id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new ASNManagementAccessor().CreateShelfLocation(id))
                {
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }

        //取消操作
        public Response<bool> ASNDelete(int id)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                if (new ASNManagementAccessor().ASNDelete(id))
                {
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        //批量取消操作
        public bool StatusBacks(string asnid)
        {
            bool ve = true;
            try
            {
                ve = new ASNManagementAccessor().StatusBacks(asnid);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return ve;
        }
        //批量完成操作
        public bool CompletALLSelect(string asnid)
        {
            bool ve = true;
            try
            {
                ve = new ASNManagementAccessor().CompletALLSelect(asnid);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return ve;
        }

        //完成操作
        public bool Complet(int ID)
        {
            bool ve = true;
            try
            {
                ve = new ASNManagementAccessor().Complet(ID);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return ve;
        }


        /// <summary>
        /// 更新订单上传状态
        /// </summary>
        /// <param name="Type">Type 更新的订单类型  1, 入库单  2, 出库单一次回传 3 出库单二次回传 ,4 调整 5, 其他</param>
        /// <param name="IDs">要更新的订单ID集合,多个用英文逗号分割 如 122,1123,111  ,传入预入库单或预出库单ID </param>
        public void UpdateConfirmStatus(string Type, string IDs, string DocNumber)
        {

            try
            {
                new ASNManagementAccessor().UpdateConfirmStatus(Type, IDs, DocNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IEnumerable<Receipt> CountReceipt(int ID)
        {
            IEnumerable<Receipt> s = new List<Receipt>();
            try
            {
                s = new ASNManagementAccessor().CountReceipt(ID);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return s;
        }
        public int CountAsn(string ExternNumber, long customerID)
        {
            int s = 0;
            try
            {
                s = new ASNManagementAccessor().CountAsn(ExternNumber, customerID);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return s;
        }
        //得到ASN表信息
        public Response<IEnumerable<ASN>> GetASNInfo()
        {
            Response<IEnumerable<ASN>> response = new Response<IEnumerable<ASN>>();
            try
            {
                response.Result = new ASNManagementAccessor().GetASNInfo();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }
        public bool UpdateMDNAkzo(string DocNumber)
        {
            try
            {
                new ASNManagementAccessor().UpdateMDNAkzo(DocNumber);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查询asn异常跟踪信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>    
        public IEnumerable<ASNAbnormalTracking> GetASNAbnormalList(ASNAbnormalSearchCondition search, out string msg, out int rowcounts)
        {
            msg = "";
            rowcounts = 0;
            try
            {
                return new ASNManagementAccessor().GetASNAbnormalList(search, out rowcounts);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                rowcounts = 0;
                return null;
            }
        }

        /// <summary>
        /// 新增、修改asn异常信息
        /// </summary>
        /// <param name="Abnormal"></param>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddorUpdateASNAbnormal(ASNAbnormalTracking Abnormal, int type, out string msg)
        {
            msg = "";
            try
            {
                return new ASNManagementAccessor().AddorUpdateASNAbnormal(Abnormal, type, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        /// <summary>
        /// 导出asn异常跟踪信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>    
        public IEnumerable<ASNAbnormalTracking> ExportASNAbnormal(ASNAbnormalSearchCondition search)
        {
            try
            {
                return new ASNManagementAccessor().ExportASNAbnormal(search);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除收货异常信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteASNAbnormal(long ID, out string msg)
        {
            msg = "";
            try
            {
                return new ASNManagementAccessor().DeleteASNAbnormal(ID);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        //NIKE退货仓-查询箱号
        public Response<GetASNNewBoxLabelByConditionResponse> GetASNNewBoxLabelByConditionResponse(GetASNNewBoxLabelByConditionRequest request)
        {
            Response<GetASNNewBoxLabelByConditionResponse> response = new Response<GetASNNewBoxLabelByConditionResponse>() { Result = new GetASNNewBoxLabelByConditionResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetASNNewBoxLabelByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ASNManagementAccessor accessor = new ASNManagementAccessor();
                int RowCount;
                response.Result = accessor.GetASNNewBoxLabelByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        //NIKE退货仓-
        public int Insertnewbox(string customerid, string ExternReceiptNumber, int total,string warehouseid,string GoodsType)
        {
            int s = 0;
            try
            {
                s = new ASNManagementAccessor().Insertnewbox(customerid, ExternReceiptNumber, total, warehouseid, GoodsType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }
        //NIKE退货仓-
        public IEnumerable<ASNNewBoxLabel> GetPrintASNNewBoxLabel(string ids, int type)
        {
            try
            {
                return new ASNManagementAccessor().GetPrintASNNewBoxLabel(ids, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //NIKE退货仓-
        public string GetLocationLabelBySKU(string AsnNumber, string ScanSKU,string GoodsType)
        {
            try
            {
                return new ASNManagementAccessor().GetLocationLabelBySKU( AsnNumber,  ScanSKU, GoodsType);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<ASNDetailLocation> GetLocationLabelByASNNumber(string AsnNumber)
        {
            try
            {
                return new ASNManagementAccessor().GetLocationLabelByASNNumber(AsnNumber);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int ExternKeyCheck_TH(string pon, string ern, string plno, string flag, long CustomerID)
        {
            int i = 0;
            try
            {
                i = new ASNManagementAccessor().ExternKeyCheck_TH(pon, ern, plno, flag, CustomerID);
                
            }
            catch (Exception ex)
            {
                LogError(ex);

            }
            return i;
        }
        //NIKE退货仓-
        public int UDnewboxPrintedTimes(string customerid, string warehouseid, string boxids)
        {
            int s = 0;
            try
            {
                s = new ASNManagementAccessor().UDnewboxPrintedTimes(customerid, warehouseid, boxids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return s;
        }

    }
}
